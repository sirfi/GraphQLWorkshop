using System.Text;
using GraphQL;
using GraphQL.Authorization;
using GraphQL.EntityFramework;
using GraphQL.Server;
using GraphQL.Validation;
using GraphQLWorkshop.Data.Contexts;
using GraphQLWorkshop.Data.Entities;
using GraphQLWorkshop.GraphQL;
using GraphQLWorkshop.GraphQL.Mutations;
using GraphQLWorkshop.GraphQL.Queries;
using GraphQLWorkshop.GraphQL.Schemas;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace GraphQLWorkshop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddDbContext<AppDbContext>((provider, builder) =>
                {
                    builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });
            services
                .AddIdentity<AppUser, AppRole>(options =>
                {

                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
            services.AddControllersWithViews();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Question.Read", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/Question/Read/Allow"));
                options.AddPolicy("QuestionCategory.Read", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/QuestionCategory/Read/Allow"));
                options.AddPolicy("QuestionAnswer.Read", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/QuestionAnswer/Read/Allow"));
                options.AddPolicy("Question.Write", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/Question/Write/Allow"));
                options.AddPolicy("QuestionCategory.Write", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/QuestionCategory/Write/Allow"));
                options.AddPolicy("QuestionAnswer.Write", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/QuestionAnswer/Write/Allow"));
            });

            EfGraphQLConventions.RegisterInContainer<AppDbContext>(
                services,
                model: AppDbContext.StaticModel);
            EfGraphQLConventions.RegisterConnectionTypesInContainer(services);
            services.AddSingleton<QuestionSchema>();
            services.AddSingleton<QuestionQuery>();
            services.AddSingleton<QuestionMutation>();
            services.AddGraphQL(options =>
                {
                    options.EnableMetrics = true;
                })
                .AddGraphTypes()
                .AddDataLoader()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddSystemTextJson()
                .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User });
            services.AddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>()
                .AddTransient<IValidationRule, AuthorizationValidationRule>()
                .AddTransient(s =>
                {
                    var authSettings = new AuthorizationSettings();
                    authSettings.AddPolicy("Question.Read", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/Question/Read/Allow"));
                    authSettings.AddPolicy("QuestionCategory.Read", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/QuestionCategory/Read/Allow"));
                    authSettings.AddPolicy("QuestionAnswer.Read", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/QuestionAnswer/Read/Allow"));
                    authSettings.AddPolicy("Question.Write", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/Question/Write/Allow"));
                    authSettings.AddPolicy("QuestionCategory.Write", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/QuestionCategory/Write/Allow"));
                    authSettings.AddPolicy("QuestionAnswer.Write", builder => builder.RequireAuthenticatedUser().RequireClaim("Permission", "App/QuestionAnswer/Write/Allow"));
                    return authSettings;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseGraphQL<QuestionSchema>();

            app.UseGraphQLPlayground();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
