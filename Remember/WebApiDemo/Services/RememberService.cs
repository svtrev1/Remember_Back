using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace WebApiDemo.Services
{
    public class RememberService : IRememberService
    {
        private List<User> users;
        private List<Word> words;
        private List<Category> categories;

        private string usersFilePath = "/Users/svtrev/Desktop/6sem/Kursach_TRPO/Remember_Back/Remember/WebApiDemo/Json/users.json";
        private string wordsFilePath = "/Users/svtrev/Desktop/6sem/Kursach_TRPO/Remember_Back/Remember/WebApiDemo/Json/words.json";
        private string categoriesFilePath = "/Users/svtrev/Desktop/6sem/Kursach_TRPO/Remember_Back/Remember/WebApiDemo/Json/categories.json";
        public RememberService()
        {
            if (File.Exists(usersFilePath))
            {
                var usersData = File.ReadAllText(usersFilePath);
                users = JsonSerializer.Deserialize<List<User>>(usersData);
            }
            else
            {
                users = new List<User>();
            }

            if (File.Exists(wordsFilePath))
            {
                var wordsData = File.ReadAllText(wordsFilePath);
                words = JsonSerializer.Deserialize<List<Word>>(wordsData);
            }
            else
            {
                words = new List<Word>();
            }

            if (File.Exists(categoriesFilePath))
            {
                var categoriesData = File.ReadAllText(categoriesFilePath);
                categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
            }
            else
            {
                categories = new List<Category>();
            }
        }

        private void SaveData()
        {
            var usersData = JsonSerializer.Serialize(users);
            File.WriteAllText(usersFilePath, usersData);
            var wordsData = JsonSerializer.Serialize(words);
            File.WriteAllText(wordsFilePath, wordsData);
            var categoriesData = JsonSerializer.Serialize(categories);
            File.WriteAllText(categoriesFilePath, categoriesData);
        }

        // WORD

        public List<Word> GetAllWords()
        {
            return words;
        }

        public void AddWord(Word newWord)
        {
            words.Add(newWord);
            SaveData();
        }

        public List<Word> GetWordsInCategory(int category_id)
        {
            return words.FindAll(w => w.category_id == category_id);
        }
        public int GetCountWordsInCategory(int category_id)
        {
            return words.Count(w => w.category_id == category_id);
        }

        public Word GetWordById(int word_id)
        {
            return words.Find(c => c.id == word_id);
        }

        public void RemoveWord(Word word)
        {
            words.Remove(word);
            SaveData();
        }

        // CATEGORY

        public List<Category> GetAllCategories()
        {
            return categories;
        }

        public List<Category> GetCategoriesByUserId(int user_id)
        {
            return categories.FindAll(c => c.user_id == user_id);

        }

        public Category GetCategoryById(int category_id)
        {
            return categories.Find(c => c.id == category_id);
        }

        public void RemoveCategory(Category category)
        {
            categories.Remove(category);
            SaveData();
        }

        public void AddCategory(Category newCategory)
        {
            categories.Add(newCategory);
            SaveData();
        }

        // USER

        public User GetUserById(int id)
        {
            return users.Find(u => u.id == id);
        }
        public User GetUserByName(string username)
        {
            User temp = users.Find(u => u.name == username);
            if (temp == null)
            {
                return null;
            }
            return temp;
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        public void RemoveUser(User user)
        {
            users.Remove(user);
            SaveData();
        }

        public async Task<string> RegisterUser(User newUser)
        {
            if (users.Any(u => u.name == newUser.name))
            {
                return "Username already exist";
            }
            newUser.password = HashPassword(newUser.password);
            users.Add(newUser);
            SaveData();
            return "Register successfully!";
        }

        public async Task<int> LoginUser(User user)
        {
            user.password = HashPassword(user.password);
            User oldUser = users.FirstOrDefault(u => u.name == user.name);
            if (oldUser == null)
            {
                return -1;
            }
            if (oldUser.password != user.password)
            {
                return -2;
            }
            string token = Guid.NewGuid().ToString();
          
            SaveData();
            //return "Auth successfully!";
            return 1;
        }

        private string HashPassword(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Convert byte to hexadecimal string
                }

                return builder.ToString();
            }
        }
    }
}
