# Allows instances in the private subnet to access the internet without exposing them to inbound connections from the internet
resource "aws_nat_gateway" "nat_gateway" {
  allocation_id = aws_eip.elastic_ip_nat_gateway.allocation_id
  subnet_id     = aws_subnet.public1.id

  tags = merge(var.root_tags, {
    Name = "${var.resource_prefix}-nat"
  })

  # Make sure external communication is created first
  depends_on = [aws_internet_gateway.internet_gateway]
}