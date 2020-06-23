rem daily
dtexec /FILE "\\sqlrdp\artifacts\prod\com.pharmmd.outboundDataFeed.HighmarkDispositionActivityReport\HighmarkDispositionActivityReport\bin\Production\ReportingPackage.dtsx"  /CONFIGFILE "\\sqlrdp\g\artifacts\prod\com.pharmmd.outboundDataFeed.HighmarkDispositionActivityReport\HighmarkDispositionActivityReport\MainConfigDaily.dtsConfig" /X86  /CHECKPOINTING OFF /REPORTING E

rem weekly
dtexec /FILE "\\sqlrdp\artifacts\prod\com.pharmmd.outboundDataFeed.HighmarkDispositionActivityReport\HighmarkDispositionActivityReport\bin\Production\ReportingPackage.dtsx"  /CONFIGFILE "\\sqlrdp\g\artifacts\prod\com.pharmmd.outboundDataFeed.HighmarkDispositionActivityReport\HighmarkDispositionActivityReport\MainConfigWeekly.dtsConfig" /X86  /CHECKPOINTING OFF /REPORTING E