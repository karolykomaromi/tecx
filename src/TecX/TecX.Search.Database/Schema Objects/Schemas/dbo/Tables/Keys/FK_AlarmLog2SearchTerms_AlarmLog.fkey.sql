ALTER TABLE [dbo].[Messages2SearchTerms]
    ADD CONSTRAINT [FK_AlarmLog2SearchTerms_AlarmLog] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

