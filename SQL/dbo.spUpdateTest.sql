CREATE PROCEDURE [dbo].[spUpdateTest]
	@testID int,
	@name nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON
	UPDATE Tests
	SET Name = @name
	WHERE TestID = @testID
END