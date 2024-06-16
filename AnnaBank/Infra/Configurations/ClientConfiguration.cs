using AnnaBank.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnnaBank.Infra.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(x => x.IBAN)
                .HasColumnType("nvarchar(33)")
                .IsRequired();

            builder.Property(x => x.Balance)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}