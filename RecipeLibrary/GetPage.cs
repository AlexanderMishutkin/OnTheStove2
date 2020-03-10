﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using RecipeLibrary.Objects;
using RecipeLibrary.Parser.ParserPage.Core;
using RecipeLibrary.Parser.ParserPage.WebSites;

namespace RecipeLibrary
{
    public partial class GetData
    {
        public RecipeShort[] GetPage(string section, int page, string findName = null)
        {
            #region Povarenok.ru

            ParserPage<RecipeShort[]> povarenok = new ParserPage<RecipeShort[]>
                (new PovarenokPageParser(), new PovarenokPageSettings(section, page, findName));

            povarenok.OnNewData += Parser_OnNewData;
            povarenok.StartParsePage();

            #endregion

            #region Povar

            // TODO: Заменить фотографии.
            ParserPage<RecipeShort[]> povar = new ParserPage<RecipeShort[]>
                (new PovarPageParser(), new PovarPageSettings(section, page, findName));

            povar.OnNewData += Parser_OnNewData;
            povar.StartParsePage();

            #endregion
            
            #region Edimdoma

            ParserPage<RecipeShort[]> edimdoma = new ParserPage<RecipeShort[]>
                (new EdimDomaPageParser(), new EdimDomaPageSettings(section, page, findName));

            edimdoma.OnNewData += Parser_OnNewData;
            edimdoma.StartParsePage();

            #endregion

            while (!_isCompleted)
            {
                Thread.Sleep(100);
            }

            return RecipeShorts.ToArray();
        }

        private bool _isCompleted = false;
        private void GetCompleted()
        {
            _isCompleted = true;
        }
    }
}