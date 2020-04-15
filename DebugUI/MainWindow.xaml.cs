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
using AlbionProcessor;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using AlbionProcessor.MemoryStorage;

namespace DebugLoggerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Boolean AutoScroll = true;
        private AlbionProcessor.AlbionProcessor _logger = null;

        Timer backgroundUpdater = null;

        public MainWindow()
        {            
            try
            {
                InitializeComponent();
                _logger = new AlbionProcessor.AlbionProcessor();

                backgroundUpdater = new Timer(new TimerCallback(UIUpdate), null, 0, 100);

                CharacterDB.Instance.PlayerAdded += PlayerAddHandler;
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace);
            }
        }

        private void PlayerAddHandler(object sender, PlayerEventArgs pea)
        {
            log.Info(pea.Player);
        }

        private void UIUpdate(object state)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                Dictionary<String, int> newEvents = new Dictionary<String, int>(PacketHandler.Instance.triggeredEvents);
                this.Events.ItemsSource = newEvents;
            });
        }

        private void ScrollViewer_ScrollChanged(Object sender, ScrollChangedEventArgs e)
        {
            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (EventScroller.VerticalOffset == EventScroller.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    AutoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    AutoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (AutoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                EventScroller.ScrollToVerticalOffset(EventScroller.ExtentHeight);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            // Don't want to deal with the fucking open threads this guy made
            Environment.Exit(0);
        }
    }
}
