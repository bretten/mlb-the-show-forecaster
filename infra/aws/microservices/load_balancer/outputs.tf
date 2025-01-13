output "load_balancer_target_group_arn" {
  description = "The ARN of the load balancer's target group for the gateway"
  value       = aws_lb_target_group.target_group_gateway.arn
}