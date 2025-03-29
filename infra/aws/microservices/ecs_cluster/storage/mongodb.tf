# mongodb logs
resource "aws_cloudwatch_log_group" "logs_mongodb" {
  name              = "/ecs/${var.resource_prefix}-mongodb"
  retention_in_days = 7

  tags = var.root_tags
}

# mongodb
resource "aws_ecs_task_definition" "task_definition_mongodb" {
  family                   = "${var.resource_prefix}-mongodb"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = "512"
  memory                   = "1024"
  task_role_arn            = null
  execution_role_arn       = var.task_execution_role_arn
  skip_destroy             = false

  container_definitions = jsonencode(
    [
      {
        essential = true
        name      = "${var.resource_prefix}-mongodb"
        image     = "mongo:noble"
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-group         = aws_cloudwatch_log_group.logs_mongodb.name
            awslogs-region        = var.aws_region
            awslogs-stream-prefix = "ecs"
            max-buffer-size       = "25m"
            mode                  = "non-blocking"
          }
          secretOptions = []
        }
        portMappings = [
          {
            appProtocol   = "http"
            containerPort = 27017
            hostPort      = 27017
            name          = "${var.resource_prefix}-mongodb-27017-tcp"
            protocol      = "tcp"
          },
        ]
        healthCheck = {
          command     = ["CMD-SHELL", "echo 'db.runCommand(\"ping\").ok' | mongosh localhost:27017/test --quiet"]
          interval    = 300
          retries     = 5
          startPeriod = 30
          timeout     = 60
        }
        environment = [
          {
            name  = "MONGO_INITDB_ROOT_USERNAME"
            value = var.mongodb_user
          },
          {
            name  = "MONGO_INITDB_ROOT_PASSWORD"
            value = var.mongodb_pass
          }
        ]
        environmentFiles = []
        mountPoints = [
          {
            sourceVolume  = "mongodb-volume"
            containerPath = "/data/db"
          }
        ]
        systemControls = []
        ulimits        = []
        volumesFrom    = []
      },
    ]
  )

  runtime_platform {
    cpu_architecture        = "X86_64"
    operating_system_family = "LINUX"
  }

  volume {
    name = "mongodb-volume"

    efs_volume_configuration {
      file_system_id          = aws_efs_file_system.efs_storage.id
      transit_encryption      = "ENABLED"
      transit_encryption_port = 2999
      authorization_config {
        access_point_id = aws_efs_access_point.efs_access_storage_mongodb.id
      }
    }
  }

  tags = var.root_tags

  depends_on = [aws_efs_access_point.efs_access_storage_mongodb]
}

# Service discovery for mongodb
resource "aws_service_discovery_service" "discovery_service_mongodb" {
  name          = "mongodb"
  description   = null
  namespace_id  = var.private_dns_namespace_id
  force_destroy = false

  dns_config {
    namespace_id   = var.private_dns_namespace_id
    routing_policy = "MULTIVALUE"

    dns_records {
      ttl  = 15
      type = "A"
    }
  }

  health_check_custom_config {
    failure_threshold = 1
  }

  tags = var.root_tags
}

# mongodb service
resource "aws_ecs_service" "ecs_service_mongodb" {
  name            = "${var.resource_prefix}-mongodb"
  cluster         = var.main_cluster_id
  task_definition = aws_ecs_task_definition.task_definition_mongodb.arn
  desired_count   = 1

  deployment_maximum_percent         = 200
  deployment_minimum_healthy_percent = 100

  enable_ecs_managed_tags           = true
  enable_execute_command            = false
  wait_for_steady_state             = false
  health_check_grace_period_seconds = 0
  platform_version                  = "LATEST"
  propagate_tags                    = "NONE"
  scheduling_strategy               = "REPLICA"
  triggers                          = {}

  deployment_circuit_breaker {
    enable   = true
    rollback = true
  }

  deployment_controller {
    type = "ECS"
  }

  network_configuration {
    assign_public_ip = !var.use_nat_gateway
    security_groups = [
      var.security_group_id_private
    ]
    subnets = [
      var.subnet_id_private
    ]
  }

  service_registries {
    container_name = null
    container_port = 0
    port           = 0
    registry_arn   = aws_service_discovery_service.discovery_service_mongodb.arn
  }

  capacity_provider_strategy {
    base              = 1
    weight            = 100
    capacity_provider = var.capacity_provider
  }

  tags = var.root_tags
}