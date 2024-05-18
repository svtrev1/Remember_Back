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
        public IActionResult addCategory (string category_name, int user_id)
        {
            var temp = chatService.GetAllCategories().FirstOrDefault(c => c.name.ToUpper() == category_name.ToUpper());
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
                name = category_name,
                user_id = user_id
            };
            chatService.AddCategory(newCategory);
            return Ok(newCategory);
        }

        [HttpGet("checkUserCategory")]
        public IActionResult CheckUserCategory(int category_id, int user_id)
        {
            var category = chatService.GetCategoryById(category_id);
            if (category == null)
            {
                return NotFound("Category isn't found!");
            }
            var temp = chatService.CheckUserCategory(category_id, user_id);
            return Ok(temp);
        }
        [HttpPost("copyCategory")]
        public IActionResult CopyCategory(int category_id, int user_id)
        {
            var sourceCategory = chatService.GetCategoryById(category_id);
            if (sourceCategory == null)
            {
                return NotFound("Category not found");
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
                name = sourceCategory.name,
                user_id = user_id
            };
            chatService.AddCategory(newCategory);
            int lastIdWord;
            if (chatService.GetAllWords().Count > 0)
            {
                lastIdWord = chatService.GetAllWords().Max(u => u.id);
            }
            else
            {
                lastIdWord = 0;
            }
            var sourceWords = chatService.GetWordsInCategory(category_id);
            foreach (var sourceWord in sourceWords)
            {
                lastIdWord++;
                Word newWord = new()
                {
                    id = lastIdWord,
                    category_id = newCategory.id,
                    english = sourceWord.english,
                    russian = sourceWord.russian
                };
                chatService.AddWord(newWord);
            }
            return Ok(newCategory.id);
        }
    }
}