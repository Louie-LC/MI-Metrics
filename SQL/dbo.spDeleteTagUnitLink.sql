CREATE PROCEDURE [dbo].[spDeleteTagUnitLink]
	@tagID int,
	@unitID int
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM TagUnitLinks
	WHERE TagID = @tagID AND UnitID = @unitID;
END