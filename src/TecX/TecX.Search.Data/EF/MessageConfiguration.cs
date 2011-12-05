namespace TecX.Search.Data.EF
{
    using System.Data.Entity.ModelConfiguration;

    public class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageConfiguration()
        {
            ////            HasKey(b => b.Id);

            ////            Property(b => b.Id)
            ////                .HasColumnName("id")
            ////                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
            ////                .IsRequired();

            ////            ////Ignore(b => b.CurrentPoll);
            ////            Ignore(b => b.AttributeId);

            ////            ToTable("Messages");
        }
    }
}