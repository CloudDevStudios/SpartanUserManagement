USE [Spartan]
GO

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
@AppSetting nvarchar(400),
@AppTheme nvarchar(400),
@UserName nvarchar(10),
@DisplayName nvarchar(50),
@PhotoUrl nvarchar(400),
@ShortCuts nvarchar(300),
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
@ApartmentNumber nvarchar(10),
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
           ,[AppSetting]
           ,[AppTheme]
           ,[UserName]
           ,[DisplayName]
           ,[PhotoUrl]
           ,[ShortCuts]
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
		   ,[ApartmentNumber]
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
				@AppSetting,
				@AppTheme,
				@UserName,
				@DisplayName,
				@PhotoUrl,
				@ShortCuts,
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
				@ApartmentNumber,
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
