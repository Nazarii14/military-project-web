using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryProject.Domain.Entity;


namespace MilitaryProject.DAL.Configurations
{
    public class BrigadeStorageConfiguration : IEntityTypeConfiguration<BrigadeStorage>
    {
        public void Configure(EntityTypeBuilder<BrigadeStorage> builder)
        {
            builder.HasKey(bs => bs.ID);

            builder.HasOne(bs => bs.Brigade)
                   .WithMany(b => b.BrigadeStorages)
                   .HasForeignKey(bs => bs.BrigadeID);

            builder.HasOne(bs => bs.Weapon)
                   .WithMany(w => w.BrigadeStorages)
                   .HasForeignKey(bs => bs.WeaponID);

            builder.HasOne(bs => bs.Ammunition)
                   .WithMany(a => a.BrigadeStorages)
                   .HasForeignKey(bs => bs.AmmunitionID);

            builder.ToTable("BrigadeStorages");
        }
    }
}
