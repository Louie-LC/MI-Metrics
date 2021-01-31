CREATE PROCEDURE dbo.spSelectProduct
	@productID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT ProductID, Name, ImagePath
	FROM Products
	WHERE ProductID = @productID;
END