CREATE PROCEDURE [dbo].[spSelectTagWithProductID]
	@productID int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TagID, Description, ProductID
	FROM Tags
	WHERE ProductID = @productID;
END