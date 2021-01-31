CREATE PROCEDURE [dbo].[spGetNextControlFileID]
AS 
BEGIN
	SET NOCOUNT ON
    SELECT NEXT VALUE FOR dbo.seqControlFileID;
END