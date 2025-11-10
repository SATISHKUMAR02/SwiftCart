#!/bin/bash

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to be ready
until /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1
do
  echo "Waiting for SQL Server to start..."
  sleep 1
done

# Execute all SQL scripts found in /db-init (mounted volume)
for f in /db-init/*.sql; do
  echo "Running $f"
  /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -i "$f"
done

# Keep container running
wait
