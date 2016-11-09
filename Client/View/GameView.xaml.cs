using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.View
{
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();

            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri(@"../../Resources/gaem.mp3", UriKind.Relative));
            player.Play();
        }
    }
}
