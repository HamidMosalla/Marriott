Import-Module SQLPS -DisableNameChecking

$queueNameMatch = "marriott."
Write-Host "Purging Queues"
[Reflection.Assembly]::LoadWithPartialName("System.Messaging") | Out-Null
foreach($q in [System.Messaging.MessageQueue]::GetPrivateQueuesByMachine($env:computername) | where {$_.QueueName -match $queueNameMatch })
{
	Write-Host "   " $q.QueueName
	$q.Purge();
}

Write-Host "Dropping Marriott Database"
$Marriott = "Marriott"
$SQLInstanceName = "(LocalDB)\MSSQLLocalDB"
$Server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server -ArgumentList $SQLInstanceName
if($Server.Databases[$Marriott])
{
	Write-Host "   " $Marriott
	$Server.KillDatabase($Marriott)
}