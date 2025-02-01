# ECS cluster that runs the domain service and gateway instances
resource "aws_ecs_cluster" "main" {
  name = var.resource_prefix

  tags = var.root_tags
}

module "storage" {
  # If == 1, then use the module. Otherwise, exclude it
  count = var.use_storage == true ? 1 : 0

  source                    = "./storage"
  aws_region                = var.aws_region
  resource_prefix           = var.resource_prefix
  root_tags                 = var.root_tags
  task_execution_role_arn   = var.task_execution_role_arn
  private_dns_namespace_id  = aws_service_discovery_private_dns_namespace.private_dns_namespace.id
  main_cluster_id           = aws_ecs_cluster.main.id
  security_group_id_private = var.security_group_id_private
  subnet_id_private         = var.subnet_id_private
  aspnetcore_environment    = var.aspnetcore_environment
  capacity_provider         = var.capacity_provider
  use_nat_gateway           = var.use_nat_gateway
  pgsql_user                = var.pgsql_user
  pgsql_pass                = var.pgsql_pass
  pgsql_db_name             = var.pgsql_db_name
  mongodb_user              = var.mongodb_user
  mongodb_pass              = var.mongodb_pass
  rabbitmq_user             = var.rabbitmq_user
  rabbitmq_pass             = var.rabbitmq_pass
}