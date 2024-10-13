using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class CoinFillContext : DbContext
    {
        public CoinFillContext()
        {
        }

        public CoinFillContext(DbContextOptions<CoinFillContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Cryptocurrencies> Cryptocurrencies { get; set; }
        public virtual DbSet<CryptocurrencyNetworks> CryptocurrencyNetworks { get; set; }
        public virtual DbSet<EmailMarketingStatistics> EmailMarketingStatistics { get; set; }
        public virtual DbSet<ErrorLogs> ErrorLogs { get; set; }
        public virtual DbSet<GeneratedCryptocurrencyNetworks> GeneratedCryptocurrencyNetworks { get; set; }
        public virtual DbSet<NewsletterSubscribers> NewsletterSubscribers { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<UserActivityLogs> UserActivityLogs { get; set; }
        public virtual DbSet<UserBankAccounts> UserBankAccounts { get; set; }
        public virtual DbSet<UserCards> UserCards { get; set; }
        public virtual DbSet<Validators> Validators { get; set; }
        public virtual DbSet<ValidatorsCryptocurrencies> ValidatorsCryptocurrencies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-T38LG1B;Database=CoinFill;Trusted_Connection=True;");
            }
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

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
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

                entity.Property(e => e.City).HasMaxLength(128);

                entity.Property(e => e.Country).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(128);

                entity.Property(e => e.FullNameAddress).HasMaxLength(128);

                entity.Property(e => e.HouseNumber).HasMaxLength(128);

                entity.Property(e => e.LastName).HasMaxLength(128);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.PostalCode).HasMaxLength(128);

                entity.Property(e => e.ProfilePhoto).HasMaxLength(512);

                entity.Property(e => e.Street).HasMaxLength(128);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.Property(e => e.WebCredit).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Cryptocurrencies>(entity =>
            {
                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<CryptocurrencyNetworks>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.CryptocurrencyId).HasMaxLength(450);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.QrImage)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.HasOne(d => d.Cryptocurrency)
                    .WithMany(p => p.CryptocurrencyNetworks)
                    .HasForeignKey(d => d.CryptocurrencyId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_CryptocurrencyNetworksId_AspNetUsersId");
            });

            modelBuilder.Entity<EmailMarketingStatistics>(entity =>
            {
                entity.Property(e => e.CampaignName).HasMaxLength(1024);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.IpAddress).HasMaxLength(256);

                entity.Property(e => e.Unsubscribed)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<ErrorLogs>(entity =>
            {
                entity.Property(e => e.ActionOrMethod).HasMaxLength(64);

                entity.Property(e => e.AreaOrProject).HasMaxLength(64);

                entity.Property(e => e.ControllerOrClass).HasMaxLength(64);

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.Fixed)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.Source).HasMaxLength(512);

                entity.Property(e => e.StackTraceExecutingAssemblyName).HasMaxLength(512);

                entity.Property(e => e.StackTraceFrameMethodName).HasMaxLength(512);

                entity.Property(e => e.TargetSiteName).HasMaxLength(512);

                entity.Property(e => e.TargetSiteReflectedTypeFullName).HasMaxLength(512);

                entity.Property(e => e.UserIdOrAnonymous).HasMaxLength(450);
            });

            modelBuilder.Entity<GeneratedCryptocurrencyNetworks>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CryptocurrencyId, e.CryptocurrencyNetworkId });
            });

            modelBuilder.Entity<NewsletterSubscribers>(entity =>
            {
                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.IpAddress).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).IsRequired();

                entity.Property(e => e.VerificationCount).HasDefaultValueSql("(CONVERT([bigint],(0)))");

                entity.Property(e => e.Verified)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.Property(e => e.Body).HasMaxLength(384);

                entity.Property(e => e.Icon).HasMaxLength(32);

                entity.Property(e => e.Important)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.Read)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.ReceiverUserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Severity)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.HasOne(d => d.ReceiverUser)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.ReceiverUserId)
                    .HasConstraintName("FK_NotificationsReceiverUserId_AspNetUsersId");
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.Property(e => e.AmountShouldBe).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PaymentMethodCryptocurrencyId).HasMaxLength(450);

                entity.Property(e => e.PaymentMethodCryptocurrencyNetworkId).HasMaxLength(450);

                entity.Property(e => e.Reason).HasMaxLength(512);

                entity.Property(e => e.ReasonId).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<UserActivityLogs>(entity =>
            {
                entity.Property(e => e.Action).HasMaxLength(64);

                entity.Property(e => e.Area).HasMaxLength(64);

                entity.Property(e => e.Controller).HasMaxLength(64);

                entity.Property(e => e.IpAddress).HasMaxLength(256);

                entity.Property(e => e.MethodType).HasMaxLength(64);

                entity.Property(e => e.QueryDataJson).HasMaxLength(2048);

                entity.Property(e => e.UserAgent).HasMaxLength(1024);

                entity.Property(e => e.UserIdOrAnonymous)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<UserBankAccounts>(entity =>
            {
                entity.Property(e => e.BankAccountNumber).HasMaxLength(128);

                entity.Property(e => e.BicSwift).HasMaxLength(128);

                entity.Property(e => e.Currency).HasMaxLength(128);

                entity.Property(e => e.FirstName).HasMaxLength(128);

                entity.Property(e => e.InstitutionNumber).HasMaxLength(128);

                entity.Property(e => e.LastName).HasMaxLength(128);

                entity.Property(e => e.RoutingNumber).HasMaxLength(128);

                entity.Property(e => e.TransitNumber).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<UserCards>(entity =>
            {
                entity.Property(e => e.BackgroundImage).HasMaxLength(2048);

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Brand).HasMaxLength(128);

                entity.Property(e => e.City).HasMaxLength(128);

                entity.Property(e => e.Country).HasMaxLength(128);

                entity.Property(e => e.Cvv)
                    .HasColumnName("CVV")
                    .HasMaxLength(128);

                entity.Property(e => e.ExpirationDate).HasMaxLength(128);

                entity.Property(e => e.FullNameAddress).HasMaxLength(128);

                entity.Property(e => e.HouseNumber).HasMaxLength(128);

                entity.Property(e => e.Number).HasMaxLength(128);

                entity.Property(e => e.PaymentMethodCryptocurrencyId).HasMaxLength(450);

                entity.Property(e => e.PaymentMethodCryptocurrencyNetworkId).HasMaxLength(450);

                entity.Property(e => e.PostalCode).HasMaxLength(128);

                entity.Property(e => e.Street).HasMaxLength(128);

                entity.Property(e => e.Type).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<Validators>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.Photo).HasMaxLength(1024);
            });

            modelBuilder.Entity<ValidatorsCryptocurrencies>(entity =>
            {
                entity.HasKey(e => new { e.ValidatorId, e.CryptocurrencyId });

                entity.Property(e => e.MinimumDepositAmount).HasDefaultValueSql("((1.0000000000000000e+000))");

                entity.Property(e => e.UnlockTimeHours).HasDefaultValueSql("((1.0000000000000000e+000))");

                entity.HasOne(d => d.Cryptocurrency)
                    .WithMany(p => p.ValidatorsCryptocurrencies)
                    .HasForeignKey(d => d.CryptocurrencyId)
                    .HasConstraintName("FK_ValidatorsCryptocurrencies_Cryptocurrencies_Id");

                entity.HasOne(d => d.Validator)
                    .WithMany(p => p.ValidatorsCryptocurrencies)
                    .HasForeignKey(d => d.ValidatorId)
                    .HasConstraintName("FK_ValidatorsCryptocurrencies_Validators_Id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
