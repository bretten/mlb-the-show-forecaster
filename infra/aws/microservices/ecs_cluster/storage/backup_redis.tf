# Code for uploading Redis backups
data "archive_file" "file_upload_redis_backup" {
  type        = "zip"
  source_file = "${path.module}/lambda_upload_redis_backup.py"
  output_path = "${path.module}/lambda_upload_redis_backup.zip"
}

# Logs for Redis backup
resource "aws_cloudwatch_log_group" "logs_upload_redis_backup" {
  name              = "/lambda/${var.resource_prefix}-upload-redis-backup"
  retention_in_days = 7

  tags = var.root_tags
}

# Uploads Redis backup
resource "aws_lambda_function" "lambda_upload_redis_backup" {
  filename      = data.archive_file.file_upload_redis_backup.output_path
  function_name = "${var.resource_prefix}-upload-redis-backup"
  role          = aws_iam_role.role_backup.arn
  handler       = "lambda_upload_redis_backup.lambda_handler"
  timeout       = 900

  environment {
    variables = {
      CLUSTER_NAME   = var.main_cluster.name
      SERVICE_NAME   = aws_ecs_service.ecs_service_redis.name
      CONTAINER_NAME = jsondecode(aws_ecs_task_definition.task_definition_redis.container_definitions)[0].name
      S3_BUCKET      = var.main_bucket.bucket
      EFS_PATH       = "/mnt/backup/redis"
      LOCAL_PATH     = "/mnt/redis-backups"
    }
  }

  source_code_hash = data.archive_file.file_upload_redis_backup.output_base64sha256

  runtime = "python3.13"

  logging_config {
    log_format = "Text"
    log_group  = aws_cloudwatch_log_group.logs_upload_redis_backup.name
  }

  vpc_config {
    subnet_ids         = [var.subnet_id_private]
    security_group_ids = [var.security_group_id_private]
  }

  file_system_config {
    arn = aws_efs_access_point.efs_access_storage_redis_backups.arn

    local_mount_path = "/mnt/redis-backups"
  }

  tags = var.root_tags
}

# Allows Cloudwatch event rule to trigger the Redis upload lambda
resource "aws_lambda_permission" "allow_cloudwatch_upload_redis_backup" {
  statement_id  = "${var.resource_prefix}-allow-upload-redis-backup"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.lambda_upload_redis_backup.function_name
  principal     = "events.amazonaws.com"
  source_arn    = aws_cloudwatch_event_rule.event_rule_midnight_pst.arn
}

# Associates Cloudwatch with the Redis upload lambda
resource "aws_cloudwatch_event_target" "lambda_target_upload_redis_backup" {
  rule      = aws_cloudwatch_event_rule.event_rule_midnight_pst.name
  arn       = aws_lambda_function.lambda_upload_redis_backup.arn
  target_id = "${var.resource_prefix}-target-upload-redis-backup"
}