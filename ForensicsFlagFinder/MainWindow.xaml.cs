using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace ForensicsFlagFinder
{
    public partial class MainWindow
    {
        private string _filename = "blank";
        private int _charNr;
        private int _combo;
        private readonly List<char> _chars = new List<char>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonChoose_OnClick(object sender, RoutedEventArgs e)
        {
            Initialize();
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            _filename = dialog.FileName;
        }

        private void Initialize()
        {
            _charNr = 0;
            _combo = 0;
            _chars.Clear();
            ListView.Items.Clear();
        }
        
        private void Search(object sender, RoutedEventArgs e)
        {
            if (_filename == "blank")
            {
                Error.Content = "Nie wybrano pliku :)";
                return;
            }
            if (int.TryParse(Num.Text, out _))
            {
                if (int.Parse(Num.Text) < 1)
                {
                    Error.Content = "Wpisz poprawną wartość :)";
                    return;
                }
            }
            else
            {
                Error.Content = "Wpisz numer :)";
                return;
            }
            Initialize();
            Error.Content = "";
            foreach (var character in File.ReadAllText(_filename))
            {
                if (character < 127 && character > 32)
                {
                    _chars.Add(character);
                    _combo++;
                }
                else
                {
                    if (_combo >= int.Parse(Num.Text))
                    {
                        var content = new string(_chars.ToArray());
                        if (SearchField.Text != "")
                        {
                            if (content.Contains(SearchField.Text))
                            {
                                content = _charNr - _combo + ": " + content;
                                var listViewItem = new ListViewItem {Content = content};
                                ListView.Items.Add(listViewItem);
                            }
                        }
                        else
                        {
                            content = _charNr - _combo + ": " + content;
                            var listViewItem = new ListViewItem {Content = content};
                            ListView.Items.Add(listViewItem);
                        }
                        _chars.Clear();
                        _combo = 0;
                    }
                    else
                    {
                        _chars.Clear();
                        _combo = 0;
                    }
                }
                _charNr++;
            }
            if (_combo < int.Parse(Num.Text)) return;
            {
                var content = new string(_chars.ToArray());
                content = _charNr - _combo + ": " + content;
                var listViewItem = new ListViewItem {Content = content};
                ListView.Items.Add(listViewItem);
            }
        }
    }
}