using EfCoreAmbientContextExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreAmbientContextExample.Mapping
{
    public class EntityBMap : IEntityTypeConfiguration<EntityB>
    {
        public void Configure(EntityTypeBuilder<EntityB> builder)
        {
            builder.ToTable("EntityB").HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ValueB).HasMaxLength(100);
        }
    }
}