using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GraphQLWorkshop.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GraphQLWorkshop.Data.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        #region base
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
            foreach (EntityEntry entry in entries)
            {
                if (!(entry.Entity is ITrackable trackable)) continue;
                DateTime now = DateTime.UtcNow;
                int? userId = GetCurrentUserId();
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Properties.First(x => x.Metadata.Name == "CreatedAt").IsModified = false;
                        entry.Properties.First(x => x.Metadata.Name == "CreatedBy").IsModified = false;
                        trackable.LastUpdatedAt = now;
                        if (trackable.LastUpdatedBy == null)
                            trackable.LastUpdatedBy = userId;
                        break;

                    case EntityState.Added:
                        trackable.CreatedAt = now;
                        if (trackable.CreatedBy == null)
                            trackable.CreatedBy = userId;
                        trackable.LastUpdatedAt = now;
                        if (trackable.LastUpdatedBy == null)
                            trackable.LastUpdatedBy = userId;
                        break;
                }
            }
        }

        private int? GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString != null && int.TryParse(userIdString, out int userId))
                return userId;
            return null;
        }
        #endregion
        public DbSet<QuestionCategory> QuestionCategories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }

        public static IModel StaticModel { get; } = BuildStaticModel();
        private static IModel BuildStaticModel()
        {
            DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer("Fake");
            AppDbContext dbContext = new AppDbContext(builder.Options, new HttpContextAccessor());
            return dbContext.Model;
        }
    }
}
