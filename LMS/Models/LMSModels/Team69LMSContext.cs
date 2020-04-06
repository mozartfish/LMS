using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Models.LMSModels
{
    public partial class Team69LMSContext : DbContext
    {
        public Team69LMSContext()
        {
        }

        public Team69LMSContext(DbContextOptions<Team69LMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrators> Administrators { get; set; }
        public virtual DbSet<AssignmentCategories> AssignmentCategories { get; set; }
        public virtual DbSet<Assignments> Assignments { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<EnrollmentGrade> EnrollmentGrade { get; set; }
        public virtual DbSet<Professors> Professors { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Submission> Submission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Server=atr.eng.utah.edu;User Id=u1027713;Password=Trainspotting47;Database=Team69LMS");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrators>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<AssignmentCategories>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.ClassId, e.CategoryName })
                    .HasName("classID")
                    .IsUnique();

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ClassId).HasColumnName("classID");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.AssignmentCategories)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AssignmentCategories_ibfk_1");
            });

            modelBuilder.Entity<Assignments>(entity =>
            {
                entity.HasKey(e => e.AssignmentId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.CategoryId, e.AsgmtName })
                    .HasName("categoryID")
                    .IsUnique();

                entity.Property(e => e.AssignmentId).HasColumnName("assignmentID");

                entity.Property(e => e.AsgmtName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.Contents)
                    .IsRequired()
                    .HasColumnType("varchar(8192)");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Assignments_ibfk_1");
            });

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.HasKey(e => e.ClassId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Taught)
                    .HasName("Taught");

                entity.HasIndex(e => new { e.CourseId, e.Year, e.Season })
                    .HasName("courseID")
                    .IsUnique();

                entity.Property(e => e.ClassId).HasColumnName("classID");

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Season)
                    .IsRequired()
                    .HasColumnType("varchar(6)");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Taught)
                    .IsRequired()
                    .HasColumnType("char(8)");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_1");

                entity.HasOne(d => d.TaughtNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.Taught)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_2");
            });

            modelBuilder.Entity<Courses>(entity =>
            {
                entity.HasKey(e => e.CourseId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.DeptAbbreviation)
                    .HasName("DeptAbbreviation");

                entity.HasIndex(e => new { e.CourseNumber, e.DeptAbbreviation })
                    .HasName("CourseNumber")
                    .IsUnique();

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.DeptAbbreviation)
                    .IsRequired()
                    .HasColumnType("varchar(4)");

                entity.HasOne(d => d.DeptAbbreviationNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.DeptAbbreviation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Courses_ibfk_1");
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.HasKey(e => e.DeptAbbreviation)
                    .HasName("PRIMARY");

                entity.Property(e => e.DeptAbbreviation).HasColumnType("varchar(4)");

                entity.Property(e => e.DeptName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<EnrollmentGrade>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.ClassId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ClassId)
                    .HasName("classID");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.ClassId).HasColumnName("classID");

                entity.Property(e => e.Grade)
                    .IsRequired()
                    .HasColumnType("varchar(2)");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.EnrollmentGrade)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EnrollmentGrade_ibfk_2");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.EnrollmentGrade)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EnrollmentGrade_ibfk_1");
            });

            modelBuilder.Entity<Professors>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.WorksIn)
                    .HasName("WorksIn");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.WorksIn)
                    .IsRequired()
                    .HasColumnType("varchar(4)");

                entity.HasOne(d => d.WorksInNavigation)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.WorksIn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Professors_ibfk_1");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Major)
                    .HasName("Major");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Major)
                    .IsRequired()
                    .HasColumnType("varchar(4)");

                entity.HasOne(d => d.MajorNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.Major)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Students_ibfk_1");
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.AssignmentId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AssignmentId)
                    .HasName("assignmentID");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.AssignmentId).HasColumnName("assignmentID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("varchar(8192)");

                entity.Property(e => e.SubTime).HasColumnType("datetime");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.Submission)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Submission_ibfk_2");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Submission)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Submission_ibfk_1");
            });
        }
    }
}
