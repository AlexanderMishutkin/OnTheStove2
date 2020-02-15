﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeLibrary.Objects;
using RecipeLibrary;
using RecipeLibrary.Objects.Boxes.Elements;

namespace WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PageController : ControllerBase
    {
        private readonly ILogger<PageController> _logger;

        public PageController(ILogger<PageController> logger)
        {
            _logger = logger;
        }

        // RESPONSE: section=<section>&recipeName=<recipe>&page=<page>
        //           recipeName=<recipe>&section=<section>&page=<page>
        [HttpGet("{response}", Name = "Response")]
        public IEnumerable<RecipeShort> GetByResponse(string response)
        {
            string section = String.Empty;
            string recipeName = String.Empty;
            int pageId = 1;


            string[] responses = response.Split('&');

            foreach (string res in responses)
            {
                string lineResponse = res.Substring(res.IndexOf('=') + 1).ToLower();

                if (lineResponse == "new" || lineResponse == "random" || lineResponse == "popular" ||
                    lineResponse == "recipe")
                    section = lineResponse;

                else if (int.TryParse(lineResponse, out pageId)) ;
                else
                    recipeName = lineResponse;
            }


            GetData getData = new GetData();

            try
            {
                getData.GetPage(section, pageId, recipeName);
                while (!getData.IsCompleted)
                {
                    // TODO: Переделать этот ужасный костыль.
                }

                return Enumerable.Range(1, getData.RecipeShorts.Count).Select(index => getData.RecipeShorts[index - 1])
                    .ToArray();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);

                return Enumerable.Range(1, 1)
                    .Select(index => new RecipeShort("error", new Picture("error"), "error"))
                    .ToArray();
            }
        }
    }
}