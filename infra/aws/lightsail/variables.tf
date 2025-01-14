variable "root_tags" {
  description = "Common tags"
  type        = map(string)
  default = {
    Project = "mlb-the-show-forecaster"
  }
}

variable "container_registry_url" {
  description = "Container registry containing the application images"
  type        = string
  default     = "ghcr.io/bretten/bretten/mlb-the-show-forecaster"
}

variable "aws_region" {
  type        = string
  default     = "us-west-2"
  description = "The AWS region"
}

variable "resource_prefix" {
  description = "Name to prefix to resources"
  type        = string
  default     = "mlb-the-show-forecaster"
}

# Read from TF_VAR_ prefixed environment variables from the system
variable "image_tag" {
  type        = string
  default     = ""
  description = "Version of container image that will be deployed"
}

variable "my_ip" {
  type        = string
  default     = ""
  description = "My IP"
}

variable "email" {
  type        = string
  default     = ""
  description = "Email"
}

variable "domain_name" {
  type        = string
  default     = ""
  description = "Domain name"
}

variable "aspnetcore_environment" {
  description = "The ASP.NET Core environment"
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