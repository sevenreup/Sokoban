using Sokoban.core.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sokoban.views
{
    /// <summary>
    /// Interaction logic for LevelComplete.xaml
    /// </summary>
    public partial class LevelComplete : Window
    {
        event ReplayLevel replay;
        event NextLevel next;
        event PreviousLevel previous;
        public LevelComplete(bool status, ReplayLevel replay)
        {
            this.replay += replay;
            InitializeComponent();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            replay();
            Close();
        }
    }
}
