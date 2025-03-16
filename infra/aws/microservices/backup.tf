# Vault to store backups
resource "aws_backup_vault" "backup_vault_storage" {
  name = "${var.resource_prefix}-backup-vault"

  tags = var.root_tags
}