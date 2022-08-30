﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JokeMoke.Server.Data
{
    public partial class JokeMokeContext : DbContext
    {
        public JokeMokeContext()
        {
        }

        public JokeMokeContext(DbContextOptions<JokeMokeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Joke> Joke { get; set; }
        public virtual DbSet<JokeStatistics> JokeStatistics { get; set; }
        public virtual DbSet<JokeType> JokeType { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<JokeType> JokeTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.JokeId);

                entity.Property(e => e.JokeId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_by");

                entity.Property(e => e.IsApproved).HasColumnName("Is_approved");

                entity.Property(e => e.Value).IsRequired();

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_User");

                entity.HasOne(d => d.Joke)
                    .WithOne(p => p.Comment)
                    .HasForeignKey<Comment>(d => d.JokeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Joke");
            });

            modelBuilder.Entity<Joke>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_by");

                entity.Property(e => e.IsApproved).HasColumnName("Is_approved");

                entity.Property(e => e.Value).IsRequired();

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Joke)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Joke_User");

                entity.HasOne(d => d.JokeType)
                    .WithMany(p => p.Joke)
                    .HasForeignKey(d => d.JokeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Joke_Joke_Type");
            });

            modelBuilder.Entity<JokeStatistics>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Joke_Statistics");

                entity.HasOne(d => d.Joke)
                    .WithMany()
                    .HasForeignKey(d => d.JokeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Joke_Statistics_Joke");
            });

            modelBuilder.Entity<JokeType>(entity =>
            {
                entity.ToTable("Joke_Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(320)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}