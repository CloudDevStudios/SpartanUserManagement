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


