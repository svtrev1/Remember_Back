using Microsoft.AspNetCore.Mvc;
namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly Services.IRememberService chatService;
        public CategoryController(Services.IRememberService services)
        {
            chatService = services;
        }

        [HttpGet("categories")] 
        public IActionResult GetCategories()
        {
            var categories = chatService.GetAllCategories();
            if (categories.Count == 0)
            {
                return NotFound("No categories found!");
            }
            return Ok(categories);
        }

        [HttpGet("categoryNameById")]
        public IActionResult GetCategoryNameById(int category_id)
        {
            var categories = chatService.GetCategoryNameById(category_id);
            if (categories == null)
            {
                return NotFound("Category isn't found!");
            }
            return Ok(categories);
        }

        [HttpGet("categoriesByUserId")]
        public IActionResult GetCategoriesByUserId(int user_id)
        {
            var categories = chatService.GetCategoriesByUserId(user_id);
            if (categories.Count == 0)
            {
                return NotFound("No categories found!");
            }
            return Ok(categories);
        }

        [HttpDelete("{category_id}")]
        public IActionResult DeleteCategory (int category_id)
        {
            var category = chatService.GetCategoryById(category_id);
            if (category == null)
            {
                return NotFound("Category to remove not found!");
            }
            chatService.RemoveCategory(category);
            return Ok("Category deleted successfully");
        }

        [HttpPost("addCategory")]
        public IActionResult addCategory (string name, int user_id)
        {
            var temp = chatService.GetAllCategories().FirstOrDefault(c => c.name == name);
            if (temp != null)
            {
                return NotFound("Category with this name is already exist");
            }
            int lastId;
            if (chatService.GetAllCategories().Count > 0)
            {
                lastId = chatService.GetAllCategories().Max(c => c.id);
            }
            else
            {
                lastId = 0;
            }
            Category newCategory = new()
            {
                id = lastId + 1,
                name = name,
                user_id = user_id
            };
            chatService.AddCategory(newCategory);
            return Ok(newCategory);
        }
    }
}