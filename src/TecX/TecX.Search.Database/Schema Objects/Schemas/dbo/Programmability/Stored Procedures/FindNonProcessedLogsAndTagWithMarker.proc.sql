/*
CREATE PROCEDURE FindNonProcessedLogsAndTagWithMarker 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @currentId uniqueidentifier = NewId();

	UPDATE AlarmLog SET ProcessMarker = @currentId, InProcessSince = getdate()
	FROM AlarmLog 
		LEFT JOIN AlarmLog2SearchTerms ON AlarmLog.id = AlarmLog2SearchTerms.AlarmLogId
	WHERE ProcessMarker is null OR InProcessSince < DateAdd(hh, -1, getdate());
 
	SELECT [id]
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
      ,[ProcessMarker]
      ,[InProcessSince]
    FROM [AlarmLog] WHERE ProcessMarker = @currentId;
END*/