# IAM Role for backing up storage data
resource "aws_iam_role" "role_backup" {
  name = "mlbForecasterBackupRole"
  assume_role_policy = jsonencode(
    {
      Statement = [
        {
          Action = "sts:AssumeRole"
          Effect = "Allow"
          Principal = {
            Service = "backup.amazonaws.com"
          }
          Sid = ""
        },
        {
          Action = "sts:AssumeRole"
          Effect = "Allow"
          Principal = {
            Service = "lambda.amazonaws.com"
          }
          Sid = ""
        },
      ]
      Version = "2008-10-17"
    }
  )

  tags = var.root_tags
}

# Policy for backing up storage data from EFS to S3
resource "aws_iam_role_policy" "policy_backup_storage" {
  name = "mlbForecasterBackupStorage"
  role = aws_iam_role.role_backup.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "s3:*"
        Effect = "Allow"
        Resource = [
          var.main_bucket.arn,
          "${var.main_bucket.arn}/*"
        ]
      },
      {
        # Allows the backup Lambda to create an network interface
        Action = [
          "ec2:CreateNetworkInterface",
          "ec2:DescribeNetworkInterfaces",
          "ec2:DeleteNetworkInterface"
        ]
        Effect   = "Allow"
        Resource = "*"
      },
      {
        Action = [
          "ecs:ExecuteCommand",
          "ecs:DescribeTasks",
          "ecs:ListTasks"
        ]
        Effect = "Allow"
        Resource = [
          var.main_cluster.arn,        # Cluster
          "${var.main_cluster.arn}/*", # Anything in the cluster
          "arn:aws:ecs:${var.aws_region}:${var.account_id}:container-instance/${var.resource_prefix}/*",
          "arn:aws:ecs:${var.aws_region}:${var.account_id}:task/${var.resource_prefix}/*"
        ]
      },
      {
        Action = [
          "logs:CreateLogStream",
          "logs:CreateLogGroup",
          "logs:PutLogEvents"
        ]
        Effect   = "Allow"
        Resource = "*"
      }
    ]
  })
}

# Managed policy for backups
resource "aws_iam_role_policy_attachment" "attachment_backup" {
  role       = aws_iam_role.role_backup.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSBackupServiceRolePolicyForBackup"
}