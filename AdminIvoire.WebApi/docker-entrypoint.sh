#!/bin/bash
set -e

# Read secrets and export as environment variables
if [ -f /run/secrets/postgres_password ]; then
    export POSTGRES_PASSWORD=$(cat /run/secrets/postgres_password)
fi

if [ -f /run/secrets/postgres_user ]; then
    export POSTGRES_USER=$(cat /run/secrets/postgres_user)
fi

if [ -f /run/secrets/googlemaps_apikey ]; then
    export GOOGLEMAPS_APIKEY=$(cat /run/secrets/googlemaps_apikey)
fi

# Construct connection string if POSTGRES_PASSWORD is set
if [ -n "$POSTGRES_PASSWORD" ]; then
    export ConnectionStrings__LocaliteContext="Host=adminivoire.db;Database=${POSTGRES_DB:-adminivoire};Username=${POSTGRES_USER:-adminivoire};Password=${POSTGRES_PASSWORD};Port=5432"
fi

# Set Google Maps API key if available
if [ -n "$GOOGLEMAPS_APIKEY" ]; then
    export GoogleMaps__ApiKey="$GOOGLEMAPS_APIKEY"
fi

# Execute the original entrypoint
exec dotnet AdminIvoire.WebApi.dll "$@"


