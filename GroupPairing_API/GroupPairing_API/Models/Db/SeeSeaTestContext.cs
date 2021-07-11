using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GroupPairing_API.Models.Db
{
    public partial class SeeSeaTestContext : DbContext
    {
        public SeeSeaTestContext()
        {
        }

        public SeeSeaTestContext(DbContextOptions<SeeSeaTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityApplicant> ActivityApplicants { get; set; }
        public virtual DbSet<ActivityParticipant> ActivityParticipants { get; set; }
        public virtual DbSet<ActivityRoom> ActivityRooms { get; set; }
        public virtual DbSet<DivingPoint> DivingPoints { get; set; }
        public virtual DbSet<MessageBoard> MessageBoards { get; set; }
        public virtual DbSet<UserFavoriteActivity> UserFavoriteActivities { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<ActivityApplicant>(entity =>
            {
                entity.HasKey(e => new { e.ActivityId, e.ApplicantId });

                entity.ToTable("ActivityApplicant");

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.ApplicantId).HasColumnName("ApplicantID");

                entity.Property(e => e.ApplicatingDateTime).HasColumnType("datetime");

                entity.Property(e => e.ApplicatingDescription)
                    .HasMaxLength(200)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<ActivityParticipant>(entity =>
            {
                entity.HasKey(e => new { e.ActivityId, e.ParticipantId });

                entity.ToTable("ActivityParticipant");

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            });

            modelBuilder.Entity<ActivityRoom>(entity =>
            {
                entity.HasKey(e => e.ActivityId);

                entity.ToTable("ActivityRoom");

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.ActivityDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.ActivityDescription).HasMaxLength(500);

                entity.Property(e => e.ActivityName)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.Property(e => e.ActivityPicture).HasMaxLength(100);

                entity.Property(e => e.HostId).HasColumnName("HostID");

                entity.Property(e => e.MeetingPlace).HasMaxLength(20);
            });

            modelBuilder.Entity<DivingPoint>(entity =>
            {
                entity.ToTable("DivingPoint");

                entity.Property(e => e.DivingPointId).HasColumnName("DivingPointID");

                entity.Property(e => e.DivingPlaceDescription).HasMaxLength(200);

                entity.Property(e => e.DivingPointName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.DivingPointPicture).HasMaxLength(100);

                entity.Property(e => e.DivingTypeTag)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude).HasColumnType("decimal(17, 15)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(17, 14)");
            });

            modelBuilder.Entity<MessageBoard>(entity =>
            {
                entity.HasKey(e => e.MessageId);

                entity.ToTable("MessageBoard");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MessageDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<UserFavoriteActivity>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FavoriteActivityId });

                entity.ToTable("UserFavoriteActivity");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.FavoriteActivityId).HasColumnName("FavoriteActivityID");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_UserInfo_1");

                entity.ToTable("UserInfo");

                entity.HasIndex(e => e.UserAccount, "UQ__UserInfo__D68041C078A28AEC")
                    .IsUnique();

                entity.HasIndex(e => new { e.UserId, e.UserAccount, e.UserEmail }, "UQ__UserInfo__E2E8AB3C92D97372")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.AreaTag)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DivingTypeTag)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserAccount)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserDescription).HasMaxLength(200);

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UserEmailId)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("UserEmailID");

                entity.Property(e => e.UserImage).HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.UserNickName).HasMaxLength(25);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
