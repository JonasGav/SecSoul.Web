using Microsoft.EntityFrameworkCore;
using SecSoul.Model.Entity;

namespace SecSoul.Model.Context
{
    public partial class SecSoulContext : DbContext
    {

        public SecSoulContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<ScanRequest> ScanRequest { get; set; }
        public virtual DbSet<ScanNmap> ScanNmap { get; set; }
        public virtual DbSet<ScanVirusTotal> ScanVirusTotal { get; set; }
        public virtual DbSet<ScanDirb> ScanDirb { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Discriminator).IsRequired();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FullName).HasMaxLength(150);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<ScanNmap>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("ScanNmap_pk")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("ScanNmap_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.ScanResult).IsUnicode(false);

                entity.HasOne(d => d.ScanRequest)
                    .WithMany(p => p.ScanNmap)
                    .HasForeignKey(d => d.ScanRequestId)
                    .HasConstraintName("ScanNmap_ScanRequest_Id_fk");
            });
            
            modelBuilder.Entity<ScanVirusTotal>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("ScanVirusTotal_pk")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("ScanVirusTotal_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.ScanResult).IsUnicode(false);

                entity.HasOne(d => d.ScanRequest)
                    .WithMany(p => p.ScanVirusTotal)
                    .HasForeignKey(d => d.ScanRequestId)
                    .HasConstraintName("ScanVirusTotal_ScanRequest_Id_fk");
            });
            
            modelBuilder.Entity<ScanDirb>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("ScanDirb_pk")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("ScanDirb_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.ScanRequest)
                    .WithMany(p => p.ScanDirb)
                    .HasForeignKey(d => d.ScanRequestId)
                    .HasConstraintName("ScanDirb_ScanRequest_Id_fk");
            });
            
            modelBuilder.Entity<ScanRequest>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.RequestDate).HasColumnType("datetime");

                entity.Property(e => e.WebsiteFtp)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                
                entity.Property(e => e.FtpUsername)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                
                entity.Property(e => e.FtpPassword)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WebsiteUrl)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ScanRequest)
                    .HasForeignKey(d => d.UserId);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
