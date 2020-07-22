
--IF NOT EXISTS(SELECT 1 FROM sys.databases WHERE name='ae_code_chellange')
--    CREATE DATABASE [ae_code_challenge]
--GO
USE ae_code_challenge
GO
if not exists (select * from sysobjects where name='server_response_log' and xtype='U')
BEGIN
CREATE TABLE [dbo].[server_response_log](
	[Starttime] [datetime] NULL,
	[Endtime] [datetime] NULL,
	[HTTPStatus] [smallint] NULL,
	[ResponseText] [varchar](255) NULL,
	[ErrorCode] [smallint] NULL
)

END

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'ClusteredIndex-20200721-191346' AND object_id = OBJECT_ID('server_response_log'))
    BEGIN
       CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20200721-191346] ON [dbo].[server_response_log]
(
	[Starttime] ASC,
	[Endtime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    END

	IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spMostRecentResponseLogs]') AND type in (N'P', N'PC'))
BEGIN 
execute ('CREATE PROCEDURE [dbo].[spMostRecentResponseLogs]  
		(@timespan int)  
		AS  
		BEGIN  
		SELECT TOP(5) Starttime, ResponseText
		FROM dbo.server_response_log
		WHERE DATEDIFF(second, GetDate(), Starttime) <= @timespan 
		ORDER BY Starttime desc
		END')
END

IF EXISTS(SELECT 1 FROM sys.views 
     WHERE Name = 'dbo.ServerResponseErrors')
BEGIN
execute ('CREATE VIEW dbo.ServerResponseErrors AS
		SELECT Count(ErrorCode)as ErrorCodeCount, Max(ErrorCode) as ErrorCode, MAX(FORMAT([Starttime], ''MMM dd yyyy, hh'')) as StartHour
		FROM server_response_log
		GROUP BY DATEPART(hour, [Starttime])')
END

	



