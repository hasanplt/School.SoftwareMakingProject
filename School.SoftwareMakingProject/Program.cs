using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using School.SoftwareMakingProject;
using School.SoftwareMakingProject.Helpers.JWT;
using School.SoftwareMakingProject.Persistence.Interfaces;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigurationAppManager._config = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.Cookie.Name = "project.exam.session";
	options.IdleTimeout = TimeSpan.FromDays(1);
	options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
var type = typeof(IRepository);

var interfacesRepository = AppDomain.CurrentDomain.GetAssemblies()
	.SelectMany(x => x.GetTypes())
	.Where(x => x.GetInterfaces().Contains(type))
	.ToList();

foreach (var repoInterface in interfacesRepository)
{
	if (repoInterface == null)
		continue;
	var name = repoInterface.Name.Replace("I", "");

	var repo = interfacesRepository
		.Where(x => x.GetInterfaces().Contains(repoInterface)).FirstOrDefault();

	if (repo == null)
		continue;

	builder.Services.AddTransient(repoInterface, repo);
}
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = "ProjectExam",
					ValidAudience = "ProjectExam",
					IssuerSigningKey =
							JwtSecurityKey.Create(ConfigurationAppManager._config.GetSection("PrivateTokenKey").Value)
				};

				options.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = context => {
						Console.WriteLine("OnAuthenticationFailed :" + context.Exception.Message);
						return Task.CompletedTask;
					},
					OnTokenValidated = context => {
						Console.WriteLine("OnTokenValidated :" + context.SecurityToken);
						return Task.CompletedTask;
					},
				};
			});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseSession();

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
	var token = context.Session.GetString("Token");
	if (!string.IsNullOrEmpty(token))
	{
		context.Request.Headers.Add("Authorization", "Bearer " + token);
	}
	await next();
});

app.UseStatusCodePages(context =>
{
	var response = context.HttpContext.Response;
	var request = context.HttpContext.Request;

	if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
		response.StatusCode == (int)HttpStatusCode.Forbidden ||
		response.StatusCode == (int)HttpStatusCode.NotFound)
	{
		response.Redirect("/auth");
	}
	return Task.CompletedTask;
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
