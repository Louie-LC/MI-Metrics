CREATE PROCEDURE [dbo].[spSelectControlFileWithProductID]
	@productID int
AS
BEGIN
	SET NOCOUNT ON
	SELECT ControlFileID, Number, Type, Part, Version, ProductID
	FROM ControlFiles
	WHERE ProductID = @productID;
END