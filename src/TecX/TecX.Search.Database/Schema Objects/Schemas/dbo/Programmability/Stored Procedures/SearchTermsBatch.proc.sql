-- =============================================
-- Author:		
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SearchTermsBatch] 
	-- Add the parameters for the stored procedure here
	@searchTerms SearchTerm readonly	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @MessageId int
	DECLARE @Text nvarchar(200)
	DECLARE SearchTermList CURSOR FOR SELECT MessageId, [TEXT] FROM @searchTerms
	
	OPEN SearchTermList
	
	FETCH NEXT FROM SearchTermList
	  INTO @MessageId, @Text
	  
	WHILE @@FETCH_STATUS = 0
	BEGIN
	  EXEC CreateOrUpdateSearchTerm @MessageId, @Text
	  FETCH NEXT FROM SearchTermList 
	    INTO @MessageId, @Text
	END
	
	CLOSE SearchTermList
	DEALLOCATE SearchTermList

END