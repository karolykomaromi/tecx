
CREATE PROCEDURE [dbo].[CreateOrUpdateSearchTerm] 
	@messageId int,	
	@searchTerm nvarchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @searchTermId int;
	
	SELECT @searchTermId = id FROM SearchTerms WHERE SearchTerm = @searchTerm
	IF (@searchTermId IS NULL)
	BEGIN
		INSERT INTO SearchTerms (SearchTerm) values (@searchTerm);
		SET @searchTermId = SCOPE_IDENTITY();
	END
	IF NOT EXISTS (SELECT TOP(1) * FROM Messages2SearchTerms WHERE MessageId = @messageId AND SearchTermId = @searchTermId)
	BEGIN
		INSERT INTO Messages2SearchTerms(MessageId, SearchTermId) values (@messageId, @searchTermId);
	END
END