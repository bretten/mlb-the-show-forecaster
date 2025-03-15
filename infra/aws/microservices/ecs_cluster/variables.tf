variable "aws_region" {
  description = "The AWS region"
  type        = string
}

variable "resource_prefix" {
  description = "Name to prefix to resources with names"
  type        = string
}

variable "root_tags" {
  description = "Common tags"
  type        = map(string)
}

variable "task_def_gateway_image" {
  description = "The container image for the gateway app"
  type        = string
}

variable "task_def_player_tracker_image" {
  description = "The container image for player tracker"
  type        = string
}

variable "task_def_performance_tracker_image" {
  description = "The container image for performance tracker"
  type        = string
}

variable "task_def_marketplace_watcher_image" {
  description = "The container image for marketplace watcher"
  type        = string
}

variable "task_execution_role_arn" {
  description = "Execution role"
  type        = string
}

variable "load_balancer_target_group_arn" {
  description = "The ARN of the load balancer's target group for the gateway"
  type        = string
}

variable "vpc_id" {
  description = "The associated VPC"
  type        = string
}

variable "security_group_id_public" {
  description = "Security Group ID for public instances"
  type        = string
}

variable "security_group_id_private" {
  description = "Security Group ID for private instances"
  type        = string
}

variable "security_group_id_private_access" {
  description = "Security Group ID for accessing the private security group"
  type        = string
}

variable "subnet_id_public1" {
  description = "Subnet IDs for public1"
  type        = string
}

variable "subnet_id_public2" {
  description = "Subnet IDs for public2"
  type        = string
}

variable "subnet_id_private" {
  description = "Subnet IDs for private"
  type        = string
}

variable "use_storage" {
  description = "True to use the storage module, otherwise false"
  type        = bool
}

variable "use_nat_gateway" {
  description = "True to use a NAT gateway, otherwise false"
  type        = bool
}

variable "capacity_provider" {
  description = "ECS capacity provider"
  type        = string
}

variable "health_alerts_arn" {
  description = "ARN for health alerts SNS topic"
  type        = string
}

variable "backup_vault_name" {
  description = "Backup vault"
  type        = string
}

# Domain service ports
variable "port_gateway" {
  description = "The port for the Gateway"
  type        = number
}

variable "port_player_tracker" {
  description = "The port for the Player Tracker"
  type        = number
}

variable "port_performance_tracker" {
  description = "The port for the Performance Tracker"
  type        = number
}

variable "port_marketplace_watcher" {
  description = "The port for the Marketplace Watcher"
  type        = number
}

# These are used for task definition ENV variables
variable "aspnetcore_environment" {
  description = "The ASP.NET Core environment"
  type        = string
}

variable "scheme_private_access" {
  description = "The scheme that should be used when communicating to the private domain services"
  type        = string
}

variable "jwt_authority" {
  description = "JWT Authority"
  type        = string
  sensitive   = true
}

variable "jwt_audience" {
  description = "JWT Audience"
  type        = string
  sensitive   = true
}

variable "pgsql_user" {
  description = "PGSQL user"
  type        = string
  sensitive   = true
}

variable "pgsql_pass" {
  description = "PGSQL password"
  type        = string
  sensitive   = true
}

variable "pgsql_db_name" {
  description = "PGSQL dbname"
  type        = string
  sensitive   = true
}

variable "mongodb_user" {
  description = "mongodb user"
  type        = string
  sensitive   = true
}

variable "mongodb_pass" {
  description = "mongodb password"
  type        = string
  sensitive   = true
}

variable "rabbitmq_user" {
  description = "rabbitmq user"
  type        = string
  sensitive   = true
}

variable "rabbitmq_pass" {
  description = "rabbitmq password"
  type        = string
  sensitive   = true
}

variable "redis_pass" {
  description = "redis password"
  type        = string
  sensitive   = true
}