param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [string]$OutputDirectory = (Join-Path $RepoRoot "marketing\google-play\generated")
)

$ErrorActionPreference = "Stop"

if (-not $IsWindows) {
    throw "This generator uses System.Drawing and must be run on Windows. SVG sources are available in marketing/google-play/source."
}

Add-Type -AssemblyName System.Drawing.Common
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

function New-RoundedPath {
    param([float]$X, [float]$Y, [float]$Width, [float]$Height, [float]$Radius)

    $path = [System.Drawing.Drawing2D.GraphicsPath]::new()
    $diameter = $Radius * 2
    $path.AddArc($X, $Y, $diameter, $diameter, 180, 90)
    $path.AddArc($X + $Width - $diameter, $Y, $diameter, $diameter, 270, 90)
    $path.AddArc($X + $Width - $diameter, $Y + $Height - $diameter, $diameter, $diameter, 0, 90)
    $path.AddArc($X, $Y + $Height - $diameter, $diameter, $diameter, 90, 90)
    $path.CloseFigure()
    return $path
}

function New-GradientBrush {
    param([System.Drawing.RectangleF]$Rectangle, [string]$Start, [string]$End)
    return [System.Drawing.Drawing2D.LinearGradientBrush]::new(
        $Rectangle,
        [System.Drawing.ColorTranslator]::FromHtml($Start),
        [System.Drawing.ColorTranslator]::FromHtml($End),
        45.0)
}

function Draw-Mark {
    param(
        [System.Drawing.Graphics]$Graphics,
        [float]$X,
        [float]$Y,
        [float]$Scale
    )

    $blueRect = [System.Drawing.RectangleF]::new($X, $Y, 120 * $Scale, 120 * $Scale)
    $violetRect = [System.Drawing.RectangleF]::new($X + 136 * $Scale, $Y, 120 * $Scale, 120 * $Scale)
    $greenRect = [System.Drawing.RectangleF]::new($X, $Y + 136 * $Scale, 256 * $Scale, 136 * $Scale)

    $blue = New-GradientBrush $blueRect "#38BDF8" "#2563EB"
    $violet = New-GradientBrush $violetRect "#A78BFA" "#7C3AED"
    $green = New-GradientBrush $greenRect "#34D399" "#0D9488"

    $bluePath = New-RoundedPath $blueRect.X $blueRect.Y $blueRect.Width $blueRect.Height (34 * $Scale)
    $violetPath = New-RoundedPath $violetRect.X $violetRect.Y $violetRect.Width $violetRect.Height (34 * $Scale)
    $greenPath = New-RoundedPath $greenRect.X $greenRect.Y $greenRect.Width $greenRect.Height (38 * $Scale)

    $Graphics.FillPath($blue, $bluePath)
    $Graphics.FillPath($violet, $violetPath)
    $Graphics.FillPath($green, $greenPath)

    $white = [System.Drawing.Pen]::new([System.Drawing.Color]::White, 22 * $Scale)
    $white.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $white.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    $white.LineJoin = [System.Drawing.Drawing2D.LineJoin]::Round

    $Graphics.DrawLines($white, [System.Drawing.PointF[]]@(
        [System.Drawing.PointF]::new($X + 64 * $Scale, $Y + 212 * $Scale),
        [System.Drawing.PointF]::new($X + 98 * $Scale, $Y + 156 * $Scale),
        [System.Drawing.PointF]::new($X + 158 * $Scale, $Y + 156 * $Scale),
        [System.Drawing.PointF]::new($X + 192 * $Scale, $Y + 212 * $Scale)
    ))
    $Graphics.DrawLine($white, $X + 85 * $Scale, $Y + 194 * $Scale, $X + 171 * $Scale, $Y + 194 * $Scale)

    $Graphics.FillEllipse([System.Drawing.Brushes]::White, $X + 44 * $Scale, $Y + 44 * $Scale, 32 * $Scale, 32 * $Scale)

    $star = [System.Drawing.Drawing2D.GraphicsPath]::new()
    $cx = $X + 186 * $Scale
    $cy = $Y + 60 * $Scale
    $star.AddPolygon([System.Drawing.PointF[]]@(
        [System.Drawing.PointF]::new($cx, $cy - 26 * $Scale),
        [System.Drawing.PointF]::new($cx + 9 * $Scale, $cy - 9 * $Scale),
        [System.Drawing.PointF]::new($cx + 26 * $Scale, $cy),
        [System.Drawing.PointF]::new($cx + 9 * $Scale, $cy + 9 * $Scale),
        [System.Drawing.PointF]::new($cx, $cy + 26 * $Scale),
        [System.Drawing.PointF]::new($cx - 9 * $Scale, $cy + 9 * $Scale),
        [System.Drawing.PointF]::new($cx - 26 * $Scale, $cy),
        [System.Drawing.PointF]::new($cx - 9 * $Scale, $cy - 9 * $Scale)
    ))
    $Graphics.FillPath([System.Drawing.Brushes]::White, $star)

    $blue.Dispose(); $violet.Dispose(); $green.Dispose(); $white.Dispose()
    $bluePath.Dispose(); $violetPath.Dispose(); $greenPath.Dispose(); $star.Dispose()
}

function Save-Icon {
    $path = Join-Path $OutputDirectory "app-icon-512.png"
    $bitmap = [System.Drawing.Bitmap]::new(512, 512, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $graphics.Clear([System.Drawing.ColorTranslator]::FromHtml("#0B1220"))

    $background = New-GradientBrush ([System.Drawing.RectangleF]::new(0, 0, 512, 512)) "#172554" "#020617"
    $outer = New-RoundedPath 0 0 512 512 112
    $graphics.FillPath($background, $outer)
    Draw-Mark $graphics 128 112 1.0

    $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $background.Dispose(); $outer.Dispose(); $graphics.Dispose(); $bitmap.Dispose()
    return $path
}

function Save-FeatureGraphic {
    $path = Join-Path $OutputDirectory "feature-graphic-1024x500.png"
    $bitmap = [System.Drawing.Bitmap]::new(1024, 500, [System.Drawing.Imaging.PixelFormat]::Format24bppRgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $graphics.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::AntiAliasGridFit

    $background = New-GradientBrush ([System.Drawing.RectangleF]::new(0, 0, 1024, 500)) "#172554" "#052E2B"
    $graphics.FillRectangle($background, 0, 0, 1024, 500)

    $titleFont = [System.Drawing.Font]::new("Arial", 58, [System.Drawing.FontStyle]::Bold, [System.Drawing.GraphicsUnit]::Pixel)
    $productFont = [System.Drawing.Font]::new("Arial", 45, [System.Drawing.FontStyle]::Bold, [System.Drawing.GraphicsUnit]::Pixel)
    $bodyFont = [System.Drawing.Font]::new("Arial", 26, [System.Drawing.FontStyle]::Regular, [System.Drawing.GraphicsUnit]::Pixel)
    $smallFont = [System.Drawing.Font]::new("Arial", 21, [System.Drawing.FontStyle]::Regular, [System.Drawing.GraphicsUnit]::Pixel)

    $graphics.DrawString("AppFactory", $titleFont, [System.Drawing.Brushes]::White, 92, 145)
    $graphics.DrawString("Pomocniki", $productFont, [System.Drawing.SolidBrush]::new([System.Drawing.ColorTranslator]::FromHtml("#38BDF8")), 92, 212)
    $graphics.DrawString("20 praktycznych narzędzi w jednej aplikacji", $bodyFont, [System.Drawing.SolidBrush]::new([System.Drawing.ColorTranslator]::FromHtml("#CBD5E1")), 92, 286)
    $graphics.DrawString("Dom • styl • hobby • codzienne diagnozy", $smallFont, [System.Drawing.SolidBrush]::new([System.Drawing.ColorTranslator]::FromHtml("#94A3B8")), 92, 337)

    $panelBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(184, 2, 6, 23))
    $panel = New-RoundedPath 660 66 310 354 76
    $graphics.FillPath($panelBrush, $panel)
    Draw-Mark $graphics 690 104 0.98

    $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)

    $background.Dispose(); $panelBrush.Dispose(); $panel.Dispose()
    $titleFont.Dispose(); $productFont.Dispose(); $bodyFont.Dispose(); $smallFont.Dispose()
    $graphics.Dispose(); $bitmap.Dispose()
    return $path
}

$icon = Save-Icon
$feature = Save-FeatureGraphic

$iconInfo = [System.IO.FileInfo]$icon
if ($iconInfo.Length -gt 1MB) {
    throw "Google Play icon exceeds 1 MB: $($iconInfo.Length) bytes"
}

Write-Host "Generated Google Play assets:"
Write-Host "- $icon"
Write-Host "- $feature"
