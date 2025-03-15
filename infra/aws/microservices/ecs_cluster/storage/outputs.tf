output "service_discovery_name_pgsql" {
  description = "Service discovery name for pgsql"
  value       = aws_service_discovery_service.discovery_service_postgresql.name
}

output "service_discovery_name_mongodb" {
  description = "Service discovery name for mongodb"
  value       = aws_service_discovery_service.discovery_service_mongodb.name
}

output "service_discovery_name_rabbitmq" {
  description = "Service discovery name for rabbitmq"
  value       = aws_service_discovery_service.discovery_service_rabbitmq.name
}

output "service_discovery_name_redis" {
  description = "Service discovery name for redis"
  value       = aws_service_discovery_service.discovery_service_redis.name
}