namespace PatientManagerWebAPI.Data;

using Microsoft.EntityFrameworkCore;
using PatientManagerWebAPI.Model;


public class AppDbContext : DbContext
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    OnModelUser(modelBuilder);
    OnModelPatient(modelBuilder);
    OnModelRecommendation(modelBuilder);
  }

  private void OnModelUser(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>().ToTable("pm_user");
    modelBuilder.Entity<User>().HasKey(user => user.Id);
    modelBuilder.Entity<User>().
        Property(user => user.Id).HasColumnName("id");
    modelBuilder.Entity<User>().
        Property(user => user.Name).HasColumnName("name");
    modelBuilder.Entity<User>().
        Property(user => user.PhoneNumber).HasColumnName("phone_number");
    modelBuilder.Entity<User>().
        Property(user => user.Email).HasColumnName("email");
    modelBuilder.Entity<User>().
        Property(user => user.Role).HasColumnName("role").
        HasConversion<string>();
    modelBuilder.Entity<User>().
        Property(user => user.Login).HasColumnName("login");
    modelBuilder.Entity<User>().
        Property(user => user.Password).HasColumnName("password");
    modelBuilder.Entity<User>().
        Property(user => user.CreatedAt).HasColumnName("created_at");
  }

  private void OnModelPatient(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Patient>().ToTable("pm_patient");
    modelBuilder.Entity<Patient>().HasKey(patient => patient.Id);
    modelBuilder.Entity<Patient>().
        Property(patient => patient.Id).HasColumnName("id");
    modelBuilder.Entity<Patient>().
        Property(patient => patient.Name).HasColumnName("name");
    modelBuilder.Entity<Patient>().
        Property(patient => patient.Birthdate).HasColumnName("birthdate");
    modelBuilder.Entity<Patient>().
        Property(patient => patient.PhoneNumber).HasColumnName("phone_number");
    modelBuilder.Entity<Patient>().
        Property(patient => patient.Email).HasColumnName("email");
    modelBuilder.Entity<Patient>().
        Property(patient => patient.CreatedAt).HasColumnName("created_at");
  }

  private void OnModelRecommendation(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Recommendation>().ToTable("pm_recommendation");
    modelBuilder.Entity<Recommendation>().HasKey(r => r.Id);
    modelBuilder.Entity<Recommendation>().
        HasOne<Patient>().WithMany().HasForeignKey(r => r.PatientId);
    modelBuilder.Entity<Recommendation>().
        Property(r => r.Id).HasColumnName("id");
    modelBuilder.Entity<Recommendation>().
        Property(r => r.PatientId).HasColumnName("patient_id");
    modelBuilder.Entity<Recommendation>().
        Property(r => r.Description).HasColumnName("description");
    modelBuilder.Entity<Recommendation>().
        Property(r => r.Completed).HasColumnName("completed");
    modelBuilder.Entity<Recommendation>().
        Property(r => r.CreatedAt).HasColumnName("created_at");
  }

  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}


  public DbSet<User> User {get; set;}
  public DbSet<Patient> Patient {get; set;}
  public DbSet<Recommendation> Recommendation {get; set;}
}