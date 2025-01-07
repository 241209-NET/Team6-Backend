using Microsoft.EntityFrameworkCore;
using SocialMedia.API.Data;
using SocialMedia.API.Hubs;
using SocialMedia.API.Repo;
using SocialMedia.API.Service;

var builder = WebApplication.CreateBuilder(args);

//Add dbcontext and connect it to connection string
builder.Services.AddDbContext<SocialMediaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SocialMedia"))
);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//SignalR adding
builder.Services.AddSignalR();

//Dependency Inject the proper services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITweetService, TweetService>();

//Dependency Inject the proper repositories
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ITweetRepo, TweetRepo>();

//Add controller
builder.Services.AddControllers();

//Adding Cors to allow front end
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173") // specify your React dev origin
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // allow credentials
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// controller routing
app.MapControllers();

//Map the hubs
app.MapHub<SocialMediaHub>("/socialMediaHub");

app.Run();
