# Persistent file system for storage
resource "aws_efs_file_system" "efs_storage" {
  creation_token = "${var.resource_prefix}-storage-fs"

  throughput_mode                 = "provisioned"
  provisioned_throughput_in_mibps = 20

  tags = var.root_tags
}

# Mount EFS to the same subnet the storage services are in
resource "aws_efs_mount_target" "efs_mount_private" {
  file_system_id  = aws_efs_file_system.efs_storage.id
  subnet_id       = var.subnet_id_private
  security_groups = [var.security_group_id_private]
}

# Access point for Postgres data
# NOTE: using for_each to try and define access points for Postgres, MongoDB, and RabbitMQ all at once created an issue where the access points were not synced
resource "aws_efs_access_point" "efs_access_storage_postgres" {
  file_system_id = aws_efs_file_system.efs_storage.id

  posix_user {
    gid = 0
    uid = 0
  }

  root_directory {
    creation_info {
      owner_gid   = 0
      owner_uid   = 0
      permissions = "755"
    }
    path = "/postgres-data"
  }

  tags = var.root_tags
}

# Access point for Postgres backups
resource "aws_efs_access_point" "efs_access_storage_postgres_backups" {
  file_system_id = aws_efs_file_system.efs_storage.id

  posix_user {
    gid = 0
    uid = 0
  }

  root_directory {
    creation_info {
      owner_gid   = 0
      owner_uid   = 0
      permissions = "755"
    }
    path = "/postgres-backups"
  }

  tags = var.root_tags
}

# Access point for MongoDB
resource "aws_efs_access_point" "efs_access_storage_mongodb" {
  file_system_id = aws_efs_file_system.efs_storage.id

  posix_user {
    gid = 999 # 999 is the user/group for the mongodb Docker image
    uid = 999
  }

  root_directory {
    creation_info {
      owner_gid   = 999
      owner_uid   = 999
      permissions = "777"
    }
    path = "/mongodb-data"
  }

  tags = var.root_tags
}

# Access point for RabbitMQ
resource "aws_efs_access_point" "efs_access_storage_rabbitmq" {
  file_system_id = aws_efs_file_system.efs_storage.id

  posix_user {
    gid = 999 # 999 is the user/group for the rabbitmq Docker image
    uid = 999
  }

  root_directory {
    creation_info {
      owner_gid   = 999
      owner_uid   = 999
      permissions = "755"
    }
    path = "/rabbitmq-data"
  }

  tags = var.root_tags
}

# Access point for Redis data
resource "aws_efs_access_point" "efs_access_storage_redis" {
  file_system_id = aws_efs_file_system.efs_storage.id

  posix_user {
    gid = 1000 # 1000/999 is the user/group for the redis Docker image
    uid = 999
  }

  root_directory {
    creation_info {
      owner_gid   = 1000
      owner_uid   = 999
      permissions = "755"
    }
    path = "/redis-data"
  }

  tags = var.root_tags
}

# Access point for Redis backups
resource "aws_efs_access_point" "efs_access_storage_redis_backups" {
  file_system_id = aws_efs_file_system.efs_storage.id

  posix_user {
    gid = 1000 # 1000/999 is the user/group for the redis Docker image
    uid = 999
  }

  root_directory {
    creation_info {
      owner_gid   = 1000
      owner_uid   = 999
      permissions = "755"
    }
    path = "/redis-backups"
  }

  tags = var.root_tags
}