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
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LuckyMining.Views
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        public static Users user;

        public Register()
        {
            InitializeComponent();
            Accounts.accounts.buttonback.Click += btnReturn_Click;
        }

        private async void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (Accounts.accounts.WindowState == WindowState.Maximized)
            {
                Accounts.accounts.WindowState = WindowState.Maximized;
                Accounts accounts = new Accounts();
                Accounts.accounts.WindowState = WindowState.Maximized;
                accounts.Show();
                await Task.Delay(200);
                Window.GetWindow(this).Close();
            }
            else
            {
                Accounts accounts = new Accounts();
                accounts.Show();
                await Task.Delay(100);
                Window.GetWindow(this).Close();
            }
        }

        /// <summary>
        /// register
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var notificationManager = new NotificationManager(NotificationPosition.TopRight);
                //chech if textbox and passwordbox empty or not
                if (txtUsername.Text.Length == 0)
                {
                    await notificationManager.ShowAsync(
                    new NotificationContent { Title = "Register", Type = NotificationType.Error, Message = "Enter an username" },
                    areaName: "WindowArea");
                    return;
                }
                else if (txtPassword.Password.Length == 0)
                {
                    await notificationManager.ShowAsync(
                    new NotificationContent { Title = "Register", Type = NotificationType.Error, Message = "Enter an password" },
                    areaName: "WindowArea");
                    return;
                }
                else if (txtPassword.Password != txtConfirmPassword.Password)
                {
                    await notificationManager.ShowAsync(
                    new NotificationContent { Title = "Register", Type = NotificationType.Error, Message = "Password not matching" },
                    areaName: "WindowArea");
                    txtConfirmPassword.Focus();
                    return;
                }
                else
                {
                    //api stuff

                    ////check if username or email already exist
                    //RestClient<Users> restClientt = new RestClient<Users>();

                    //user = await restClientt.GetAsync("api" + $"username={txtUsername.Text}&password={txtPassword.Password}&email={txtemail.Text}");

                    //if (user.Error != "USER_DOESNT_EXIST")
                    //{
                    //    //check if username or email already exist
                    //    await notificationManager.ShowAsync(
                    //    new NotificationContent { Title = "Register", Type = NotificationType.Error, Message = "UserName Already Taken" },
                    //    areaName: "WindowArea");
                    //}
                    //else
                    //{
                    //    //USER_DOESNT_EXIST
                    //    await restClientt.PostAsync(new Users(txtUsername.Text, txtPassword.Password, $"{txtemail.Text}"), "api");
                    //    Debug.WriteLine($"testboi" + restClientt.GetResponse());
                    user = new Users(txtUsername.Text, txtPassword.Password, txtemail.Text, null, null, null);
                    //saving

                    SaveManager.WriteToXmlFile<Users>(user, "data", "account");
                    //go to mainwindow

                    MainWindow mainwindow = new MainWindow();
                    mainwindow.Show();
                    //}
                }
            }
            catch
            {
            }
        }

        //allow only alphanumric
        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9a-zA-Z]", "");
            textboxSender.SelectionStart = cursorPosition;
        }

        //allow only alphanumric and some other symbols
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9a-zA-Z@.]", "");
            textboxSender.SelectionStart = cursorPosition;
        }

        private void checkbox_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}