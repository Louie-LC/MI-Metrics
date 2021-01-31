CREATE PROCEDURE [dbo].[spSelectTagUnitLinkWithTagID]
	@tagID int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TagID, UnitID
	FROM TagUnitLinks
	WHERE TagID = @tagID;
END