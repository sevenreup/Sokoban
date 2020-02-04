using Sokoban.core;
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
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace Sokoban.views
{
    /// <summary>
    /// Interaction logic for StatusDialog.xaml
    /// </summary>
    public delegate void NextLevel();
    public delegate void PreviousLevel();
    public delegate void RetryLevel();

    public partial class StatusDialog : Window
    {
        String message;
        bool status;
        event NextLevel nextLevel;
        event PreviousLevel previousLevel;
        event RetryLevel retryLevel;

        public StatusDialog(bool status, String message, NextLevel nextLevel, RetryLevel retryLevel, PreviousLevel previousLevel)
        {
            this.message = message;
            this.status = status;
            this.nextLevel += nextLevel;
            this.previousLevel += previousLevel;
            this.retryLevel = retryLevel;

            InitializeComponent();
            if(status)
            {
                init();
                complete();
            } else
            {
                init();
                failed();
            }
        }
        private void init()
        {
            statusMessage.Content = message;

        }
        private void failed()
        {
            next.Visibility = Visibility.Collapsed;
        }
        private void complete()
        {
        }

        private void prev_MouseDown(object sender, MouseButtonEventArgs e)
        {
            previousLevel.Invoke();
            Close();
        }

        private void replay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            retryLevel.Invoke();
            Close();
        }

        private void next_MouseDown(object sender, MouseButtonEventArgs e)
        {
            nextLevel.Invoke();
            Close();
        }
    }
}
