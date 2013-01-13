ALTER TABLE [dbo].[Messages2SearchTerms]
    ADD CONSTRAINT [FK_AlarmLog2SearchTerms_SearchTerms] FOREIGN KEY ([SearchTermId]) REFERENCES [dbo].[SearchTerms] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

