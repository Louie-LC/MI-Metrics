CREATE PROCEDURE dbo.spSelectAllProducts
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ID, Name, ImagePath FROM Products;
END