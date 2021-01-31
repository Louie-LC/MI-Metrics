CREATE PROCEDURE dbo.spUpdateUnit
	@unitID int,
	@partNumber bigint,
	@description nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON
	UPDATE Units
	SET PartNumber = @partNumber, Description = @description
	WHERE UnitID = @unitID;
END