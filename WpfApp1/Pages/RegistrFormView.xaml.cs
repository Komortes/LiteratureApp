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
using LiteratureApp.Core;
using System.Security;

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для RegistrFormview.xaml
    /// </summary>
    public partial class RegistrFormView : BasePage<RegisterPanelViewForm>, IHavePassword
    {
        public RegistrFormView()
        {
            InitializeComponent();
            CalendarDateRange cdr = new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue);
            datereg.BlackoutDates.Add(cdr);
        }
        public SecureString SecurePassword => passreg.SecurePassword;
        public SecureString SecurePasswordConf => passregcon.SecurePassword;
    }
}
