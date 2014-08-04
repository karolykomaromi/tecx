CREATE TABLE [dbo].[Messages] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [MessageText]    NVARCHAR (MAX)   NOT NULL,
    [Priority]       INT              NOT NULL,
    [SentAt]         DATETIME         NOT NULL,
    [Source]         NVARCHAR (64)    NOT NULL,
    [ProcessMarker]  UNIQUEIDENTIFIER NULL,
    [InProcessSince] DATETIME         NULL
);

