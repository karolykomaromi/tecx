-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[FullTextSearch] 
	-- Add the parameters for the stored procedure here
	@s1 as nvarchar(200),
	@maxResultCount as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id]
      ,[MessageText]
      ,[Priority]
      ,[SentAt]
      ,[Source]
      ,[ProcessMarker]
      ,[InProcessSince]
	FROM [Messages], 
		(SELECT TOP(@maxResultCount) m.Id AS MessageId, COUNT(*) AS HitCount
		FROM [Messages] m
			INNER JOIN Messages2SearchTerms m2s ON m.Id = m2s.MessageId
			INNER JOIN SearchTerms s ON s.Id = m2s.SearchTermId
		WHERE s.SearchTerm IN ('DDRFH', 'EXPECT', '102')
		GROUP BY m.Id
		ORDER BY HitCount DESC) AS Match
	WHERE [Messages].Id = Match.MessageId
END