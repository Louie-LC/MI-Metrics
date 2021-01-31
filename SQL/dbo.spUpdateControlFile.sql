CREATE PROCEDURE [dbo].[spUpdateControlFile]
	@controlFileID int,
	@number bigint,
	@type nvarchar(4),
	@part nvarchar(4),
	@version nvarchar(3)
AS
BEGIN
	SET NOCOUNT ON
	UPDATE ControlFiles
	SET Number = @number, Type = @type, Part = @part, Version = @version
	WHERE ControlFileID = @controlFileID;
END