
CREATE PROCEDURE [dbo].[FindNonProcessedMessagesAndTagWithMarker] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @currentId uniqueidentifier = NewId();

	UPDATE [Messages] SET ProcessMarker = @currentId, InProcessSince = getdate()
	FROM [Messages] 
		LEFT JOIN Messages2SearchTerms ON [Messages].Id = Messages2SearchTerms.MessageId
	WHERE ProcessMarker is null OR InProcessSince < DateAdd(hh, -1, getdate());
 
	SELECT [Id]
      ,[MessageText]
      ,[Priority]
      ,[SentAt]
      ,[Source]
      ,[ProcessMarker]
      ,[InProcessSince]
    FROM [Messages] WHERE ProcessMarker = @currentId;
END