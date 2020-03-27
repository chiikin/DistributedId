using DistributedId.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedId.Web.Data
{
    public class IdStoreContext:DbContext
    {
        public IdStoreContext(DbContextOptions<IdStoreContext> options) :base(options)
        {

        }

        public DbSet<SequencedIdEntity> SequencedIds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SequencedIdEntity>()
                .Property<byte[]>("RowVersion").IsRowVersion();
        }
    }
}
