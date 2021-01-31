CREATE PROCEDURE dbo.spSelectUnitWithProductID
	@productID int
AS
BEGIN
	SELECT UnitID, PartNumber, Description, ProductID 
	FROM Units
	WHERE ProductID = @productID
END