# Public subnet for instances exposed to the internet (gateway app)
resource "aws_subnet" "public1" {
  vpc_id                                         = aws_vpc.main.id
  cidr_block                                     = var.cidr_subnet_public1
  assign_ipv6_address_on_creation                = false
  availability_zone                              = var.availability_zone_subnet_public1
  customer_owned_ipv4_pool                       = null
  enable_dns64                                   = false
  enable_resource_name_dns_a_record_on_launch    = false
  enable_resource_name_dns_aaaa_record_on_launch = false
  ipv6_cidr_block                                = null
  ipv6_cidr_block_association_id                 = null
  ipv6_native                                    = false
  map_public_ip_on_launch                        = false
  outpost_arn                                    = null
  private_dns_hostname_type_on_launch            = "ip-name"
  tags = merge(var.root_tags, {
    "Name" = "${var.resource_prefix}-public-subnet"
  })
}

# Additional public subnet for the load balancer since it needs two availability zones
resource "aws_subnet" "public2" {
  vpc_id     = aws_vpc.main.id
  cidr_block = var.cidr_subnet_public2

  assign_ipv6_address_on_creation                = false
  availability_zone                              = var.availability_zone_subnet_public2
  customer_owned_ipv4_pool                       = null
  enable_dns64                                   = false
  enable_resource_name_dns_a_record_on_launch    = false
  enable_resource_name_dns_aaaa_record_on_launch = false
  ipv6_cidr_block                                = null
  ipv6_cidr_block_association_id                 = null
  ipv6_native                                    = false
  map_public_ip_on_launch                        = false
  outpost_arn                                    = null
  private_dns_hostname_type_on_launch            = "ip-name"
  tags = merge(var.root_tags, {
    "Name" = "${var.resource_prefix}-public subnet 2"
  })
}

# Private subnet for instances that don't need to be exposed to the internet
resource "aws_subnet" "private" {
  vpc_id     = aws_vpc.main.id
  cidr_block = var.cidr_subnet_private

  assign_ipv6_address_on_creation                = false
  availability_zone                              = var.availability_zone_subnet_private
  customer_owned_ipv4_pool                       = null
  enable_dns64                                   = false
  enable_resource_name_dns_a_record_on_launch    = false
  enable_resource_name_dns_aaaa_record_on_launch = false
  ipv6_cidr_block                                = null
  ipv6_cidr_block_association_id                 = null
  ipv6_native                                    = false
  map_public_ip_on_launch                        = false
  outpost_arn                                    = null
  private_dns_hostname_type_on_launch            = "ip-name"
  tags = merge(var.root_tags, {
    "Name" = "${var.resource_prefix}-private-subnet"
  })
}