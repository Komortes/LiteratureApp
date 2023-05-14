using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace LiteratureApp
{
    public static class FrameworkElementAnimations
    {
        public static async Task SlideAndFadeInFromRightAsync(this FrameworkElement element, float seconds = 0.3f, bool keepMargin = true)
        {
            var sb = new Storyboard();
            sb.AddSlideFromRight(seconds, element.ActualWidth, keepMargin: keepMargin);
            sb.AddFadeIn(seconds);
            sb.Begin(element);
            element.Visibility = Visibility.Visible;
            await Task.Delay((int)(seconds * 1000));
        }

   
        public static async Task SlideAndFadeInFromLeftAsync(this FrameworkElement element, float seconds = 0.3f, bool keepMargin = true)
        {
            var sb = new Storyboard();
            sb.AddSlideFromLeft(seconds, element.ActualWidth, keepMargin: keepMargin);
            sb.AddFadeIn(seconds);
            sb.Begin(element);
            element.Visibility = Visibility.Visible;
            await Task.Delay((int)(seconds * 1000));
        }

        public static async Task SlideAndFadeOutToLeftAsync(this FrameworkElement element, float seconds = 0.3f, bool keepMargin = true)
        {
            var sb = new Storyboard();
            sb.AddSlideToLeft(seconds, element.ActualWidth, keepMargin: keepMargin);
            sb.AddFadeOut(seconds);
            sb.Begin(element);
            element.Visibility = Visibility.Visible;
            await Task.Delay((int)(seconds * 1000));
        }
        public static async Task SlideAndFadeOutToRightAsync(this FrameworkElement element, float seconds = 0.3f, bool keepMargin = true)
        {
            var sb = new Storyboard();
            sb.AddSlideToRight(seconds, element.ActualWidth, keepMargin: keepMargin);
            sb.AddFadeOut(seconds);
            sb.Begin(element);
            element.Visibility = Visibility.Visible;
            await Task.Delay((int)(seconds * 1000));
        }

    }
}
