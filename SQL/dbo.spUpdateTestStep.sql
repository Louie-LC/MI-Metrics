CREATE PROCEDURE [dbo].[spUpdateTestStep]
	@testStepID int,
	@step nvarchar(12),
	@mStep nvarchar(3),
	@subStep nvarchar(3),
	@lowerLimit float,
	@upperLimit float
AS
BEGIN
	SET NOCOUNT ON
	UPDATE TestSteps
	SET Step = @step, MStep = @mStep, Substep = @subStep, LowerLimit = @lowerLimit, UpperLimit = @upperLimit
	WHERE TestStepID = @testStepID;
END