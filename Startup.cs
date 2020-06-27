using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bookstore.Models;
using Bookstore.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bookstore
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IBookstoreRepository<Author>,AuthorRepository>();
            services.AddScoped<IBookstoreRepository<Book>, BookRepository>();
            services.AddDbContext<BookstoreDBContext>(options => 
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                //options.User.RequireUniqueEmail = false;
               // Identity password complexity
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireNonAlphanumeric = false;

            })
            .AddEntityFrameworkStores<BookstoreDBContext>(); //ApplicationDbContext

            services.AddMvc(options=> {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                //options.EnableEndpointRouting = false;
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMvcWithDefaultRoute();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Author}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //app.UseMvc(route => {

            //    route.MapRoute("", "");
            //});


        }

 
    }
}
