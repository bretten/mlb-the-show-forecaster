output "private_dns_namespace_id" {
  description = "ID of the private DNS namespace"
  value       = aws_service_discovery_private_dns_namespace.private_dns_namespace.id
}

output "main_cluster_id" {
  description = "Main cluster ID"
  value       = aws_ecs_cluster.main.id
}