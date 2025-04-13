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


# Allows S3 interaction
resource "aws_vpc_endpoint" "endpoint_s3" {
  vpc_id          = aws_vpc.main.id
  service_name    = "com.amazonaws.us-west-2.s3"
  ip_address_type = null

  policy = jsonencode(
    {
      Statement = [
        {
          Action    = "*"
          Effect    = "Allow"
          Principal = "*"
          Resource  = "*"
        },
      ]
      Version = "2008-10-17"
    }
  )

  private_dns_enabled = false

  route_table_ids = [
    aws_route_table.route_table_internet_gateway.id
  ]
  security_group_ids = []
  subnet_ids         = []

  vpc_endpoint_type = "Gateway"

  tags = var.root_tags
}

# Allows ECS interaction
resource "aws_vpc_endpoint" "endpoint_ecs" {
  vpc_id          = aws_vpc.main.id
  service_name    = "com.amazonaws.us-west-2.ecs"
  ip_address_type = "ipv4"

  policy = jsonencode(
    {
      Statement = [
        {
          Action    = "*"
          Effect    = "Allow"
          Principal = "*"
          Resource  = "*"
        },
      ]
      Version = "2008-10-17"
    }
  )

  private_dns_enabled = true

  route_table_ids    = []
  security_group_ids = [aws_security_group.sg_private.id]
  subnet_ids         = [aws_subnet.private.id]

  vpc_endpoint_type = "Interface"

  dns_options {
    dns_record_ip_type                             = "ipv4"
    private_dns_only_for_inbound_resolver_endpoint = false
  }

  tags = var.root_tags
}