# redis logs
resource "aws_cloudwatch_log_group" "logs_redis" {
  name              = "/ecs/${var.resource_prefix}-redis"
  retention_in_days = 7

  tags = var.root_tags
}

# redis
resource "aws_ecs_task_definition" "task_definition_redis" {
  family                   = "${var.resource_prefix}-redis"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = "1024"
  memory                   = "8192"
  task_role_arn            = null
  execution_role_arn       = var.task_execution_role_arn
  skip_destroy             = false

  container_definitions = jsonencode(
    [
      {
        essential = true
        name      = "${var.resource_prefix}-redis"
        image     = "redis/redis-stack:7.4.0-v3"
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-group         = aws_cloudwatch_log_group.logs_redis.name
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
            containerPort = 6379
            hostPort      = 6379
            name          = "${var.resource_prefix}-redis-6379-tcp"
            protocol      = "tcp"
          },
          {
            appProtocol   = "http"
            containerPort = 8001
            hostPort      = 8001
            name          = "${var.resource_prefix}-redis-insight-8001-tcp"
            protocol      = "tcp"
          },
        ]
        healthCheck = {
          command     = ["CMD-SHELL", "redis-cli -a ${var.redis_pass} ping"]
          interval    = 300
          retries     = 5
          startPeriod = 30
          timeout     = 60
        }
        environment = [
          {
            name  = "REDIS_ARGS"
            value = "--requirepass ${var.redis_pass} --appendonly yes --appendfsync everysec"
          }
        ]
        environmentFiles = []
        mountPoints = [
          {
            sourceVolume  = "redis-volume"
            containerPath = "/data"
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
    name = "redis-volume"

    efs_volume_configuration {
      file_system_id          = aws_efs_file_system.efs_storage.id
      transit_encryption      = "ENABLED"
      transit_encryption_port = 2999
      authorization_config {
        access_point_id = aws_efs_access_point.efs_access_storage_redis.id
      }
    }
  }

  tags = var.root_tags

  depends_on = [aws_efs_access_point.efs_access_storage_redis]
}

# Service discovery for redis
resource "aws_service_discovery_service" "discovery_service_redis" {
  name          = "redis"
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

# redis service
resource "aws_ecs_service" "ecs_service_redis" {
  name            = "${var.resource_prefix}-redis"
  cluster         = var.main_cluster_id
  task_definition = aws_ecs_task_definition.task_definition_redis.arn
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
    registry_arn   = aws_service_discovery_service.discovery_service_redis.arn
  }

  capacity_provider_strategy {
    base              = 1
    weight            = 100
    capacity_provider = "FARGATE"
  }

  tags = var.root_tags
}