using System;
using System.Diagnostics;
using System.Globalization;
using LiteratureApp.Core;

namespace LiteratureApp
{
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Login:
                    return new LoginFromView();

                case ApplicationPage.Register:
                    return new RegistrFormView();

                case ApplicationPage.Reset:
                    return new ResetFromView();

                case ApplicationPage.Settings:
                    return new SettibgSubPageView();

                case ApplicationPage.Data:
                    return new DataGridSubPageView();

                case ApplicationPage.Lib:
                    return new LibSubPage();

                case ApplicationPage.Profile:
                    return new ProfileSubPageView();

                case ApplicationPage.Book:
                    return new BookPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
