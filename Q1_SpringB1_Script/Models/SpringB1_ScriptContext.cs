using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Q1_SpringB1_Script.Models
{
    public partial class SpringB1_ScriptContext : DbContext
    {
        public SpringB1_ScriptContext()
        {
        }

        public SpringB1_ScriptContext(DbContextOptions<SpringB1_ScriptContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<EmployeeProject> EmployeeProjects { get; set; } = null!;
        public virtual DbSet<EmployeeSkill> EmployeeSkills { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database = SpringB1_Script; uid=nta1310;pwd=12345678;Trusted_Connection=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DepartmentName).HasMaxLength(255);

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK_Department_Manager");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Position).HasMaxLength(255);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Employees__Depar__398D8EEE");
            });

            modelBuilder.Entity<EmployeeProject>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.ProjectId })
                    .HasName("PK__Employee__6DB1E41C1EF16937");

                entity.ToTable("Employee_Projects");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.JoinDate).HasColumnType("date");

                entity.Property(e => e.LeaveDate).HasColumnType("date");

                entity.Property(e => e.Role).HasMaxLength(255);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeProjects)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employee___Emplo__412EB0B6");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.EmployeeProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employee___Proje__4222D4EF");
            });

            modelBuilder.Entity<EmployeeSkill>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.SkillId })
                    .HasName("PK__Employee__172A46EF1F980EE7");

                entity.ToTable("Employee_Skills");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.Property(e => e.AcquiredDate).HasColumnType("date");

                entity.Property(e => e.ProficiencyLevel).HasMaxLength(255);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeSkills)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employee___Emplo__44FF419A");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.EmployeeSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employee___Skill__45F365D3");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.Budget).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.ProjectName).HasMaxLength(255);

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.Property(e => e.SkillName).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
