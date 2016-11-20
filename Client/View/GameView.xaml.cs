using Client.Model;
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
            Initialize();
        }

        public GameView(GameState state)
        {
            Initialize();
            viewModel.RefreshGame(state);
        }

        private void Initialize()
        {
            InitializeComponent();
            viewModel = (GameViewModel)base.DataContext;
            viewModel.View = this;
        }

        private void OnClickHexagon(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Path p = sender as Path;
            viewModel.ChooseColour(p.GetValue(TagProperty).ToString());
        }
    }
}
