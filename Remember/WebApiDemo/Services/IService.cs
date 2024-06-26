﻿using System;
using System.Collections.Generic;
namespace WebApiDemo.Services
{
    public interface IRememberService
    {
        User GetUserById(int id);
        User GetUserByName(string username);
        List<User> GetAllUsers();
        void RemoveUser(User user);
        Task<string> RegisterUser(User user);
        Task<int> LoginUser(User user);

        List<Word> GetAllWords();
        int AddWord(Word newWord);
        List<Word> GetWordsInCategory(int category_id);
        int GetCountWordsInCategory(int category_id);
        Word GetWordById(int word_id);
        void RemoveWord(Word word);

        List<Category> GetAllCategories();
        List<Category> GetCategoriesByUserId(int user_id);
        string GetCategoryNameById(int category_id);
        Category GetCategoryById(int category_id);
        void RemoveCategory(Category category);
        void AddCategory(Category newCategory);
        bool CheckUserCategory(int category_id, int user_id);
    }
}
