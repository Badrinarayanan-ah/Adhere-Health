USE [ETLCore]
GO
/****** Object:  Table [dbo].[USPSPatientAddressCheckList]    Script Date: 12/14/2017 2:16:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USPSPatientAddressCheckList](
	[PatientID] [bigint] NOT NULL,
	[Address1] [varchar](50) NOT NULL,
	[InsertDate] [datetime] NOT NULL CONSTRAINT [DF_USPSPatientAddressCheckList_InsertDate]  DEFAULT (getdate()),
	[LastVerifiedDate] [datetime] NULL,
 CONSTRAINT [PK_USPSPatientAddressCheckList] PRIMARY KEY CLUSTERED 
(
	[PatientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[USPSReturnVerification]    Script Date: 12/14/2017 2:16:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USPSReturnVerification](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PMDPatientID] [bigint] NOT NULL,
	[InputFullAddress] [varchar](150) NOT NULL,
	[ReturnedFullAddress] [varchar](150) NULL,
	[Verified] [bit] NULL,
	[Match] [bit] NULL,
	[Differences] [varchar](100) NULL,
	[SoundexMatch] [bit] NULL,
	[OriginalAddress1] [varchar](100) NULL,
	[OriginalAddress2] [varchar](100) NULL,
	[OriginalCity] [varchar](100) NULL,
	[OriginalState] [varchar](100) NULL,
	[OriginalZip] [varchar](10) NULL,
	[NewAddress1] [varchar](100) NULL,
	[NewAddress2] [varchar](100) NULL,
	[NewCity] [varchar](100) NULL,
	[NewState] [varchar](100) NULL,
	[NewZip] [varchar](10) NULL,
	[InsertDate] [datetime] NOT NULL CONSTRAINT [DF_USPSReturnVerification_InsertDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_USPSReturnVerification] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[InsertUsedPatientRecord]    Script Date: 12/14/2017 2:16:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Brian Williams>
-- Create date: <9/19/2017>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertUsedPatientRecord]
	-- Add the parameters for the stored procedure here
(
@PatientID bigint,
@Address1 varchar(50)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into USPSPatientAddressCheckList(PatientID, Address1)
	values(@PatientID, @Address1)

	SELECT -1
END

GO
/****** Object:  StoredProcedure [dbo].[InsertUSPSReturnVerification]    Script Date: 12/14/2017 2:16:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertUSPSReturnVerification]
	-- Add the parameters for the stored procedure here
(
@PMDPatientID bigint,
@InputFullAddress varchar(150),
@ReturnedFullAddress varchar(150)=null,
@Verified bit=null,
@Match bit=null,
@Differences varchar(100)=null,
@SoundexMatch bit=null,
@Address1 varchar(100)=null,
@Address2 varchar(100)=null,
@City varchar(100)=null,
@State varchar(100)=null,
@Zip varchar(10)=null,
@NewAddress1 varchar(100)=null,
@NewAddress2 varchar(100)=null,
@NewCity varchar(100)=null,
@NewState varchar(100)=null,
@NewZip varchar(10)=null
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into USPSReturnVerification(pmdpatientid, inputFulladdress, returnedFulladdress, verified, match, differences, soundexmatch,
	originaladdress1, originaladdress2, originalcity, originalstate, originalzip,
	newaddress1, newaddress2, newcity, newstate, newzip
	)
	values(@pmdpatientid, @inputFulladdress, @returnedFulladdress, @verified, @match, @differences, @soundexmatch,
	@address1, @address2, @city, @state, @zip,
	@newaddress1, @newaddress2, @newcity, @newstate, @newzip
	)

	SELECT -1
END

GO
/****** Object:  StoredProcedure [dbo].[ReturnUSPSAPIEligibleList]    Script Date: 12/14/2017 2:16:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ReturnUSPSAPIEligibleList]
	-- Add the parameters for the stored procedure here
(
@PmdClientID int=null,
@Top int=null
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if isnull(@Top,-30)=-30
	BEGIN
		SET @Top=1000
	END

    -- Insert statements for procedure here
	select distinct 
	top(@top)
	patient.pmd_patient_id, patient.cardholder_addr_1, patient.cardholder_addr_2, patient.cardholder_city, patient.cardholder_state, patient.cardholder_zip, null, client.client from patient(nolock) join client(nolock) on patient.pmd_client_id = client.pmdclientid
    where len(patient.cardholder_addr_1) > 0 and pmd_patient_id not in (select distinct a.patientid from USPSPatientAddressCheckList a where a.address1 = patient.cardholder_addr_1)
	and
	(
	(isnull(@PmdClientID,-1)=-1)
	or 
	(patient.pmd_client_id=@Pmdclientid)
	)

END

GO
