USE [Spartan]
GO

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


