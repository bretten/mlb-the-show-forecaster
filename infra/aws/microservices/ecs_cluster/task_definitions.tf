# Task definition for gateway app
resource "aws_ecs_task_definition" "task_definition_gateway" {
  family                   = "${var.resource_prefix}-gateway"
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
        name      = "${var.resource_prefix}-gateway"
        image     = var.task_def_gateway_image
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-group         = aws_cloudwatch_log_group.logs_gateway.name
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
            containerPort = var.port_gateway
            hostPort      = var.port_gateway
            name          = "${var.resource_prefix}-${var.port_gateway}-tcp"
            protocol      = "tcp"
          },
        ]
        environment = [
          {
            name  = "ASPNETCORE_ENVIRONMENT"
            value = var.aspnetcore_environment
          },
          {
            name  = "Urls"
            value = "http://*:${var.port_gateway}"
          },
          {
            name  = "Auth__Jwt__Authority"
            value = var.jwt_authority
          },
          {
            name  = "Auth__Jwt__Audience"
            value = var.jwt_audience
          },
          {
            name  = "Auth__AllowCors"
            value = "false"
          },
          {
            name  = "Aws__Region"
            value = var.aws_region
          },
          {
            name  = "Spa__Active"
            value = "true"
          },
          {
            name  = "SignalRMultiplexer__Interval"
            value = "00:00:00:05"
          },
          {
            name  = "TargetApps__PlayerTracker__Scheme"
            value = "http"
          },
          {
            name  = "TargetApps__PlayerTracker__Host"
            value = local.dns_name_player_tracker
          },
          {
            name  = "TargetApps__PerformanceTracker__Scheme"
            value = "http"
          },
          {
            name  = "TargetApps__PerformanceTracker__Host"
            value = local.dns_name_performance_tracker
          },
          {
            name  = "TargetApps__MarketplaceWatcher__Scheme"
            value = "http"
          },
          {
            name  = "TargetApps__MarketplaceWatcher__Host"
            value = local.dns_name_marketplace_watcher
          }
        ]
        environmentFiles = []
        mountPoints      = []
        systemControls   = []
        ulimits          = []
        volumesFrom      = []
      },
    ]
  )

  runtime_platform {
    cpu_architecture        = "X86_64"
    operating_system_family = "LINUX"
  }

  tags = var.root_tags
}

# Task definition for player tracker
resource "aws_ecs_task_definition" "task_definition_player_tracker" {
  family                   = "${var.resource_prefix}-player-tracker"
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
        name      = "${var.resource_prefix}-player-tracker"
        image     = var.task_def_player_tracker_image
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-group         = aws_cloudwatch_log_group.logs_player_tracker.name
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
            containerPort = var.port_player_tracker
            hostPort      = var.port_player_tracker
            name          = "${var.resource_prefix}-player-tracker-${var.port_player_tracker}-tcp"
            protocol      = "tcp"
          },
        ]
        environment = [
          {
            name  = "ASPNETCORE_ENVIRONMENT"
            value = var.aspnetcore_environment
          },
          {
            name  = "RunMigrations"
            value = "true"
          },
          {
            name  = "Jobs__RunOnStartup"
            value = "false"
          },
          {
            name  = "Urls"
            value = "http://*:${var.port_player_tracker}"
          },
          {
            name  = "ConnectionStrings__Players"
            value = local.pgsql_cs
          },
          {
            name  = "Messaging__RabbitMq__HostName"
            value = local.rabbitmq_dns_name
          },
          {
            name  = "Messaging__RabbitMq__UserName"
            value = var.rabbitmq_user
          },
          {
            name  = "Messaging__RabbitMq__Password"
            value = var.rabbitmq_pass
          }
        ]
        environmentFiles = []
        mountPoints      = []
        systemControls   = []
        ulimits          = []
        volumesFrom      = []
      },
    ]
  )

  runtime_platform {
    cpu_architecture        = "X86_64"
    operating_system_family = "LINUX"
  }

  tags = var.root_tags
}

# Task definition for performance tracker
resource "aws_ecs_task_definition" "task_definition_performance_tracker" {
  family                   = "${var.resource_prefix}-performance-tracker"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = "1024"
  memory                   = "2048"
  task_role_arn            = null
  execution_role_arn       = var.task_execution_role_arn
  skip_destroy             = false

  container_definitions = jsonencode(
    [
      {
        essential = true
        name      = "${var.resource_prefix}-performance-tracker"
        image     = var.task_def_performance_tracker_image
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-group         = aws_cloudwatch_log_group.logs_performance_tracker.name
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
            containerPort = var.port_performance_tracker
            hostPort      = var.port_performance_tracker
            name          = "${var.resource_prefix}-performance-tracker-${var.port_performance_tracker}-tcp"
            protocol      = "tcp"
          },
        ]
        environment = [
          {
            name  = "ASPNETCORE_ENVIRONMENT"
            value = var.aspnetcore_environment
          },
          {
            name  = "RunMigrations"
            value = "true"
          },
          {
            name  = "Jobs__RunOnStartup"
            value = "false"
          },
          {
            name  = "Urls"
            value = "http://*:${var.port_performance_tracker}"
          },
          {
            name  = "ConnectionStrings__PlayerSeasons"
            value = local.pgsql_cs
          },
          {
            name  = "Messaging__RabbitMq__HostName"
            value = local.rabbitmq_dns_name
          },
          {
            name  = "Messaging__RabbitMq__UserName"
            value = var.rabbitmq_user
          },
          {
            name  = "Messaging__RabbitMq__Password"
            value = var.rabbitmq_pass
          }
        ]
        environmentFiles = []
        mountPoints      = []
        systemControls   = []
        ulimits          = []
        volumesFrom      = []
      },
    ]
  )

  runtime_platform {
    cpu_architecture        = "X86_64"
    operating_system_family = "LINUX"
  }

  tags = var.root_tags
}

# Task definition for marketplace watcher
resource "aws_ecs_task_definition" "task_definition_marketplace_watcher" {
  family                   = "${var.resource_prefix}-marketplace-watcher"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = "1024"
  memory                   = "2048"
  task_role_arn            = null
  execution_role_arn       = var.task_execution_role_arn
  skip_destroy             = false

  container_definitions = jsonencode(
    [
      {
        essential = true
        name      = "${var.resource_prefix}-marketplace-watcher"
        image     = var.task_def_marketplace_watcher_image
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-group         = aws_cloudwatch_log_group.logs_marketplace.name
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
            containerPort = var.port_marketplace_watcher
            hostPort      = var.port_marketplace_watcher
            name          = "${var.resource_prefix}-marketplace-watcher-${var.port_marketplace_watcher}-tcp"
            protocol      = "tcp"
          },
        ]
        environment = [
          {
            name  = "ASPNETCORE_ENVIRONMENT"
            value = var.aspnetcore_environment
          },
          {
            name  = "RunMigrations"
            value = "true"
          },
          {
            name  = "Jobs__RunOnStartup"
            value = "false"
          },
          {
            name  = "Urls"
            value = "http://*:${var.port_marketplace_watcher}"
          },
          {
            name  = "Api__Performance__BaseAddress"
            value = local.url_performance_tracker
          },
          {
            name  = "Forecasting__PlayerMatcher__BaseAddress"
            value = local.url_player_tracker
          },
          {
            name  = "ConnectionStrings__Cards"
            value = local.pgsql_cs
          },
          {
            name  = "ConnectionStrings__Forecasts"
            value = local.pgsql_cs
          },
          {
            name  = "ConnectionStrings__Marketplace"
            value = local.pgsql_cs
          },
          {
            name  = "ConnectionStrings__TrendsMongoDb"
            value = local.mongodb_cs
          },
          {
            name  = "Messaging__RabbitMq__HostName"
            value = local.rabbitmq_dns_name
          },
          {
            name  = "Messaging__RabbitMq__UserName"
            value = var.rabbitmq_user
          },
          {
            name  = "Messaging__RabbitMq__Password"
            value = var.rabbitmq_pass
          }
        ]
        environmentFiles = []
        mountPoints      = []
        systemControls   = []
        ulimits          = []
        volumesFrom      = []
      },
    ]
  )

  runtime_platform {
    cpu_architecture        = "X86_64"
    operating_system_family = "LINUX"
  }

  tags = var.root_tags
}