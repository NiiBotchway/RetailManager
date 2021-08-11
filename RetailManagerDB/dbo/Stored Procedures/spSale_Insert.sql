CREATE PROCEDURE spSale_Insert
@CashierId nvarchar(128),
@SaleDate datetime2,
@Subtotal money,
@Tax money,
@Total money
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Sale (CashierId, SaleDate, Subtotal, Tax, Total)
	VALUES (@CashierId,@SaleDate,@Subtotal,@Tax,@Total)

	SELECT CAST(SCOPE_IDENTITY() as int)
END
GO
