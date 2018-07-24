USE [Spartan]
GO

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
      ,[DateCreated]
  FROM [Spartan].[dbo].[Users]
  WHERE UserName = @UserName
  ORDER BY DateCreated DESC
  Go
