using System.Collections.Generic;
using Android.Graphics.Drawables;

namespace SimpleDroid
{
    public static class AnimationDrawableExtensions
    {
        public static IEnumerable<Drawable> Frames(this AnimationDrawable animation)
        {
            for (var i = 0; i < animation.NumberOfFrames; i++)
            {
                yield return animation.GetFrame(i);
            }
        }

        public static IEnumerable<Drawable> Drawables(this LayerDrawable layer)
        {
            for (var i = 0; i < layer.NumberOfLayers; i++)
            {
                yield return layer.GetDrawable(i);
            }
        }
    }
}