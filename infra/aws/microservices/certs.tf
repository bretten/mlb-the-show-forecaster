# Cert for domain name
resource "aws_acm_certificate" "cert" {
  domain_name               = "${var.resource_prefix}.brettnamba.com"
  validation_method         = "DNS"
  certificate_authority_arn = null
  early_renewal_duration    = null
  key_algorithm             = "RSA_2048"
  subject_alternative_names = [
    "${var.resource_prefix}.brettnamba.com",
  ]
  tags = {}

  options {
    certificate_transparency_logging_preference = "ENABLED"
  }
  lifecycle {
    create_before_destroy = true
  }
}