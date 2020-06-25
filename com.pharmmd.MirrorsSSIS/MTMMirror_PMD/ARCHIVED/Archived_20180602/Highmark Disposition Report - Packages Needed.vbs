'if the mirrors fail, these are the tables needed to produce the highmark disposition report
'BW 4/10/2018

dim objShell
dim packagename

'for testing only
Set objShell = WScript.CreateObject ("WScript.shell")
packagename="\\sqlrdp\SSIS\_Jobs\DW\Mirrors\MTMMirror_PMD\Table-Users.dtsx"
'objShell.run packagename

subs

set objShell=nothing


function subs

packagename="\\sqlrdp\SSIS\_Jobs\DW\Mirrors\MTMMirror_PMD\Table-Events.dtsx"
objShell.run packagename

packagename="\\sqlrdp\SSIS\_Jobs\DW\Mirrors\MTMMirror_PMD\Table-Users.dtsx"
objShell.run packagename

packagename="\\sqlrdp\SSIS\_Jobs\DW\Mirrors\MTMMirror_PMD\Table-Patients.dtsx"
objShell.run packagename

packagename="\\sqlrdp\SSIS\_Jobs\DW\Mirrors\MTMMirror_PMD\Table-Organizations.dtsx"
objShell.run packagename

packagename="\\sqlrdp\SSIS\_Jobs\DW\Mirrors\MTMMirror_PMD\Table-Patient_Calls.dtsx"
objShell.run packagename

end function