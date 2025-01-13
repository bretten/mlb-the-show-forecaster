variable "aws_region" {
  type        = string
  default     = "us-west-2"
  description = "The AWS region"
}

variable "app_version" {
  type        = string
  default     = ""
  description = "Version that will be deployed"
}

variable "my_ip" {
  type        = string
  default     = ""
  description = "My IP"
}

variable "email" {
  type        = string
  default     = ""
  description = "Email"
}

variable "domain_name" {
  type        = string
  default     = ""
  description = "Domain name"
}

variable "root_tags" {
  description = "Common tags"
  type        = map(string)
  default = {
    Project = "mlb-the-show-forecaster"
  }
}

variable "jwt_authority" {
  type        = string
  sensitive   = true
  description = "JWT Authority"
}

variable "jwt_audience" {
  type        = string
  sensitive   = true
  description = "JWT Audience"
}
