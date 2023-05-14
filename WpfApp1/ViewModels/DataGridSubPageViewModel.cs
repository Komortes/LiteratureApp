using System.Windows;
using System.Windows.Input;
using System.Security;
using System.Threading.Tasks;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using LiteratureApp.Core;
using System.Windows.Media.Imaging;
using System.Collections;

namespace LiteratureApp {
    public class DataGridSubPageViewModel : BaseViewModel
    {
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");

        private SolidColorBrush elcolor1 = black;
        public SolidColorBrush Elcolor1
        {
            get { return elcolor1; }
            set
            {
                elcolor1 = value;
                OnPropertyChanged("elcolor1");
            }
        }


        private SolidColorBrush backcolor = new SolidColorBrush(lightpanel);
        public SolidColorBrush Backcolor
        {
            get { return backcolor; }
            set
            {
                backcolor = value;
                OnPropertyChanged("backcolor");
            }
        }

        private SolidColorBrush elcolor2 = white;
        public SolidColorBrush Elcolor2
        {
            get { return elcolor2; }
            set
            {
                elcolor2 = value;
                OnPropertyChanged("elcolor2");
            }
        }
        private Visibility havPer = Visibility.Hidden;
        public Visibility HavPer
        {
            get { return havPer; }
            set
            {
                havPer = value;
                OnPropertyChanged("havPer");
            }
        }


        public DataGridSubPageViewModel()
        {
            if (DataBank.theme)
            {
                Elcolor1 = white;
                Elcolor2 = black;
                Backcolor = new SolidColorBrush(darkpanel);
            }
            if (DataBank.curuser.IsAdmin == 7)
            {
                HavPer = Visibility.Visible;
            }
        }

        
    }
}
