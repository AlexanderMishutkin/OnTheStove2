﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ObjectsLibrary;
using ObjectsLibrary.Components;
using SQLite;

namespace XamarinAppLibrary
{
    public static class RecipeData
    {
        private static string path =
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "onstove.db3");
        private static string GetFileRecipeName(string url)
        {
            // Пример: https://www.povarenok.ru/recipes/show/163893/
            if (url.Contains("https://www.povarenok.ru"))
            {
                return "pk" + url[38..^1];
            }
            // Пример: https://povar.ru/recipes/postnyi_apelsinovyi_keks-80038.html
            else if (url.Contains("https://povar.ru"))
            {
                return "pr" + url.Split('-')[^1].Split('.')[0];
            }
            // Пример: https://www.edimdoma.ru/retsepty/137347-syrnik-s-izyumom-i-tsukatami
            else if (url.Contains("https://www.edimdoma.ru"))
            {
                return "edm" + url[33..^1].Split('-')[0];
            }
            // Пример: https://eda.ru/recepty/zavtraki/amerikanskie-bliny-30600
            else if (url.Contains("https://eda.ru/"))
            {
                return "eda" + url.Split('-')[^1];
            }
            return url[8..^1];
        }

        public static RecipeShort GetRecipe(string url)
        {
            string fileName = GetFileRecipeName(url);

            var db = new SQLiteConnection(dbPath);
            byte[] recipe = db.Table<RecipeTable>().First(x => x.Name == fileName).Recipe;

            return Data.ByteArrayToObject<RecipeShort>(recipe);

        }

        public static RecipeShort[] GetArrayRecipes()
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<RecipeTable>();

            var recipes = db.Table<RecipeTable>().ToArray();

            RecipeShort[] recipeShorts = new RecipeShort[recipes.Length];

            for (int i = 0; i < recipes.Length; i++)
            {
                recipeShorts[i] = Data.ByteArrayToObject<RecipeShort>(recipes[i].Recipe);
            }

            return recipeShorts;
        }

        public static bool ExistsRecipe(string url)
        {
            string fileName = GetFileRecipeName(url);

            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

            var db = new SQLiteConnection(dbPath);
            db.CreateTable<RecipeTable>();

            if (db.Table<RecipeTable>().Count() == 0)
                return false;

            return db.Table<RecipeTable>().FirstOrDefault(x => x.Name == fileName) == null ? false : true;

        }


        public static void DeleteRecipe(string url)
        {
            string fileName = GetFileRecipeName(url);

            var db = new SQLiteConnection(dbPath);
            db.CreateTable<RecipeTable>();

            int id = db.Table<RecipeTable>().First(x => x.Name == fileName).Id;

            db.Delete<RecipeTable>(id);

        }


        public static void SaveRecipe(string url, RecipeShort recipeShort)
        {
            string fileName = GetFileRecipeName(url);

            var db = new SQLiteConnection(dbPath);

            db.CreateTable<RecipeTable>();

            RecipeTable recipeTable = new RecipeTable();

            recipeTable.Name = fileName;
            recipeTable.Recipe = Data.RecipeToByteArray(recipeShort);

            db.Insert(recipeTable);

        }



    }
}