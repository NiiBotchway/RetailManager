CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
Begin
	Set nocount on;

	SELECT Id, FirstName, LastName, EmailAddress, CreatedDate
	from dbo.[User]
	where Id = @Id
end
