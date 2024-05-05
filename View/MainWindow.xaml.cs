using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Annotations;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        MainViewModel _mainViewModel;

        public MainViewModel MainViewModelVm
        {
            get => _mainViewModel;
            set
            {
                _mainViewModel = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            MainViewModelVm = new MainViewModel();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.ItemsSource == null) return;
            progressBar.Maximum = UsersDataGrid.SelectedItems.Count == 0 ? 1 : UsersDataGrid.SelectedItems.Count;
        }

        private void SearchText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (UsersDataGrid.ItemsSource == null)
            {
                MessageBox.Show("В таблице отсутствуют данные");
                return;
            }

            if (string.IsNullOrWhiteSpace(ColumnChoose.Text))
            {
                MessageBox.Show("Выберите по какой колонке искать");
                return;
            }

            MainViewModelVm.Search_Execute();
        }

        private void ColumnChoose_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainViewModelVm.SearchedText = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
