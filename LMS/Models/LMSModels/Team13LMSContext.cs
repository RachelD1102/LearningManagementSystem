using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Models.LMSModels
{
    public partial class Team13LMSContext : DbContext
    {
        public Team13LMSContext()
        {
        }

        public Team13LMSContext(DbContextOptions<Team13LMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrators> Administrators { get; set; }
        public virtual DbSet<AssignmentCategories> AssignmentCategories { get; set; }
        public virtual DbSet<Assignments> Assignments { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Professors> Professors { get; set; }
        public virtual DbSet<StudentEnrollment> StudentEnrollment { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Submission> Submission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Server=atr.eng.utah.edu;User Id=u1275459;Password=OPklnm90;Database=Team13LMS");
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
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FirstName).HasColumnType("varchar(100)");

                entity.Property(e => e.LastName).HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<AssignmentCategories>(entity =>
            {
                entity.HasKey(e => e.AssignmentCategoryId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ClassId)
                    .HasName("ClassId");

                entity.HasIndex(e => new { e.Name, e.ClassId })
                    .HasName("Name")
                    .IsUnique();

                entity.Property(e => e.AssignmentCategoryId).HasColumnType("varchar(5)");

                entity.Property(e => e.ClassId)
                    .IsRequired()
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.GradingWeight).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.AssignmentCategories)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("AssignmentCategories_ibfk_1");
            });

            modelBuilder.Entity<Assignments>(entity =>
            {
                entity.HasKey(e => e.AssignmentId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.AssignmentCategoryId, e.Name })
                    .HasName("AssignmentCategoryId")
                    .IsUnique();

                entity.Property(e => e.AssignmentId).HasColumnType("varchar(5)");

                entity.Property(e => e.AssignmentCategoryId)
                    .IsRequired()
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.Contents).HasColumnType("mediumtext");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.MaxPointValue).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.AssignmentCategory)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.AssignmentCategoryId)
                    .HasConstraintName("Assignments_ibfk_1");
            });

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.HasKey(e => e.ClassId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CourseNumber)
                    .HasName("CourseNumber");

                entity.HasIndex(e => e.ProfessorId)
                    .HasName("ProfessorId");

                entity.HasIndex(e => new { e.SemesterSeason, e.SemesterYear, e.CourseNumber })
                    .HasName("SemesterSeason")
                    .IsUnique();

                entity.Property(e => e.ClassId).HasColumnType("varchar(5)");

                entity.Property(e => e.CourseNumber).HasColumnType("int(11)");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Location).HasColumnType("varchar(100)");

                entity.Property(e => e.ProfessorId).HasColumnType("varchar(8)");

                entity.Property(e => e.SemesterSeason)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.CourseNumberNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.CourseNumber)
                    .HasConstraintName("Classes_ibfk_1");

                entity.HasOne(d => d.Professor)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.ProfessorId)
                    .HasConstraintName("Classes_ibfk_2");
            });

            modelBuilder.Entity<Courses>(entity =>
            {
                entity.HasKey(e => e.CourseNumber)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.SubjectAbbreviation)
                    .HasName("SubjectAbbreviation");

                entity.HasIndex(e => new { e.CourseNumber, e.SubjectAbbreviation })
                    .HasName("CourseNumber")
                    .IsUnique();

                entity.Property(e => e.CourseNumber).HasColumnType("int(11)");

                entity.Property(e => e.CatalogId)
                    .IsRequired()
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.SubjectAbbreviation)
                    .IsRequired()
                    .HasColumnType("varchar(4)");

                entity.HasOne(d => d.SubjectAbbreviationNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.SubjectAbbreviation)
                    .HasConstraintName("Courses_ibfk_1");
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.HasKey(e => e.SubjectAbbreviation)
                    .HasName("PRIMARY");

                entity.Property(e => e.SubjectAbbreviation).HasColumnType("varchar(4)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Professors>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.SubjectAbbreviation)
                    .HasName("SubjectAbbreviation");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FirstName).HasColumnType("varchar(100)");

                entity.Property(e => e.LastName).HasColumnType("varchar(100)");

                entity.Property(e => e.SubjectAbbreviation)
                    .IsRequired()
                    .HasColumnType("varchar(4)");

                entity.HasOne(d => d.SubjectAbbreviationNavigation)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.SubjectAbbreviation)
                    .HasConstraintName("Professors_ibfk_1");
            });

            modelBuilder.Entity<StudentEnrollment>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.ClassId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ClassId)
                    .HasName("ClassId");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.ClassId).HasColumnType("varchar(5)");

                entity.Property(e => e.Grade).HasColumnType("varchar(2)");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.StudentEnrollment)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("StudentEnrollment_ibfk_2");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.StudentEnrollment)
                    .HasForeignKey(d => d.UId)
                    .HasConstraintName("StudentEnrollment_ibfk_1");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.SubjectAbbreviation)
                    .HasName("SubjectAbbreviation");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FirstName).HasColumnType("varchar(100)");

                entity.Property(e => e.LastName).HasColumnType("varchar(100)");

                entity.Property(e => e.SubjectAbbreviation)
                    .IsRequired()
                    .HasColumnType("varchar(4)");

                entity.HasOne(d => d.SubjectAbbreviationNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.SubjectAbbreviation)
                    .HasConstraintName("Students_ibfk_1");
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.AssignmentId, e.SubmitTime })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AssignmentId)
                    .HasName("AssignmentId");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.AssignmentId).HasColumnType("varchar(5)");

                entity.Property(e => e.SubmitTime).HasColumnType("datetime");

                entity.Property(e => e.Contents).HasColumnType("mediumtext");

                entity.Property(e => e.Score).HasColumnType("int(11)");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.Submission)
                    .HasForeignKey(d => d.AssignmentId)
                    .HasConstraintName("Submission_ibfk_2");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Submission)
                    .HasForeignKey(d => d.UId)
                    .HasConstraintName("Submission_ibfk_1");
            });
        }
    }
}
