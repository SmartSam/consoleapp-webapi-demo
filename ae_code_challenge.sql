
--IF NOT EXISTS(SELECT 1 FROM sys.databases WHERE name='ae_code_chellange')
--    CREATE DATABASE [ae_code_challenge]
--GO
USE ae_code_challenge
GO
if not exists (select * from sysobjects where name='server_response_log' and xtype='U')
BEGIN
CREATE TABLE server_response_log(
 Starttime DateTime,--  time when the request was sent to the server
 Endtime DateTime, -- time when a response was received from the server (or a timeout occurred)
 HTTPStatus tinyint, -- response HTTP Status Code (if available)
 ResponseText varchar(255), -- Response received from the server as a string (ASCII Encoding should be fine)
 ErrorCode smallint  --( 1 -> HTTP StatusCode 200 response received from the server, 2 -> Another HTTP StatusCode received from the server, -999 timeout)
);
END

