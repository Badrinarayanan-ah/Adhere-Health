create or replace function x_weekly_referral_notes() returns character varying
as

$$
BEGIN

	delete from x_weekly_referral_data where x_weekly_referral_id in (select id from x_weekly_referral where attempted_date is not null and succeeded_date is null and failed_date is null);
	delete from x_weekly_referral where attempted_date is not null and succeeded_date is null and failed_date is null;

--Step 1: queue up patients that haven't been sent yet.
	insert into x_weekly_referral	(pmd_patient_id, patient_id,member_id, event_id, attempted_date)
		select sq.*,now() from (
		     SELECT pt.pmd_patient_id , pt.id, pt.member_id,
		     MAX(e.id) AS eventid
		     FROM   patients pt inner join events e on pt.id=e.patient_id
		     WHERE  e.description like '%1121]%'
		     GROUP BY pt.pmd_patient_id,pt.id,pt.member_id
		) sq
		left join x_weekly_referral x on sq.eventid = event_id and sq.pmd_patient_id = x.pmd_patient_id and failed_date is null
		     where x.id is null;

--Step 2: capture snapshot comments for these patients that haven't been sent yet.
	insert into x_weekly_referral_data (x_weekly_referral_id,snapshot_comment_id)
		select xwr.id,sc.id from snapshot_comments sc 
			inner join refactor_snapshots rs on sc.refactor_snapshot_id=rs.id
			inner join x_weekly_referral xwr on rs.patient_id = xwr.patient_id
			left join x_weekly_referral_data xwrd on xwr.id=xwrd.x_weekly_referral_id and sc.id=xwrd.snapshot_comment_id
		where sc.text like '%1121]%' and xwrd.id is null;

		
	return 'success';
EXCEPTION
	WHEN OTHERS THEN
         
         return 'failure';
END;
$$
language plpgsql;

create or replace function x_weekly_referral_close(pSuccess int) returns character varying
as

$$
BEGIN

	if pSuccess=1 then
		update x_weekly_referral set succeeded_date = now() where succeeded_date is null and failed_date is null;
	elsif pSuccess=0 then 
		update x_weekly_referral set failed_date = now() where succeeded_date is null and failed_date is null;
	end if;	

	return 'success';
EXCEPTION
	when others then 
	return 'failure';
END
$$
Language plpgsql;

--select * from x_weekly_referral;
--delete from x_weekly_referral;
--delete from x_weekly_referral_data;

drop function x_weekly_Referral_output();
CREATE OR REPLACE FUNCTION x_weekly_referral_output()
  RETURNS TABLE(pmd_patient_id integer, MemberId character varying, RefDate timestamp without time zone, MemberConsent character varying,RefNote text)
AS
$$
begin
return query
select xwr.pmd_patient_id, xwr.member_id , sc.created_at  , '1'::character varying , replace(replace(replace(text,chr(9),' '),chr(10),' '),chr(13),' ')  from x_weekly_referral xwr inner join x_weekly_referral_data xwrd on xwr.id=xwrd.x_weekly_referral_id 
inner join snapshot_comments sc on xwrd.snapshot_comment_id =sc.id 
where xwr.succeeded_date is null and xwr.failed_date is null;
end;
$$
language plpgsql;
