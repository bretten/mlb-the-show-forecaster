# Elastic IP that the NAT gateway uses when directing traffic for the private subnet
resource "aws_eip" "elastic_ip_nat_gateway" {
  count                = var.use_nat_gateway == true ? 1 : 0
  domain               = "vpc"
  network_border_group = var.aws_region
  public_ipv4_pool     = "amazon"
  tags                 = var.root_tags

  depends_on = [aws_internet_gateway.internet_gateway]
}