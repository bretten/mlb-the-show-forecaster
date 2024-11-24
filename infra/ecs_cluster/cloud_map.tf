# Private DNS namespace for the VPC private instances
resource "aws_service_discovery_private_dns_namespace" "private_dns_namespace" {
  name        = "${var.resource_prefix}-namespace"
  description = null
  vpc         = var.vpc_id

  tags = var.root_tags
}

# Service discovery for player tracker
resource "aws_service_discovery_service" "discovery_service_player_tracker" {
  name          = "player-tracker"
  description   = null
  namespace_id  = aws_service_discovery_private_dns_namespace.private_dns_namespace.id
  force_destroy = false

  dns_config {
    namespace_id   = aws_service_discovery_private_dns_namespace.private_dns_namespace.id
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

# Service discovery for performance tracker
resource "aws_service_discovery_service" "discovery_service_performance_tracker" {
  name          = "performance-tracker"
  description   = null
  namespace_id  = aws_service_discovery_private_dns_namespace.private_dns_namespace.id
  force_destroy = false

  dns_config {
    namespace_id   = aws_service_discovery_private_dns_namespace.private_dns_namespace.id
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

# Service discovery for marketplace watcher
resource "aws_service_discovery_service" "discovery_service_marketplace_watcher" {
  name          = "marketplace-watcher"
  description   = null
  namespace_id  = aws_service_discovery_private_dns_namespace.private_dns_namespace.id
  force_destroy = false

  dns_config {
    namespace_id   = aws_service_discovery_private_dns_namespace.private_dns_namespace.id
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