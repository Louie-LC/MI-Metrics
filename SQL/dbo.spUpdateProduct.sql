CREATE PROCEDURE dbo.spUpdateProduct
	@productID int,
	@name NVARCHAR(100),
	@image NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON
	UPDATE Products
	SET Name = @name, ImagePath = @image
	WHERE ProductID = @productID
END