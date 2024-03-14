using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilitaryProject.Models;

namespace MilitaryProject.Configurations
{
    public class MilitaryRouteConfiguration : IEntityTypeConfiguration<MilitaryRoute>
    {
        public void Configure(EntityTypeBuilder<MilitaryRoute> builder)
        {
            builder.HasKey(rt => rt.ID);

            builder.HasOne(rt => rt.Volunteer)
                   .WithMany() // Specify the navigation property in the User class if there's one representing the collection of routes.
                   .HasForeignKey(rt => rt.VolunteerID)
                   .OnDelete(DeleteBehavior.Restrict); // Use Restrict to prevent cascading deletes.

            builder.HasOne(rt => rt.Weapon)
                   .WithMany(w => w.MilitaryRoutes) // Ensure there's a navigation property in Weapon for routes.
                   .HasForeignKey(rt => rt.WeaponID);

            builder.HasOne(rt => rt.Ammunition)
                   .WithMany(a => a.MilitaryRoutes) // Ensure there's a navigation property in Ammunition for routes.
                   .HasForeignKey(rt => rt.AmmunitionID);

            builder.ToTable("MilitaryRoutes"); // Ensure the table name matches what you intend to use in the database.
        }
    }
}
