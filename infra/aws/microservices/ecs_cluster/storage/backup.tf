# Backup plan
resource "aws_backup_plan" "backup_plan_storage_efs" {
  name = "${var.resource_prefix}-efs-backup-plan"

  rule {
    rule_name         = "${var.resource_prefix}-efs-midnight-rule"
    target_vault_name = var.backup_vault_name
    schedule          = "cron(0 0 * * ? *)" # Everyday at midnight

    lifecycle {
      delete_after = 7
    }
  }
}

# Associates the EFS storage with the backup vault
resource "aws_backup_selection" "backup_selection_efs" {
  iam_role_arn = aws_iam_role.role_backup.arn
  name         = "${var.resource_prefix}-backup-selection-efs"
  plan_id      = aws_backup_plan.backup_plan_storage_efs.id

  resources = [
    aws_efs_file_system.efs_storage.arn
  ]
}

# Event rule for everyday at midnight PST
resource "aws_cloudwatch_event_rule" "event_rule_midnight_pst" {
  name        = "${var.resource_prefix}-midnight-pst"
  description = "At midnight PST"

  schedule_expression = "cron(0 8,12 * * ? *)"

  tags = var.root_tags
}