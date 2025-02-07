# Handles alerts for application health
resource "aws_sns_topic" "health_alerts" {
  name = "${var.resource_prefix}-health-alerts"

  tags = var.root_tags
}

# Subscription to the alerts for the admin
resource "aws_sns_topic_subscription" "health_alert_admin_subscription" {
  topic_arn = aws_sns_topic.health_alerts.arn
  protocol  = "email"
  endpoint  = var.admin_email
}