using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorkshopAspNetCore.Models;
using Xunit;

namespace UnitTest
{
    public class PeopleControllerIntegrationTest : BaseIntegrationTest
    {

		private const string BaseUrl = "/api/People";
        public PeopleControllerIntegrationTest(BaseTestFixture fixture) : base(fixture)
        {

        }

		[Fact]
		public async Task ShouldReturnEmptyPeopleList()
		{
			var request = await Client.GetAsync(BaseUrl);
			request.EnsureSuccessStatusCode();

			var response = await request.Content.ReadAsStringAsync();

			var data = JsonConvert.DeserializeObject<List<Person>>(response);

			Assert.Equal(data.Count, 0);
		}

		[Fact]
		public async Task ShouldReturnPeopleList()
		{
			var person = new Person()
			{
				FirstName = "Teste",
				LastName = "123",
				Email = "teste@123.com"
			};
			
			await Context.AddAsync(person);
			await Context.SaveChangesAsync();

			var request = await Client.GetAsync(BaseUrl);
			request.EnsureSuccessStatusCode();

			var response = await request.Content.ReadAsStringAsync();

			var data = JsonConvert.DeserializeObject<List<Person>>(response);

			Assert.Equal(data.Count, 1);
			Assert.Contains(data, x => x.FirstName == person.FirstName);
		}
    }
}