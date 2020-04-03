﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ObjectsLibrary.Objects;
using Square.Picasso;

namespace XamarinApp
{
    public class RecipeAdapter : RecyclerView.Adapter
    {
        private RecipeShort[] _recipeShorts;
        private Activity _activity;

        public event EventHandler<int> ItemClick;


        public RecipeAdapter(RecipeShort[] recipeShorts, Activity activity)
        {
            _recipeShorts = recipeShorts;
            _activity = activity;
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecipeViewHolder vh = holder as RecipeViewHolder;

            vh.Title.Text = _recipeShorts[position]?.Title;
            
            var urlArray = _recipeShorts[position]?.Url.Split('/');
            
            if (urlArray != null && urlArray.Length >= 2)
                vh.Link.Text = urlArray[2];

            var url = _recipeShorts[position]?.Picture.Url;

           Picasso.With(_activity)
                .Load(url)
                .Into(vh.Image);
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From (parent.Context).
                Inflate (Resource.Layout.list_item, parent, false);
            RecipeViewHolder vh = new RecipeViewHolder (itemView, OnClick);
            return vh;
        }

        public override int ItemCount => _recipeShorts.Length;

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }

    }
    
    public class RecipeViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        public TextView Title { get; private set; }
        public TextView Link { get; private set;}

        public RecipeViewHolder (View itemView, Action<int> listener) : base (itemView)
        {
            Image = itemView.FindViewById<ImageView> (Resource.Id.imageTitle);
            Title = itemView.FindViewById<TextView> (Resource.Id.title);
            Link = itemView.FindViewById<TextView> (Resource.Id.textLink);

            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }
}