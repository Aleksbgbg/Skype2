namespace HttpServer.Database
{
    using Microsoft.EntityFrameworkCore;

    using Shared.Models;

    public class Skype2Context : DbContext
    {
        public Skype2Context(DbContextOptions<Skype2Context> options) : base(options)
        {
        }

        public virtual DbSet<Message> Messages { get; protected set; }

        public virtual DbSet<User> Users { get; protected set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("message");

                entity.Property(e => e.Id)
                      .HasColumnName("Id")
                      .HasDefaultValueSql("shard_1.id_generator()");

                entity.Property(e => e.Content)
                      .IsRequired()
                      .HasColumnName("Content");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("CreatedAt")
                      .HasColumnType("timestamptz");

                entity.Property(e => e.SenderId)
                      .HasColumnName("SenderId")
                      .HasDefaultValueSql("'1853920687798879245'::bigint");

                entity.HasOne(d => d.Sender)
                      .WithMany(p => p.Messages)
                      .HasForeignKey(d => d.SenderId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("sender");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                      .HasColumnName("Id")
                      .HasDefaultValueSql("shard_1.id_generator()");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasColumnName("Name")
                      .HasDefaultValueSql("'NewUser'::text");
            });

            modelBuilder.HasSequence("seq")
                        .HasMax(2147483647);

            modelBuilder.HasSequence("global_id_sequence");

            modelBuilder.HasSequence("global_id_sequence");
        }
    }
}