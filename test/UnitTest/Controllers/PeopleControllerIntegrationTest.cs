using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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

		[Fact]
		public async Task ShouldCreatePerson()
		{
			var person = new Person()
			{
				FirstName = "Integration",
				LastName = "Test",
				Email = "integration@test.com"
			};

			var stringContent = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");
			var request = await Client.PostAsync(BaseUrl, stringContent);

			Assert.Equal(HttpStatusCode.Created, request.StatusCode);

			var response = await request.Content.ReadAsStringAsync();
			var createdPerson = JsonConvert.DeserializeObject<Person>(response);

			Assert.Equal(person.FirstName, createdPerson.FirstName);
			Assert.Equal(person.LastName, createdPerson.LastName);
			Assert.Equal(person.Email, createdPerson.Email);
			Assert.True(createdPerson.Id > 0);
		}

		[Fact]
		public async Task ShouldFailToCreatePersonWithInvalidData()
		{
			var person = new Person()
			{
				FirstName = null, // Invalid data
				LastName = "Test",
				Email = "invalid@test.com"
			};

			var stringContent = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");
			var request = await Client.PostAsync(BaseUrl, stringContent);

			Assert.Equal(HttpStatusCode.BadRequest, request.StatusCode);
		}
    }
}