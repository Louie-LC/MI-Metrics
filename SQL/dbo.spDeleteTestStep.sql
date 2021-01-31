CREATE PROCEDURE [dbo].[spDeleteTestStep]
	@testStepID int
AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM TestSteps
	WHERE TestStepID = @testStepID
END