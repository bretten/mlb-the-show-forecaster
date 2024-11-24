# Gateway app service
resource "aws_ecs_service" "ecs_service_gateway" {
  name            = "${var.resource_prefix}-gateway"
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.task_definition_gateway.arn
  desired_count   = 1

  deployment_maximum_percent         = 200
  deployment_minimum_healthy_percent = 100

  enable_ecs_managed_tags           = true
  enable_execute_command            = false
  wait_for_steady_state             = false
  health_check_grace_period_seconds = 0
  launch_type                       = "FARGATE"
  platform_version                  = "LATEST"
  propagate_tags                    = "NONE"
  scheduling_strategy               = "REPLICA"
  triggers = {}

  deployment_circuit_breaker {
    enable   = true
    rollback = true
  }

  deployment_controller {
    type = "ECS"
  }

  load_balancer {
    container_name   = var.resource_prefix
    container_port   = var.port_gateway
    elb_name         = null
    target_group_arn = var.load_balancer_target_group_arn
  }

  network_configuration {
    assign_public_ip = true
    security_groups = [
      var.security_group_id_public,
      var.security_group_id_private_access
    ]
    subnets = [
      var.subnet_id_public1,
      var.subnet_id_public2,
      var.subnet_id_private
    ]
  }

  tags = var.root_tags
}

# Player tracker service
resource "aws_ecs_service" "ecs_service_player_tracker" {
  name            = "${var.resource_prefix}-player-tracker"
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.task_definition_player_tracker.arn
  desired_count   = 1

  deployment_maximum_percent         = 200
  deployment_minimum_healthy_percent = 100

  enable_ecs_managed_tags           = true
  enable_execute_command            = false
  wait_for_steady_state             = false
  health_check_grace_period_seconds = 0
  launch_type                       = "FARGATE"
  platform_version                  = "LATEST"
  propagate_tags                    = "NONE"
  scheduling_strategy               = "REPLICA"
  triggers = {}

  deployment_circuit_breaker {
    enable   = true
    rollback = true
  }

  deployment_controller {
    type = "ECS"
  }

  network_configuration {
    assign_public_ip = false
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
    registry_arn   = aws_service_discovery_service.discovery_service_player_tracker.arn
  }

  tags = var.root_tags
}

# Performance tracker service
resource "aws_ecs_service" "ecs_service_performance_tracker" {
  name            = "${var.resource_prefix}-performance-tracker"
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.task_definition_performance_tracker.arn
  desired_count   = 1

  deployment_maximum_percent         = 200
  deployment_minimum_healthy_percent = 100

  enable_ecs_managed_tags           = true
  enable_execute_command            = false
  wait_for_steady_state             = false
  health_check_grace_period_seconds = 0
  launch_type                       = "FARGATE"
  platform_version                  = "LATEST"
  propagate_tags                    = "NONE"
  scheduling_strategy               = "REPLICA"
  triggers = {}

  deployment_circuit_breaker {
    enable   = true
    rollback = true
  }

  deployment_controller {
    type = "ECS"
  }

  network_configuration {
    assign_public_ip = false
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
    registry_arn   = aws_service_discovery_service.discovery_service_performance_tracker.arn
  }

  tags = var.root_tags
}

# Marketplace watcher service
resource "aws_ecs_service" "ecs_service_marketplace_watcher" {
  name            = "${var.resource_prefix}-marketplace-watcher"
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.task_definition_marketplace_watcher.arn
  desired_count   = 1

  deployment_maximum_percent         = 200
  deployment_minimum_healthy_percent = 100

  enable_ecs_managed_tags           = true
  enable_execute_command            = false
  wait_for_steady_state             = false
  health_check_grace_period_seconds = 0
  launch_type                       = "FARGATE"
  platform_version                  = "LATEST"
  propagate_tags                    = "NONE"
  scheduling_strategy               = "REPLICA"
  triggers = {}

  deployment_circuit_breaker {
    enable   = true
    rollback = true
  }

  deployment_controller {
    type = "ECS"
  }

  network_configuration {
    assign_public_ip = false
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
    registry_arn   = aws_service_discovery_service.discovery_service_marketplace_watcher.arn
  }

  tags = var.root_tags
}