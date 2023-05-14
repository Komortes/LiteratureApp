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
using System.Windows.Shapes;
using System.Security;
using LiteratureApp.Core;

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для LoginFromView.xaml
    /// </summary>
    public partial class LoginFromView : BasePage<LoginPanelViewModel> ,IHavePassword
    {
        public LoginFromView()
        {
            InitializeComponent();
        }

        public SecureString SecurePassword => password.SecurePassword;
        public SecureString SecurePasswordConf => password.SecurePassword;

    }
}
