using api.Entities;
using api.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<PermissionRepository>();
builder.Services.AddScoped<TenantRepository>();
builder.Services.AddScoped<FormTypeRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RolePermissionRepository>();



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllers().AddOData(options => options.Select()
    .Filter()
    .OrderBy());

// builder.Services.AddControllers().AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()).Filter().Select());

var secretKey = builder.Configuration["AppSettings:SecretKey"];
var secretKeyBytes = UTF8Encoding.UTF8.GetBytes(secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// IEdmModel GetEdmModel()
// {
//     var builder = new ODataConventionModelBuilder();
//     builder.EntitySet<User>("Users");
//     return builder.GetEdmModel();
// }