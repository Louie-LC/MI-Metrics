CREATE PROCEDURE [dbo].[spSelectTestStepWithControlFileID]
	@controlFileID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT TestStepID, Step, MStep, SubStep, LowerLimit, UpperLimit, ControlFileID, TestID
	FROM TestSteps
	WHERE ControlFileID = @controlFileID;
END