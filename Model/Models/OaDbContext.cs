using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Model.Models;

public partial class OaDbContext : DbContext
{
    public OaDbContext()
    {
    }

    public OaDbContext(DbContextOptions<OaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Handin> Handins { get; set; }

    public virtual DbSet<Homework> Homeworks { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Write> Writes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=oa-1;Username=postgres;Password=12345678");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("en_US.UTF-8");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("groups_pkey");

            entity.ToTable("groups");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
            entity.Property(e => e.Owner).HasColumnName("owner");

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.Owner)
                .HasConstraintName("groups_owner_fkey");
        });

        modelBuilder.Entity<Handin>(entity =>
        {
            entity.HasKey(e => new { e.HomeworkId, e.UserId, e.TestId }).HasName("handins_pkey");

            entity.ToTable("handins");

            entity.Property(e => e.HomeworkId).HasColumnName("homework_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Feedback).HasColumnName("feedback");

            entity.HasOne(d => d.Homework).WithMany(p => p.Handins)
                .HasForeignKey(d => d.HomeworkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("handins_homework_id_fkey");

            entity.HasOne(d => d.Write).WithMany(p => p.Handins)
                .HasForeignKey(d => new { d.UserId, d.TestId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("handins_user_id_test_id_fkey");
        });

        modelBuilder.Entity<Homework>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("homewokrs_pkey");

            entity.ToTable("homeworks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creation_date");
            entity.Property(e => e.DueDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("due_date");
            entity.Property(e => e.GroupId).HasColumnName("group_id");

            entity.HasOne(d => d.Group).WithMany(p => p.Homeworks)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("homeworks_group_id_fkey");

            entity.HasMany(d => d.Tests).WithMany(p => p.Homeworks)
                .UsingEntity<Dictionary<string, object>>(
                    "HomeworkComposition",
                    r => r.HasOne<Test>().WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("homework_composition_test_id_fkey"),
                    l => l.HasOne<Homework>().WithMany()
                        .HasForeignKey("HomeworkId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("homework_composition_homework_id_fkey"),
                    j =>
                    {
                        j.HasKey("HomeworkId", "TestId").HasName("homework_composition_pkey");
                        j.ToTable("homework_composition");
                        j.IndexerProperty<Guid>("HomeworkId").HasColumnName("homework_id");
                        j.IndexerProperty<Guid>("TestId").HasColumnName("test_id");
                    });
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.GroupId }).HasName("member_pkey");

            entity.ToTable("member");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.Accepted).HasColumnName("accepted");

            entity.HasOne(d => d.Group).WithMany(p => p.Members)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("member_group_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Members)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("member_user_id_fkey");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => new { e.Name, e.Category }).HasName("tags_pkey");

            entity.ToTable("tags");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
            entity.Property(e => e.Category)
                .HasMaxLength(40)
                .HasColumnName("category");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Tags)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tags_category_fkey");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tests_pkey");

            entity.ToTable("tests");

            entity.Property(e => e.Id)
                .HasColumnName("id")
              .HasDefaultValueSql("gen_random_uuid()")
              .ValueGeneratedOnAdd()
              .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Description)
                .HasMaxLength(1200)
                .HasColumnName("description");
            entity.Property(e => e.GradingScheme).HasColumnName("grading_scheme");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .HasColumnName("name");

            entity.HasMany(d => d.Tags).WithMany(p => p.Tests)
                .UsingEntity<Dictionary<string, object>>(
                    "Tagged",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagName", "TagCategory")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("tagged_tag_name_tag_category_fkey"),
                    l => l.HasOne<Test>().WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("tagged_test_id_fkey"),
                    j =>
                    {
                        j.HasKey("TestId", "TagName", "TagCategory").HasName("tagged_pkey");
                        j.ToTable("tagged");
                        j.IndexerProperty<Guid>("TestId").HasColumnName("test_id");
                        j.IndexerProperty<string>("TagName")
                            .HasMaxLength(40)
                            .HasColumnName("tag_name");
                        j.IndexerProperty<string>("TagCategory")
                            .HasMaxLength(40)
                            .HasColumnName("tag_category");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.FullName)
                .HasMaxLength(40)
                .HasColumnName("full_name");
            entity.Property(e => e.Username)
                .HasMaxLength(40)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Write>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.TestId }).HasName("writes_pkey");

            entity.ToTable("writes");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.Answers).HasColumnName("answers");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Results).HasColumnName("results");

            entity.HasOne(d => d.Test).WithMany(p => p.Writes)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("writes_test_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Writes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("writes_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
