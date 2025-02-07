# Load balancer that handles traffic to the Gateway App
resource "aws_lb" "load_balancer_gateway" {
  name               = "${var.resource_prefix}-lb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [var.security_group_id]

  subnet_mapping {
    allocation_id        = null
    ipv6_address         = null
    outpost_id           = null
    private_ipv4_address = null
    subnet_id            = var.subnet_id_public1
  }
  subnet_mapping {
    allocation_id        = null
    ipv6_address         = null
    outpost_id           = null
    private_ipv4_address = null
    subnet_id            = var.subnet_id_public2
  }

  client_keep_alive                                            = 3600
  customer_owned_ipv4_pool                                     = null
  desync_mitigation_mode                                       = "defensive"
  drop_invalid_header_fields                                   = false
  enable_cross_zone_load_balancing                             = true
  enable_deletion_protection                                   = false
  enable_http2                                                 = true
  enable_tls_version_and_cipher_suite_headers                  = false
  enable_waf_fail_open                                         = false
  enable_xff_client_port                                       = false
  enforce_security_group_inbound_rules_on_private_link_traffic = null
  idle_timeout                                                 = 60
  ip_address_type                                              = "ipv4"
  name_prefix                                                  = null
  preserve_host_header                                         = false
  xff_header_processing_mode                                   = "append"

  tags = var.root_tags
}

# Target group that represents the Gateway App
resource "aws_lb_target_group" "target_group_gateway" {
  name             = "${var.resource_prefix}-target"
  port             = var.port_gateway
  protocol         = "HTTP"
  protocol_version = "HTTP1"
  vpc_id           = var.vpc_id
  target_type      = "ip"

  lambda_multi_value_headers_enabled = false
  proxy_protocol_v2                  = false
  deregistration_delay               = "300"
  ip_address_type                    = "ipv4"
  load_balancing_algorithm_type      = "round_robin"
  load_balancing_anomaly_mitigation  = "off"
  load_balancing_cross_zone_enabled  = "use_load_balancer_configuration"
  name_prefix                        = null
  slow_start                         = 0

  health_check {
    enabled             = true
    healthy_threshold   = 5
    interval            = 30
    matcher             = "401" # This checks both if the endpoint is available and authentication is enabled
    path                = "/healthz"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = 5
    unhealthy_threshold = 2
  }

  stickiness {
    cookie_duration = 86400
    cookie_name     = null
    enabled         = false
    type            = "lb_cookie"
  }

  tags = var.root_tags

  depends_on = [aws_lb.load_balancer_gateway]
}

# Load balancer listener for HTTPS traffic
resource "aws_lb_listener" "https" {
  load_balancer_arn = aws_lb.load_balancer_gateway.arn
  port              = "443"
  protocol          = "HTTPS"
  ssl_policy        = "ELBSecurityPolicy-TLS13-1-2-2021-06"
  certificate_arn   = var.certificate_arn

  default_action {
    order            = 1
    target_group_arn = aws_lb_target_group.target_group_gateway.arn
    type             = "forward"

    forward {
      stickiness {
        duration = 3600
        enabled  = false
      }
      target_group {
        arn    = aws_lb_target_group.target_group_gateway.arn
        weight = 1
      }
    }
  }

  lifecycle {
    create_before_destroy = true
  }

  tags = var.root_tags
}

# Load balancer listener for HTTP traffic
resource "aws_lb_listener" "http" {
  load_balancer_arn = aws_lb.load_balancer_gateway.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    order            = 1
    target_group_arn = aws_lb_target_group.target_group_gateway.arn
    type             = "forward"

    forward {
      stickiness {
        duration = 3600
        enabled  = false
      }
      target_group {
        arn    = aws_lb_target_group.target_group_gateway.arn
        weight = 1
      }
    }
  }

  lifecycle {
    create_before_destroy = true
  }

  tags = var.root_tags
}
