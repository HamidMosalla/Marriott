Import-Module SQLPS -DisableNameChecking

Write-Host "Dropping Marriott Database"

$Marriott = "Marriott"
$SQLInstanceName = "(LocalDB)\MSSQLLocalDB"
$Server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server -ArgumentList $SQLInstanceName

if($Server.Databases[$Marriott])
{
	Write-Host "   " $Marriott
	$Server.KillDatabase($Marriott)
}