CREATE PROCEDURE [dbo].[spSelectTagUnitLinkWithUnitID]
	@unitID int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TagID, UnitID
	FROM TagUnitLinks
	WHERE UnitID = @unitID
END