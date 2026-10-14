# RimWorld 1.7 unstable: run the ten-scenario REAL-provider Grains matrix.
# 1.6 entry points and production source/art/metadata remain untouched.
[CmdletBinding()]
param(
    [string]$GameRoot = 'D:\SteamLibrary\steamapps\common\RimWorld',
    [ValidateSet('all','vanilla','vanilla-ccto','mo','mo-ccto')][string]$Profile = 'all',
    [ValidateRange(60,1800)][int]$TimeoutSeconds = 540
)
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'RimWorldRuntime.ps1')
. (Join-Path $PSScriptRoot 'GrainsTestProfiles.ps1')
. (Join-Path $PSScriptRoot 'GrainsSourceState.ps1')
$game = Get-RimWorldRuntime $GameRoot
if (-not $game.Modern) { throw 'Use run-grains-tests.bat for RimWorld 1.6.' }
$repo = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot '..')).Path
$mods = Join-Path $game.Root 'Mods'
$target = Join-Path $mods 'AncientMedievalJapanCore.E2ETarget'
$test = Join-Path $mods 'AncientMedievalJapanCore.E2E'
foreach ($path in @($target, $test)) {
    if (Test-Path -LiteralPath $path) {
        throw "Existing Grains test Mod requires inspection; refusing overwrite: $path"
    }
}
$normal = Join-Path $env:USERPROFILE 'AppData/LocalLow/Ludeon Studios/RimWorld by Ludeon Studios/Config'
$originals = @{}
foreach ($name in @('ModsConfig.xml','Prefs.xml')) {
    $originals[$name] = (Get-FileHash -LiteralPath (Join-Path $normal $name) -Algorithm SHA256).Hash
}
$sourceHash = (Get-GrainsSourceState $repo).Hash
$workshop = [IO.Path]::GetFullPath((Join-Path $game.Root '../../workshop/content/294100'))
$quick = Find-RimWorldModAssembly (Join-Path $workshop '3793646067') 'Quickstarts.dll' '1.7'
$pickle = Find-RimWorldModAssembly (Join-Path $workshop '3791648678') 'RimWorks.Pickle.dll' '1.7'
$profiles = if ($Profile -eq 'all') { @('vanilla','vanilla-ccto','mo','mo-ccto') } else { @($Profile) }
$result = Join-Path $repo ('TestResults/Grains17-' + (Get-Date -Format 'yyyyMMdd-HHmmss-fff'))
New-Item -ItemType Directory -Force -Path $result | Out-Null
$owned = $false
$matrix = New-Object 'System.Collections.Generic.List[object]'
try {
    foreach ($name in $profiles) {
        $spec = Get-GrainsTestProfile $name
        if ($owned) {
            foreach ($path in @($target, $test)) {
                $marker = Join-Path $path '.AMJG-17-OWNED'
                if (-not (Test-Path -LiteralPath $marker) -or
                    (Get-Content -LiteralPath $marker -Raw).Trim() -ne 'sucro.ancientmedievaljapan.core.e2e17') {
                    throw 'Refusing to replace an unmarked Grains test fixture.'
                }
            }
        }
        & (Join-Path $PSScriptRoot 'Stage-GrainsTestProfile.ps1') -RepositoryRoot $repo -ModsRoot $mods -Profile $name
        if (-not (Test-Path -LiteralPath $target) -or -not (Test-Path -LiteralPath $test)) {
            throw 'Grains profile staging did not produce both test Mods.'
        }
        $owned = $true
        foreach ($path in @($target, $test)) {
            Set-Content -LiteralPath (Join-Path $path '.AMJG-17-OWNED') -Value 'sucro.ancientmedievaljapan.core.e2e17' -Encoding Ascii
            [xml]$about = Read-RimWorldTestXml (Join-Path $path 'About/About.xml')
            $supported = $about.SelectSingleNode('/ModMetaData/supportedVersions')
            if ($null -eq $supported) { throw "Missing test-only supportedVersions: $path" }
            if (@($supported.li | ForEach-Object { [string]$_ }) -notcontains '1.7') {
                $li = $about.CreateElement('li')
                $li.InnerText = '1.7'
                [void]$supported.AppendChild($li)
            }
            $about.Save((Join-Path $path 'About/About.xml'))
        }
        # Add v1.7 only to the disposable Grains test target's conditional loader.
        # Source loadFolders.xml stays byte-identical in the repository.
        $loader = Join-Path $target 'loadFolders.xml'
        [xml]$xml = Read-RimWorldTestXml $loader
        $v16 = $xml.SelectSingleNode('/loadFolders/v1.6')
        if ($null -eq $v16 -or $null -ne $xml.SelectSingleNode('/loadFolders/v1.7')) {
            throw 'Unexpected source v1.6/v1.7 loader contract'
        }
        $v17 = $xml.CreateElement('v1.7')
        foreach ($child in $v16.ChildNodes) {
            [void]$v17.AppendChild($xml.ImportNode($child, $true))
        }
        [void]$xml.DocumentElement.AppendChild($v17)
        $xml.Save($loader)
        $testAbout = Join-Path $test 'About/About.xml'
        # The copied test-only About identity remains constant across profiles.
        Invoke-RimWorldTestCompile $game.Root (Join-Path $test 'Assemblies/AncientMedievalJapanCore.E2E.dll') `
            @((Join-Path $repo 'Tests/E2E/AmjStageAQuickstart.cs')) @($quick)
        $stepSources = @('StageASteps.cs','GrainsProfileSteps.cs','GrainsSimulationSteps.cs') |
            ForEach-Object { Join-Path $repo ('Tests/E2E/' + $_) }
        Invoke-RimWorldTestCompile $game.Root (Join-Path $test 'Pickle/Assemblies/AncientMedievalJapanCore.E2E.Steps.dll') `
            $stepSources @($pickle)

        $profileRoot = Join-Path $result $name
        $report = Join-Path $profileRoot 'Reports'
        $save = Join-Path $profileRoot 'SaveData'
        New-Item -ItemType Directory -Force -Path $report | Out-Null
        & (Join-Path $PSScriptRoot 'Prepare-TestSaveData.ps1') -OutputRoot $save -Profile $name -RimWorldRoot $game.Root
        if ($LASTEXITCODE -ne 0) { throw "Failed to prepare isolated Grains 1.7 profile $name" }
        [xml]$config = Read-RimWorldTestXml (Join-Path $save 'Config/ModsConfig.xml')
        $config.ModsConfigData.version = $game.Version.Split(' ')[0]
        $config.Save((Join-Path $save 'Config/ModsConfig.xml'))
        [xml]$prefs = Read-RimWorldTestXml (Join-Path $save 'Config/Prefs.xml')
        foreach ($pair in @(@('resetModsConfigOnCrash','False'), @('fullscreen','False'), @('runInBackground','True'))) {
            $node = $prefs.DocumentElement.SelectSingleNode($pair[0])
            if ($null -eq $node) {
                $node = $prefs.CreateElement($pair[0])
                [void]$prefs.DocumentElement.AppendChild($node)
            }
            $node.InnerText = $pair[1]
        }
        $prefs.Save((Join-Path $save 'Config/Prefs.xml'))
        $arguments = '-savedatafolder="' + $save + '" -pickle-run="' + $spec.Feature + '" -pickle-mode=fast ' +
            '-pickle-report-dir="' + $report + '" -pickle-no-browser -pickle-no-http -pickle-run-timeout=7'
        $code = 0; $detail = ''
        try {
            Invoke-RimWorldIsolated $game.Root $arguments (Join-Path $report 'Player.log') $TimeoutSeconds
            & (Join-Path $PSScriptRoot 'Validate-GrainsPickleSummary.ps1') -SummaryPath (Join-Path $report 'summary.json') -Profile $name
            if ($LASTEXITCODE -ne 0) { throw 'Profile summary gate failed.' }
            & (Join-Path $PSScriptRoot 'Validate-RuntimeLog.ps1') -LogPath (Join-Path $report 'Player.log') -ModIdPrefixes 'sucro.ancientmedievaljapan.core' -FailOnAnyError
            if ($LASTEXITCODE -ne 0) { throw 'Profile runtime ERROR gate failed.' }
        }
        catch { $code = 2; $detail = $_.Exception.Message; Write-Host "[FAIL] $name - $detail" }
        $matrix.Add([pscustomobject]@{Profile=$name;ExitCode=$code;Diagnostic=$detail;Reports=$report;Version=$game.Version})
        $matrix | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath (Join-Path $result 'matrix.json') -Encoding UTF8
    }
}
finally {
    if ($owned) {
        foreach ($path in @($target,$test)) {
            $marker = Join-Path $path '.AMJG-17-OWNED'
            if ((Test-Path -LiteralPath $marker) -and
                (Get-Content -LiteralPath $marker -Raw).Trim() -eq 'sucro.ancientmedievaljapan.core.e2e17') {
                Remove-Item -LiteralPath $path -Recurse -Force
            } else { throw "Refusing unsafe Grains fixture cleanup: $path" }
        }
    }
    if ((Get-GrainsSourceState $repo).Hash -ne $sourceHash) { throw 'Grains production source changed during 1.7 test.' }
    foreach ($name in $originals.Keys) {
        if ((Get-FileHash -LiteralPath (Join-Path $normal $name) -Algorithm SHA256).Hash -ne $originals[$name]) {
            throw "Normal RimWorld $name changed during 1.7 test."
        }
    }
}
if (@($matrix | Where-Object { $_.ExitCode -ne 0 }).Count) { throw "One or more Grains 1.7 profiles failed: $result" }
Write-Output "[OK] Grains 1.7 profiles: $($profiles -join ', ') $($spec.Scenarios.Count)/$($spec.Scenarios.Count) each, native ERROR 0. Evidence: $result"
