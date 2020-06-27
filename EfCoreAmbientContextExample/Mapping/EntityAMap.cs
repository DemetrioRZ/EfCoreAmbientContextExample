using EfCoreAmbientContextExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreAmbientContextExample.Mapping
{
    public class EntityAMap : IEntityTypeConfiguration<EntityA>
    {
        public void Configure(EntityTypeBuilder<EntityA> builder)
        {
            builder.ToTable("EntityA").HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ValueA).HasMaxLength(100);
        }
    }
}