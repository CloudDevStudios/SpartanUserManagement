USE [Spartan]
GO

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


