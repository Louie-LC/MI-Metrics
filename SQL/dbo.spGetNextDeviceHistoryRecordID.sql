CREATE PROCEDURE [dbo].[spGetNextDeviceHistoryRecordID]
AS 
BEGIN
	SET NOCOUNT ON
    SELECT NEXT VALUE FOR dbo.seqDeviceHistoryRecordID;
END