﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeLibrary.Parse
{
    public class HtmlLoader
    {
        private readonly HttpClient client;
        private readonly string url;

        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();

            url = "доделать";
            // TODO: url =
               
            

        }

        public async Task<string> GetSource(int idPage)
        {
            var currentUrl = url.Replace("{IdPage}", idPage.ToString());
            var response = await client.GetAsync(currentUrl);

            Console.WriteLine($"Загружаю страничку: {currentUrl}");

            string source;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
                source = await response.Content.ReadAsStringAsync();
            else
                throw new ParserException("Страница не может быть загружена.");

            return source;
        }
    }
}
