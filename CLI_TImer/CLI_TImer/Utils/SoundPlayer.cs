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

namespace CLI_TImer.Utils
{
    public static class SoundPlayer
    {
        static bool playingAudio = false;
        public static MediaPlayer media = new MediaPlayer();
        private static DispatcherTimer playTime = new DispatcherTimer();

        public static void playSound(string path, int duration = 0)
        {
            media.MediaEnded += (sender, eventarges) => Media_Ended();
            playTime.Tick += new EventHandler(StopMedia);
            playTime.Interval = new TimeSpan(0, 0, duration);
            playingAudio = true;
            media.Open(new Uri(path));
            media.Play();
            playTime.Start();
        }

        private static void StopMedia(object? sender, EventArgs e)
        {
            media.MediaEnded -= (sender, eventarges) => Media_Ended();
            playTime.Tick -= new EventHandler(StopMedia);
            playTime.Stop();
            media.Stop();
            media.Close();
            playingAudio = false;
        }

        private static void Media_Ended()
        {
            media.Position = TimeSpan.Zero;
            media.Play();
        }
    }
}
