function Invoke-Grep {
	(Measure-Command { cat .\samples\p10.txt | grep elit --color=always }).TotalMilliseconds
}

$PROJECT = ".\src\Grepz\Grepz.csproj";
$SAMPLE = ".\samples\p10.txt";
$DIST = ".\src\Grepz\bin\Release\net8.0\publish\win-x64\Grepz.exe"
$DIST = "C:\bin\Grepz.exe"
$msbuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\\MSBuild\Current\Bin\amd64\MSBuild.exe"

function Init {
	# dotnet tool install -g nbgv
	dotnet tool install --tool-path .tools nbgv
	winget install --id Microsoft.NuGet
}

function Build {
# . "${msbuild}" /bl `
  # $PROJECT `
  # -p:Configuration=Release `
  # "/t:Clean;Build" `
   # -p:Deterministic=true
   dotnet build $PROJECT  -c Release -r win-x64
}


function Test {
  (Measure-Command { cat $SAMPLE | . $DIST "ex" -i }).TotalMilliseconds
}

function Debug {
  cat $SAMPLE | dotnet run -c Debug --project $PROJECT -- "ex" -i
}

function Publish {
  nbgv prepare-release
  dotnet restore src/Grepz.sln
  Build
  dotnet publish $PROJECT -r win-x64
  dotnet pack $PROJECT -o dist/ --no-build --configuration Release
}

function Release {
	Publish
	  dotnet nuget push `
    "dist/Grepz.$(nbgv get-version -v NuGetPackageVersion).nupkg" `
    --source https://www.myget.org/F/guneysu/api/v2/package --api-key=$env:MYGET_API_KEY
}

function Get-NugetVersion {
	$nugetVersion = $(nbgv get-version -v NuGetPackageVersion)
	Write-Host $nugetVersion
}