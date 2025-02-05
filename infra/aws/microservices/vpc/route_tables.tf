# Routes traffic from the public subnets to the internet gateway
resource "aws_route_table" "route_table_internet_gateway" {
  vpc_id = aws_vpc.main.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.internet_gateway.id
  }

  tags = merge(var.root_tags, {
    "Name" = "${var.resource_prefix}-rt-igw"
  })
}

# Routes traffic from the private subnet to the NAT gateway
resource "aws_route_table" "route_table_nat_gateway" {
  count  = var.use_nat_gateway == true ? 1 : 0
  vpc_id = aws_vpc.main.id

  route {
    cidr_block     = "0.0.0.0/0"
    nat_gateway_id = aws_nat_gateway.nat_gateway[count.index].id
  }

  tags = merge(var.root_tags, {
    "Name" = "${var.resource_prefix}-rt-nat"
  })
}

# Public subnet association with the internet gateway route table
resource "aws_route_table_association" "route_table_igw_public1" {
  subnet_id      = aws_subnet.public1.id
  route_table_id = aws_route_table.route_table_internet_gateway.id
}

# Public subnet association with the internet gateway route table
resource "aws_route_table_association" "route_table_igw_public2" {
  subnet_id      = aws_subnet.public2.id
  route_table_id = aws_route_table.route_table_internet_gateway.id
}

# Private subnet association with the NAT gateway route table
resource "aws_route_table_association" "route_table_nat_private" {
  count          = var.use_nat_gateway == true ? 1 : 0
  subnet_id      = aws_subnet.private.id
  route_table_id = aws_route_table.route_table_nat_gateway[count.index].id
}

# If no NAT gateway is being used, the private subnet can use the internet gateway
resource "aws_route_table_association" "route_table_igw_private" {
  count          = var.use_nat_gateway == false ? 1 : 0
  subnet_id      = aws_subnet.private.id
  route_table_id = aws_route_table.route_table_internet_gateway.id
}