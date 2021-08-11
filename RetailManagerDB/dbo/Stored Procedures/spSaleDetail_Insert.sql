CREATE PROCEDURE spSaleDetail_Insert
@SaleId int,
@ProductId int,
@Quantity int,
@PurchasePrice money,
@Tax money
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO SaleDetail
				 (SaleId, ProductId, Quantity, PurchasePrice, Tax)
	VALUES (@SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax)

	SELECT CAST(SCOPE_IDENTITY() as int)
END
GO



