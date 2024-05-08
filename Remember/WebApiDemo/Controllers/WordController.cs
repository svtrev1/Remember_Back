using Microsoft.AspNetCore.Mvc;
namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordController : ControllerBase
    {
        private readonly Services.IRememberService chatService;
        public WordController(Services.IRememberService services)
        {
            chatService = services;
        }

        [HttpGet("words")] 
        public IActionResult GetWords()
        {
            var words = chatService.GetAllWords();
            if (words.Count == 0)
            {
                return NotFound("No words found!");
            }
            return Ok(words);
        }

        [HttpGet("wordsByCategotyId")]
        public IActionResult GetWordsInCategory(int category_id)
        {
            var words = chatService.GetWordsInCategory(category_id);
            if (words.Count == 0)
            {
                return NotFound("No words found!");
            }
            return Ok(words);
        }

        [HttpGet("countWordsByCategotyId")]
        public IActionResult GetCountWordsInCategory(int category_id)
        {
            var words = chatService.GetCountWordsInCategory(category_id);
            return Ok(words);
        }

        [HttpDelete("{word_id}")]
        public IActionResult DeleteWord (int word_id)
        {
            var word = chatService.GetWordById(word_id);
            if (word == null)
            {
                return NotFound("Word to remove not found!");
            }
            chatService.RemoveWord(word);
            return Ok("Word deleted successfully");
        }

        [HttpPost("addWord")]
        public IActionResult addWord (string english, string russian, int category_id)
        {
            int lastId;
            if (chatService.GetAllWords().Count > 0)
            {
                lastId = chatService.GetAllWords().Max(u => u.id);
            }
            else
            {
                lastId = 0;
            }
            Word newWord = new()
            {
                id = lastId + 1,
                category_id = category_id,
                english = english,
                russian = russian
            };
            chatService.AddWord(newWord);
            return Ok(newWord);
        }
    }
}