CREATE PROCEDURE [dbo].[spSelectTagUnitLink]
	@tagID int,
	@unitID int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TagID, UnitID
	FROM TagUnitLinks
	WHERE TagID = @tagID AND UnitID = @unitID
END