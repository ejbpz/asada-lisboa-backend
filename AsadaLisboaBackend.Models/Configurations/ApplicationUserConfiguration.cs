using AsadaLisboaBackend.Models.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsadaLisboaBackend.Models.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.FirstLastName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.SecondLastName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(x => x.Charge)
                .WithMany()
                .HasForeignKey(x => x.ChargeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
