CREATE PROCEDURE [dbo].[spSelectTestWithProductID]
	@productID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT TestID, Name, ProductID
	FROM Tests
	WHERE ProductID = @productID;
END