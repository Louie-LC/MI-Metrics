CREATE PROCEDURE [dbo].[spInsertTestResult]
	@testResultID bigint,
	@value float,
	@date date,
	@testID int,
	@deviceHistoryRecordID int
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO TestResults (TestResultID, Value, Date, TestID, DeviceHistoryRecordID)
	VALUES (@testResultID, @value, @date, @testID, @deviceHistoryRecordID);
END