CREATE PROCEDURE dbo.spInsertUnit
	@unitID int,
	@partNumber bigint,
	@description nvarchar(100),
	@productID int
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO Units (UnitID, PartNumber, Description, ProductID)
	VALUES (@unitID, @partNumber, @description, @productID)
END