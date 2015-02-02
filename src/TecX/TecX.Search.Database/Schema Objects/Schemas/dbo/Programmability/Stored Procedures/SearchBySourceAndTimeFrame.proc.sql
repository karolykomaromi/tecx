-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SearchBySourceAndTimeFrame]
	-- Add the parameters for the stored procedure here
	@maxResultCount int,
	@totalRowsCount int OUTPUT,
	@source nvarchar(64),
	@after datetime,
	@before datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	

	SELECT @totalRowsCount = COUNT(Id) 
	FROM [Messages] 
	WHERE [Source] LIKE @source
		AND [SentAt] >= @after 
		AND [SentAt] <= @before

    -- Insert statements for procedure here
	SELECT TOP(@maxResultCount) [Id]
      ,[MessageText]
      ,[Priority]
      ,[SentAt]
      ,[Source]
      ,[ProcessMarker]
      ,[InProcessSince]
	FROM [Messages] 
	WHERE [Source] LIKE @source
		AND [SentAt] >= @after 
		AND [SentAt] <= @before
	ORDER BY [id] DESC
END