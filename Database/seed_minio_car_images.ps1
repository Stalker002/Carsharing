param(
    [string]$ComposeDir = (Join-Path $PSScriptRoot "..\Carsharing"),
    [string]$BucketName = "carsharing-images",
    [string]$AccessKey = "minioadmin",
    [string]$SecretKey = "minioadmin"
)

$ErrorActionPreference = "Stop"

$imagesDir = Join-Path $PSScriptRoot "seed_images"

if (-not (Test-Path -LiteralPath $imagesDir)) {
    throw "Seed images directory not found: $imagesDir"
}

$requiredImages = @(
    "front-renault-logan.png",
    "front-vw-polo.png",
    "front-tesla-model-3.png",
    "front-kia-sportage.png",
    "front-bmw-3.png",
    "front-audi-a4.png"
)

foreach ($image in $requiredImages) {
    $path = Join-Path $imagesDir $image
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Seed image not found: $path"
    }
}

Push-Location $ComposeDir
try {
    docker compose cp "$imagesDir/." "minio:/tmp/front-seed-cars"

    $minioCommand = @"
set -e
mc alias set local http://localhost:9000 "$AccessKey" "$SecretKey"
mc mb --ignore-existing "local/$BucketName"
mc cp --recursive /tmp/front-seed-cars/ "local/$BucketName/images/cars/"
mc anonymous set download "local/$BucketName" >/dev/null
rm -rf /tmp/front-seed-cars
"@

    $minioCommand | docker compose exec -T minio sh
}
finally {
    Pop-Location
}

Write-Host "Seed car images uploaded to MinIO bucket '$BucketName'."
