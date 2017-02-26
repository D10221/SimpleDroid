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
    }
}