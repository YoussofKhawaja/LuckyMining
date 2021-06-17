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

using DiscordRPC;
using LuckyMining.Models;
using LuckyMining.Saving;
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LuckyMining.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public static Users user;

        public static int longg;

        public Settings()
        {
            InitializeComponent();
            Data();
        }

        public void Data()
        {
            try
            {
                //load user from SaveManager
                user = SaveManager.ReadFromXmlFile<Users>("data", "account");
                welcomeuser.Content = "Welcome" + " " + user.username;
                username.Content = user.username;
                if (user.Email == null)
                {
                    return;
                }
                else
                {
                    email.Content = user.Email;
                }
            }
            catch
            {
            }
        }

        //logout button pressed
        private void btcLogOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //delete user
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/LuckyMining/", true);

                //go back to login page
                Accounts accounts = new Accounts();
                accounts.Show();
                Window.GetWindow(this).Close();
            }
            catch
            {
            }
        }

        //change password
        private async void changepassword()
        {
            var notificationManager = new NotificationManager(NotificationPosition.TopRight);
            //check if old password correct
            if (oldpassword.Password == user.password)
            {
            }
            else
            {
                await notificationManager.ShowAsync(
                new NotificationContent { Title = "ChangePassword", Message = "Old Password was wrong" },
                areaName: "WindowArea");
            }

            //chech if textbox and passwordbox empty or not
            if (newpassword.Password.Length == 0)
            {
                await notificationManager.ShowAsync(
                new NotificationContent { Title = "ChangePassword", Message = "Enter an password" },
                areaName: "WindowArea");
            }
            else if (confirmpassword.Password.Length == 0)
            {
                await notificationManager.ShowAsync(
                new NotificationContent { Title = "ChangePassword", Message = "Enter an password in Confirmspassword box" },
                areaName: "WindowArea");
            }
            else if (newpassword.Password != confirmpassword.Password)
            {
                await notificationManager.ShowAsync(
                new NotificationContent { Title = "ChangePassword", Message = "Password not matching" },
                areaName: "WindowArea");
                confirmpassword.Focus();
                return;
            }
        }

        //change password button on pressed
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (oldpassword.Visibility == Visibility.Collapsed && newpassword.Visibility == Visibility.Collapsed && confirmpassword.Visibility == Visibility.Collapsed && forgetpassword.Visibility == Visibility.Collapsed && submitbutton.Visibility == Visibility.Collapsed)
            {
                arrow.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowDropDown;
                oldpassword.Visibility = Visibility.Visible;
                newpassword.Visibility = Visibility.Visible;
                confirmpassword.Visibility = Visibility.Visible;
                forgetpassword.Visibility = Visibility.Visible;
                submitbutton.Visibility = Visibility.Visible;
            }
            else if (oldpassword.Visibility == Visibility.Visible && newpassword.Visibility == Visibility.Visible && confirmpassword.Visibility == Visibility.Visible)
            {
                arrow.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowDropUp;
                oldpassword.Visibility = Visibility.Collapsed;
                newpassword.Visibility = Visibility.Collapsed;
                confirmpassword.Visibility = Visibility.Collapsed;
                forgetpassword.Visibility = Visibility.Collapsed;
                submitbutton.Visibility = Visibility.Collapsed;
            }
        }

        //chnage password submit button click
        private void submitbutton_Click(object sender, RoutedEventArgs e)
        {
            changepassword();
        }
    }
}