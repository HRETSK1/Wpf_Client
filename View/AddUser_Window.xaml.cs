using System.Windows;
using WpfApp1.Model;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for AddUser_Window.xaml
    /// </summary>
    public partial class AddUser_Window : Window
    {
        public User User { get; private set; }
        public AddUser_Window(User user)
        {
            InitializeComponent();
            User = user;
            DataContext = User;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(EmailTextBox.Text)) 
                MessageBox.Show("Заполните все поля!");
            else
                DialogResult = true;
        }
    }
}
