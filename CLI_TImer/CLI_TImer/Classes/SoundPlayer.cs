using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Media;
using System.Diagnostics;

namespace CLI_TImer.Classes
{
    public class SoundPlayer
    {
        public void playSound(string path, int duration = 0)
        {
            Trace.WriteLine("Sound");
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(path));
            mediaPlayer.Play();
        }
    }
}
