# Enables communication between the internet and the public subnets
resource "aws_internet_gateway" "internet_gateway" {
  vpc_id = aws_vpc.main.id

  tags = merge(var.root_tags, {
    Name = "${var.resource_prefix}-igw"
  })
}