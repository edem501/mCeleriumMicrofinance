USE [master]
GO
/****** Object:  Database [SMSServers]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SMSServers')
BEGIN
CREATE DATABASE [SMSServers]

END

GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SMSServers].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SMSServers] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SMSServers] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SMSServers] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SMSServers] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SMSServers] SET ARITHABORT OFF 
GO
ALTER DATABASE [SMSServers] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [SMSServers] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [SMSServers] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SMSServers] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SMSServers] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SMSServers] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SMSServers] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SMSServers] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SMSServers] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SMSServers] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SMSServers] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SMSServers] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SMSServers] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SMSServers] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SMSServers] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SMSServers] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SMSServers] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SMSServers] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SMSServers] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SMSServers] SET  MULTI_USER 
GO
ALTER DATABASE [SMSServers] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SMSServers] SET DB_CHAINING OFF 
GO
USE [SMSServers]
GO
/****** Object:  StoredProcedure [spToBePosted]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spToBePosted]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spToBePosted]
	-- Add the parameters for the stored procedure here
	  @date1 DATETIME,
	  @date2 DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        TTransaction.AgentId, Commerciaux.AgentName, COUNT(*) AS NTrans, SUM(TTransaction.Debit) AS SPaiement, SUM(TTransaction.Credit) AS SCollet, SUM(TTransaction.Description)*500 AS SNouveauClient
FROM            TTransaction INNER JOIN
                         Commerciaux ON TTransaction.AgentId = Commerciaux.AgentId
WHERE     Posted=''false'' AND   (TTransaction.DateOperation BETWEEN @date1 AND @date2)
GROUP BY TTransaction.AgentId, Commerciaux.AgentName
ORDER BY dbo.Commerciaux.AgentName
END
' 
END
GO
/****** Object:  StoredProcedure [spToBePostedUploaded]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spToBePostedUploaded]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spToBePostedUploaded]
	-- Add the parameters for the stored procedure here
	  @date1 DATETIME,
	  @date2 DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        TTransactionUpload.AgentId, Commerciaux.AgentName, COUNT(*) AS NTrans, SUM(TTransactionUpload.Debit) AS SPaiement, SUM(TTransactionUpload.Credit) AS SCollet, SUM(TTransactionUpload.Description)*500 AS SNouveauClient
FROM            TTransactionUpload INNER JOIN
                         Commerciaux ON TTransactionUpload.AgentId = Commerciaux.AgentId
WHERE     Posted=''false'' AND   (TTransactionUpload.DateOperation BETWEEN @date1 AND @date2)
GROUP BY TTransactionUpload.AgentId, Commerciaux.AgentName
ORDER BY dbo.Commerciaux.AgentName
END
' 
END
GO
/****** Object:  StoredProcedure [spToBeValidated]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spToBeValidated]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spToBeValidated]
	-- Add the parameters for the stored procedure here
	  @date1 DATETIME,
	  @date2 DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	(SELECT        TTransaction.AgentId, Commerciaux.AgentName, COUNT(*) AS NTrans, SUM(TTransaction.Debit) AS SPaiement, SUM(TTransaction.Credit) AS SCollet, SUM(TTransaction.Description*500) AS SNouveauClient
FROM            TTransaction INNER JOIN
                         Commerciaux ON TTransaction.AgentId = Commerciaux.AgentId
WHERE     Posted=''false'' AND   (TTransaction.DateOperation BETWEEN @date1 AND @date2)
GROUP BY TTransaction.AgentId, Commerciaux.AgentName
)
Union
(SELECT        TTransactionUpload.AgentId, Commerciaux.AgentName, COUNT(*) AS NTrans, SUM(TTransactionUpload.Debit) AS SPaiement, SUM(TTransactionUpload.Credit) AS SCollet, SUM(TTransactionUpload.Description*500) AS SNouveauClient
FROM            TTransactionUpload INNER JOIN
                         Commerciaux ON TTransactionUpload.AgentId = Commerciaux.AgentId
WHERE     Posted=''false'' AND   (TTransactionUpload.DateOperation BETWEEN @date1 AND @date2)
GROUP BY TTransactionUpload.AgentId, Commerciaux.AgentName
)
ORDER BY dbo.Commerciaux.AgentName
END
' 
END
GO
/****** Object:  StoredProcedure [spTransDay]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spTransDay]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spTransDay]
	-- Add the parameters for the stored procedure here
	  @date1 VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH g AS (SELECT        DATEPART(HOUR, DateOperation) AS Heure, SUM(Credit) AS c
                         FROM            dbo.TTransaction
                         WHERE        (CONVERT(DATE, DateOperation) = @date1)
                         GROUP BY DATEPART(HOUR, DateOperation))
    SELECT        TOP (100) PERCENT g_1.Heure, g_1.c AS Total, SUM(g2.c) AS cTotal
     FROM            g AS g_1 INNER JOIN
                              g AS g2 ON g_1.Heure >= g2.Heure
     GROUP BY g_1.Heure, g_1.c
     ORDER BY g_1.Heure
END
' 
END
GO
/****** Object:  StoredProcedure [spTransDayColPay]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spTransDayColPay]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spTransDayColPay]
	-- Add the parameters for the stored procedure here
	  @date1 VARCHAR(50),
	  @key INTEGER
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@key =0)
	BEGIN
	WITH g AS (SELECT        DATEPART(HOUR, DateOperation) AS Heure, SUM(Debit) AS c
                         FROM            dbo.TTransaction
                         WHERE        (CONVERT(DATE, DateOperation) = @date1) AND Credit=0
                         GROUP BY DATEPART(HOUR, DateOperation))
    SELECT        TOP (100) PERCENT g_1.Heure, g_1.c AS Total, SUM(g2.c) AS cTotal
     FROM            g AS g_1 INNER JOIN
                              g AS g2 ON g_1.Heure >= g2.Heure
     GROUP BY g_1.Heure, g_1.c
     ORDER BY g_1.Heure
	 END
	 ELSE
	 BEGIN
	 WITH g AS (SELECT        DATEPART(HOUR, DateOperation) AS Heure, SUM(Credit) AS c
                         FROM            dbo.TTransaction
                         WHERE        (CONVERT(DATE, DateOperation) = @date1) AND Debit=0
                         GROUP BY DATEPART(HOUR, DateOperation))
    SELECT        TOP (100) PERCENT g_1.Heure, g_1.c AS Total, SUM(g2.c) AS cTotal
     FROM            g AS g_1 INNER JOIN
                              g AS g2 ON g_1.Heure >= g2.Heure
     GROUP BY g_1.Heure, g_1.c
     ORDER BY g_1.Heure
	 	END
	
END
' 
END
GO
/****** Object:  StoredProcedure [spTransPie]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spTransPie]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spTransPie]
	-- Add the parameters for the stored procedure here
	  @date1 VARCHAR(50)
AS
DECLARE @All INTEGER
BEGIN
SELECT @All =SUM(Credit)
FROM           TTransaction INNER JOIN
               Commerciaux ON TTransaction.AgentId = Commerciaux.AgentId LEFT JOIN
               Zones ON Commerciaux.ZoneID = Zones.ID
WHERE CONVERT(DATE,DateOperation)=@date1 AND Debit=0

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT         Zones.ZoneName,SUM(Credit)/@All *100 Total
FROM           TTransaction INNER JOIN
               Commerciaux ON TTransaction.AgentId = Commerciaux.AgentId LEFT JOIN
               Zones ON Commerciaux.ZoneID = Zones.ID
WHERE CONVERT(DATE,DateOperation)=@date1 AND Debit=0
GROUP BY Zones.ZoneName
	
END
' 
END
GO
/****** Object:  StoredProcedure [spUpdatePosted]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spUpdatePosted]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [spUpdatePosted]
	-- Add the parameters for the stored procedure here
	  @id INTEGER
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE dbo.TTransaction
	SET posted=''true''
	WHERE id=@id

	UPDATE dbo.TTransactionUpload
	SET Posted=''true''
	WHERE id=@id
END
' 
END
GO
/****** Object:  StoredProcedure [spVoteAvailableRegion]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spVoteAvailableRegion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create  PROCEDURE [spVoteAvailableRegion]
	-- Add the parameters for the stored procedure here
	  --@date1 VARCHAR(50)
AS
DECLARE @All INTEGER
BEGIN
SELECT        Distinct( Regions.RegionName)
FROM            Districts INNER JOIN
                         Constituencies ON Districts.ID = Constituencies.DistrictID INNER JOIN
                         Regions ON Districts.RegionID = Regions.ID INNER JOIN
                         CollationEntries ON Constituencies.ConstituencyNo = CollationEntries.ConstituencyID
ORDER BY Regions.RegionName

END
' 
END
GO
/****** Object:  StoredProcedure [spVotePartybar]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spVotePartybar]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create  PROCEDURE [spVotePartybar]
	-- Add the parameters for the stored procedure here
	  --@date1 VARCHAR(50)
AS
DECLARE @All INTEGER
BEGIN
SELECT        Regions.RegionName, Parties.Abrev, ISNULL(SUM(CollationEntries.NumberOfVote), 0) AS Total
FROM            Regions INNER JOIN
                         Agents ON Regions.ID = Agents.RegionID RIGHT OUTER JOIN
                         CollationEntries RIGHT OUTER JOIN
                         Parties ON CollationEntries.PartyID = Parties.ID ON Agents.UserID = CollationEntries.AgentID
GROUP BY Regions.RegionName, Parties.Abrev
ORDER BY RegionName,Abrev

	
END
' 
END
GO
/****** Object:  StoredProcedure [spVotePartyRegionTransPie]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spVotePartyRegionTransPie]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [spVotePartyRegionTransPie]
	-- Add the parameters for the stored procedure here
	  @RegionName VARCHAR(50)
AS
DECLARE @All INTEGER
BEGIN
SELECT  @All= SUM([NumberOfVote])
FROM           CollationEntries INNER JOIN
               Agents ON CollationEntries.AgentID = Agents.UserID  RIGHT OUTER JOIN
               dbo.Regions ON Regions.ID = Agents.RegionID
			   where regions.RegionName = @RegionName

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT        Parties.PartyName, Parties.Abrev, ISNULL(SUM(CollationEntries.NumberOfVote) * 1.0 / @All * 100, 0) AS Total, Regions.RegionName
FROM            Parties RIGHT OUTER JOIN
                         CollationEntries LEFT OUTER JOIN
                         Regions INNER JOIN
                         Agents ON Regions.ID = Agents.RegionID ON CollationEntries.AgentID = Agents.UserID ON Parties.ID = CollationEntries.PartyID
WHERE        (regions.RegionName = @RegionName)
GROUP BY Parties.PartyName, Parties.Abrev, Regions.RegionName

	
END

' 
END
GO
/****** Object:  StoredProcedure [spVotePartyTransPie]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spVotePartyTransPie]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [spVotePartyTransPie]
	-- Add the parameters for the stored procedure here
	  --@date1 VARCHAR(50)
AS
DECLARE @All INTEGER
BEGIN
SELECT  @All= SUM([NumberOfVote])
FROM           CollationEntries INNER JOIN
               Agents ON CollationEntries.AgentID = Agents.UserID  RIGHT OUTER JOIN
               dbo.Regions ON Regions.ID = Agents.RegionID

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT        Parties.PartyName,Parties.Abrev, ISNULL(SUM(CollationEntries.NumberOfVote)*1.0/@All*100,0) AS Total
FROM            CollationEntries LEFT JOIN Parties ON CollationEntries.PartyID=Parties.ID
GROUP BY Parties.PartyName,Parties.Abrev

	
END
' 
END
GO
/****** Object:  StoredProcedure [spVotePerRegion]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spVotePerRegion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
CREATE  PROCEDURE [spVotePerRegion]
	-- Add the parameters for the stored procedure here
	  --@date1 VARCHAR(50)
AS
DECLARE @All INTEGER
BEGIN
SELECT        Regions.RegionName, COUNT(Constituencies.ConstituencyName) AS [Number of constituency], RTRIM(ISNULL(Parties.Abrev,''-'')) PARTY, ISNULL(SUM(CollationEntries.NumberOfVote),0) AS TOTAL
FROM            CollationEntries INNER JOIN
                         Agents ON CollationEntries.AgentID = Agents.UserID INNER JOIN
                         Constituencies ON Agents.ContituencyID = Constituencies.ID INNER JOIN
                         Parties ON CollationEntries.PartyID = Parties.ID RIGHT OUTER JOIN
                         Regions ON Agents.RegionID = Regions.ID
GROUP BY Regions.RegionName, Parties.Abrev
ORDER BY Regions.RegionName

	
END

' 
END
GO
/****** Object:  StoredProcedure [spVoteTransPie]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spVoteTransPie]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [spVoteTransPie]
	-- Add the parameters for the stored procedure here
	  --@date1 VARCHAR(50)
AS
DECLARE @All INTEGER
BEGIN
SELECT @All=SUM([NumberOfVote])
FROM           CollationEntries INNER JOIN
               Agents ON CollationEntries.AgentID = Agents.UserID LEFT JOIN
               dbo.Regions ON Regions.ID = Agents.RegionID


	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT        Regions.RegionName, ISNULL(SUM(CollationEntries.NumberOfVote)*1.0/ @All  ,0) AS Total
FROM            CollationEntries LEFT OUTER JOIN
                         Agents ON CollationEntries.AgentID = Agents.UserID RIGHT OUTER JOIN
                         Regions ON Regions.ID = Agents.RegionID
GROUP BY Regions.RegionName
	
END
' 
END
GO
/****** Object:  StoredProcedure [toRealign]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[toRealign]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [toRealign]
	-- Add the parameters for the stored procedure here
	  @AgentId varchar(50),
	  @date DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [AgentId]
      ,[ClientId]
      ,[Credit]
      ,[Debit]
      ,[SoldeOuverture]
      ,[SoldeFermeture]
      ,[Description]
      ,[Posted]
  FROM [SMSServers].[dbo].[TTransactionUpload]
  where agentiD = @AgentId AND CONVERT(date,DateOperation,10) = @date
  EXCEPT
  SELECT [AgentId]
      ,[ClientId]
      ,[Credit]
      ,[Debit]
      ,[SoldeOuverture]
      ,[SoldeFermeture]
      ,[Description]
      ,[Posted]
  FROM [SMSServers].[dbo].[TTransaction]
  where agentiD = @AgentId AND CONVERT(date,DateOperation,10) = @date
END

' 
END
GO
/****** Object:  StoredProcedure [Validations]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Validations]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Validations]
	-- Add the parameters for the stored procedure here
	  @date1 DATETIME,
	  @date2 DATETIME,
	  @UserID Varchar(60)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        PostedTansactions.PostingStamp, AspNetUsers.FullName, Commerciaux.AgentName, PostedTansactions.Numtrans, PostedTansactions.SPaiments, PostedTansactions.SCollectes, 
                         PostedTansactions.SPayable
FROM            PostedTansactions LEFT OUTER JOIN
                         Commerciaux ON PostedTansactions.AgentInfo = Commerciaux.AgentName LEFT OUTER JOIN
                         AspNetUsers ON PostedTansactions.UserID = AspNetUsers.UserName
						 where PostedTansactions.UserID=@UserID and (PostedTansactions.PostingStamp between @date1 and @date2 )
						  order by PostedTansactions.PostingStamp
END
' 
END
GO
/****** Object:  Table [__MigrationHistory]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[__MigrationHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [AgentAssignClient]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AgentAssignClient]') AND type in (N'U'))
BEGIN
CREATE TABLE [AgentAssignClient](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[AgentId] [varchar](5) NOT NULL,
	[ClientId] [varchar](10) NOT NULL,
	[DateAssignee] [datetime] NOT NULL,
 CONSTRAINT [PK_AgentAssignClient] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Agents]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Agents]') AND type in (N'U'))
BEGIN
CREATE TABLE [Agents](
	[UserID] [nvarchar](128) NOT NULL,
	[RegionID] [int] NOT NULL,
	[DistrictID] [int] NOT NULL,
	[ContituencyID] [int] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[PollingStationID] [int] NOT NULL,
 CONSTRAINT [PK_Agents_1] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [AspNetRoles]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AspNetRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [AspNetUserClaims]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AspNetUserClaims]') AND type in (N'U'))
BEGIN
CREATE TABLE [AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[User_Id] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [AspNetUserLogins]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AspNetUserLogins]') AND type in (N'U'))
BEGIN
CREATE TABLE [AspNetUserLogins](
	[UserId] [nvarchar](128) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [AspNetUserRoles]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AspNetUserRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [AspNetUsers]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AspNetUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [AuditRecords]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AuditRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [AuditRecords](
	[AuditID] [uniqueidentifier] NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[IPAddress] [varchar](50) NOT NULL,
	[AreaAccessed] [varchar](max) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_AuditRecords] PRIMARY KEY CLUSTERED 
(
	[AuditID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Clients]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Clients]') AND type in (N'U'))
BEGIN
CREATE TABLE [Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [varchar](10) NULL,
	[ClientTel] [varchar](50) NULL,
	[Mise] [float] NULL,
	[Solde] [float] NULL,
	[Name] [varchar](50) NULL,
	[Sexe] [varchar](10) NULL,
	[DateCreated] [datetime] NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ClientsUpload]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ClientsUpload]') AND type in (N'U'))
BEGIN
CREATE TABLE [ClientsUpload](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [varchar](10) NULL,
	[ClientTel] [varchar](50) NULL,
	[Mise] [float] NULL,
	[Solde] [float] NULL,
	[Name] [varchar](50) NULL,
	[Sexe] [varchar](10) NULL,
	[DateCreated] [datetime] NULL,
	[Posted] [bit] NULL,
 CONSTRAINT [PK_ClientsUpload] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [CollationEntries]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CollationEntries]') AND type in (N'U'))
BEGIN
CREATE TABLE [CollationEntries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AgentID] [nvarchar](max) NOT NULL,
	[ConstituencyID] [varchar](50) NOT NULL,
	[PartyID] [int] NOT NULL,
	[DateSend] [datetime] NOT NULL,
	[NumberOfVote] [int] NOT NULL,
	[PollingStationID] [int] NULL,
 CONSTRAINT [PK_CollationEntries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Commerciaux]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Commerciaux]') AND type in (N'U'))
BEGIN
CREATE TABLE [Commerciaux](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AgentId] [varchar](50) NOT NULL,
	[AgentPass] [varchar](50) NOT NULL,
	[AgentName] [varchar](50) NOT NULL,
	[AgentActif] [bit] NOT NULL,
	[AgentTel] [varchar](50) NOT NULL,
	[ZoneID] [int] NOT NULL,
 CONSTRAINT [PK_Commerciaux] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Constituencies]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Constituencies]') AND type in (N'U'))
BEGIN
CREATE TABLE [Constituencies](
	[ID] [int] NOT NULL,
	[ConstituencyName] [varchar](max) NOT NULL,
	[ConstituencyNo] [varchar](max) NOT NULL,
	[DistrictID] [int] NOT NULL,
 CONSTRAINT [PK_Constituencies] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Credits]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Credits]') AND type in (N'U'))
BEGIN
CREATE TABLE [Credits](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [varchar](10) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateDisbursed] [datetime] NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[ContractID] [varchar](50) NOT NULL,
	[Amount] [float] NOT NULL,
	[DateFirstPyt] [datetime] NOT NULL,
	[TypeID] [nchar](5) NOT NULL,
	[status] [bit] NOT NULL,
 CONSTRAINT [PK_Credits] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [CreditTypes]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CreditTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [CreditTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeID] [nchar](5) NOT NULL,
	[TypeName] [varchar](150) NOT NULL,
	[EcheanceID] [int] NOT NULL,
	[InterestRate] [float] NOT NULL,
	[Duration] [int] NOT NULL,
 CONSTRAINT [PK_CreditTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Districts]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Districts]') AND type in (N'U'))
BEGIN
CREATE TABLE [Districts](
	[ID] [int] NOT NULL,
	[DistrictName] [varchar](max) NOT NULL,
	[RegionID] [int] NOT NULL,
 CONSTRAINT [PK_Districts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Echeances]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Echeances]') AND type in (N'U'))
BEGIN
CREATE TABLE [Echeances](
	[EcheanceID] [int] IDENTITY(1,1) NOT NULL,
	[EcheanceName] [varchar](50) NOT NULL,
	[DPart] [varchar](50) NULL,
 CONSTRAINT [PK_Echeances] PRIMARY KEY CLUSTERED 
(
	[EcheanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ElectoralAreas]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ElectoralAreas]') AND type in (N'U'))
BEGIN
CREATE TABLE [ElectoralAreas](
	[ID] [int] NOT NULL,
	[ElectoralAreaName] [varchar](max) NOT NULL,
	[Constituency] [int] NOT NULL,
 CONSTRAINT [PK_ElectoralAreas] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [MessageIn]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MessageIn]') AND type in (N'U'))
BEGIN
CREATE TABLE [MessageIn](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SendTime] [datetime] NOT NULL,
	[ReceiveTime] [datetime] NULL,
	[MessageFrom] [nvarchar](80) NULL,
	[MessageTo] [nvarchar](80) NULL,
	[SMSC] [nvarchar](80) NULL,
	[MessageText] [nvarchar](max) NULL,
	[MessageType] [nvarchar](80) NULL,
	[MessagePDU] [nvarchar](max) NULL,
	[Gateway] [nvarchar](80) NULL,
	[UserId] [nvarchar](80) NULL,
	[Processed] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [MessageLog]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MessageLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [MessageLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SendTime] [datetime] NOT NULL,
	[ReceiveTime] [datetime] NULL,
	[StatusCode] [int] NULL,
	[StatusText] [nvarchar](80) NULL,
	[MessageTo] [nvarchar](80) NULL,
	[MessageFrom] [nvarchar](80) NULL,
	[MessageText] [nvarchar](max) NULL,
	[MessageType] [nvarchar](80) NULL,
	[MessageId] [nvarchar](80) NULL,
	[ErrorCode] [nvarchar](80) NULL,
	[ErrorText] [nvarchar](80) NULL,
	[Gateway] [nvarchar](80) NULL,
	[MessagePDU] [nvarchar](max) NULL,
	[UserId] [nvarchar](80) NULL,
	[UserInfo] [nvarchar](max) NULL,
	[Accounting] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [MessageOut]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MessageOut]') AND type in (N'U'))
BEGIN
CREATE TABLE [MessageOut](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MessageTo] [nvarchar](80) NULL,
	[MessageFrom] [nvarchar](80) NULL,
	[MessageText] [nvarchar](max) NULL,
	[MessageType] [nvarchar](80) NULL,
	[Gateway] [nvarchar](80) NULL,
	[UserId] [nvarchar](80) NULL,
	[UserInfo] [nvarchar](max) NULL,
	[Priority] [int] NULL,
	[Scheduled] [datetime] NULL,
	[IsRead] [bit] NOT NULL,
	[IsSent] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [Parties]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Parties]') AND type in (N'U'))
BEGIN
CREATE TABLE [Parties](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PartyName] [varchar](150) NOT NULL,
	[Abrev] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Parties] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [PollingStations]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PollingStations]') AND type in (N'U'))
BEGIN
CREATE TABLE [PollingStations](
	[ID] [int] NOT NULL,
	[PollingStationNo] [varchar](max) NOT NULL,
	[PollingStationName] [varchar](max) NOT NULL,
	[RegisteredVoters] [int] NULL,
	[TransferedAway] [int] NULL,
	[Transfered_into] [int] NULL,
	[ElectoralAreaID] [int] NULL,
 CONSTRAINT [PK_PollingStations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [PostedTansactions]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PostedTansactions]') AND type in (N'U'))
BEGIN
CREATE TABLE [PostedTansactions](
	[PostingID] [int] IDENTITY(1,1) NOT NULL,
	[PostingStamp] [datetime] NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[AgentInfo] [varchar](50) NOT NULL,
	[Numtrans] [int] NOT NULL,
	[SPaiments] [decimal](18, 2) NOT NULL,
	[SCollectes] [decimal](18, 2) NOT NULL,
	[SPayable] [decimal](18, 2) NOT NULL,
	[SNouveauClient] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_PostedTansactions] PRIMARY KEY CLUSTERED 
(
	[PostingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Regions]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Regions]') AND type in (N'U'))
BEGIN
CREATE TABLE [Regions](
	[ID] [int] NOT NULL,
	[RegionName] [varchar](50) NOT NULL,
	[Symbol] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Regions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Settings]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Settings]') AND type in (N'U'))
BEGIN
CREATE TABLE [Settings](
	[HasManualEntry] [bit] NOT NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [TerminalAssigned]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TerminalAssigned]') AND type in (N'U'))
BEGIN
CREATE TABLE [TerminalAssigned](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TerminalID] [int] NOT NULL,
	[AgentID] [int] NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[DateAssigned] [datetime] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [Terminals]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Terminals]') AND type in (N'U'))
BEGIN
CREATE TABLE [Terminals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TerminalID] [nchar](100) NOT NULL,
	[Type] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[EMEI] [nchar](40) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [TerminalTypes]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TerminalTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [TerminalTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Tester]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Tester]') AND type in (N'U'))
BEGIN
CREATE TABLE [Tester](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Variables] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Tester] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [TTransaction]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TTransaction]') AND type in (N'U'))
BEGIN
CREATE TABLE [TTransaction](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[AgentId] [varchar](5) NOT NULL,
	[ClientId] [varchar](10) NOT NULL,
	[Credit] [float] NOT NULL,
	[Debit] [float] NOT NULL,
	[SoldeOuverture] [float] NOT NULL,
	[SoldeFermeture] [float] NOT NULL,
	[Description] [float] NOT NULL,
	[DateOperation] [datetime] NOT NULL,
	[Posted] [bit] NULL,
 CONSTRAINT [PK_TTransaction] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [TTransactionUpload]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TTransactionUpload]') AND type in (N'U'))
BEGIN
CREATE TABLE [TTransactionUpload](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[AgentId] [varchar](5) NULL,
	[ClientId] [varchar](10) NULL,
	[Credit] [float] NULL,
	[Debit] [float] NULL,
	[SoldeOuverture] [float] NULL,
	[SoldeFermeture] [float] NULL,
	[Description] [float] NULL,
	[DateOperation] [datetime] NULL,
	[Posted] [bit] NULL,
 CONSTRAINT [PK_TTransaction1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [UserProfile]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[UserProfile]') AND type in (N'U'))
BEGIN
CREATE TABLE [UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](56) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [webpages_Membership]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[webpages_Membership]') AND type in (N'U'))
BEGIN
CREATE TABLE [webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [webpages_OAuthMembership]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[webpages_OAuthMembership]') AND type in (N'U'))
BEGIN
CREATE TABLE [webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider] ASC,
	[ProviderUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [webpages_Roles]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[webpages_Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [webpages_UsersInRoles]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[webpages_UsersInRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [Zones]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Zones]') AND type in (N'U'))
BEGIN
CREATE TABLE [Zones](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ZoneID] [varchar](50) NOT NULL,
	[ZoneName] [varchar](150) NOT NULL,
 CONSTRAINT [PK_Zones] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [vConstituencies]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vConstituencies]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [vConstituencies]
AS
SELECT        dbo.Constituencies.ID, dbo.Constituencies.ConstituencyName, dbo.Constituencies.ConstituencyNo, dbo.Constituencies.DistrictID, dbo.Regions.ID AS RegionID
FROM            dbo.Constituencies INNER JOIN
                         dbo.Districts ON dbo.Constituencies.DistrictID = dbo.Districts.ID INNER JOIN
                         dbo.Regions ON dbo.Districts.RegionID = dbo.Regions.ID
WHERE        (dbo.Constituencies.ConstituencyNo NOT IN
                             (SELECT        ConstituencyID
                               FROM            dbo.CollationEntries))
' 
GO
/****** Object:  View [vTransactions]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vTransactions]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [vTransactions]
AS
SELECT        TOP (100) PERCENT dbo.TTransaction.DateOperation AS Date, dbo.Commerciaux.AgentName AS [Nom Commercial], dbo.Clients.Name AS [Nom Client], dbo.TTransaction.SoldeOuverture AS [Solde Ouverture], 
                         dbo.TTransaction.Debit, dbo.TTransaction.Credit, dbo.TTransaction.SoldeFermeture AS [Solde Fermeture], dbo.TTransaction.id, dbo.TTransaction.AgentId
FROM            dbo.TTransaction LEFT OUTER JOIN
                         dbo.Commerciaux ON dbo.TTransaction.AgentId = dbo.Commerciaux.AgentId LEFT OUTER JOIN
                         dbo.Clients ON dbo.TTransaction.ClientId = dbo.Clients.ClientId
ORDER BY Date
' 
GO
/****** Object:  View [vTransDay]    Script Date: 5/7/2015 12:35:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vTransDay]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [vTransDay]
AS
WITH g AS (SELECT        DATEPART(HOUR, DateOperation) AS Heure, COUNT(*) AS c
                         FROM            dbo.TTransaction
                         GROUP BY DATEPART(HOUR, DateOperation))
    SELECT        TOP (100) PERCENT g_1.Heure, g_1.c AS Total, SUM(g2.c) AS cTotal
     FROM            g AS g_1 INNER JOIN
                              g AS g2 ON g_1.Heure >= g2.Heure
     GROUP BY g_1.Heure, g_1.c
     ORDER BY g_1.Heure
' 
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_User_Id]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[AspNetUserClaims]') AND name = N'IX_User_Id')
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_Id] ON [AspNetUserClaims]
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[AspNetUserLogins]') AND name = N'IX_UserId')
CREATE NONCLUSTERED INDEX [IX_UserId] ON [AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_RoleId]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[AspNetUserRoles]') AND name = N'IX_RoleId')
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[AspNetUserRoles]') AND name = N'IX_UserId')
CREATE NONCLUSTERED INDEX [IX_UserId] ON [AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_CreditTypes]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CreditTypes]') AND name = N'IX_CreditTypes')
CREATE UNIQUE NONCLUSTERED INDEX [IX_CreditTypes] ON [CreditTypes]
(
	[TypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_MessageId]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[MessageLog]') AND name = N'IDX_MessageId')
CREATE NONCLUSTERED INDEX [IDX_MessageId] ON [MessageLog]
(
	[MessageId] ASC,
	[SendTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IDX_IsRead]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[MessageOut]') AND name = N'IDX_IsRead')
CREATE NONCLUSTERED INDEX [IDX_IsRead] ON [MessageOut]
(
	[IsRead] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Terminal_EMEI]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Terminals]') AND name = N'IX_Terminal_EMEI')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Terminal_EMEI] ON [Terminals]
(
	[EMEI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TerminalType_Type]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[TerminalTypes]') AND name = N'IX_TerminalType_Type')
CREATE UNIQUE NONCLUSTERED INDEX [IX_TerminalType_Type] ON [TerminalTypes]
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Zones]    Script Date: 5/7/2015 12:35:17 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Zones]') AND name = N'IX_Zones')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Zones] ON [Zones]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__MessageIn__SendT__145C0A3F]') AND type = 'D')
BEGIN
ALTER TABLE [MessageIn] ADD  DEFAULT (getdate()) FOR [SendTime]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_MessageIn_Processed]') AND type = 'D')
BEGIN
ALTER TABLE [MessageIn] ADD  CONSTRAINT [DF_MessageIn_Processed]  DEFAULT ((0)) FOR [Processed]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__MessageLo__SendT__173876EA]') AND type = 'D')
BEGIN
ALTER TABLE [MessageLog] ADD  DEFAULT (getdate()) FOR [SendTime]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__MessageOu__IsRea__108B795B]') AND type = 'D')
BEGIN
ALTER TABLE [MessageOut] ADD  DEFAULT ((0)) FOR [IsRead]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__MessageOu__IsSen__117F9D94]') AND type = 'D')
BEGIN
ALTER TABLE [MessageOut] ADD  DEFAULT ((0)) FOR [IsSent]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__webpages___IsCon__40F9A68C]') AND type = 'D')
BEGIN
ALTER TABLE [webpages_Membership] ADD  DEFAULT ((0)) FOR [IsConfirmed]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__webpages___Passw__41EDCAC5]') AND type = 'D')
BEGIN
ALTER TABLE [webpages_Membership] ADD  DEFAULT ((0)) FOR [PasswordFailuresSinceLastSuccess]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_AgentAssignClient_AgentAssignClient]') AND parent_object_id = OBJECT_ID(N'[AgentAssignClient]'))
ALTER TABLE [AgentAssignClient]  WITH CHECK ADD  CONSTRAINT [FK_AgentAssignClient_AgentAssignClient] FOREIGN KEY([id])
REFERENCES [AgentAssignClient] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_AgentAssignClient_AgentAssignClient]') AND parent_object_id = OBJECT_ID(N'[AgentAssignClient]'))
ALTER TABLE [AgentAssignClient] CHECK CONSTRAINT [FK_AgentAssignClient_AgentAssignClient]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Agents_AspNetUsers]') AND parent_object_id = OBJECT_ID(N'[Agents]'))
ALTER TABLE [Agents]  WITH CHECK ADD  CONSTRAINT [FK_Agents_AspNetUsers] FOREIGN KEY([UserID])
REFERENCES [AspNetUsers] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Agents_AspNetUsers]') AND parent_object_id = OBJECT_ID(N'[Agents]'))
ALTER TABLE [Agents] CHECK CONSTRAINT [FK_Agents_AspNetUsers]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Agents_Constituencies]') AND parent_object_id = OBJECT_ID(N'[Agents]'))
ALTER TABLE [Agents]  WITH CHECK ADD  CONSTRAINT [FK_Agents_Constituencies] FOREIGN KEY([ContituencyID])
REFERENCES [Constituencies] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Agents_Constituencies]') AND parent_object_id = OBJECT_ID(N'[Agents]'))
ALTER TABLE [Agents] CHECK CONSTRAINT [FK_Agents_Constituencies]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Agents_Districts]') AND parent_object_id = OBJECT_ID(N'[Agents]'))
ALTER TABLE [Agents]  WITH CHECK ADD  CONSTRAINT [FK_Agents_Districts] FOREIGN KEY([DistrictID])
REFERENCES [Districts] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Agents_Districts]') AND parent_object_id = OBJECT_ID(N'[Agents]'))
ALTER TABLE [Agents] CHECK CONSTRAINT [FK_Agents_Districts]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Agents_PollingStations]') AND parent_object_id = OBJECT_ID(N'[Agents]'))
ALTER TABLE [Agents]  WITH CHECK ADD  CONSTRAINT [FK_Agents_PollingStations] FOREIGN KEY([PollingStationID])
REFERENCES [PollingStations] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Agents_PollingStations]') AND parent_object_id = OBJECT_ID(N'[Agents]'))
ALTER TABLE [Agents] CHECK CONSTRAINT [FK_Agents_PollingStations]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Agents_Regions]') AND parent_object_id = OBJECT_ID(N'[Agents]'))
ALTER TABLE [Agents]  WITH CHECK ADD  CONSTRAINT [FK_Agents_Regions] FOREIGN KEY([RegionID])
REFERENCES [Regions] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Agents_Regions]') AND parent_object_id = OBJECT_ID(N'[Agents]'))
ALTER TABLE [Agents] CHECK CONSTRAINT [FK_Agents_Regions]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id]') AND parent_object_id = OBJECT_ID(N'[AspNetUserClaims]'))
ALTER TABLE [AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id] FOREIGN KEY([User_Id])
REFERENCES [AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id]') AND parent_object_id = OBJECT_ID(N'[AspNetUserClaims]'))
ALTER TABLE [AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[AspNetUserLogins]'))
ALTER TABLE [AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[AspNetUserLogins]'))
ALTER TABLE [AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[AspNetUserRoles]'))
ALTER TABLE [AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [AspNetRoles] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[AspNetUserRoles]'))
ALTER TABLE [AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[AspNetUserRoles]'))
ALTER TABLE [AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[AspNetUserRoles]'))
ALTER TABLE [AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Constituencies_Districts]') AND parent_object_id = OBJECT_ID(N'[Constituencies]'))
ALTER TABLE [Constituencies]  WITH CHECK ADD  CONSTRAINT [FK_Constituencies_Districts] FOREIGN KEY([DistrictID])
REFERENCES [Districts] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Constituencies_Districts]') AND parent_object_id = OBJECT_ID(N'[Constituencies]'))
ALTER TABLE [Constituencies] CHECK CONSTRAINT [FK_Constituencies_Districts]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Credits_AspNetUsers]') AND parent_object_id = OBJECT_ID(N'[Credits]'))
ALTER TABLE [Credits]  WITH CHECK ADD  CONSTRAINT [FK_Credits_AspNetUsers] FOREIGN KEY([UserID])
REFERENCES [AspNetUsers] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Credits_AspNetUsers]') AND parent_object_id = OBJECT_ID(N'[Credits]'))
ALTER TABLE [Credits] CHECK CONSTRAINT [FK_Credits_AspNetUsers]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_CreditTypes_Echeances]') AND parent_object_id = OBJECT_ID(N'[CreditTypes]'))
ALTER TABLE [CreditTypes]  WITH CHECK ADD  CONSTRAINT [FK_CreditTypes_Echeances] FOREIGN KEY([EcheanceID])
REFERENCES [Echeances] ([EcheanceID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_CreditTypes_Echeances]') AND parent_object_id = OBJECT_ID(N'[CreditTypes]'))
ALTER TABLE [CreditTypes] CHECK CONSTRAINT [FK_CreditTypes_Echeances]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Districts_Regions]') AND parent_object_id = OBJECT_ID(N'[Districts]'))
ALTER TABLE [Districts]  WITH CHECK ADD  CONSTRAINT [FK_Districts_Regions] FOREIGN KEY([RegionID])
REFERENCES [Regions] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Districts_Regions]') AND parent_object_id = OBJECT_ID(N'[Districts]'))
ALTER TABLE [Districts] CHECK CONSTRAINT [FK_Districts_Regions]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_ElectoralAreas_Constituencies]') AND parent_object_id = OBJECT_ID(N'[ElectoralAreas]'))
ALTER TABLE [ElectoralAreas]  WITH CHECK ADD  CONSTRAINT [FK_ElectoralAreas_Constituencies] FOREIGN KEY([Constituency])
REFERENCES [Constituencies] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_ElectoralAreas_Constituencies]') AND parent_object_id = OBJECT_ID(N'[ElectoralAreas]'))
ALTER TABLE [ElectoralAreas] CHECK CONSTRAINT [FK_ElectoralAreas_Constituencies]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_PollingStations_ElectoralAreas]') AND parent_object_id = OBJECT_ID(N'[PollingStations]'))
ALTER TABLE [PollingStations]  WITH CHECK ADD  CONSTRAINT [FK_PollingStations_ElectoralAreas] FOREIGN KEY([ElectoralAreaID])
REFERENCES [ElectoralAreas] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_PollingStations_ElectoralAreas]') AND parent_object_id = OBJECT_ID(N'[PollingStations]'))
ALTER TABLE [PollingStations] CHECK CONSTRAINT [FK_PollingStations_ElectoralAreas]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_TerminalAssigned_AspNetUsers]') AND parent_object_id = OBJECT_ID(N'[TerminalAssigned]'))
ALTER TABLE [TerminalAssigned]  WITH CHECK ADD  CONSTRAINT [FK_TerminalAssigned_AspNetUsers] FOREIGN KEY([UserID])
REFERENCES [AspNetUsers] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_TerminalAssigned_AspNetUsers]') AND parent_object_id = OBJECT_ID(N'[TerminalAssigned]'))
ALTER TABLE [TerminalAssigned] CHECK CONSTRAINT [FK_TerminalAssigned_AspNetUsers]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_TerminalAssigned_Commerciaux]') AND parent_object_id = OBJECT_ID(N'[TerminalAssigned]'))
ALTER TABLE [TerminalAssigned]  WITH CHECK ADD  CONSTRAINT [FK_TerminalAssigned_Commerciaux] FOREIGN KEY([AgentID])
REFERENCES [Commerciaux] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_TerminalAssigned_Commerciaux]') AND parent_object_id = OBJECT_ID(N'[TerminalAssigned]'))
ALTER TABLE [TerminalAssigned] CHECK CONSTRAINT [FK_TerminalAssigned_Commerciaux]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_TerminalAssigned_Terminals]') AND parent_object_id = OBJECT_ID(N'[TerminalAssigned]'))
ALTER TABLE [TerminalAssigned]  WITH CHECK ADD  CONSTRAINT [FK_TerminalAssigned_Terminals] FOREIGN KEY([TerminalID])
REFERENCES [Terminals] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_TerminalAssigned_Terminals]') AND parent_object_id = OBJECT_ID(N'[TerminalAssigned]'))
ALTER TABLE [TerminalAssigned] CHECK CONSTRAINT [FK_TerminalAssigned_Terminals]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Terminals_TerminalTypes]') AND parent_object_id = OBJECT_ID(N'[Terminals]'))
ALTER TABLE [Terminals]  WITH CHECK ADD  CONSTRAINT [FK_Terminals_TerminalTypes] FOREIGN KEY([Type])
REFERENCES [TerminalTypes] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Terminals_TerminalTypes]') AND parent_object_id = OBJECT_ID(N'[Terminals]'))
ALTER TABLE [Terminals] CHECK CONSTRAINT [FK_Terminals_TerminalTypes]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[fk_RoleId]') AND parent_object_id = OBJECT_ID(N'[webpages_UsersInRoles]'))
ALTER TABLE [webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_RoleId] FOREIGN KEY([RoleId])
REFERENCES [webpages_Roles] ([RoleId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[fk_RoleId]') AND parent_object_id = OBJECT_ID(N'[webpages_UsersInRoles]'))
ALTER TABLE [webpages_UsersInRoles] CHECK CONSTRAINT [fk_RoleId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[fk_UserId]') AND parent_object_id = OBJECT_ID(N'[webpages_UsersInRoles]'))
ALTER TABLE [webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_UserId] FOREIGN KEY([UserId])
REFERENCES [UserProfile] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[fk_UserId]') AND parent_object_id = OBJECT_ID(N'[webpages_UsersInRoles]'))
ALTER TABLE [webpages_UsersInRoles] CHECK CONSTRAINT [fk_UserId]
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'vConstituencies', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[8] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Constituencies"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 229
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Regions"
            Begin Extent = 
               Top = 6
               Left = 267
               Bottom = 119
               Right = 437
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Districts"
            Begin Extent = 
               Top = 6
               Left = 475
               Bottom = 119
               Right = 645
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vConstituencies'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'vConstituencies', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vConstituencies'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'vTransactions', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[34] 4[4] 2[29] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TTransaction"
            Begin Extent = 
               Top = 8
               Left = 227
               Bottom = 138
               Right = 399
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Clients"
            Begin Extent = 
               Top = 12
               Left = 10
               Bottom = 142
               Right = 180
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "Commerciaux"
            Begin Extent = 
               Top = 0
               Left = 463
               Bottom = 130
               Right = 633
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 2025
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vTransactions'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'vTransactions', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vTransactions'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'vTransDay', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[37] 2[4] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "g_1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "g2"
            Begin Extent = 
               Top = 6
               Left = 262
               Bottom = 102
               Right = 448
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vTransDay'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'vTransDay', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vTransDay'
GO
USE [master]
GO
ALTER DATABASE [SMSServers] SET  READ_WRITE 
GO
