using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WorkshopAspNetCore;
using WorkshopAspNetCore.Models;

namespace UnitTest
{
	public class BaseTestFixture : IDisposable
	{

		public readonly TestServer Server;
		public readonly HttpClient Client;
		public readonly DataContext Context;

		protected readonly IConfigurationRoot Configuration;

		public BaseTestFixture()
		{
			var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			var builder = new ConfigurationBuilder()
						.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
						.AddJsonFile($"appsettings.{envName}.json", optional: true)
						.AddEnvironmentVariables();

			Configuration = builder.Build();

			var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
			optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			Context = new DataContext(optionsBuilder.Options);
			SetupDatabase();

			Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

			Client = Server.CreateClient();
		}

        private void SetupDatabase()
        {
            try
            {
                Context.Database.EnsureCreated();
                Context.Database.Migrate();
            }
            catch
            {
                // TODO: 
            }
        
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseTestFixture() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
            Context.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
        #endregion

    }
}