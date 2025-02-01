# rabbitmq
resource "aws_ecs_task_definition" "task_definition_rabbitmq" {
  family                   = "${var.resource_prefix}-rabbitmq"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = "256"
  memory                   = "512"
  task_role_arn            = null
  execution_role_arn       = var.task_execution_role_arn
  skip_destroy             = false

  container_definitions = jsonencode(
    [
      {
        essential = true
        name      = "${var.resource_prefix}-rabbitmq"
        image     = "rabbitmq:3-management"
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-create-group  = "true"
            awslogs-group         = "/ecs/${var.resource_prefix}-rabbitmq"
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
            containerPort = 5672
            hostPort      = 5672
            name          = "${var.resource_prefix}-rabbitmq-5672-tcp"
            protocol      = "tcp"
          },
          {
            appProtocol   = "http"
            containerPort = 15672
            hostPort      = 15672
            name          = "${var.resource_prefix}-rabbitmq-management-15672-tcp"
            protocol      = "tcp"
          },
        ]
        environment = [
          {
            name  = "RABBITMQ_DEFAULT_USER"
            value = var.rabbitmq_user
          },
          {
            name  = "RABBITMQ_DEFAULT_PASS"
            value = var.rabbitmq_pass
          },
          # Node name determines how data is stored and by default it is the container hostname. Set a constant value to ensure persistence. Adding @localhost prevents RabbitMQ from appending the container's @ip
          {
            name  = "RABBITMQ_NODENAME",
            value = "rabbit@localhost"
          }
        ]
        environmentFiles = []
        mountPoints = [
          {
            sourceVolume  = "rabbitmq-volume"
            containerPath = "/var/lib/rabbitmq"
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
    name = "rabbitmq-volume"

    efs_volume_configuration {
      file_system_id          = aws_efs_file_system.efs_storage.id
      transit_encryption      = "ENABLED"
      transit_encryption_port = 2999
      authorization_config {
        access_point_id = aws_efs_access_point.efs_access_storage_rabbitmq.id
      }
    }
  }

  tags = var.root_tags

  depends_on = [aws_efs_access_point.efs_access_storage_rabbitmq]
}

# Service discovery for rabbitmq
resource "aws_service_discovery_service" "discovery_service_rabbitmq" {
  name          = "rabbitmq"
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

# rabbitmq service
resource "aws_ecs_service" "ecs_service_rabbitmq" {
  name            = "${var.resource_prefix}-rabbitmq"
  cluster         = var.main_cluster_id
  task_definition = aws_ecs_task_definition.task_definition_rabbitmq.arn
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
    registry_arn   = aws_service_discovery_service.discovery_service_rabbitmq.arn
  }

  capacity_provider_strategy {
    base              = 1
    weight            = 100
    capacity_provider = var.capacity_provider
  }

  tags = var.root_tags
}