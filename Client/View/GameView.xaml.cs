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

        private void OnClickHexa(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Path p = sender as Path;
            viewModel.DecodeColor(p.Fill);
        }
    }
}
