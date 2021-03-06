﻿using System.Collections.Generic;
using Android.Graphics.Drawables;

namespace SimpleDroid
{
    public static class LayerDrawableExtensions
    {
        public static IEnumerable<Drawable> Drawables(this LayerDrawable layer)
        {
            for (var i = 0; i < layer.NumberOfLayers; i++)
            {
                yield return layer.GetDrawable(i);
            }
        }
    }
}