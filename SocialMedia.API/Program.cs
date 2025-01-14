using Microsoft.EntityFrameworkCore;
using SocialMedia.API.Data;
using SocialMedia.API.Hubs;
using SocialMedia.API.Repo;
using SocialMedia.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Add dbcontext and connect it to connection string "SocialMedia" or "Azure_SocialMedia" -> if you need my connectionstring just let me know
// Note about the azure db, it pauses when not and use and takes like 1-2 minutes to spin up the first time you make a connection request so don't panic if nothing happens.
builder.Services.AddDbContext<SocialMediaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineSocial"))
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

//Add Service for Jwt Bearer Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => 
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.UseHttpsRedirection();

// controller routing
app.MapControllers();

//Map the hubs
app.MapHub<SocialMediaHub>("/socialMediaHub");

app.Run();
