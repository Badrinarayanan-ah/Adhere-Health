SELECT	CONVERT(VARCHAR(MAX), DATEADD(MONTH, -1,DATEADD(DAY, -DAY(GETDATE()) + 1, GETDATE())), 112) report
UNION ALL
SELECT	CONVERT(VARCHAR(5), ContractNumber) + '|'
		+ CASE WHEN HICN IS NULL THEN CONVERT(VARCHAR(12), ClientMemberId) ELSE hicn end+ '|'
		+ CONVERT(VARCHAR(30), FirstName) + '|'
		+ CONVERT(VARCHAR(1), MiddleInitial) + '|'
		+ CONVERT(VARCHAR(30), LastName) + '|' + CONVERT(VARCHAR(8), DoB, 112)+ '|'
		+ CASE	WHEN IsTargetCMSCriteriaMet = 1 THEN 'Y'
						ELSE 'N'
				END + '|'
		+ CASE IsCognitivelyImpaired WHEN 1 THEN 'Y'
						WHEN 0 THEN 'N'
						ELSE 'U'
				END + '|'
		+ CONVERT(VARCHAR(8), ISNULL(CONVERT(VARCHAR(8), MTMEnrollmentDate, 112),
										'')) + '|'
		+ CONVERT(VARCHAR(8), ISNULL(CONVERT(VARCHAR(8), TargetCMSCriteriaMetDate, 112),
										'')) + '|'
		+ CONVERT(VARCHAR(8), ISNULL(CONVERT(VARCHAR(8), MTMOptOutDate, 112), '')) + '|'
		+ CONVERT(VARCHAR(2), ( CASE	WHEN MTMOptOutReasonCd IS NULL THEN ''
										ELSE '0' + CAST(MTMOptOutReasonCd AS VARCHAR(1))
								END )) + '|'
		+ CASE	WHEN IsAnnualCMROffered = 1 THEN 'Y'
				ELSE 'N'
			END + '|'
		+ CONVERT(VARCHAR(8), ISNULL(CONVERT(VARCHAR(8), CMROfferedDate, 112),'')) + '|'
		+ CASE	WHEN IsAnnualCMRReceivedInStdCMSFmt = 1 THEN 'Y'
				ELSE 'N'
			END + '|' 
		+ CONVERT(VARCHAR(2), ISNULL(CMRCount, 0)) + '|'
		+ CONVERT(VARCHAR(8), ISNULL(CONVERT(VARCHAR(8), CMRReceivedDate1, 112),'')) + '|'
		+ CONVERT(VARCHAR(8), ISNULL(CONVERT(VARCHAR(8), CMRReceivedDate2, 112),'')) + '|'
		+ CONVERT(VARCHAR(8), ISNULL(CONVERT(VARCHAR(8), CMRReceivedDate3, 112),'')) + '|'
		+ CONVERT(VARCHAR(8), ISNULL(CONVERT(VARCHAR(8), CMRReceivedDate4, 112),'')) + '|'
		+ CONVERT(VARCHAR(8), ISNULL(CONVERT(VARCHAR(8), CMRReceivedDate5, 112),'')) + '|'
		+ CONVERT(VARCHAR(2), ( CASE	WHEN CMRDeliveryMethod IS NULL THEN ' '
										ELSE '0' + CAST(CMRDeliveryMethod AS VARCHAR(1))
								END )) + '|'
		+ CONVERT(VARCHAR(2), ( CASE	WHEN CMRInitialProvider IS NULL THEN ''
										ELSE CAST(CMRInitialProvider AS VARCHAR(2))
								END )) + '|'
		+ CONVERT(VARCHAR(2), ( CASE	WHEN CMRRecipient IS NULL THEN ''
										ELSE '0' + CAST(CMRRecipient AS VARCHAR(1))
								END )) + '|'
		+ CONVERT(VARCHAR(2), ( CASE	WHEN TMRCount IS NULL THEN ' '
										ELSE CAST(TMRCount AS VARCHAR(2))
								END )) + '|'
		+ CONVERT(VARCHAR(2), ( CASE	WHEN DTPRecommendationsCount IS NULL
										THEN ' '
										ELSE CAST(DTPRecommendationsCount AS VARCHAR(2))
								END )) + '|'
		+ CONVERT(VARCHAR(2), ( CASE	WHEN DTPResolutionsCount IS NULL
										THEN ' '
										ELSE CAST(DTPResolutionsCount AS VARCHAR(2))
								END )) + '|'
		--+REPLACE(REPLACE(CONVERT(VARCHAR(75), ISNULL(REPLACE(TopicsDiscussed1,'*',''), '')), CHAR(13), ''), CHAR(10), '') 
		+ '|'
		--+ REPLACE(REPLACE(CONVERT(VARCHAR(75), ISNULL(REPLACE(TopicsDiscussed2,'*',''), '')), CHAR(13), ''), CHAR(10), '') 
		+ '|'
		--+ REPLACE(REPLACE(CONVERT(VARCHAR(75), ISNULL(REPLACE(TopicsDiscussed3,'*',''), '')), CHAR(13), ''), CHAR(10), '') 
		+ '|'
		--+ REPLACE(REPLACE(CONVERT(VARCHAR(75), ISNULL(REPLACE(TopicsDiscussed4,'*',''), '')), CHAR(13), ''), CHAR(10), '') 
		+ '|'
		--+ REPLACE(REPLACE(CONVERT(VARCHAR(75), ISNULL(REPLACE(TopicsDiscussed5,'*',''), '')), CHAR(13), ''), CHAR(10), '')
		 + '|'
		+ p.Cardholder_ID AS CMSBeneficiaryData
FROM	dbo.CMSBeneficiaryStaging cms
		JOIN SQLETL.ETLCore.dbo.Patient p ON cms.PMDPatientID = p.PMD_Patient_ID
WHERE	cms.PMDClientID = 54