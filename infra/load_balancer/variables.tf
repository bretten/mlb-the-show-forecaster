variable "resource_prefix" {
  description = "Name to prefix to resources with names"
  type        = string
}

variable "root_tags" {
  description = "Common tags"
  type        = map(string)
}

variable "security_group_id" {
  description = "Load balancer security group"
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

variable "vpc_id" {
  description = "The associated VPC"
  type        = string
}

variable "certificate_arn" {
  description = "ARN of the SSL certificate for the load balancer HTTPS listener"
  type        = string
}

variable "port_gateway" {
  description = "The port for the Gateway"
  type        = number
}