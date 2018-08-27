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

        public virtual DbSet<UserImage> UserImages { get; protected set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("message");

                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .HasDefaultValueSql("shard_1.id_generator()");

                entity.Property(e => e.Content)
                      .IsRequired()
                      .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasColumnType("timestamp with time zone")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.SenderId)
                      .HasColumnName("sender_id")
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
                      .HasColumnName("id")
                      .HasDefaultValueSql("shard_1.id_generator()");

                entity.Property(e => e.ImageId)
                      .HasColumnName("image_id")
                      .HasDefaultValueSql("'1855266345831107587'::bigint");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasColumnName("name")
                      .HasDefaultValueSql("'NewUser'::text");

                entity.HasOne(d => d.Image)
                      .WithMany(p => p.AttachedUsers)
                      .HasForeignKey(d => d.ImageId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("image_reference");
            });

            modelBuilder.Entity<UserImage>(entity =>
            {
                entity.ToTable("user_image");

                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .HasDefaultValueSql("shard_3.id_generator()");

                entity.Property(e => e.Extension)
                      .IsRequired()
                      .HasColumnName("extension");
            });

            modelBuilder.HasSequence("seq")
                        .HasMax(2147483647);

            modelBuilder.HasSequence("global_id_sequence");

            modelBuilder.HasSequence("global_id_sequence");

            modelBuilder.HasSequence("global_id_sequence");
        }
    }
}