using AlbionMarshaller.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace LootUI
{
    public class PlayerView : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private Visibility _isHidden = Visibility.Visible;
        public Visibility IsHidden
        {
            get
            {
                return _isHidden;
            }
            set
            {
                _isHidden = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsHidden"));
            }
        }

        private string name;
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        private string alliance;
        public String Alliance
        {
            get
            {
                return alliance;
            }
            set
            {
                alliance = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        private string guild;
        public String Guild
        {
            get
            {
                return guild;
            }
            set
            {
                guild = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }
        public ObservableCollection<Loot> Loot { get; set; } = new ObservableCollection<Loot>();

        public String Text
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                if(!String.IsNullOrEmpty(Alliance))
                {
                    builder.Append($"[{Alliance}]");
                }
                if (!String.IsNullOrEmpty(Guild))
                {
                    builder.Append($"[{Guild}]");
                }
                if (!String.IsNullOrEmpty(Name))
                {
                    builder.Append($" {Name}");
                }
                return builder.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
