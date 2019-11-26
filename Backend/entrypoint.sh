#!/bin/bash

set -e
run_cmd="dotnet watch -p DevTest run"

dotnet restore

>&2 echo "DB is up - executing command"
exec $run_cmd