using ProjectCore.DAL;
using ProjectCore.Web.Middlewares;
using ProjectCore.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped(_ => new DBContext().CreateDbConnection());
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<AuthTokenCache>();
builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "Ver:1.0.0",
        Title = "学生管理系统",
        Description = "学生管理系统：包括学生列表、年级管理等。",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "UserName",
            Email = "***@hotmail.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // 将生成的 JSON 与 UI 界面上显示的版本名称绑定
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "学生信息API V1.0");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<RequestFilterController>();
app.UseMiddleware<EncryptFilterController>();
app.UseMiddleware<BearerTokenMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
