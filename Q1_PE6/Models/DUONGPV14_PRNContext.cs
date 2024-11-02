using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Q1_PE6.Models
{
    public partial class DUONGPV14_PRNContext : DbContext
    {
        public DUONGPV14_PRNContext()
        {
        }

        public DUONGPV14_PRNContext(DbContextOptions<DUONGPV14_PRNContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Lecturer> Lecturers { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentSubject> StudentSubjects { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database = DUONGPV14_PRN ; uid=nta1310;pwd=12345678;Trusted_Connection=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.ClassName)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.FullName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Note).HasColumnType("ntext");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.FullName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Nationality).HasColumnType("ntext");

                entity.Property(e => e.Note).HasColumnType("ntext");

                entity.HasOne(d => d.Lecture)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.LectureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Students_Lecturers");

                entity.HasMany(d => d.Classes)
                    .WithMany(p => p.Students)
                    .UsingEntity<Dictionary<string, object>>(
                        "StudentClass",
                        l => l.HasOne<Class>().WithMany().HasForeignKey("ClassId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Student_Class_Classes"),
                        r => r.HasOne<Student>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Student_Class_Students"),
                        j =>
                        {
                            j.HasKey("StudentId", "ClassId");

                            j.ToTable("Student_Class");
                        });
            });

            modelBuilder.Entity<StudentSubject>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.SubjectId });

                entity.ToTable("Student_Subject");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentSubjects)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_Subject_Students");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.StudentSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_Subject_Subjects");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Subject1)
                    .HasMaxLength(10)
                    .HasColumnName("Subject")
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
