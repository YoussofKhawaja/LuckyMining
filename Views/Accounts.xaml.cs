//.____ __                 _____    .__           .__              ____
//|    |      __ __    ____   |  | __  ___.__.   /     \   |__|   ____   |__|   ____     / ___\
//|    |     |  |  \ _/ ___\  |  |/ / <   |  |  /  \ /  \  |  |  /    \  |  |  /    \   / /_/  >
//|    |___  |  |  / \  \___  |    <   \___  | /    Y    \ |  | |   |  \ |  | |   |  \  \___  /
//|_______ \ |____/   \___  > |__|_ \  / ____| \____|__  / |__| |___|  / |__| |___|  / /_____/
// |
// Copyright 2021 by YK303
// |
// Licensed under the Apache License , Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// |
// http://www.apache.org/licenses/
// |
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using LuckyMining.Models;
using LuckyMining.RestClient;
using LuckyMining.Saving;
using Newtonsoft.Json;
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LuckyMining.Views
{
    /// <summary>
    /// Accounts.xaml
    /// </summary>
    public partial class Accounts : Window
    {
        public static Users user;

        public static EmailToUserName.Root emailuser;

        public static Accounts accounts { get; set; }

        public Accounts()
        {
            this.InitializeComponent();
            accounts = this;
        }

        /// <summary>
        /// login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private async void loginclick(object sender, RoutedEventArgs e)
        {
            try
            {
                var notificationManager = new NotificationManager(NotificationPosition.TopRight);
                //chech if textbox and passwordbox empty or not
                if (txtusername.Text.Length == 0)
                {
                    await notificationManager.ShowAsync(
                    new NotificationContent { Title = "Login", Message = "Enter an email or username", Type = NotificationType.Error },
                    areaName: "WindowArea");
                }
                else if (txtPassword.Password.Length == 0)
                {
                    await notificationManager.ShowAsync(
                    new NotificationContent { Title = "Login", Message = "Enter an password", Type = NotificationType.Error },
                    areaName: "WindowArea");
                }
                else
                {
                    //api stuff

                    ////check username and password from api
                    //RestClient<Users> restClient = new RestClient<Users>();
                    //RestClient<EmailToUserName.Root> restClientt = new RestClient<EmailToUserName.Root>();

                    //user = await restClient.GetAsync("api" + $"username={txtusername.Text}&password={txtPassword.Password}");

                    //if (user != null)
                    //{
                    //    //username or email and password are correct
                    //    if (user.Error == "false")
                    //    {
                    user = new Users(txtusername.Text, txtPassword.Password, null, null, null, null);
                    //save

                    SaveManager.WriteToXmlFile<Users>(user, "data", "account");

                    //go to mainwindow

                    MainWindow Testing = new MainWindow();
                    Testing.Show();
                    this.Close();
                    //    }

                    //api stuff

                    //    //here username wrong
                    //    else if (user.Error == "INVALID_USERNAME")
                    //    {
                    //        emailuser = await restClientt.GetAsync("api" + $"email={txtusername.Text}");
                    //        Debug.WriteLine(restClientt.GetResponse());
                    //        user = await restClient.GetAsync("api" + $"username={emailuser.Username}&password={txtPassword.Password}");
                    //        user = new Users(emailuser.Username, txtPassword.Password, txtusername.Text);
                    //        //save

                    //        SaveManager.WriteToXmlFile<Users>(user, "data", "account");
                    //        MainWindow Testing = new MainWindow();
                    //        Testing.Show();
                    //        this.Close();
                    //        Debug.WriteLine("value=" + emailuser.Username + txtusername.Text);
                    //    }
                    //    else
                    //    {
                    //        await notificationManager.ShowAsync(
                    //        new NotificationContent { Title = "Login", Message = "Wrong UserName or Email or password" },
                    //        areaName: "WindowArea");
                    //    }
                    //}
                }
            }
            catch
            {
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            //in Register
        }

        private void registerclick(object sender, RoutedEventArgs e)
        {
            //go to register page
            grid.Children.Clear();
            grid.Children.Add(new Register());
            backtologin.Visibility = Visibility.Visible;
        }

        //exit app
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                //on button click exit Application
                if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") >= 1)
                {
                    foreach (Process process in Process.GetProcessesByName("lolMiner"))
                    {
                        process.Kill();
                    }
                }
                System.Windows.Application.Current.Shutdown();
            }
            catch
            {
            }
        }

        //minimize app
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //Minimize window
            this.WindowState = WindowState.Minimized;
        }

        //move system window
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //Move window on leftbutton mouse clicked
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //var btn = (Button)sender;

            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        //eye click events
        private void ShowPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e) => ShowPasswordFunction();

        private void ShowPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e) => HidePasswordFunction();

        private void ShowPassword_MouseLeave(object sender, MouseEventArgs e) => HidePasswordFunction();

        //eye click show password
        private void ShowPasswordFunction()
        {
            PasswordUnmask.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Hidden;
            PasswordUnmask.Text = txtPassword.Password;
        }

        //eye click hide password
        private void HidePasswordFunction()
        {
            PasswordUnmask.Visibility = Visibility.Hidden;
            txtPassword.Visibility = Visibility.Visible;
        }

        private async void discordlogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                buttonforcanceldiscord.Visibility = Visibility.Visible;
                this.Dispatcher.Invoke((Action)(() =>
                {
                    progress1.IsActive = true;
                    progress1.Visibility = Visibility.Visible;
                    stackpanelhider.Visibility = Visibility.Visible;
                }));
                await Task.Run(Processstart);

                if (user.username != null && user.Email != null && user.id != null && user.avatar != null && user.discriminator != null)
                {
                    progress1.IsActive = false;
                    progress1.Visibility = Visibility.Collapsed;
                    stackpanelhider.Visibility = Visibility.Collapsed;

                    MainWindow Testing = new MainWindow();
                    Testing.Show();
                    this.Close();
                }
                else
                {
                    var notificationManager = new NotificationManager(NotificationPosition.TopRight);
                    //chech if textbox and passwordbox empty or not

                    await notificationManager.ShowAsync(
                    new NotificationContent { Title = "Login", Message = "Something Went Wrong" },
                    areaName: "WindowArea");
                }
                buttonforcanceldiscord.Visibility = Visibility.Collapsed;
            }
            catch
            {
            }
        }

        private void Processstart()
        {
            try
            {
                var url = "https://discord.com/api/oauth2/authorize?client_id=&redirect_uri=http%3A%2F%2Flocalhost%3A56400%2Fcallback&response_type=code&scope=identify%20email";

                Process newProcess = Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

                HttpListener listener = new HttpListener();
                listener.Prefixes.Add("http://localhost:56400/");
                listener.Start();
                httpsel(listener);
            }
            catch
            {
            }
        }

        private static void httpsel(HttpListener listener)
        {
            try
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                void bye()
                {
                    using (StreamWriter a = new StreamWriter(response.OutputStream))
                        a.Write("Bye");
                }
                string path = request.RawUrl;
                if (path.StartsWith("/callback"))
                {
                    var query = request.QueryString;
                    if (query.AllKeys.Length != 0 && query["code"] != "")
                    {
                        Console.WriteLine("Authentication Attempts: " + query["code"]);
                        user_info(query["code"]);
                        bye();
                    }
                    else bye();
                }
                else bye();
            }
            catch
            {
            }
        }

        private static string discord_access(string code)
        {
            string CLIENT_ID = "//Your CLIENT_ID";
            string CLIENT_SECRET = "//Your CLIENT_SECRET";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://discord.com/api/oauth2/token");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (StreamWriter requestStream = new StreamWriter(request.GetRequestStream()))
            {
                requestStream.Write($"client_id={CLIENT_ID}&client_secret={CLIENT_SECRET}&grant_type=authorization_code&code={code}&redirect_uri={WebUtility.UrlEncode("http://localhost:56400/callback")}&scope={WebUtility.UrlEncode("identify email guilds connections")}");
            }
            try
            {
                return (new StreamReader(request.GetResponse().GetResponseStream())).ReadToEnd();
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Debug.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                        return null;
                    }
                }
            }
        }

        private static void user_info(string code)
        {
            try
            {
                string html = discord_access(code);
                if (html == null)
                {
                    Debug.WriteLine("It's not the right approach.");
                    return;
                }
                DiscordData json = JsonConvert.DeserializeObject<DiscordData>(html);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://discord.com/api/users/@me");
                request.Headers.Add("Authorization", "Bearer " + json.access_token);
                string end = (new StreamReader(request.GetResponse().GetResponseStream())).ReadToEnd();
                DiscordData a = JsonConvert.DeserializeObject<DiscordData>(end);

                user = new Users(a.username, null, a.email, a.id, a.avatar, a.discriminator);
                //save

                SaveManager.WriteToXmlFile<Users>(user, "data", "account");
            }
            catch
            {
            }
        }

        private void checkbox_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buttonforcanceldiscord_Click(object sender, RoutedEventArgs e)
        {
            progress1.IsActive = false;
            progress1.Visibility = Visibility.Hidden;
            stackpanelhider.Visibility = Visibility.Hidden;
            buttonforcanceldiscord.Visibility = Visibility.Collapsed;
        }
    }
}