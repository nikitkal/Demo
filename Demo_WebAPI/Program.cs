using Demo_WebAPI.Data;
using Demo_WebAPI.Interface;
using Demo_WebAPI.Middleware;
using Demo_WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddScoped<IEmployee, EmployeeService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>();
//builder.Services.AddDbContext<DbContext>();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else {
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler(
        option => {

            option.Run(
                async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    if (ex != null)
                    {
                        await context.Response.WriteAsync(ex.Error.Message);
                    }
                }
                );
            }
        );
}
app.UseAuthentication();
app.UseAuthorization();

//1st way to implement exception handling
//app.Use(async(context, next) =>
//{
//    try
//    {
//        await next(context);

//    }
//    catch (Exception e)
//    {

//        context.Response.StatusCode = 500;
//    }
//});


//2nd way to implemant exception handling through Global excecption handler
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

//3rd way to implement exception handling through custom middleware


app.MapControllers();

app.Run();
