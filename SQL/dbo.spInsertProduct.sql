CREATE PROCEDURE dbo.spInsertProduct
	@productID int,
	@name NVARCHAR(100),
	@image NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO Products (ProductID, Name, ImagePath)
	VALUES (@productID, @name, @image)
END