using Client.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Client.View
{
    public partial class GameView : UserControl
    {
        private GameViewModel viewModel;

        public GameView()
        {
            InitializeComponent();
            viewModel = (GameViewModel)base.DataContext;
        }

        private void OnClickHexagon(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!viewModel.IsCounterRunning)
            {
                return;
            }

            Path p = sender as Path;
            viewModel.ChooseColour((float)p.GetValue(Canvas.LeftProperty), (float)p.GetValue(Canvas.TopProperty));
        }
    }
}
