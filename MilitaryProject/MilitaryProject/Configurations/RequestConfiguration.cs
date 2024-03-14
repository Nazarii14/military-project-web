using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryProject.Models;

namespace MilitaryProject.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasKey(r => r.ID);

            builder.HasOne(r => r.Brigade)
                   .WithMany(b => b.Requests)
                   .HasForeignKey(r => r.BrigadeID);

            builder.ToTable("Requests");
        }
    }
}
