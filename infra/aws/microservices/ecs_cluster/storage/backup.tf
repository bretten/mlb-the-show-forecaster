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
  iam_role_arn = var.task_execution_role_arn
  name         = "${var.resource_prefix}-backup-selection-efs"
  plan_id      = aws_backup_plan.backup_plan_storage_efs.id

  resources = [
    aws_efs_file_system.efs_storage.arn
  ]
}