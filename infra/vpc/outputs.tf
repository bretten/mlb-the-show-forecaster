output "vpc_id" {
  description = "VPC ID"
  value       = aws_vpc.main.id
}

output "security_group_id_load_balancer" {
  description = "Security Group ID for the Load Balancer"
  value       = aws_security_group.sg_load_balancer.id
}

output "security_group_id_public" {
  description = "Security Group ID for public instances"
  value       = aws_security_group.sg_public.id
}

output "security_group_id_private" {
  description = "Security Group ID for private instances"
  value       = aws_security_group.sg_private.id
}

output "security_group_id_private_access" {
  description = "Security Group ID for accessing the private security group"
  value       = aws_security_group.sg_private_access.id
}

output "subnet_id_public1" {
  description = "Subnet IDs for public subnet 1"
  value       = aws_subnet.public1.id
}

output "subnet_id_public2" {
  description = "Subnet IDs for public subnet 2"
  value       = aws_subnet.public2.id
}

output "subnet_id_private" {
  description = "Subnet IDs for the private subnet"
  value       = aws_subnet.private.id
}