using Microsoft.EntityFrameworkCore;

namespace FeedbackApi;

public class FeedbackDbContext : DbContext
{
    public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options)
        : base(options)
    { 
    }

    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
}
