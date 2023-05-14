using System.Windows.Controls;
using System.Windows;
using LiteratureApp.Core;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System;

namespace LiteratureApp
{
    public class BasePage : Page
    {

        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToRigth;
        public float SlideSeconds { get; set; } = 0.4f;
        public bool ShouldAnimateOut { get; set; }
        public BasePage()
        {
            Loaded += BasePage_LoadedAsync;
        }

        private async void BasePage_LoadedAsync(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ShouldAnimateOut)
                await AnimateOutAsync();
            else
                await AnimateInAsync();
        }

        public async Task AnimateInAsync()
        {
            if (PageLoadAnimation == PageAnimation.None)
                return;

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromRight:
                    await this.SlideAndFadeInFromRightAsync(SlideSeconds);
                    break;


                case PageAnimation.SlideAndFadeInFromLeft:
                    await this.SlideAndFadeInFromLeftAsync(SlideSeconds);
                    break;
            }
        }

        public async Task AnimateOutAsync()
        {
            if (PageUnloadAnimation == PageAnimation.None)
                return;

            switch (PageUnloadAnimation)
            {
                case PageAnimation.SlideAndFadeOutToRigth:

                    await this.SlideAndFadeOutToRightAsync(SlideSeconds);

                    break;

                case PageAnimation.SlideAndFadeOutToLeft:

                    await this.SlideAndFadeOutToLeftAsync(SlideSeconds);

                    break;
            }
        }

    }
    public class BasePage<VM> : BasePage
        where VM : BaseViewModel, new()
    {
        private VM mViewModel;
        public VM ViewModel
        {
            get => mViewModel;
            set
            {
                if (mViewModel == value)
                    return;

                mViewModel = value;
                DataContext = mViewModel;
            }
        }
        public BasePage() : base()
        {
            ViewModel = new VM();
        }

    }
}
