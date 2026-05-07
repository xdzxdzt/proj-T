using Microsoft.EntityFrameworkCore;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess
{
    public class TeachlyDbContext : DbContext
    {
        public TeachlyDbContext(DbContextOptions<TeachlyDbContext> options) : base(options)
        {
        }

        public DbSet<TutorFeedbackEntity> TutorFeedbacks { get; set; }
        public DbSet<ReviewEntity> Reviews { get; set; }
        public DbSet<SolutionEntity> Solutions { get; set; }
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<LessonPackageEntity> LessonPackages { get; set; }
        public DbSet<TutorTaskEntity> TutorTasks { get; set; }
        public DbSet<TutorEntity> Tutors { get; set; }
        public DbSet<TutorSubjectEntity> TutorSubjects { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<InstitutionEntity> Institutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeachlyDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
