CREATE PROCEDURE [dbo].[spUpdateTag]
	@tagID int,
	@description nvarchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Tags
	SET Description = @description
	WHERE TagID = @tagID;
END