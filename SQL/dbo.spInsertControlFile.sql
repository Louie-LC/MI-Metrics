CREATE PROCEDURE [dbo].[spInsertControlFile]
	@controlFileID int,
	@number bigint,
	@type nvarchar(4),
	@part nvarchar(4),
	@version nvarchar(3),
	@productID int
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO ControlFiles (ControlFileID, Number, Type, Part, Version, ProductID)
	VALUES (@controlFileID, @number, @type, @part, @version, @productID);
END