using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Annotations;
using WpfApp1.Model;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        MainViewModel _mainVM;

        public MainViewModel MainVM
        {
            get => _mainVM;
            set
            {
                _mainVM = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            MainVM = new MainViewModel();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.ItemsSource == null) return;
            progressBar.Maximum = UsersDataGrid.SelectedItems.Count == 0 ? 1 : UsersDataGrid.SelectedItems.Count;
        }

        private void ColumnChoose_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainVM.SearchedText = "";

            switch (ColumnChoose.SelectedValue)
            {
                case "Id":
                    SearchText.ItemsSource = MainVM.Users.Select(x => x.Id).ToList();
                    break;
                case "Name":
                    SearchText.ItemsSource = MainVM.Users.Select(x => x.Name).ToList();
                    break;
                case "Email":
                    SearchText.ItemsSource = MainVM.Users.Select(x => x.Email).ToList();
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
