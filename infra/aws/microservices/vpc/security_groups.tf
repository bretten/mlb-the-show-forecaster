# Security group for allowing public access
resource "aws_security_group" "sg_public" {
  name                   = "${var.resource_prefix}-public"
  description            = "public VPC security group"
  vpc_id                 = aws_vpc.main.id
  revoke_rules_on_delete = true

  tags = merge(var.root_tags, {
    Name = "${var.resource_prefix}-public"
  })
}

# Public security group: Allow inbound gateway app to be accessed by load balancer
resource "aws_vpc_security_group_ingress_rule" "sg_public_in_allow_load_balancer" {
  security_group_id            = aws_security_group.sg_public.id
  ip_protocol                  = "tcp"
  from_port                    = var.port_gateway
  to_port                      = var.port_gateway
  referenced_security_group_id = aws_security_group.sg_load_balancer.id
}

# Public security group: Allow outbound https
resource "aws_vpc_security_group_egress_rule" "sg_public_out_allow_https" {
  security_group_id = aws_security_group.sg_public.id
  ip_protocol       = "tcp"
  from_port         = 443
  to_port           = 443
  cidr_ipv4         = var.cidr_ipv4_anywhere
}

# Security group for establishing rules for access within the isolated network
resource "aws_security_group" "sg_private" {
  name                   = "${var.resource_prefix}-private"
  description            = "private VPC security group"
  vpc_id                 = aws_vpc.main.id
  revoke_rules_on_delete = false

  tags = merge(var.root_tags, {
    Name = "${var.resource_prefix}-private"
  })
}

# Security group for private subnet: Allow gateway app to access domain services
resource "aws_vpc_security_group_ingress_rule" "sg_private_in_allow_domain_services" {
  for_each = toset([
    tostring(var.port_player_tracker), tostring(var.port_performance_tracker), tostring(var.port_marketplace_watcher)
  ])
  security_group_id            = aws_security_group.sg_private.id
  ip_protocol                  = "tcp"
  from_port                    = each.value
  to_port                      = each.value
  referenced_security_group_id = aws_security_group.sg_private_access.id
}

# Security group for private subnet: Allow domain services to access each other within the private security group
resource "aws_vpc_security_group_ingress_rule" "sg_private_in_allow_private_access_domain_services" {
  for_each = toset([
    tostring(var.port_player_tracker), tostring(var.port_performance_tracker), tostring(var.port_marketplace_watcher)
  ])
  security_group_id            = aws_security_group.sg_private.id
  ip_protocol                  = "tcp"
  from_port                    = each.value
  to_port                      = each.value
  referenced_security_group_id = aws_security_group.sg_private.id
}

# Security group for private subnet: Allow communication on storage related ports within the private security group
resource "aws_vpc_security_group_ingress_rule" "sg_private_in_allow_storage" {
  for_each                     = toset(["5432", "27017", "5672", "15672", "2049"]) # PostgreSQL, MongoDB, RabbitMQ, RabbitMQ Management, NFS
  security_group_id            = aws_security_group.sg_private.id
  ip_protocol                  = "tcp"
  from_port                    = each.value
  to_port                      = each.value
  referenced_security_group_id = aws_security_group.sg_private.id
}

# Security group for private subnet: Allow domain services to access each other within the private security group
resource "aws_vpc_security_group_egress_rule" "sg_private_out_allow_domain_services" {
  for_each = toset([
    tostring(var.port_player_tracker), tostring(var.port_performance_tracker), tostring(var.port_marketplace_watcher)
  ])
  security_group_id            = aws_security_group.sg_private.id
  ip_protocol                  = "tcp"
  from_port                    = each.value
  to_port                      = each.value
  referenced_security_group_id = aws_security_group.sg_private.id
}

# Security group for private subnet: Allow private subnet to access the internet (https)
resource "aws_vpc_security_group_egress_rule" "sg_private_out_allow_https" {
  security_group_id = aws_security_group.sg_private.id
  ip_protocol       = "tcp"
  from_port         = 443
  to_port           = 443
  cidr_ipv4         = var.cidr_ipv4_anywhere
}

# Security group for private subnet: Allow private subnet to access the internet (http)
resource "aws_vpc_security_group_egress_rule" "sg_private_out_allow_http" {
  security_group_id = aws_security_group.sg_private.id
  ip_protocol       = "tcp"
  from_port         = 80
  to_port           = 80
  cidr_ipv4         = var.cidr_ipv4_anywhere
}

# Security group for private subnet: Allow communication on storage related ports within the private security group
resource "aws_vpc_security_group_egress_rule" "sg_private_out_allow_storage" {
  for_each                     = toset(["5432", "27017", "5672", "15672", "2049"]) # PostgreSQL, MongoDB, RabbitMQ, RabbitMQ Management, NFS
  security_group_id            = aws_security_group.sg_private.id
  ip_protocol                  = "tcp"
  from_port                    = each.value
  to_port                      = each.value
  referenced_security_group_id = aws_security_group.sg_private.id
}

# Security group that allows gateway to access the private security group
resource "aws_security_group" "sg_private_access" {
  name                   = "${var.resource_prefix}-private-access"
  description            = "VPC security group that can access the private security group"
  vpc_id                 = aws_vpc.main.id
  revoke_rules_on_delete = false

  tags = merge(var.root_tags, {
    Name = "${var.resource_prefix}-private-access"
  })
}

# PrivateAccess security group: Allow PrivateAccess security group to resolve DNS when communicating with the private security group (for ECS service discovery)
resource "aws_vpc_security_group_egress_rule" "sg_private_access_out_allow_private_dns" {
  security_group_id = aws_security_group.sg_private_access.id
  ip_protocol       = "tcp"
  from_port         = 53
  to_port           = 53
  cidr_ipv4         = var.cidr_subnet_private
}

# PrivateAccess security group: Allow outbound traffic to the domain services
resource "aws_vpc_security_group_egress_rule" "sg_private_access_out_allow_domain_services" {
  for_each = toset([
    tostring(var.port_player_tracker), tostring(var.port_performance_tracker), tostring(var.port_marketplace_watcher)
  ])
  security_group_id            = aws_security_group.sg_private_access.id
  ip_protocol                  = "tcp"
  from_port                    = each.value
  to_port                      = each.value
  referenced_security_group_id = aws_security_group.sg_private.id
}

# Security group for load balancer to allow public access and direct traffic to the gateway app
resource "aws_security_group" "sg_load_balancer" {
  name                   = "${var.resource_prefix}-lb"
  description            = "SG for the load balancer"
  vpc_id                 = aws_vpc.main.id
  revoke_rules_on_delete = false

  tags = merge(var.root_tags, {
    Name = "${var.resource_prefix}-lb"
  })
}

# Security group for load balancer: Allow inbound https to the load balancer
resource "aws_vpc_security_group_ingress_rule" "sg_load_balancer_in_allow_https" {
  security_group_id = aws_security_group.sg_load_balancer.id
  ip_protocol       = "tcp"
  from_port         = 443
  to_port           = 443
  cidr_ipv4         = var.cidr_ipv4_anywhere
}

# Security group for load balancer: Allow inbound http to the load balancer
resource "aws_vpc_security_group_ingress_rule" "sg_load_balancer_in_allow_http" {
  security_group_id = aws_security_group.sg_load_balancer.id
  ip_protocol       = "tcp"
  from_port         = 80
  to_port           = 80
  cidr_ipv4         = var.cidr_ipv4_anywhere
}

# Security group for load balancer: Allow outbound gateway app
resource "aws_vpc_security_group_egress_rule" "sg_load_balancer_out_allow_gateway_app" {
  security_group_id            = aws_security_group.sg_load_balancer.id
  ip_protocol                  = "tcp"
  from_port                    = var.port_gateway
  to_port                      = var.port_gateway
  referenced_security_group_id = aws_security_group.sg_public.id
}