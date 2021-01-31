CREATE PROCEDURE dbo.spSelectUnitWithUnitID
	@unitID int
AS
BEGIN
	SELECT UnitID, PartNumber, Description, ProductID 
	FROM Units
	WHERE UnitID = @unitID
END