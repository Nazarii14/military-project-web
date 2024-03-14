using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryProject.Models;

namespace MilitaryProject.Configurations
{
    public class UserItemsConfiguration : IEntityTypeConfiguration<UserItems>
    {
        public void Configure(EntityTypeBuilder<UserItems> builder)
        {
            builder.HasKey(ui => ui.ID);

            builder.HasOne(ui => ui.User)
                   .WithMany(u => u.UserItems)
                   .HasForeignKey(ui => ui.UserID);

            builder.HasOne(ui => ui.Weapon)
                   .WithMany(w => w.UserItems)
                   .HasForeignKey(ui => ui.WeaponID);

            builder.HasOne(ui => ui.Ammunition)
                   .WithMany(a => a.UserItems)
                   .HasForeignKey(ui => ui.AmmunitionID);

            builder.ToTable("UserItems");
        }
    }
}
