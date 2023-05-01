using System.Threading;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using CLI_TImer.MVVM.ViewModel;
using System.Windows.Media;

namespace CLI_TImer.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {       
        public MainWindow()
        {
            InitializeComponent();

            DataContext = App.MainViewModel;

            Left = SystemParameters.PrimaryScreenWidth - Width - 20;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //Scroll to the bottom of the command list
        private void list_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            Border border = (Border)VisualTreeHelper.GetChild(list, 0);
            ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
            scrollViewer.ScrollToBottom();
        }
    }
}