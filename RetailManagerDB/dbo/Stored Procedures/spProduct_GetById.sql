CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id int
AS
BEGIN
	set nocount on;

	SELECT Id, ProductName, Description, RetailPrice, QuantityInStock, IsTaxable
	FROM Product
	WHERE Id = @Id
END
