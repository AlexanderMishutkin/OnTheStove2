﻿using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;
using RecipeLibrary.Objects;
using RecipeLibrary.Objects.Boxes.Elements;
using RecipeLibrary.Parser.ParserPage.Core;

namespace RecipeLibrary.Parser.ParserPage.WebSites
{
    class EdimDomaPageParser : IParserPage<RecipeShort[]>
    {
        public RecipeShort[] Parse(IHtmlDocument document)
        {
            var recipeCards = document.QuerySelectorAll("article")
                .Where(x => x.ClassName != null && x.ClassName == "card").Select(x => x.FirstElementChild).ToArray();

            return (from recipeCard in recipeCards
                let url = "https://www.edimdoma.ru/" + recipeCard.Attributes[0].Value
                let title = recipeCard.FirstElementChild.FirstElementChild.Attributes[1].Value
                let pictureUrl = recipeCard.FirstElementChild.FirstElementChild.Attributes[2].Value
                let picture = new Picture(pictureUrl)
                select new RecipeShort(title, picture, url)).ToArray();
        }
    }
}