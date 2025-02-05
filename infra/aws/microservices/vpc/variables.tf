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

variable "cidr_vpc" {
  description = "CIDR for the VPC"
  type        = string
}

variable "cidr_subnet_public1" {
  description = "CIDR for VPC's public subnet"
  type        = string
}

variable "cidr_subnet_public2" {
  description = "CIDR for VPC's public subnet #2"
  type        = string
}

variable "cidr_subnet_private" {
  description = "CIDR for VPC's private subnet"
  type        = string
}

variable "cidr_ipv4_anywhere" {
  description = "CIDR for any destination"
  type        = string
}

variable "availability_zone_subnet_public1" {
  description = "Availability zone for public subnet"
  type        = string
}

variable "availability_zone_subnet_public2" {
  description = "Availability zone for public subnet #2"
  type        = string
}

variable "availability_zone_subnet_private" {
  description = "Availability zone for private subnet"
  type        = string
}

variable "use_nat_gateway" {
  description = "True to use a NAT gateway, otherwise false"
  type        = bool
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