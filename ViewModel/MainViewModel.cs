using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Model;
using WpfApp1.View;

namespace WpfApp1.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Api _api;

        //private List<User> _selectedUsers;
        private string _selectedColumn;
        private string _searchedText;
        private bool _isDataLoad = false;
        private bool _isAsyncMethodWorking = false;

        private ObservableCollection<User> _users;
        private ObservableCollection<User> _defaultUsersCollection;

        private int _progressBarValue;
        private bool _isProgressBarVisible;

        private Command _addCommand;
        private Command _editCommand;
        private Command _deleteCommand;
        private Command _searchCommand;
        public MainViewModel()
        {
            MainViewModelAsync();
        }

      //  public Task ViewModelInitAsync { get; private set; }
        private async Task MainViewModelAsync()
        {
            _api = new Api("https://localhost:7294");
            try
            {
                _defaultUsersCollection = await _api.GetAsync();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Ошибка получения пользователей. \nВозможно отсутствует соединение.");
            }

            Users = new ObservableCollection<User>(_defaultUsersCollection);
            IsDataLoad = Users != null;
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged("Users");
            }
        }
        //public List<User> SelectedUsers
        //{
        //    get => _selectedUsers;
        //    set
        //    {
        //        _selectedUsers = value;
        //        OnPropertyChanged("SelectedUser");
        //    }
        //}

        public string SelectedColumn
        {
            get => _selectedColumn;
            set
            {
                _selectedColumn = value;
                OnPropertyChanged("SelectedColumn");
            }
        }

        public int ProgressBarValue
        {
            get => _progressBarValue;
            set
            {
                _progressBarValue = value;
                OnPropertyChanged("ProgressBarValue");
            }
        }

        public bool IsProgressBarVisible
        {
            get => _isProgressBarVisible;
            set
            {
                _isProgressBarVisible = value;
                OnPropertyChanged("IsProgressBarVisible");
            }
        }
        
        public bool IsAsyncMethodWorking
        {
            get => _isAsyncMethodWorking;
            set
            {
                _isAsyncMethodWorking = value;
                OnPropertyChanged("IsAsyncMethodWorking");
            }
        }

        public string SearchedText
        {
            get => _searchedText;
            set
            {
                _searchedText = value;
                OnPropertyChanged("SearchedText");
            }
        }

        public bool IsDataLoad
        {
            get => _isDataLoad;
            set
            {
                _isDataLoad = value;
                OnPropertyChanged("IsDataLoad");
            }
        }

        public Command AddCommand => _addCommand ?? (_addCommand = new Command(AddCommand_Execute));
        private async void AddCommand_Execute(object o)
        {
            var addUserWindow = new AddUser_Window(new User());
            if (addUserWindow.ShowDialog() != true) return;

            var newUser = addUserWindow.User;
            var responseMessage = await _api.CreateAsync(newUser);
            var rs = responseMessage.Content.ReadAsStringAsync().Result;
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var createdUser = await responseMessage.Content.ReadAsAsync<User>();
                Users.Add(createdUser);
            }

        }

        public Command EditCommand => _editCommand ?? (_editCommand = new Command(EditCommand_Execute));

        private async void EditCommand_Execute(object selectedUser)
        {
            if (selectedUser is User user)
            {
                var responseMessage = await _api.EditAsync(user);
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                    MessageBox.Show("Редактирование прошло успешно!");
            }
        }

        public Command DeleteCommand => _deleteCommand ?? (_deleteCommand = new Command(DeleteCommand_Execute));

        private async void DeleteCommand_Execute(object selectedUsers)
        {

            try
            {
                var selectedUsersList = _users.Where(x => x.IsSelected).ToList();
                IsProgressBarVisible = true;
                IsAsyncMethodWorking = true;
                foreach (var user in selectedUsersList)
                {
                    var responseMessage = await _api.DeleteAsync(user.Id);

                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        ProgressBarValue += 1;
                        Users.Remove(user);
                        await Task.Delay(1000);
                    }
                }

            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Непредвиденная ошибка");
            }
            finally
            {
                ProgressBarValue = 0;
                IsProgressBarVisible = false;
                IsAsyncMethodWorking = false;
            }

        }

        public Command SearchCommand => _searchCommand ?? (_searchCommand = new Command(Search_Execute));
        public void Search_Execute(object s)
        {
            switch (SelectedColumn)
            {
                case "Id":
                    Users = new ObservableCollection<User>(_defaultUsersCollection.Where(x => x.Id.ToString().Contains(SearchedText)));
                    break;
                case "Name":
                    Users = new ObservableCollection<User>(_defaultUsersCollection.Where(x => x.Name != null && x.Name.Contains(SearchedText)));
                    break;
                case "Email":
                    Users = new ObservableCollection<User>(_defaultUsersCollection.Where(x => x.Email != null && x.Email.Contains(SearchedText)));
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

