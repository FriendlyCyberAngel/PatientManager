using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PatientManagerWebAPI.Data;
using PatientManagerWebAPI.Error;
using PatientManagerWebAPI.Service;


var builder = WebApplication.CreateBuilder(args);
{
  builder.Services.AddControllers(options =>
  {
    options.Filters.Add<HttpResponseExceptionFilter>();
  }).
  AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
  });
  builder.Services.AddDbContext<AppDbContext>(optionsBuilder => 
  {
    optionsBuilder.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"));
  });

  // Services
  builder.Services.AddScoped<IUserService, UserService>();
  builder.Services.AddScoped<IPatientService, PatientService>();
  builder.Services.AddScoped<IRecommendationService, RecommendationService>();

  // Repositories
  builder.Services.AddScoped<IUserRepository, UserRepository>();
  builder.Services.AddScoped<IPatientRepository, PatientRepository>();
  builder.Services.AddScoped<IRecommendationRepository, RecommendationRepository>();

  // Encoders
  builder.Services.AddSingleton(HtmlEncoder.Default);
  builder.Services.AddSingleton(JavaScriptEncoder.Default);
  builder.Services.AddSingleton(UrlEncoder.Default);


  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.UseSwaggerUI(c => c.SwaggerEndpoint("swagger/v1/swagger.json", "Web API V1"));
  }

  app.UseExceptionHandler("/api/error");
  app.UseHttpsRedirection();
  app.MapControllers();
  app.Run();
}
