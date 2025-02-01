# postgresql
resource "aws_ecs_task_definition" "task_definition_postgresql" {
  family                   = "${var.resource_prefix}-postgresql"
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
        name      = "${var.resource_prefix}-postgresql"
        image     = "postgres:16-alpine"
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-create-group  = "true"
            awslogs-group         = "/ecs/${var.resource_prefix}-postgresql"
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
            containerPort = 5432
            hostPort      = 5432
            name          = "${var.resource_prefix}-postgresql-5432-tcp"
            protocol      = "tcp"
          },
        ]
        environment = [
          {
            name  = "LANG"
            value = "en_US.utf8"
          },
          {
            name  = "POSTGRES_INITDB_ARGS"
            value = "--locale-provider=icu --icu-locale=en-US"
          },
          {
            name  = "POSTGRES_DB"
            value = var.pgsql_db_name
          },
          {
            name  = "POSTGRES_USER"
            value = var.pgsql_user
          },
          {
            name  = "POSTGRES_PASSWORD"
            value = var.pgsql_pass
          }
        ]
        command = [
          "postgres",
          "-c", "log_connections=on",
          "-c", "log_disconnections=on",
          "-c", "log_statement=all"
        ]
        environmentFiles = []
        mountPoints = [
          {
            sourceVolume  = "postgres-volume"
            containerPath = "/var/lib/postgresql/data"
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
    name = "postgres-volume"

    efs_volume_configuration {
      file_system_id          = aws_efs_file_system.efs_storage.id
      transit_encryption      = "ENABLED"
      transit_encryption_port = 2999
      authorization_config {
        access_point_id = aws_efs_access_point.efs_access_storage_postgres.id
      }
    }
  }

  tags = var.root_tags

  depends_on = [aws_efs_access_point.efs_access_storage_postgres]
}

# Service discovery for Postgresql
resource "aws_service_discovery_service" "discovery_service_postgresql" {
  name          = "postgresql"
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

# Postgresql service
resource "aws_ecs_service" "ecs_service_postgresql" {
  name            = "${var.resource_prefix}-postgresql"
  cluster         = var.main_cluster_id
  task_definition = aws_ecs_task_definition.task_definition_postgresql.arn
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
    registry_arn   = aws_service_discovery_service.discovery_service_postgresql.arn
  }

  capacity_provider_strategy {
    base              = 1
    weight            = 100
    capacity_provider = var.capacity_provider
  }

  tags = var.root_tags
}