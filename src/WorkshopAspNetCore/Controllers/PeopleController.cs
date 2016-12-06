using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkshopAspNetCore
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private readonly DataContext _data;

        public PeopleController(DataContext dataContext)
        {
            _data = dataContext;
        }

        public async Task<IActionResult> GetPeople()
        {
            var people = await _data.People.ToListAsync();

            return Ok(people);
        }

        [HttpPost]
        public async Task<IActionResult> PostPeople([FromBody] Person person)
        {
            var newPerson = await _data.People.AddAsync(person);
            await _data.SaveChangesAsync();

            return Ok();
        }
    }
}
