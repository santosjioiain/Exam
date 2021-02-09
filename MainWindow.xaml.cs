using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
namespace WebScraping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginAuthentication login;
        public MainWindow()
        {
            InitializeComponent();
            login = new LoginAuthentication();
        }


        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            login.JSon();
        }
    }


    public class LoginAuthentication : MainWindow
    {
        public void JSon()
        {
            WebClient client = new WebClient();
            string _base = client.DownloadString("https://github.com");

            EventArea.Text = _base;
        }
    }
}
