dim appname
dim foldername
appname="G:\artifacts\prod\com.pharmmd.com.OfficeToPDFConversions\PDFConverter\bin\Release\PDFConverter.exe"
foldername="G:\artifacts\prod\com.pharmmd.com.OfficeToPDFConversions\PDFConverter\bin\Release"

dim filepath
filepath="G:\Users\BW\HealthSpringHSAZQ2TMR_102099(1pt)(1-3DTP).docx"
filepath="G:\Users\BW\HealthSpringMAPDQ2TMR_102097(1pt)(1-3DTP).docx"

Dim objShell
Set objShell = WScript.CreateObject("WScript.Shell")

'objShell.run "cmd /k CD " & foldername
objShell.run appname & " " & """" & filepath & """"