CREATE PROCEDURE [dbo].[spInsertProductWithJustName]
	@productID int,
	@name NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO Products (ProductID, Name)
	VALUES (@productID, @name)
END