using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using LiteratureApp.Core;
using System.Security;

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для SettibgSubPageView.xaml
    /// </summary>
    public partial class SettibgSubPageView : BasePage<SettingsSubPageViewModel> ,IHavePassword
    {
        public SettibgSubPageView()
        {
            InitializeComponent();
            this.PageLoadAnimation = PageAnimation.SlideAndFadeInFromLeft;
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToLeft;
        }
        public SecureString SecurePassword => curpas.SecurePassword;
        public SecureString SecurePasswordConf => newpas.SecurePassword;
    }
}
