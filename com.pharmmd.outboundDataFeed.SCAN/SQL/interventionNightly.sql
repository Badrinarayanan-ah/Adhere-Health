SELECT 
zi.member_id AS member_id
, DATE_PART('year', current_date) AS Rpt_yr
, DATE_PART('quarter', current_date) AS Rpt_Qtr
, cast(current_date as timestamp without time zone) AS Pub_Date
, zi.member_state As Mbr_State
, zi.created_at AS DTP_Date
, CAST(zi.recommendation_details AS VARCHAR(500)) AS Recommendation_details 
,cc.review_reason_desc as Review_ID
FROM zInterventions zi
LEFT OUTER JOIN disease_categories dc ON zi.disease_category_id = dc.id
INNER JOIN  Client_Case_notes cc On zi.member_id=cc.member_id
INNER JOIN patients p on cc.patient_id=p.id
WHERE zi.status = 'identified' and endpoint_type = 'Patient' 
and (filterstep is null or filterstep =12) and cc.review_decision_desc like '%Clinical Consultation%' 
and p.call_queue_id_last=9 and p.organization_id='1015'

ORDER BY zi.filterstep desc, zi.member_id
;

