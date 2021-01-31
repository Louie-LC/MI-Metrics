CREATE PROCEDURE [dbo].[spUpdateTestResult]
	@testResultID bigint,
	@value float,
	@date date
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE TestResults 
	SET Value = @value, Date = @date
	WHERE TestResultID = @testResultID;
END