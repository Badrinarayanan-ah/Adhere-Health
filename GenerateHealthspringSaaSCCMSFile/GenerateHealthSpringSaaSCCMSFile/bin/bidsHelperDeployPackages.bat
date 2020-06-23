"I:\SQL2008r2\x86\100\DTS\binn\dtutil.exe" /FCreate DTS;"MSDB";"Outbound" /SourceServer sqldw 
"I:\SQL2008r2\x86\100\DTS\binn\dtutil.exe" /FILE "Z:\artifacts\prod\GenerateHealthspringSaaSCCMSFile\GenerateHealthSpringSaaSCCMSFile\GenerateHealthspringCCMSFile.dtsx" /DestServer sqldw /COPY DTS;"MSDB\Outbound\GenerateHealthspringCCMSFile" /Q
