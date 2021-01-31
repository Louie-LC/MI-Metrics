CREATE PROCEDURE [dbo].[spSelectTestResultWithDHRID]
	@deviceHistoryRecordID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT TestResultID, Value, Date, TestID, DeviceHistoryRecordID
	FROM TestResults
	WHERE DeviceHistoryRecordID = @deviceHistoryRecordID;
END