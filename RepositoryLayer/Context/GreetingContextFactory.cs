using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RepositoryLayer.Context
{
        public class GreetingContextFactory : IDesignTimeDbContextFactory<GreetingContext>
        {
            public GreetingContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath("C:\\Users\\Lenovo\\OneDrive\\Desktop\\UC2\\GreetingApp_2115100002\\HelloGreetingApplication")
                    .AddJsonFile("appsettings.json")
                    .Build();

                var builder = new DbContextOptionsBuilder<GreetingContext>();
                var connectionString = configuration.GetConnectionString("SqlConnection");

                builder.UseSqlServer(connectionString);

                return new GreetingContext(builder.Options);
            }
        }
    }
