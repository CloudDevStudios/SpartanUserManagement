USE [Spartan]
GO

DROP TABLE [dbo].[Users]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[RowId] [uniqueidentifier] NOT NULL,
	[UpdateId] [uniqueidentifier],
	[AppName] [nvarchar](30) NULL,
	[UserName] [nvarchar](10) NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](20) NULL,
	[Company] [nvarchar](50) NULL,
	[GivenName] [nvarchar](30) NULL,
	[MiddleName] [nvarchar](30) NULL,
	[Surname] [nvarchar](30) NULL,
	[FullName] [nvarchar](100) NULL,
	[NickName][nvarchar](100) NULL,
	[Gender] [nvarchar](10) NULL,
	[MaritalStatus] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NOT NULL,
	[EmailSignature] [nvarchar](100) NULL,
	[EmailProvider] [nvarchar](20) NULL,
	[JobTitle] [nvarchar](50) NULL,
	[BusinessPhone] [nvarchar](15) NULL,
	[HomePhone] [nvarchar](15) NULL,
	[MobilePhone] [nvarchar](15) NULL,
	[FaxNumber] [nvarchar](15) NULL,
	[Address] [nvarchar](100) NULL,
	[Address1] [nvarchar](100) NULL,
	[City] [nvarchar](20) NULL,
	[State] [nvarchar](20) NULL,
	[Province] [nvarchar](30) NULL,
	[ZipCode] [nvarchar](10) NULL,
	[Country] [nvarchar](50) NULL,
	[CountryOrigin] [nvarchar](100) NULL,
	[Citizenship] [nvarchar](100) NULL,
	[WebPage] [nvarchar](400) NULL,
	[Avatar] [nvarchar](400) NULL,
	[About] [nvarchar](400) NULL,
	[DoB] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[AccessFailedCount] [smallint] NOT NULL,
	[LockEnabled] [bit] NOT NULL,
	[LockoutDescription] [nvarchar](400) NULL,
	[AccountNotes] [nvarchar](400) NULL,
	[ReportsToId] [uniqueidentifier] NULL,
	[DateCreated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

--002
IF  EXISTS (SELECT * FROM sys .objects WHERE object_id = OBJECT_ID(N'[dbo].[User_InsertOrUpdate]' ) AND type in ( N'P', N'PC'))
DROP PROCEDURE [dbo]. User_InsertOrUpdate
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[User_InsertOrUpdate]
@Id uniqueidentifier,
@AppName nvarchar(30),
@UserName nvarchar(10),
@PasswordHash nvarchar(max),
@Type nvarchar(20),
@Company nvarchar(50),
@GivenName nvarchar(30),
@MiddleName nvarchar(30),
@Surname nvarchar(30),
@FullName nvarchar(100),
@NickName nvarchar(100),
@Gender nvarchar(10),
@MaritalStatus nvarchar(20),
@Email nvarchar(50),
@EmailSignature nvarchar(100),
@EmailProvider nvarchar(20),
@JobTitle nvarchar(50),
@BusinessPhone nvarchar(15),
@HomePhone nvarchar(15),
@MobilePhone nvarchar(15),
@FaxNumber nvarchar(15),
@Address nvarchar(100),
@Address1 nvarchar(100),
@City nvarchar(20),
@State nvarchar(20),
@Province nvarchar(30),
@ZipCode nvarchar(10),
@Country nvarchar(50),
@CountryOrigin nvarchar(100),
@Citizenship nvarchar(100),
@WebPage nvarchar(400),
@Avatar nvarchar(400),
@About nvarchar(400),
@DoB datetime2(7),
@IsActive bit,
@AccessFailedCount smallint,
@LockEnabled bit,
@LockoutDescription nvarchar(400),
@AccountNotes nvarchar(400),
@ReportsToId uniqueidentifier,
@DateCreated datetime2(7)

AS
DECLARE @newID uniqueidentifier  
SET @newID = NEWID()  

UPDATE Users SET UpdateId = @newID WHERE Id = @Id AND updateid IS NULL;

INSERT INTO [dbo].[Users]
           ([Id]
           ,[RowId]
           ,[UpdateId]
           ,[AppName]
           ,[UserName]
           ,[PasswordHash]
           ,[Type]
           ,[Company]
           ,[GivenName]
           ,[MiddleName]
           ,[Surname]
           ,[FullName]
           ,[NickName]
           ,[Gender]
           ,[MaritalStatus]
           ,[Email]
           ,[EmailSignature]
           ,[EmailProvider]
           ,[JobTitle]
           ,[BusinessPhone]
           ,[HomePhone]
           ,[MobilePhone]
           ,[FaxNumber]
           ,[Address]
           ,[Address1]
           ,[City]
           ,[State]
           ,[Province]
           ,[ZipCode]
           ,[Country]
           ,[CountryOrigin]
           ,[Citizenship]
           ,[WebPage]
           ,[Avatar]
           ,[About]
           ,[DoB]
           ,[IsActive]
           ,[AccessFailedCount]
           ,[LockEnabled]
           ,[LockoutDescription]
           ,[AccountNotes]
		   ,[ReportsToId]
           ,[DateCreated])
     VALUES
           (
				@Id,
				@newID,
				NULL,
				@AppName,
				@UserName,
				@PasswordHash,
				@Type,
				@Company,
				@GivenName,
				@MiddleName,
				@Surname,
				@FullName,
				@NickName,
				@Gender,
				@MaritalStatus,
				@Email,
				@EmailSignature,
				@EmailProvider,
				@JobTitle,
				@BusinessPhone,
				@HomePhone,
				@MobilePhone,
				@FaxNumber,
				@Address,
				@Address1,
				@City,
				@State,
				@Province,
				@ZipCode,
				@Country,
				@CountryOrigin,
				@Citizenship,
				@WebPage,
				@Avatar,
				@About,
				@DoB,
				@IsActive,
				@AccessFailedCount,
				@LockEnabled,
				@LockoutDescription,
				@AccountNotes,
				@ReportsToId,
				@DateCreated
		   )
GO

--03
IF  EXISTS (SELECT * FROM sys .objects WHERE object_id = OBJECT_ID(N'[dbo].[User_ById]' ) AND type in ( N'P', N'PC'))
DROP PROCEDURE [dbo]. User_ById
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[User_ById]
@Id uniqueidentifier

AS
SELECT [Id]
      ,[RowId]
      ,[UpdateId]
      ,[AppName]
      ,[UserName]
      ,[PasswordHash]
      ,[Type]
      ,[Company]
      ,[GivenName]
      ,[MiddleName]
      ,[Surname]
      ,[FullName]
      ,[NickName]
      ,[Gender]
      ,[MaritalStatus]
      ,[Email]
      ,[EmailSignature]
      ,[EmailProvider]
      ,[JobTitle]
      ,[BusinessPhone]
      ,[HomePhone]
      ,[MobilePhone]
      ,[FaxNumber]
      ,[Address]
      ,[Address1]
      ,[City]
      ,[State]
      ,[Province]
      ,[ZipCode]
      ,[Country]
      ,[CountryOrigin]
      ,[Citizenship]
      ,[WebPage]
      ,[Avatar]
      ,[About]
      ,[DoB]
      ,[IsActive]
      ,[AccessFailedCount]
      ,[LockEnabled]
      ,[LockoutDescription]
	  ,[AccountNotes]
      ,[ReportsToId]
      ,[DateCreated]
  FROM [Spartan].[dbo].[Users]
  WHERE ID = @Id
  ORDER BY DateCreated DESC
  Go

--04
IF  EXISTS (SELECT * FROM sys .objects WHERE object_id = OBJECT_ID(N'[dbo].[User_ByUserName]' ) AND type in ( N'P', N'PC'))
DROP PROCEDURE [dbo].User_ByUserName
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[User_ByUserName]
@UserName nvarchar(10)

AS
SELECT [Id]
      ,[RowId]
      ,[UpdateId]
      ,[AppName]
      ,[UserName]
      ,[PasswordHash]
      ,[Type]
      ,[Company]
      ,[GivenName]
      ,[MiddleName]
      ,[Surname]
      ,[FullName]
      ,[NickName]
      ,[Gender]
      ,[MaritalStatus]
      ,[Email]
      ,[EmailSignature]
      ,[EmailProvider]
      ,[JobTitle]
      ,[BusinessPhone]
      ,[HomePhone]
      ,[MobilePhone]
      ,[FaxNumber]
      ,[Address]
      ,[Address1]
      ,[City]
      ,[State]
      ,[Province]
      ,[ZipCode]
      ,[Country]
      ,[CountryOrigin]
      ,[Citizenship]
      ,[WebPage]
      ,[Avatar]
      ,[About]
      ,[DoB]
      ,[IsActive]
      ,[AccessFailedCount]
      ,[LockEnabled]
      ,[LockoutDescription]
      ,[AccountNotes]
      ,[ReportsToId]
      ,[DateCreated]
  FROM [Spartan].[dbo].[Users]
  WHERE UserName = @UserName
  ORDER BY DateCreated DESC
  Go

--05
IF  EXISTS (SELECT * FROM sys .objects WHERE object_id = OBJECT_ID(N'[dbo].[User_ByEmail]' ) AND type in ( N'P', N'PC'))
DROP PROCEDURE [dbo].User_ByEmail
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[User_ByEmail]
@Email nvarchar(50)

AS
SELECT [Id]
      ,[RowId]
      ,[UpdateId]
      ,[AppName]
      ,[UserName]
      ,[PasswordHash]
      ,[Type]
      ,[Company]
      ,[GivenName]
      ,[MiddleName]
      ,[Surname]
      ,[FullName]
      ,[NickName]
      ,[Gender]
      ,[MaritalStatus]
      ,[Email]
      ,[EmailSignature]
      ,[EmailProvider]
      ,[JobTitle]
      ,[BusinessPhone]
      ,[HomePhone]
      ,[MobilePhone]
      ,[FaxNumber]
      ,[Address]
      ,[Address1]
      ,[City]
      ,[State]
      ,[Province]
      ,[ZipCode]
      ,[Country]
      ,[CountryOrigin]
      ,[Citizenship]
      ,[WebPage]
      ,[Avatar]
      ,[About]
      ,[DoB]
      ,[IsActive]
      ,[AccessFailedCount]
      ,[LockEnabled]
      ,[LockoutDescription]
      ,[AccountNotes]
      ,[ReportsToId]
      ,[DateCreated]
  FROM [Spartan].[dbo].[Users]
  WHERE Email = @Email
  ORDER BY DateCreated DESC
  Go

--06
IF  EXISTS (SELECT * FROM sys .objects WHERE object_id = OBJECT_ID(N'[dbo].[User_GetActiveUsers]' ) AND type in ( N'P', N'PC'))
DROP PROCEDURE [dbo].User_GetActiveUsers
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[User_GetActiveUsers]

AS

SELECT  [Id]
      ,[RowId]
      ,[UpdateId]
      ,[AppName]
      ,[UserName]
      ,[PasswordHash]
      ,[Type]
      ,[Company]
      ,[GivenName]
      ,[MiddleName]
      ,[Surname]
      ,[FullName]
      ,[NickName]
      ,[Gender]
      ,[MaritalStatus]
      ,[Email]
      ,[EmailSignature]
      ,[EmailProvider]
      ,[JobTitle]
      ,[BusinessPhone]
      ,[HomePhone]
      ,[MobilePhone]
      ,[FaxNumber]
      ,[Address]
      ,[Address1]
      ,[City]
      ,[State]
      ,[Province]
      ,[ZipCode]
      ,[Country]
      ,[CountryOrigin]
      ,[Citizenship]
      ,[WebPage]
      ,[Avatar]
      ,[About]
      ,[DoB]
      ,[IsActive]
      ,[AccessFailedCount]
      ,[LockEnabled]
      ,[LockoutDescription]
      ,[AccountNotes]
      ,[ReportsToId]
      ,[DateCreated]
  FROM [Spartan].[dbo].[Users]
  WHERE	IsActive = 1
  AND	LockEnabled = 0
  AND	UpdateId IS NULL
  ORDER BY DateCreated DESC

 --07

GO

DROP TABLE [dbo].[Roles]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Roles](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--08
DROP TABLE [dbo].[RoleUsers]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoleUsers](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_RoleUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--09

DROP TABLE [dbo].[Menus]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Menus](
	[Id] [uniqueidentifier] NOT NULL,
	[ReportsToId] [uniqueidentifier] NULL,
	[Text] [nvarchar](100) NOT NULL,
	[Route] [nvarchar](100) NULL,
	[ToolTip] [nvarchar](100) NULL,
	[Icon] [nvarchar](100) NULL,
	[Type] [nvarchar](100) NULL,
	[Badge] [nvarchar](100) NULL,
	[AliasNames] [nvarchar](200) NULL,
	[URL] [nvarchar](400) NULL,
	[SortOrder] [int] NULL,
	[isVisible] [bit] NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


--010


DROP TABLE [dbo].[MenuPermissions]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MenuPermissions](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[MenuId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[SortOrder] [int] NULL,
	[IsCreate] [bit] NULL,
	[IsRead] [bit] NULL,
	[IsUpdate] [bit] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_MenuPermissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
