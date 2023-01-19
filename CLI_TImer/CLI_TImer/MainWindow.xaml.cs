using System.Threading;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using CLI_TImer.Models;
namespace CLI_TImer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int hours = 0;
        int minutes = 0;
        int seconds = 0;

        string _minutes;
        string _seconds;

        int bhours= 0;
        int bminutes= 0;
        int bseconds= 0;

        bool pause = false;

        Timers work = new Timers { hours = 0, minutes = 1, seconds = 20, name = "work"};
        

        Thread timerThread;

        public MainWindow()
        {
            InitializeComponent();

            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - 20;

            timerThread = new Thread(new ThreadStart(timer));

            timerThread.Start();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TextBox_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            commandEntered(InputField.Text);

            InputField.Text = null;
        }

        private void commandEntered(string command)
        {
            updateLastImputs();

            switch(command) 
            {
                case "work":
                    latesInput.Text = "work";
                    latesInputInfo.Text = "we started working";
                    startTimer(work);
                    break;

                case "break":
                    latesInput.Text = "break";
                    latesInputInfo.Text = "taking a break";
                    startBreak();
                    break;

                case "setup":
                    latesInput.Text = "setup";
                    latesInputInfo.Text = "Type to choose a setting: times";
                    break;

                case "close":
                    this.Close();
                    break;

                default:
                    latesInput.Text = "Error";
                    latesInputInfo.Text = "Unknown Command";
                    break;                    
            }
            if(TopPointer.IsVisible == false) updatePointers();
        }

        private void updatePointers()
        {
            if (latesInput.Text != "") BottomPointer.Visibility = Visibility.Visible;
            if (secondLatesInput.Text != "") MiddlePointer.Visibility = Visibility.Visible;
            if (thirdLatesInput.Text != "") TopPointer.Visibility = Visibility.Visible;
        }

        private void updateLastImputs()
        {
            thirdLatesInput.Text = secondLatesInput.Text;
            thirdLatesInputInfo.Text = secondLatesInputInfo.Text;

            secondLatesInput.Text = latesInput.Text;
            secondLatesInputInfo.Text = latesInputInfo.Text;
        }

        private void startTimer(Timers timer)
        {
            hours = timer.hours;
            minutes = timer.minutes;
            seconds = timer.seconds + 1;
        }

        private void startBreak()
        {
            
        }

        private void timer()
        {
            while(true) 
            {
                if (pause == false)
                {
                    _minutes = Convert.ToString(minutes);
                    _seconds = Convert.ToString(seconds);

                    if (_minutes.Length == 1) _minutes = "0" + _minutes;
                    if (_seconds.Length == 1) _seconds = "0" + _seconds;

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Countdown.Text = Convert.ToString($"{hours}h {_minutes}m {_seconds}s");
                    }));


                    if (hours == 0 && minutes == 0 && seconds == 0) Thread.Sleep(10);

                    else
                    {
                        if (seconds == 0)
                        {
                            minutes--;
                            seconds = 59;
                        }

                        if (minutes == 0 && hours > 0)
                        {
                            minutes = 59;
                            hours--;
                        }

                        seconds--;

                        Thread.Sleep(1000);
                    }
                }

                else if (pause == true)
                {
                    if (bseconds == 0)
                    {
                        bminutes--;
                        bseconds = 59;
                    }

                    if (bminutes == 0 && bhours > 0)
                    {
                        bminutes = 59;
                        bhours--;
                    }

                    bseconds--;

                    if (bseconds == 0 && bminutes == 0 &&bhours == 0) pause = false;

                    Thread.Sleep(1000);
                }
            }
        }
    }
}
