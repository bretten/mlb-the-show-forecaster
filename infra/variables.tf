variable "aws_region" {
  description = "The AWS region"
  type        = string
  default     = "us-west-2"
}

variable "resource_prefix" {
  description = "Name to prefix to resources"
  type        = string
  default     = "mlb-the-show-forecaster"
}

variable "root_tags" {
  description = "Common tags"
  type        = map(string)
  default = {
    Project = "mlb-the-show-forecaster"
  }
}

variable "cidr_vpc" {
  description = "CIDR for the VPC"
  type        = string
  default     = "11.0.0.0/16"
}

variable "cidr_subnet_public1" {
  description = "CIDR for VPC's public subnet"
  type        = string
  default     = "11.0.1.0/24"
}

variable "cidr_subnet_public2" {
  description = "CIDR for VPC's public subnet #2"
  type        = string
  default     = "11.0.2.0/24"
}

variable "cidr_subnet_private" {
  description = "CIDR for VPC's private subnet"
  type        = string
  default     = "11.0.0.0/24"
}

variable "cidr_ipv4_anywhere" {
  description = "CIDR for any destination"
  type        = string
  default     = "0.0.0.0/0"
}

variable "availability_zone_subnet_public1" {
  description = "Availability zone for public subnet"
  type        = string
  default     = "us-west-2a"
}

variable "availability_zone_subnet_public2" {
  description = "Availability zone for public subnet #2"
  type        = string
  default     = "us-west-2b"
}

variable "availability_zone_subnet_private" {
  description = "Availability zone for private subnet"
  type        = string
  default     = "us-west-2a"
}

# Domain service ports
variable "port_gateway" {
  description = "The port for the Gateway"
  type        = number
  default     = 5000
}

variable "port_player_tracker" {
  description = "The port for the Player Tracker"
  type        = number
  default     = 5001
}

variable "port_performance_tracker" {
  description = "The port for the Performance Tracker"
  type        = number
  default     = 5002
}

variable "port_marketplace_watcher" {
  description = "The port for the Marketplace Watcher"
  type        = number
  default     = 5003
}

# Read from TF_VAR_ prefixed environment variables from the system
variable "image_tag" {
  description = "The image tag that will be used for the task definitions"
  type        = string
  default     = "latest"
}

variable "use_storage" {
  description = "True to use the storage module, otherwise false"
  type        = bool
}

variable "aspnetcore_environment" {
  description = "The ASP.NET Core environment"
  type        = string
}

variable "scheme_private_access" {
  description = "The scheme that should be used when communicating to the private domain services"
  type        = string
  default     = "http"
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

# Config
variable "container_registry_url" {
  description = "Container registry containing the application images"
  type        = string
  default     = "ghcr.io/bretten/bretten/mlb-the-show-forecaster"
}