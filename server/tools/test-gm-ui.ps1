param([Parameter(Mandatory=$true)][int]$ProcessId)
$ErrorActionPreference='Stop'
Add-Type -AssemblyName UIAutomationClient
Add-Type -AssemblyName UIAutomationTypes
$process=Get-Process -Id $ProcessId
$window=[System.Windows.Automation.AutomationElement]::FromHandle($process.MainWindowHandle)
$scope=[System.Windows.Automation.TreeScope]::Descendants
$listCondition=[System.Windows.Automation.PropertyCondition]::new([System.Windows.Automation.AutomationElement]::NameProperty,'GM sections')
$navigation=$window.FindFirst($scope,$listCondition)
if($null -eq $navigation){throw 'GM navigation not found; select the GM tab first.'}
$items=$navigation.FindAll([System.Windows.Automation.TreeScope]::Children,[System.Windows.Automation.Condition]::TrueCondition)
foreach($item in $items){
    $name=$item.Current.Name
    $selection=$item.GetCurrentPattern([System.Windows.Automation.SelectionItemPattern]::Pattern)
    $selection.Select()
    Start-Sleep -Milliseconds 200
    $deadline=[DateTime]::UtcNow.AddSeconds(15)
    while(-not $navigation.Current.IsEnabled -and [DateTime]::UtcNow -lt $deadline){Start-Sleep -Milliseconds 100}
    if(-not $navigation.Current.IsEnabled){throw "GM view timed out: $name"}
    $dialogs=$window.FindAll([System.Windows.Automation.TreeScope]::Children,[System.Windows.Automation.PropertyCondition]::new([System.Windows.Automation.AutomationElement]::ControlTypeProperty,[System.Windows.Automation.ControlType]::Window))
    if($dialogs.Count -gt 0){throw "GM view opened an error dialog: $name"}
    Write-Output "GM_VIEW_PASS $name"
}
$items[0].GetCurrentPattern([System.Windows.Automation.SelectionItemPattern]::Pattern).Select()
Write-Output 'GM_UI_PASS'
