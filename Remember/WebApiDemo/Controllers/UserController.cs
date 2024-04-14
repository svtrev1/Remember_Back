using Microsoft.AspNetCore.Mvc;
namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Services.IRememberService chatService;
        public UserController(Services.IRememberService services)
        {
            chatService = services;
        }

        [HttpGet("users")] //получение всех пользователей
        public IActionResult GetUsers()
        {
            var users = chatService.GetAllUsers();
            if (users.Count == 0)
            {
                return NotFound("No users found!");
            }
            return Ok(users);
        }

        [HttpGet("{user_id}")] //нахождение пользователя по его id
        public IActionResult GetUserById(int user_id)
        {
            var user = chatService.GetUserById(user_id);
            if (user == null)
            {
                return NotFound("User not found!");
            }
            return Ok(user);
        }

        [HttpDelete("{user_id}")] //удаление пользователя по его id
        public ActionResult DeleteUserById(int user_id)
        {

            var user = chatService.GetUserById(user_id);
            if (user == null)
            {
                return NotFound("User to remove not found!");
            }
            chatService.RemoveUser(user);
            return Ok("User deleted successfully");
        }

        //Auth
        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            int lastId;
            if (chatService.GetAllUsers().Count > 0)
            {
                lastId = chatService.GetAllUsers().Max(u => u.id);
            }
            else
            {
                lastId = 0;
            }
            User newUser = new()
            {
                id = lastId + 1,
                name = username,
                password = password
            };
            string result = await chatService.RegisterUser(newUser);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            User newUser = new()
            {
                id = 0,
                name = username,
                password = password
            };
            int token = await chatService.LoginUser(newUser);
            if (token == -1)
            {
                return BadRequest("User not found!");
            }
            if (token == -2)
            {
                return BadRequest("Password is wrong");
            }
            return Ok(token);
        }
    }
}