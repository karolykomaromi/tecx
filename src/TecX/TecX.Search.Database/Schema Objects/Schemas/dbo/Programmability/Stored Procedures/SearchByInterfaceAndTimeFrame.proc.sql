/*-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SearchByInterfaceAndTimeFrame]
	-- Add the parameters for the stored procedure here
	@maxResultCount int,
	@totalRowsCount int OUTPUT,
	@interfaceName nvarchar(64),
	@after datetime,
	@before datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	

	SELECT @totalRowsCount = COUNT(id) 
	FROM [AlarmLog] 
	WHERE [MonitoredItemName] LIKE @interfaceName
		AND [AlarmTimestamp] >= @after 
		AND [AlarmTimestamp] <= @before

    -- Insert statements for procedure here
	SELECT TOP(@maxResultCount) [id]
      ,[AlarmId]
      ,[AlarmTimestamp]
      ,[MonitoredItemName]
      ,[AttributeName]
      ,[AttributeValue]
      ,[ValueExpected]
      ,[RuleOperation]
      ,[PollingValueSourceId]
      ,[ExpectedValueId]
      ,[LogMessage]
      ,[AlarmMessage]
      ,[AlarmPriority]
      ,[ArrowDirection]
      ,[DBHostname]
      ,[CoreServername]
      ,[AgentID]
      ,[AgentName]
      ,[AgentServername]
      ,[InsertedAt]
	FROM [AlarmLog] 
	WHERE [MonitoredItemName] LIKE @interfaceName
		AND [AlarmTimestamp] >= @after 
		AND [AlarmTimestamp] <= @before
	ORDER BY [id] DESC
END*/