# Gateway logs
resource "aws_cloudwatch_log_group" "logs_gateway" {
  name              = "/ecs/${var.resource_prefix}-gateway"
  retention_in_days = 90

  tags = var.root_tags
}

# Player tracker logs
resource "aws_cloudwatch_log_group" "logs_player_tracker" {
  name              = "/ecs/${var.resource_prefix}-player-tracker"
  retention_in_days = 90

  tags = var.root_tags
}

# # Performance tracker logs
resource "aws_cloudwatch_log_group" "logs_performance_tracker" {
  name              = "/ecs/${var.resource_prefix}-performance-tracker"
  retention_in_days = 90

  tags = var.root_tags
}

# Marketplace logs
resource "aws_cloudwatch_log_group" "logs_marketplace" {
  name              = "/ecs/${var.resource_prefix}-marketplace-watcher"
  retention_in_days = 90

  tags = var.root_tags
}

# Tracks health of the player tracker job
resource "aws_cloudwatch_log_metric_filter" "metric_player_tracker_job_health" {
  name           = "Player Tracker Job Health"
  pattern        = "\"Finished job PlayerStatusTrackerJob\""
  log_group_name = aws_cloudwatch_log_group.logs_player_tracker.name

  metric_transformation {
    name          = "Finished PlayerStatusTracker"
    namespace     = "${var.resource_prefix}-metrics"
    value         = "1"
    default_value = "0"
    unit          = "Count"
  }
}

# Tracks health of the performance tracker job
resource "aws_cloudwatch_log_metric_filter" "metric_performance_tracker_job_health" {
  name           = "Performance Tracker Job Health"
  pattern        = "\"Finished job PerformanceTrackerJob\""
  log_group_name = aws_cloudwatch_log_group.logs_performance_tracker.name

  metric_transformation {
    name          = "Finished PerformanceTracker"
    namespace     = "${var.resource_prefix}-metrics"
    value         = "1"
    default_value = "0"
    unit          = "Count"
  }
}

# Tracks health of the card tracker job
resource "aws_cloudwatch_log_metric_filter" "metric_card_tracker_job_health" {
  name           = "Card Tracker Job Health"
  pattern        = "\"Finished job PlayerCardTrackerJob\""
  log_group_name = aws_cloudwatch_log_group.logs_marketplace.name

  metric_transformation {
    name          = "Finished PlayerCardTracker"
    namespace     = "${var.resource_prefix}-metrics"
    value         = "1"
    default_value = "0"
    unit          = "Count"
  }
}

# Tracks health of the card price tracker job
resource "aws_cloudwatch_log_metric_filter" "metric_card_price_tracker_job_health" {
  name           = "Card Price Tracker Job Health"
  pattern        = "\"Finished job CardPriceTrackerJob\""
  log_group_name = aws_cloudwatch_log_group.logs_marketplace.name

  metric_transformation {
    name          = "Finished CardPriceTracker"
    namespace     = "${var.resource_prefix}-metrics"
    value         = "1"
    default_value = "0"
    unit          = "Count"
  }
}

# Tracks health of the roster updater job
resource "aws_cloudwatch_log_metric_filter" "metric_roster_updater_job_health" {
  name           = "Roster Updater Job Health"
  pattern        = "\"Finished job RosterUpdaterJob\""
  log_group_name = aws_cloudwatch_log_group.logs_marketplace.name

  metric_transformation {
    name          = "Finished RosterUpdater"
    namespace     = "${var.resource_prefix}-metrics"
    value         = "1"
    default_value = "0"
    unit          = "Count"
  }
}

# Tracks health of the trend reporter job
resource "aws_cloudwatch_log_metric_filter" "metric_trend_reporter_job_health" {
  name           = "Trend Reporter Job Health"
  pattern        = "\"Finished job TrendReporterJob\""
  log_group_name = aws_cloudwatch_log_group.logs_marketplace.name

  metric_transformation {
    name          = "Finished TrendReporter"
    namespace     = "${var.resource_prefix}-metrics"
    value         = "1"
    default_value = "0"
    unit          = "Count"
  }
}

# Handles alerts for job health
resource "aws_sns_topic" "job_alerts" {
  name = "${var.resource_prefix}-job-alerts"

  tags = var.root_tags
}

# Subscription to the alerts for the admin
resource "aws_sns_topic_subscription" "job_alert_admin_subscription" {
  topic_arn = aws_sns_topic.job_alerts.arn
  protocol  = "email"
  endpoint  = var.admin_email
}

locals {
  alerts = [
    {
      name      = aws_cloudwatch_log_metric_filter.metric_player_tracker_job_health.metric_transformation[0].name,
      namespace = aws_cloudwatch_log_metric_filter.metric_player_tracker_job_health.metric_transformation[0].namespace,
      period    = 3600 + 300
    },
    {
      name      = aws_cloudwatch_log_metric_filter.metric_performance_tracker_job_health.metric_transformation[0].name,
      namespace = aws_cloudwatch_log_metric_filter.metric_performance_tracker_job_health.metric_transformation[0].namespace,
      period    = 14400 + 300
    },
    {
      name      = aws_cloudwatch_log_metric_filter.metric_card_tracker_job_health.metric_transformation[0].name,
      namespace = aws_cloudwatch_log_metric_filter.metric_card_tracker_job_health.metric_transformation[0].namespace,
      period    = 7200 + 300
    },
    {
      name      = aws_cloudwatch_log_metric_filter.metric_card_price_tracker_job_health.metric_transformation[0].name,
      namespace = aws_cloudwatch_log_metric_filter.metric_card_price_tracker_job_health.metric_transformation[0].namespace,
      period    = 3600 + 300
    },
    {
      name      = aws_cloudwatch_log_metric_filter.metric_roster_updater_job_health.metric_transformation[0].name,
      namespace = aws_cloudwatch_log_metric_filter.metric_roster_updater_job_health.metric_transformation[0].namespace,
      period    = 86400 + 3600
    },
    {
      name      = aws_cloudwatch_log_metric_filter.metric_trend_reporter_job_health.metric_transformation[0].name,
      namespace = aws_cloudwatch_log_metric_filter.metric_trend_reporter_job_health.metric_transformation[0].namespace,
      period    = 3600 + 300
    },
  ]
}

# Alerts when jobs have not finished recently
resource "aws_cloudwatch_metric_alarm" "job_failed_alert" {
  for_each = {
    for index, alert in local.alerts :
    alert.name => alert
  }
  alarm_name                = each.value.name
  comparison_operator       = "LessThanThreshold"
  evaluation_periods        = 1
  metric_name               = each.value.name
  namespace                 = each.value.namespace
  period                    = each.value.period
  statistic                 = "Sum"
  threshold                 = 1
  treat_missing_data        = "breaching"
  alarm_description         = "Alerts when no job activity for: ${each.value.name}"
  insufficient_data_actions = []
  alarm_actions             = [aws_sns_topic.job_alerts.arn]

  tags = var.root_tags
}