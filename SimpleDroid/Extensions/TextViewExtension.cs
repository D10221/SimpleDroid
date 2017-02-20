using Android.Widget;

namespace SimpleDroid
{
    public static class TextViewExtension
    {
        public static void AddText(this TextView textView, string text)
        {
            textView.Text += text;
        }
    }
}