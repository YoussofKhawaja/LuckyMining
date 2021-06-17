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
using DiscordRPC.Logging;
using LuckyMining.Models;
using LuckyMining.Saving;
using LuckyMining.Views;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LuckyMining
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Users user;
        public static MainWindow mainwindow;
        public static DiscordRpcClient client;
        private static int idleTime = 0;

        public MainWindow()
        {
            this.InitializeComponent();
            startStatusBarTimer();
            Discord();

            //views pages
            mainwindow = this;
            grid.Children.Clear();
            grid.Children.Add(new MinerLauncher());
            grid.Children.Add(new Settings());

            foreach (UIElement uie in grid.Children)
            {
                uie.Visibility = Visibility.Hidden;
            }
            grid.Children[0].Visibility = Visibility.Visible;
        }

        private void startStatusBarTimer()
        {
            try
            {
                System.Timers.Timer statusTime = new System.Timers.Timer();

                statusTime.Interval = 1000;

                statusTime.Elapsed += new System.Timers.ElapsedEventHandler(data);

                statusTime.Enabled = true;
            }
            catch
            {
            }
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //on listviewmenu selected item change page

            if (grid == null)
                return;

            int index = ListViewMenu.SelectedIndex;

            switch (index)
            {
                //first page MinerLauncher

                case 0:
                    foreach (UIElement uie in grid.Children)
                    {
                        uie.Visibility = Visibility.Hidden;
                    }

                    grid.Children[0].Visibility = Visibility.Visible;
                    break;
                //second page Rewards

                case 1:
                    foreach (UIElement uie in grid.Children)
                    {
                        uie.Visibility = Visibility.Hidden;
                    }

                    grid.Children[1].Visibility = Visibility.Visible;
                    break;

                default:
                    break;
            }
        }

        //exit app
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //on button click exit Application
            if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") >= 1)
            {
                foreach (Process process in Process.GetProcessesByName("lolMiner"))
                {
                    process.Kill();
                }
            }
            client.Dispose();
            System.Windows.Application.Current.Shutdown();
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

        //double click maximize
        private long doublePressInterval_ms = 300; private DateTime lastPressTime = DateTime.Now;

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DateTime pressTime = DateTime.Now;
            if ((pressTime - lastPressTime).TotalMilliseconds <= doublePressInterval_ms)
            {
                //clicked twice
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            }
            lastPressTime = pressTime;
        }

        //maximize window button

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

        //Called when your application first starts.
        private void Discord()
        {
            user = SaveManager.ReadFromXmlFile<Users>("data", "account");
            Uri uri = new Uri($"https://cdn.discordapp.com/avatars/{user.id}/{user.avatar}.png", UriKind.Absolute);
            ImageSource imgSource = new BitmapImage(uri);
            avatar.ImageSource = imgSource;
            //Create a Discord client

            client = new DiscordRpcClient("//Your Client id");

            //Set the logger
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Debug.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Debug.WriteLine("Received Update! {0}", e.Presence);
            };

            //Connect to the RPC
            client.Initialize();
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        private static int GetLastInputTime()
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            int envTicks = Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                int lastInputTick = (int)lastInputInfo.dwTime;

                idleTime = envTicks - lastInputTick;
            }

            return (idleTime > 0) ? (idleTime / 1000) : idleTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://github.com/YoussofKhawaja/LuckyMining";

            Process newProcess = Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void data(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                GetLastInputTime();

                if (ListViewMenu.SelectedIndex == 0)
                {
                    if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") == 0 && idleTime / 1000 > 60)
                    {
                        //Invoke all the events, such as OnPresenceUpdate
                        MainWindow.client.Invoke();
                        MainWindow.client.SetPresence(new RichPresence()
                        {
                            Details = "In MinerLauncher",
                            State = "Idle Not Mining",
                            Assets = new Assets()
                            {
                                LargeImageKey = "lm",
                                LargeImageText = "LuckyMining",
                                SmallImageKey = "moon",
                                SmallImageText = "Idle"
                            }
                        });
                    }
                    else if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") == 1 && idleTime / 1000 > 60)
                    {
                        //Invoke all the events, such as OnPresenceUpdate
                        MainWindow.client.Invoke();
                        MainWindow.client.SetPresence(new RichPresence()
                        {
                            Details = "In MinerLauncher",
                            State = "Idle Mining Ethereum",
                            Assets = new Assets()
                            {
                                LargeImageKey = "mining",
                                LargeImageText = "LuckyMining",
                                SmallImageKey = "moon",
                                SmallImageText = "Idle"
                            }
                        });
                    }
                    else if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") == 0)
                    {
                        //Invoke all the events, such as OnPresenceUpdate
                        MainWindow.client.Invoke();
                        MainWindow.client.SetPresence(new RichPresence()
                        {
                            Details = "In MinerLauncher",
                            State = "Not Mining",
                            Assets = new Assets()
                            {
                                LargeImageKey = "lm",
                                LargeImageText = "LuckyMining"
                            }
                        });
                    }
                    else if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") == 1)
                    {
                        //Invoke all the events, such as OnPresenceUpdate
                        MainWindow.client.Invoke();
                        MainWindow.client.SetPresence(new RichPresence()
                        {
                            Details = "In MinerLauncher",
                            State = "Mining Ethereum",
                            Assets = new Assets()
                            {
                                LargeImageKey = "mining",
                                LargeImageText = "LuckyMining"
                            }
                        });
                    }
                }

                //second page
                else
                {
                    if (ListViewMenu.SelectedIndex == 1 && idleTime / 1000 > 60)
                    {
                        Debug.WriteLine("2" + " " + idleTime / 1000);
                        //Invoke all the events, such as OnPresenceUpdate
                        MainWindow.client.Invoke();
                        MainWindow.client.SetPresence(new RichPresence()
                        {
                            Details = "In Settings",
                            State = "Idle",
                            Assets = new Assets()
                            {
                                LargeImageKey = "settings",
                                LargeImageText = "LuckyMining",
                                SmallImageKey = "moon",
                                SmallImageText = "Idle"
                            }
                        });
                    }
                    else if (ListViewMenu.SelectedIndex == 1)
                    {
                        Debug.WriteLine("1" + " " + idleTime / 1000);
                        //Invoke all the events, such as OnPresenceUpdate
                        MainWindow.client.Invoke();
                        MainWindow.client.SetPresence(new RichPresence()
                        {
                            Details = "In Settings",
                            State = "Doing stuff",
                            Assets = new Assets()
                            {
                                LargeImageKey = "settings",
                                LargeImageText = "LuckyMining"
                            }
                        });
                    }
                }
            });
        }
    }
}