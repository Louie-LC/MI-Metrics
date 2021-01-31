CREATE PROCEDURE [dbo].[spInsertTestStep]
	@testStepID int,
	@step nvarchar(12),
	@mStep nvarchar(3),
	@subStep nvarchar(3),
	@lowerLimit float,
	@upperLimit float,
	@controlFileID int,
	@testID int
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO TestSteps (TestStepID, Step, MStep, Substep, LowerLimit, UpperLimit, ControlFileID, TestID)
	VALUES (@testStepID, @step, @mStep, @subStep, @lowerLimit, @upperLimit, @controlFileID, @testID);
END