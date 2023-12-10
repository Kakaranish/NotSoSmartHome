#!/bin/bash

migration_name="$1"
if [ -z $migration_name ] ; then
    echo "Missing migration name"
    exit -1
fi

migrations_dir="../Migrations"
project="../NotSoSmartHome.csproj"

dotnet ef migrations add --project $project $migration_name

mgiration_filename_without_extension=$(ls $migrations_dir | grep "$migration_name.cs" --colour=never | head -1 | cut -d. -f1)

dotnet ef migrations script --project $project --output "$migrations_dir/$mgiration_filename_without_extension.sql" -i $migration_name

find $migrations_dir -maxdepth 1 -name "*$migration_name*.cs" -delete