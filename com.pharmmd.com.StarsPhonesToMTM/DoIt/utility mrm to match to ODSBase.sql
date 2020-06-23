CREATE TABLE #tmpToinsert(autoid BIGINT IDENTITY(1,1),pmdpatientid BIGINT, phonenumber VARCHAR(10), sqlscript VARCHAR(500))

DECLARE @loopautoid BIGINT
DECLARE @loopsqlscript VARCHAR(500)

INSERT INTO #tmpToinsert
        ( pmdpatientid, phonenumber, sqlscript
        )
SELECT a.pmdpatientid, a.[phone number],'insert into odsbase.patientphonenumber(pmdpatientid, number, otherfirstseenon)
values(''' + CONVERT(VARCHAR,a.pmdpatientid) + ''',''' + CONVERT(VARCHAR,a.[phone number]) + ''',getdate())'
FROM utility.dbo.MRMtoMTMPhoneNumbersMapping a 
inner join odsbase.patientdetail b on a.pmdpatientid=b.pmdpatientid
WHERE a.needsloading='true'
AND a.[phone number] IS NOT NULL AND a.[Phone Number] <> '0000000000'

WHILE(SELECT COUNT(*) FROM #tmpToinsert)>0
BEGIN
	SELECT TOP 1 @loopautoid=autoid, @loopsqlscript=sqlscript FROM #tmpToinsert
	
	EXEC(@loopsqlscript)
	
	DELETE FROM #tmpToinsert WHERE autoid=@loopautoid
END


DROP TABLE #tmpToinsert


