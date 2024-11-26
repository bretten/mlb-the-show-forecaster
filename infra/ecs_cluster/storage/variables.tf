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

variable "task_execution_role_arn" {
  description = "Execution role"
  type        = string
}

variable "private_dns_namespace_id" {
  description = "ID for the private DNS namespace"
  type        = string
}

variable "main_cluster_id" {
  description = "Main cluster ID"
  type        = string
}

variable "security_group_id_private" {
  description = "Security Group ID for private instances"
  type        = string
}

variable "subnet_id_private" {
  description = "Subnet IDs for private"
  type        = string
}

# These are used for task definition ENV variables
variable "aspnetcore_environment" {
  description = "The ASP.NET Core environment"
  type        = string
}

variable "capacity_provider" {
  description = "ECS capacity provider"
  type        = string
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