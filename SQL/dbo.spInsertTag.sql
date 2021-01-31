CREATE PROCEDURE [dbo].[spInsertTag]
	@tagID int,
	@description nvarchar(20),
	@productID int
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Tags (TagID, Description, ProductID)
	VALUES (@tagID, @description, @productID);
END