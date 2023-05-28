using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Threading;
using CLI_TImer.Helpers;

namespace CLI_TImer.Classes
{
    public class SoundPlayer
    {
        bool playingAudio = false;
        MediaPlayer media;
        DispatcherTimer playTime;

        public SoundPlayer()
        {
            media = new MediaPlayer();
            media.MediaEnded += (sender, eventarges) => Media_Ended();
            playTime = new DispatcherTimer();
            playTime.Tick += new EventHandler(StopMedia);
        }

        public void playSound(string path, int duration = 0)
        {
            playTime.Interval = new TimeSpan(0, 0, duration);
            playingAudio = true;
            media.Open(new Uri(path));
            media.Play();
            playTime.Start();
        }

        private void StopMedia(object? sender, EventArgs e)
        {
            playTime.Stop();
            media.Stop();
            media.Close();
            playingAudio = false;
        }

        private void Media_Ended()
        {
            Trace.WriteLine("Hi");
            media.Position = TimeSpan.Zero;
            media.Play();
        }
    }
}
