CREATE PROCEDURE [dbo].[spInsertTest]
	@testID int,
	@name nvarchar(50),
	@productID int
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO Tests (TestID, Name, ProductID)
	VALUES (@testID, @name, @productID);
END