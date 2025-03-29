# S3 Bucket for storing app-related files
resource "aws_s3_bucket" "main_bucket" {
  bucket = var.resource_prefix

  tags = var.root_tags
}