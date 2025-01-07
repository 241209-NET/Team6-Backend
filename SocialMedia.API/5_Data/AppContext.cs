using Microsoft.EntityFrameworkCore;
using SocialMedia.API.Model;

namespace SocialMedia.API.Data;

public partial class SocialMediaContext : DbContext
{
    public SocialMediaContext() { }

    public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
        : base(options) { }

    public virtual DbSet<User>? Users { get; set; } = null!;
    public virtual DbSet<Tweet>? Tweets { get; set; } = null!;
}
