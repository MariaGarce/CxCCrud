#!/bin/sh

echo "⏳ Esperando a que SQL Server esté listo..."
sleep 20

echo "🚀 Ejecutando migraciones..."

dotnet ef database update --project . --startup-project .

echo "✅ Migraciones completadas"
