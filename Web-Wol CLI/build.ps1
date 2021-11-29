$directory = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$out = @("linux-arm", "linux-arm64", "linux-x64", "osx-arm64", "osx-x64", "win-arm", "win-arm64", "win-x64")

for ($i = 0; $i -lt $out.length; $i++) {
	$x = $out[$i]

	dotnet publish -c Release -o $directory/publish/$x --no-self-contained -r $x -p:PublishSingleFile=true
	Compress-Archive -Path $directory/publish/$x -DestinationPath $directory/publish/$x.zip -Force
}