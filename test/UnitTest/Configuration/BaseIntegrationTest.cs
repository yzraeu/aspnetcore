using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using WorkshopAspNetCore.Models;
using Xunit;

namespace UnitTest
{
	[Collection("Base Collection")]
	public abstract class BaseIntegrationTest
	{
		protected readonly TestServer Server;
		protected readonly HttpClient Client;
		protected readonly DataContext Context;

		protected BaseTestFixture Fixture{ get; }

		protected BaseIntegrationTest(BaseTestFixture fixture)
		{
			this.Fixture = fixture;

			this.Context = fixture.Context;
			this.Server = fixture.Server;
			this.Client = fixture.Client;

			ClearDatabase().Wait();
		}

		private async Task ClearDatabase()
		{
			var commands = new[]{
				"EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'",
				"EXEC sp_MSForEachTable 'DELETE FROM ?'",
				"EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'"
			};

			await Context.Database.OpenConnectionAsync();

			foreach (var command in commands)
			{
				await Context.Database.ExecuteSqlCommandAsync(command);
			}

			Context.Database.CloseConnection();
		}
	}
}