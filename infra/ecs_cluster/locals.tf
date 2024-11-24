locals {
  # DNS names for the application
  dns_name_player_tracker = "${aws_service_discovery_service.discovery_service_player_tracker.name}.${aws_service_discovery_private_dns_namespace.private_dns_namespace.name}"
  url_player_tracker      = "${local.dns_name_player_tracker}:${var.port_player_tracker}"

  dns_name_performance_tracker = "${aws_service_discovery_service.discovery_service_performance_tracker.name}.${aws_service_discovery_private_dns_namespace.private_dns_namespace.name}"
  url_performance_tracker      = "${local.dns_name_performance_tracker}:${var.port_performance_tracker}"

  dns_name_marketplace_watcher = "${aws_service_discovery_service.discovery_service_marketplace_watcher.name}.${aws_service_discovery_private_dns_namespace.private_dns_namespace.name}"
  url_marketplace_watcher      = "${local.dns_name_marketplace_watcher}:${var.port_marketplace_watcher}"

  # DNS names for storage
  # Since count is used to conditionally include storage module, it is treated as a list by tf
  pgsql_dns_name = (var.use_storage ?
    "${module.storage[0].service_discovery_name_pgsql}.${aws_service_discovery_private_dns_namespace.private_dns_namespace.name}"
    : "")
  mongodb_dns_name = (var.use_storage ?
    "${module.storage[0].service_discovery_name_mongodb}.${aws_service_discovery_private_dns_namespace.private_dns_namespace.name}"
    : "")
  rabbitmq_dns_name = (var.use_storage ?
    "${module.storage[0].service_discovery_name_rabbitmq}.${aws_service_discovery_private_dns_namespace.private_dns_namespace.name}"
    : "")

  pgsql_cs   = "Server=${local.pgsql_dns_name};Username=${var.pgsql_user};Password=${var.pgsql_pass};Port=5432;Database=${var.pgsql_db_name};"
  mongodb_cs = "mongodb://${var.mongodb_user}:${var.mongodb_pass}@${local.mongodb_dns_name}:27017/?authSource=admin"
}