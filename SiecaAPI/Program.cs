using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using SiecaAPI.Commons;
using SiecaAPI.DssPro;
using SiecaAPI.OneDrive;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
// Swagger config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("OneDriveJob");
    q.AddJob<OneDriveServiceJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("OneDriveJob-trigger")
        .WithCronSchedule(builder.Configuration["JobsSchedule:OneDriveScheduleExp"]));
    
    /*var jobKeyDssProp = new JobKey("DssProJob");
    q.AddJob<DssProServiceJob>(opts => opts.WithIdentity(jobKeyDssProp));
    q.AddTrigger(opts => opts
        .ForJob(jobKeyDssProp)
        .WithIdentity("DssProJob-trigger")
        .WithCronSchedule(builder.Configuration["JobsSchedule:DssProScheduleExp"]));*/

});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

//authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel
            .Tokens.TokenValidationParameters
        {
            ValidateIssuer = bool.Parse(AppParamsTools.GetEnvironmentVariable("Jwt:ValidateIssuer")),
            ValidateAudience = bool.Parse(AppParamsTools.GetEnvironmentVariable("Jwt:ValidateAudience")),
            ValidateLifetime = bool.Parse(AppParamsTools.GetEnvironmentVariable("Jwt:ValidateLifetime")),
            ValidateIssuerSigningKey = bool.Parse(AppParamsTools.GetEnvironmentVariable("Jwt:ValidateIssuerSigningKey")),
            ValidIssuer = AppParamsTools.GetEnvironmentVariable("Jwt:Issuer"),
            ValidAudience = AppParamsTools.GetEnvironmentVariable("Jwt:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(AppParamsTools.GetEnvironmentVariable("Jwt:Key")))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});

app.Run();
