using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StringDivideWebApp.Models;

namespace StringDivideWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StringDivideUserController : ControllerBase
    { 
        private readonly ApplicationDbContext _context;
        public StringDivideUserController(ApplicationDbContext  context)
        {
            _context = context;
        }

        [HttpGet]   
        public ActionResult<List<StringDivideAppUser>> GetUsers()
        {
            var data = _context.StringDivideAppUsers.ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]   
        public ActionResult<StringDivideAppUser> GetUserbyid(int id)
        {
            var myuser = _context.StringDivideAppUsers.Find(id);
            if(myuser == null)
            {
                return NotFound();
            }
            return Ok(myuser);
        }

        [HttpPost]
        public ActionResult<StringDivideAppUser> PostUser(StringDivideAppUser std)
        {
            var existingUser = _context.StringDivideAppUsers.FirstOrDefault(u => u.email == std.email);

            if (existingUser == null)
            {
                _context.StringDivideAppUsers.Add(std);
                _context.SaveChanges();
                return Ok(std);
            }
            else
            {
                // User with the same email already exists
                return Conflict("User with the same email already exists.");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<StringDivideAppUser> UpdateUser(int id, StringDivideAppUser updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existingUser = _context.StringDivideAppUsers.Find(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.name = updatedUser.name;
            existingUser.email = updatedUser.email;
            existingUser.password = updatedUser.password;

            _context.SaveChanges();

            return Ok(existingUser);
        }

        [HttpDelete("{id}")]
        public ActionResult<StringDivideAppUser> DeleteUser(int id)
        {
            var existingUser = _context.StringDivideAppUsers.Find(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _context.StringDivideAppUsers.Remove(existingUser);
            _context.SaveChanges();

            return Ok(existingUser);
        }
    }
}
