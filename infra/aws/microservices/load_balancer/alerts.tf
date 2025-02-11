# Alert for the load balancer connectivity to the gateway app
resource "aws_cloudwatch_metric_alarm" "gateway_unavailable_alert" {
  alarm_name                = "Gateway Health Check"
  comparison_operator       = "GreaterThanOrEqualToThreshold"
  evaluation_periods        = 1
  metric_name               = "UnHealthyHostCount"
  namespace                 = "AWS/ApplicationELB"
  period                    = 60
  statistic                 = "Sum"
  threshold                 = 1
  treat_missing_data        = "breaching"
  alarm_description         = "Alerts if the gateway is not available"
  insufficient_data_actions = []
  alarm_actions             = [var.health_alerts_arn]
  ok_actions                = [var.health_alerts_arn]
  dimensions = {
    TargetGroup  = aws_lb_target_group.target_group_gateway.arn_suffix
    LoadBalancer = aws_lb.load_balancer_gateway.arn_suffix
  }

  tags = var.root_tags
}