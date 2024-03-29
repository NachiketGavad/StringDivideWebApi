using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StringDivideWebApp.Models;
using System.Security.Cryptography;
using System.Text;

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
                // Hash the password before saving
                string hashedPassword = HashPassword(std.password);
                std.PasswordHash = hashedPassword;

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
            //var existingUser = _context.StringDivideAppUsers.FirstOrDefault(u => u.email == updatedUser.email);

            if (existingUser == null)
            {
                return NotFound();
            }

            // Check if the updated email is available
            var otherUserWithSameEmail = _context.StringDivideAppUsers.FirstOrDefault(u => u.Id != id && u.email == updatedUser.email);
            if (otherUserWithSameEmail != null)
            {
                return Conflict("Email already in use by another user.");
            }

            // Hash the password before saving
            string hashedPassword = HashPassword(updatedUser.password);
            updatedUser.PasswordHash = hashedPassword;

            existingUser.name = updatedUser.name;
            existingUser.email = updatedUser.email;
            existingUser.password = updatedUser.password;
            existingUser.PasswordHash = updatedUser.PasswordHash;

            //existingUser = updatedUser;

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


        // Method to hash the password using a hashing algorithm (e.g., SHA256)
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
