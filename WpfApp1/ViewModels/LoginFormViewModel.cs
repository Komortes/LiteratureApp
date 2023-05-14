using System.Windows;
using LiteratureApp.Core;
using System.Windows.Input;

namespace LiteratureApp
{
    public class LoginFormViewModel : BaseViewModel
    {
        private Window mWindow;
        private int mWindowRadius = 20;
        public int WindowRadius
        {
            get { return mWindowRadius;}
            set { mWindowRadius = value;}
        }
       
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Login;

        public ICommand CloseCommand { get; set; }

        public LoginFormViewModel(Window window)
        {
            mWindow = window;
            CloseCommand = new RelayCommand(() => mWindow.Close());

        }
        private Point GetMousePosition()
        {
            // Позиция курсора относительно окна
            var position = Mouse.GetPosition(mWindow);
            return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
        }
    }
}
