provider "aws" {
  region = var.aws_region
}

terraform {
  backend "s3" {
    bucket = "tformbackend"
    key    = "mlb-the-show-forecaster/terraform.tfstate"
    region = "us-west-2"
  }
}

# VPC where the gateway can interact with the private, backend domain services
module "vpc" {
  source                           = "./vpc"
  aws_region                       = var.aws_region
  root_tags                        = var.root_tags
  resource_prefix                  = var.resource_prefix
  cidr_vpc                         = var.cidr_vpc
  cidr_subnet_public1              = var.cidr_subnet_public1
  cidr_subnet_public2              = var.cidr_subnet_public2
  cidr_subnet_private              = var.cidr_subnet_private
  cidr_ipv4_anywhere               = var.cidr_ipv4_anywhere
  availability_zone_subnet_public1 = var.availability_zone_subnet_public1
  availability_zone_subnet_public2 = var.availability_zone_subnet_public2
  availability_zone_subnet_private = var.availability_zone_subnet_private
  use_nat_gateway                  = var.use_nat_gateway
  port_gateway                     = var.port_gateway
  port_player_tracker              = var.port_player_tracker
  port_performance_tracker         = var.port_performance_tracker
  port_marketplace_watcher         = var.port_marketplace_watcher
}

# Load balancer that handles traffic to the gateway app
module "load_balancer" {
  source            = "./load_balancer"
  root_tags         = var.root_tags
  resource_prefix   = var.resource_prefix
  security_group_id = module.vpc.security_group_id_load_balancer
  subnet_id_public1 = module.vpc.subnet_id_public1
  subnet_id_public2 = module.vpc.subnet_id_public2
  vpc_id            = module.vpc.vpc_id
  certificate_arn   = aws_acm_certificate.cert.arn
  port_gateway      = var.port_gateway
  health_alerts_arn = aws_sns_topic.health_alerts.arn
  depends_on        = [module.vpc]
}

# ECS cluster that runs .NET services as containers
module "ecs_cluster" {
  source                             = "./ecs_cluster"
  aws_region                         = var.aws_region
  resource_prefix                    = var.resource_prefix
  root_tags                          = var.root_tags
  task_def_gateway_image             = "${var.container_registry_url}/gateway:${var.image_tag}"
  task_def_player_tracker_image      = "${var.container_registry_url}/player-tracker:${var.image_tag}"
  task_def_performance_tracker_image = "${var.container_registry_url}/performance-tracker:${var.image_tag}"
  task_def_marketplace_watcher_image = "${var.container_registry_url}/marketplace-watcher:${var.image_tag}"
  task_execution_role_arn            = aws_iam_role.role_ecs_task_execution.arn
  load_balancer_target_group_arn     = module.load_balancer.load_balancer_target_group_arn
  vpc_id                             = module.vpc.vpc_id
  security_group_id_public           = module.vpc.security_group_id_public
  security_group_id_private          = module.vpc.security_group_id_private
  security_group_id_private_access   = module.vpc.security_group_id_private_access
  subnet_id_public1                  = module.vpc.subnet_id_public1
  subnet_id_public2                  = module.vpc.subnet_id_public2
  subnet_id_private                  = module.vpc.subnet_id_private
  port_gateway                       = var.port_gateway
  port_player_tracker                = var.port_player_tracker
  port_performance_tracker           = var.port_performance_tracker
  port_marketplace_watcher           = var.port_marketplace_watcher
  use_storage                        = var.use_storage
  use_nat_gateway                    = var.use_nat_gateway
  capacity_provider                  = var.use_spot_instances == true ? "FARGATE_SPOT" : "FARGATE"
  health_alerts_arn                  = aws_sns_topic.health_alerts.arn
  jwt_authority                      = var.jwt_authority
  jwt_audience                       = var.jwt_audience
  aspnetcore_environment             = var.aspnetcore_environment
  scheme_private_access              = var.scheme_private_access
  pgsql_user                         = var.pgsql_user
  pgsql_pass                         = var.pgsql_pass
  pgsql_db_name                      = var.pgsql_db_name
  mongodb_user                       = var.mongodb_user
  mongodb_pass                       = var.mongodb_pass
  rabbitmq_user                      = var.rabbitmq_user
  rabbitmq_pass                      = var.rabbitmq_pass
  depends_on                         = [module.vpc, module.load_balancer]
}