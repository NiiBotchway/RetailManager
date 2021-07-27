CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
	Select Id, ProductName, Description, RetailPrice, QuantityInStock
	from Product
	order by ProductName
RETURN 0
