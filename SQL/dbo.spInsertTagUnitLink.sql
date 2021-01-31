CREATE PROCEDURE [dbo].[spInsertTagUnitLink]
	@tagID int,
	@unitID int
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO TagUnitLinks (TagID, UnitID)
	VALUES (@tagID, @unitID);
END