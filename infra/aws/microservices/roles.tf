
# IAM Role that ECS Tasks assume
resource "aws_iam_role" "role_ecs_task_role" {
  name = "mlbEcsTaskRole"
  assume_role_policy = jsonencode(
    {
      Statement = [
        {
          Action = "sts:AssumeRole"
          Effect = "Allow"
          Principal = {
            Service = "ecs-tasks.amazonaws.com"
          }
          Sid = ""
        }
      ]
      Version = "2008-10-17"
    }
  )

  tags = var.root_tags
}

# IAM Role for ECS Task Execution
resource "aws_iam_role" "role_ecs_task_execution" {
  name = "mlbEcsTaskExecutionRole"
  assume_role_policy = jsonencode(
    {
      Statement = [
        {
          Action = "sts:AssumeRole"
          Effect = "Allow"
          Principal = {
            Service = "ecs-tasks.amazonaws.com"
          }
          Sid = ""
        }
      ]
      Version = "2008-10-17"
    }
  )

  tags = var.root_tags
}

# Policy for creating log groups
resource "aws_iam_role_policy" "policy_create_log_group" {
  name = "mlbEcsCreateLogGroup"
  role = aws_iam_role.role_ecs_task_execution.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action   = ["logs:CreateLogGroup"]
        Effect   = "Allow"
        Resource = "*"
      },
    ]
  })
}

# Policy for registering the load balancer
resource "aws_iam_role_policy" "policy_register_load_balancer" {
  name = "mlbEcsRegisterLoadBalancer"
  role = aws_iam_role.role_ecs_task_execution.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = [
          "elasticloadbalancing:RegisterTargets",
          "elasticloadbalancing:DeregisterTargets",
          "elasticloadbalancing:DescribeTargetGroups",
          "elasticloadbalancing:DescribeTargetHealth"
        ]
        Effect   = "Allow"
        Resource = "*"
      },
    ]
  })
}

# Policy for allowing SSM agent on ECS tasks
resource "aws_iam_role_policy" "policy_systems_manager" {
  name = "mlbForecasterSsmAgent"
  role = aws_iam_role.role_ecs_task_role.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = [
          "ssmmessages:CreateControlChannel",
          "ssmmessages:CreateDataChannel",
          "ssmmessages:OpenControlChannel",
          "ssmmessages:OpenDataChannel"
        ]
        Effect   = "Allow"
        Resource = "*"
      },
    ]
  })
}

# Policy for accessing S3
resource "aws_iam_role_policy" "s3" {
  name = "mlbForecasterS3"
  role = aws_iam_role.role_ecs_task_role.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "s3:*"
        Effect = "Allow"
        Resource = [
          aws_s3_bucket.main_bucket.arn,
          "${aws_s3_bucket.main_bucket.arn}/*"
        ]
      }
    ]
  })
}

# Managed policy for executing ECS tasks
resource "aws_iam_role_policy_attachment" "attachment_ecs_task_execution" {
  role       = aws_iam_role.role_ecs_task_execution.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}