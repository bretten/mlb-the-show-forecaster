terraform {
  required_version = ">= 1.0.0" # Ensure that the Terraform version is 1.0.0 or higher

  required_providers {
    aws = {
      source  = "hashicorp/aws" # Specify the source of the AWS provider
      version = "~> 4.0"        # Use a version of the AWS provider that is compatible with version
    }
  }
}

provider "aws" {
  region = var.aws_region
}

terraform {
  backend "s3" {
    bucket = "tformbackend"
    key    = "mlb-the-show-forecaster-lightsail/terraform.tfstate"
    region = "us-west-2"
  }
}

# Static IP
resource "aws_lightsail_static_ip" "static_ip" {
  name = "${var.resource_prefix}-static-ip"
}

# Static IP attachment
resource "aws_lightsail_static_ip_attachment" "static_ip_attachment" {
  static_ip_name = aws_lightsail_static_ip.static_ip.id
  instance_name  = aws_lightsail_instance.instance.id
}

# Firewall rules
resource "aws_lightsail_instance_public_ports" "public_ports" {
  instance_name = aws_lightsail_instance.instance.name

  # Public
  dynamic "port_info" {
    for_each = [80, 443] // HTTP, HTTPS
    content {
      protocol  = "tcp"
      from_port = port_info.value
      to_port   = port_info.value
    }
  }

  # Private
  dynamic "port_info" {
    for_each = [22, 54320, 27017, 5672, 15672] // SSH, PostgreSQL, MongoDB, RabbitMQ, RabbitMQ Management
    content {
      protocol  = "tcp"
      from_port = port_info.value
      to_port   = port_info.value
      cidrs     = [var.my_ip]
    }
  }
}

resource "aws_lightsail_instance" "instance" {
  name              = var.resource_prefix
  availability_zone = "us-west-2a"
  blueprint_id      = "ubuntu_22_04"
  bundle_id         = "medium_3_0"
  key_pair_name     = "LightsailDefaultKeyPair"
  ip_address_type   = "dualstack"

  add_on {
    type          = "AutoSnapshot"
    snapshot_time = "09:00"
    status        = "Enabled"
  }

  user_data = <<-EOF
    #!/bin/bash
    apt-get update
    apt-get install -y docker.io docker-compose git

    # Start Docker as a service
    systemctl start docker
    systemctl enable docker

    # For non-root access, add user to docker group
    usermod -aG docker ubuntu

    # Create the docker-compose file
    mkdir /home/ubuntu/app
    chown -R ubuntu:ubuntu /home/ubuntu/app
    cat <<EOT > /home/ubuntu/app/docker-compose.yml
    services:
      mlb-the-show-forecaster-gateway:
        image: ${var.container_registry_url}/gateway:${var.image_tag}
        restart: always
        environment:
          - ASPNETCORE_ENVIRONMENT=${var.aspnetcore_environment}
          - Urls=http://*:5000
          - Auth__Jwt__Authority=${var.jwt_authority}
          - Auth__Jwt__Audience=${var.jwt_audience}
          - Auth__AllowCors=false
          - Aws__Region=${var.aws_region}
          - Spa__Active=true
          - SignalRMultiplexer__Interval=00:00:00:05
          - TargetApps__PlayerTracker__Scheme=http
          - TargetApps__PlayerTracker__Host=mlb-the-show-forecaster-player-tracker
          - TargetApps__PerformanceTracker__Scheme=http
          - TargetApps__PerformanceTracker__Host=mlb-the-show-forecaster-performance-tracker
          - TargetApps__MarketplaceWatcher__Scheme=http
          - TargetApps__MarketplaceWatcher__Host=mlb-the-show-forecaster-marketplace-watcher
        ports:
          - "5000:5000"
        networks:
          - public
          - backend
        deploy:
          resources:
            limits:
              memory: 150M
            reservations:
              memory: 150M

      mlb-the-show-forecaster-marketplace-watcher:
        image: ${var.container_registry_url}/marketplace-watcher:${var.image_tag}
        restart: always
        environment:
          - ASPNETCORE_ENVIRONMENT=${var.aspnetcore_environment}
          - RunMigrations=true
          - Urls=http://*:5003
          - Api__Performance__BaseAddress=http://mlb-the-show-forecaster-performance-tracker:5002
          - Forecasting__PlayerMatcher__BaseAddress=http://mlb-the-show-forecaster-player-tracker:5001
          - ConnectionStrings__Cards=Server=postgres-db;Username=${var.pgsql_user};Password=${var.pgsql_pass};Port=5432;Database=${var.pgsql_db_name};
          - ConnectionStrings__Forecasts=Server=postgres-db;Username=${var.pgsql_user};Password=${var.pgsql_pass};Port=5432;Database=${var.pgsql_db_name};
          - ConnectionStrings__Marketplace=Server=postgres-db;Username=${var.pgsql_user};Password=${var.pgsql_pass};Port=5432;Database=${var.pgsql_db_name};
          - ConnectionStrings__TrendsMongoDb=mongodb://${var.mongodb_user}:${var.mongodb_pass}@mongo:27017/?authSource=admin
          - Messaging__RabbitMq__HostName=rabbitmq
          - Messaging__RabbitMq__UserName=${var.rabbitmq_user}
          - Messaging__RabbitMq__Password=${var.rabbitmq_pass}
          - Jobs__RunOnStartup=false
          - CardPriceTracker__UseWebsiteForHistoricalPrices=false
        depends_on:
          postgres-db:
            condition: service_healthy
          rabbitmq:
            condition: service_healthy
          mongo:
            condition: service_healthy
        networks:
          - backend
        deploy:
          resources:
            limits:
              memory: 1500M
            reservations:
              memory: 1000M

      mlb-the-show-forecaster-performance-tracker:
        image: ${var.container_registry_url}/performance-tracker:${var.image_tag}
        restart: always
        environment:
          - ASPNETCORE_ENVIRONMENT=${var.aspnetcore_environment}
          - RunMigrations=true
          - Urls=http://*:5002
          - ConnectionStrings__PlayerSeasons=Server=postgres-db;Username=${var.pgsql_user};Password=${var.pgsql_pass};Port=5432;Database=${var.pgsql_db_name};
          - Messaging__RabbitMq__HostName=rabbitmq
          - Messaging__RabbitMq__UserName=${var.rabbitmq_user}
          - Messaging__RabbitMq__Password=${var.rabbitmq_pass}
          - Jobs__RunOnStartup=false
        depends_on:
          postgres-db:
            condition: service_healthy
          rabbitmq:
            condition: service_healthy
        networks:
          - backend
        deploy:
          resources:
            limits:
              memory: 500M
            reservations:
              memory: 300M

      mlb-the-show-forecaster-player-tracker:
        image: ${var.container_registry_url}/player-tracker:${var.image_tag}
        restart: always
        environment:
          - ASPNETCORE_ENVIRONMENT=${var.aspnetcore_environment}
          - RunMigrations=true
          - Urls=http://*:5001
          - ConnectionStrings__Players=Server=postgres-db;Username=${var.pgsql_user};Password=${var.pgsql_pass};Port=5432;Database=${var.pgsql_db_name};
          - Messaging__RabbitMq__HostName=rabbitmq
          - Messaging__RabbitMq__UserName=${var.rabbitmq_user}
          - Messaging__RabbitMq__Password=${var.rabbitmq_pass}
          - Jobs__RunOnStartup=false
        depends_on:
          postgres-db:
            condition: service_healthy
          rabbitmq:
            condition: service_healthy
        networks:
          - backend
        deploy:
          resources:
            limits:
              memory: 200M
            reservations:
              memory: 100M

      postgres-db:
        image: postgres:16-alpine
        restart: always
        environment:
          - LANG=en_US.utf8
          - POSTGRES_INITDB_ARGS=--locale-provider=icu --icu-locale=en-US
          - POSTGRES_DB=${var.pgsql_db_name}
          - POSTGRES_PASSWORD=${var.pgsql_pass}
        ports:
          - "54320:5432"
        networks:
          - public # So the data can be debugged via a client
          - backend
        healthcheck:
          test: [ "CMD-SHELL", "pg_isready -U postgres -d ${var.pgsql_db_name}" ]
          interval: 10s
          retries: 5
          start_period: 30s
          timeout: 100s # 10s works on local machine but slow lightsail instance needs more
        volumes:
          - postgres-volume:/var/lib/postgresql/data
        deploy:
          resources:
            limits:
              memory: 200M
            reservations:
              memory: 100M

      rabbitmq:
        image: rabbitmq:3-management
        restart: always
        ports:
          - "5672:5672"
          - "15672:15672"
        networks:
          - public # So the data can be debugged via a client
          - backend
        healthcheck:
          test: rabbitmq-diagnostics check_port_connectivity
          interval: 10s
          retries: 5
          start_period: 30s
          timeout: 100s # 10s works on local machine but slow lightsail instance needs more
        volumes:
          - rabbitmq-volume:/var/lib/rabbitmq
        deploy:
          resources:
            limits:
              memory: 200M
            reservations:
              memory: 100M

      mongo:
        image: mongo:noble
        restart: always
        environment:
          MONGO_INITDB_ROOT_USERNAME: ${var.mongodb_user}
          MONGO_INITDB_ROOT_PASSWORD: ${var.mongodb_pass}
        ports:
          - "27017:27017"
        networks:
          - public # So the data can be debugged via a client
          - backend
        healthcheck:
          test: echo 'db.runCommand("ping").ok' | mongosh localhost:27017/test --quiet
          interval: 10s
          retries: 5
          start_period: 30s
          timeout: 100s # 10s works on local machine but slow lightsail instance needs more
        volumes:
          - mongo-volume:/data/db
        deploy:
          resources:
            limits:
              memory: 500M
            reservations:
              memory: 300M

    networks:
      public:
      backend:

    volumes:
      postgres-volume:
      rabbitmq-volume:
      mongo-volume:
    EOT

    # Create the docker-compose service
    cat <<EOS > /etc/systemd/system/docker-compose-app.service
    [Unit]
    Description=Docker Compose Application Service
    Requires=docker.service
    After=docker.service

    [Service]
    Restart=always
    WorkingDirectory=/home/ubuntu/app/
    ExecStart=/usr/bin/docker-compose -f docker-compose.yml up
    ExecStop=/usr/bin/docker-compose -f docker-compose.yml down
    TimeoutStartSec=0
    RestartSec=10

    [Install]
    WantedBy=multi-user.target
    EOS

    # Start the service
    systemctl daemon-reload
    systemctl enable docker-compose-app
    systemctl start docker-compose-app

    # Install nginx and certbot for SSL cert
    apt-get install -y certbot python3-certbot-nginx

    # Setup Nginx
    cat <<EOFNGINX > /etc/nginx/sites-available/${var.domain_name}
    server {
        listen 80;
        server_name ${var.domain_name};

        # Redirect HTTP to HTTPS
        return 301 https://\$host\$request_uri;
    }

    server {
        # HTTPS configuration
        listen 443 ssl;
        server_name ${var.domain_name};

        # SSL certificates
        ssl_certificate /etc/letsencrypt/live/${var.domain_name}/fullchain.pem;
        ssl_certificate_key /etc/letsencrypt/live/${var.domain_name}/privkey.pem;
        include /etc/letsencrypt/options-ssl-nginx.conf;
        ssl_dhparam /etc/letsencrypt/ssl-dhparams.pem;

        # Reverse proxy to Docker app
        location / {
            proxy_pass http://localhost:5000; # Forward to the app
            proxy_set_header Upgrade \$http_upgrade;
            proxy_set_header Connection "Upgrade";
            proxy_set_header Host \$host;
            proxy_set_header X-Real-IP \$remote_addr;
            proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto \$scheme;

            proxy_read_timeout 3600s;
            proxy_send_timeout 3600s;
        }
    }
    EOFNGINX

    # Setup the cert
    certbot --nginx -d ${var.domain_name} --agree-tos --non-interactive --email ${var.email} --no-eff-email --no-self-upgrade

    # Symbolic link to enable site
    ln -s /etc/nginx/sites-available/${var.domain_name} /etc/nginx/sites-enabled/

    # Remove default
    rm /etc/nginx/sites-enabled/default

    # Restart nginx
    nginx -t
    systemctl reload nginx
  EOF

  tags = var.root_tags
}