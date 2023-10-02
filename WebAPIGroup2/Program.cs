using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TestEmail.Services;
using WebAPIGroup2.Models;
using WebAPIGroup2.Respository;
using WebAPIGroup2.Respository.Implement;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Implement;
using WebAPIGroup2.Service.Inteface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(new[] { "http://localhost:3000" })
               .AllowAnyHeader()
               .AllowAnyMethod();   
    });
});
builder.Services.AddDbContext<MyImageContext>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies"; // Middleware đăng nhập mặc định
    options.DefaultChallengeScheme = "Google"; // Middleware đăng nhập Google
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddCookie("Cookies", options =>
    {
        options.Cookie.HttpOnly = true;
        // options.LoginPath = "/api/User/login-google"; // Đặt đường dẫn trang đăng nhập của bạn
    })
    .AddGoogle(options =>
    {
        options.ClientId = "349595782448-43gvctriiege3k72basdtv2qhu3f1nbq.apps.googleusercontent.com";
        options.ClientSecret = "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";
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });
//Mail option appsetting.json
var mailSettings = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSetting>(mailSettings);

//DI Repositoty
builder.Services.AddScoped(typeof(GenericRepository<>));   
builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddTransient<ITemplateRepo, TemplateRepo>();
builder.Services.AddTransient<IDescriptionTemplateRepo, DescriptionTemplateRepo>();
builder.Services.AddTransient<ITemplateImageRepo, TemplateImageRepo>();
//DI Service
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddSingleton<IUtilService, UtilService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddTransient<ICategoryRepo, CategoryRepo>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ITemplateRepo, TemplateRepo>();
//builder.Services.AddTransient<ITemplateService, TemplateSerivce>();
builder.Services.AddTransient<ICategoryTemplateRepo,CategoryTemplateRepo>();
builder.Services.AddTransient<ISizeRepo, SizeRepo>();
builder.Services.AddTransient<ISizeService,SizeService>();
//DI Service
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddSingleton<IUtilService, UtilService>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "Image")), // Đường dẫn tương đối đến thư mục "Image"
    RequestPath = "/Image" // Đường dẫn URL sẽ được sử dụng để truy cập các tệp trong thư mục "Image"
});
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCookiePolicy();
app.UseAuthorization();
app.MapControllers();

app.Run();
