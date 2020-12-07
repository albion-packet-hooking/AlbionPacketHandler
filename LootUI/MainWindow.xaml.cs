﻿using AlbionMarshaller.MemoryStorage;
using AlbionMarshaller.Model;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace LootUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean AutoScroll = true;
        private AlbionProcessor.AlbionProcessor _processor = null;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableCollection<PlayerView> players = new ObservableCollection<PlayerView>();
        private bool _filterChanged = false;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                _processor = new AlbionProcessor.AlbionProcessor();
                
                LootDB.Instance.LootAddedToPlayer += HandleLootAddedToPlayer;

                trvPlayerLoot.ItemsSource = CollectionViewSource.GetDefaultView(players);
                (trvPlayerLoot.ItemsSource as ICollectionView).Filter = new Predicate<object>(FilterLoot);
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }
        }

        public bool FilterLoot(object sender)
        {
            PlayerView player = sender as PlayerView;
            string text = (Filter as TextBox).Text;

            if(!_filterChanged || text == "")
            {
                return true;
            }

            if (player.Text.ToLower().Contains(text.ToLower()))
            {
                return true;
            }
            return false;
        }

            private void HandleLootAddedToPlayer(object sender, PlayerLootEventArgs plea)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                Loot loot = plea.Loot;
                Player player = plea.Player;
                Run run = new Run();
                run.Text = $"{player.Name} looted {loot.Quantity}x{loot.LongName} from {loot.BodyName} at {loot.LocalPickupTime}\r\n";
                LootLog.Inlines.Add(run);
                
                PlayerView pv = players.FirstOrDefault(p => p.Name == player.Name);
                if (pv == null && player.Id == -1000)
                {
                    pv = players.FirstOrDefault(p => p.Id == player.Id);
                }
                if(pv == null)
                {
                    pv = new PlayerView() { Name = player.Name, Alliance = player.Alliance, Guild = player.Guild, Id = player.Id };
                    players.Add(pv);
                }
                else
                {
                    pv.Name = player.Name;
                    pv.Guild = player.Guild;
                    pv.Alliance = player.Alliance;
                }
                pv.Loot.Add(loot);
            }));
        }

        private void ScrollViewer_ScrollChanged(Object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scroller = sender as ScrollViewer;
            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (scroller.VerticalOffset == scroller.ScrollableHeight)
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
                scroller.ScrollToVerticalOffset(scroller.ExtentHeight);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //_logger.SaveLootsToFile();

            // Don't want to deal with the fucking open threads this guy made
            Environment.Exit(0);
        }

        private void ClearLoot_Click(object sender, RoutedEventArgs e)
        {
            LootLog.Inlines.Clear();
            players.Clear();
        }

        private void SaveFilter_Click(object sender, RoutedEventArgs e)
        {
            //_service.FilterString = this.Filter.Text;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(trvPlayerLoot != null && trvPlayerLoot.ItemsSource != null)
            {
                _filterChanged = true;
                (trvPlayerLoot.ItemsSource as ICollectionView).Refresh();
            }
        }

        private void UpdateLootLog_Click(object sender, RoutedEventArgs e)
        {
            TextReader lootLogReader = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Old Loot Log";
            if (openFileDialog.ShowDialog() == true)
            {
                lootLogReader = new StreamReader(openFileDialog.FileName);
            }

            List<string[]> newResults = new List<string[]>();

            var lootlogParser = new CsvParser(lootLogReader, CultureInfo.InvariantCulture);
            string[] record = null;
            while((record = lootlogParser.Read()) != null)
            {
                if(record.Length == 9)
                {
                    MessageBox.Show("Missing ItemRefID -- Loot Log too old");
                    return;
                }

                int itemId = int.Parse(record[0]);

                Item newItem = ItemDB.Instance.FindItem(itemId);
                string itemName = newItem.UniqueName;
                if (newItem.LocalizationName != null)
                {
                    itemName += " - " + newItem.LocalizationName;
                }

                string shortName = newItem.UniqueName;
                string quality = "0";
                if (shortName.Contains("@"))
                {
                    quality = shortName.Split('@')[1];
                }

                // 5,6,7
                record[5] = shortName;
                record[6] = itemName;
                record[7] = quality;

                newResults.Add(record);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Location";
            if (saveFileDialog.ShowDialog() == true)
            {
                using(FileStream fs = saveFileDialog.OpenFile() as FileStream)
                using(TextWriter sr = new StreamWriter(fs))
                using(CsvWriter lootWriter = new CsvWriter(sr, CultureInfo.InvariantCulture))
                {
                    foreach(string[] item in newResults)
                    {
                        foreach(string itemValue in item)
                        {
                            lootWriter.WriteField(itemValue);
                        }
                        lootWriter.NextRecord();
                    }
                    //lootWriter.WriteRecords(newResults.AsEnumerable<string[]>());
                }
            }

            MessageBox.Show("New log written to --> " + saveFileDialog.FileName);
        }
    }
}
