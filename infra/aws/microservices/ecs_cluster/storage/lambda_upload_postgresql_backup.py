import boto3
import os
import socket
import time
from datetime import datetime, timezone
from zoneinfo import ZoneInfo
from enum import Enum

ecs = boto3.client("ecs")
s3 = boto3.client("s3")

CLUSTER_NAME = os.getenv("CLUSTER_NAME")
SERVICE_NAME = os.getenv("SERVICE_NAME")
CONTAINER_NAME = os.getenv("CONTAINER_NAME")
S3_BUCKET = os.getenv("S3_BUCKET")
DATABASE_NAME = os.getenv("DATABASE_NAME")
DATABASE_USER = os.getenv("DATABASE_USER")
EFS_PATH = os.getenv("EFS_PATH")
LOCAL_PATH = os.getenv("LOCAL_PATH")
DUMP_FILE_NAME = "mlb-forecaster.dump"
DONE_FILE = f"{LOCAL_PATH}/done.gz"
DUMP_FILE = f"{LOCAL_PATH}/{DUMP_FILE_NAME}"

class Status(Enum):
    READY = 0
    IN_PROGRESS = 1
    DONE = 2

def lambda_handler(event, context):
    print("---Start")
#     print(f"CLUSTER_NAME = {CLUSTER_NAME}")
#     print(f"SERVICE_NAME = {SERVICE_NAME}")
#     print(f"CONTAINER_NAME = {CONTAINER_NAME}")
#     print(f"S3_BUCKET = {S3_BUCKET}")
#     print(f"DATABASE_NAME = {DATABASE_NAME}")
#     print(f"DATABASE_USER = {DATABASE_USER}")
#     print(f"EFS_PATH = {EFS_PATH}")
#     print(f"LOCAL_PATH = {LOCAL_PATH}")
#     print(f"DUMP_FILE_NAME = {DUMP_FILE_NAME}")
#     print(f"DONE_FILE = {DONE_FILE}")
#     print(f"DUMP_FILE = {DUMP_FILE}")

    status = backup_status(DONE_FILE)

    if status == Status.IN_PROGRESS:
        print("---In Progress")
    elif status == Status.DONE:
        print("---Backup done")
        upload(DUMP_FILE)
        task = get_task()
        remove_done_file(task)
    else:
        print("---Starting backup")
        task = get_task()
        run_pg_dump(task)

    print("---Done")

# Checks if the backup is done by looking for the output file with a > 0 file size
def backup_status(file):
    print("---Checking if backup is done...")
    exists = os.path.exists(file)
    print(f"exists: {exists}")
    if not exists:
        return Status.READY

    # Check if the backup is stale
    file_modified_time = os.path.getmtime(file)
    now = time.time()
    hours_since_modified = (now - file_modified_time) / 60 / 60 # seconds -> minutes -> hours
    if hours_since_modified > 12: # Stale, so begin again
        print("---Backup stale")
        return Status.READY

    size = os.path.getsize(file)
    print(f"size: {size}")

    if size > 0:
        return Status.DONE
    else:
        return Status.IN_PROGRESS

# Gets the currently running ECS task so it can be used with aws ecs execute-command
def get_task():
    print("---Getting task...")
    tasks = ecs.list_tasks(cluster=CLUSTER_NAME, serviceName=SERVICE_NAME)

    if tasks["taskArns"]:
        print(f"Got task {tasks["taskArns"][0]}")
        return tasks["taskArns"][0]
    else:
        print("No running tasks")
        return None

# Runs pg_dump
def run_pg_dump(task_id):
    print("---Starting pg_dump...")
    command = f"sh -c 'pg_dump -U {DATABASE_USER} -d {DATABASE_NAME} -F c -f {EFS_PATH}/{DUMP_FILE_NAME} | gzip - > {EFS_PATH}/done.gz'"

    response = ecs.execute_command(
        cluster=CLUSTER_NAME,
        task=task_id,
        container=CONTAINER_NAME,
        command=command,
        interactive=True
    )
    print(f"pg_dump: {response}")

# Uploads the file to S3
def upload(file):
    print("---Uploading dump...")

    utc_now = datetime.now(timezone.utc)
    pst_now = utc_now.astimezone(ZoneInfo("America/Los_Angeles"))
    date = pst_now.strftime("%Y-%m-%d")

    s3.upload_file(file, S3_BUCKET, f"backups/postgresql/{date}_{DUMP_FILE_NAME}")
    print("Upload done")

# Deletes the file that indicates pg_dump is done
def remove_done_file(task_id):
    print("---Removing done file...")
    command = f"sh -c 'rm -rf {EFS_PATH}/done.gz'"

    response = ecs.execute_command(
        cluster=CLUSTER_NAME,
        task=task_id,
        container=CONTAINER_NAME,
        command=command,
        interactive=True
    )
    print(f"removing done file: {response}")