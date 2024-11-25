# Isolated network that hides domain apps from the internet, but exposes the gateway app so that it can direct traffic to the domain apps
resource "aws_vpc" "main" {
  cidr_block                           = var.cidr_vpc
  assign_generated_ipv6_cidr_block     = false
  enable_dns_hostnames                 = true
  enable_dns_support                   = true
  enable_network_address_usage_metrics = false
  instance_tenancy                     = "default"
  ipv6_association_id                  = null
  ipv6_cidr_block                      = null
  ipv6_cidr_block_network_border_group = null
  ipv6_ipam_pool_id                    = null
  tags = {
    "Description" = "Isolates services for ${var.resource_prefix}"
    "Name"        = "${var.resource_prefix}-vpc"
  }
}