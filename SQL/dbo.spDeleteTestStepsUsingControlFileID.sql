CREATE PROCEDURE [dbo].[spDeleteTestStepsUsingControlFileID]
	@controlFileID int
AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM TestSteps
	WHERE ControlFileID = @controlFileID
END