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
using LuckyMining.RestClient;
using LuckyMining.Saving;
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace LuckyMining.Views
{
    /// <summary>
    /// Interaction logic for MinerLauncher.xaml
    /// </summary>
    public partial class MinerLauncher : UserControl
    {
        public static Users user;
        public static CryptoEco.Root eth;
        public static BlockM.Root BL;
        public static UserHash.Root hash;
        public static Workers.Root workerscount;
        public static MinerBalance.Root minerbalance;
        public static SharesInfo.Root sharessinfo;
        public static string genArgs;

        public MinerLauncher()
        {
            InitializeComponent();
            Data();
            startStatusBarTimer();
            minerapi();
        }

        //timer
        private void startStatusBarTimer()
        {
            try
            {
                System.Timers.Timer statusTime = new System.Timers.Timer();

                statusTime.Interval = 1500;

                statusTime.Elapsed += new System.Timers.ElapsedEventHandler(data);

                statusTime.Enabled = true;
            }
            catch
            {
            }
        }

        private async void minerapi()
        {
            try
            {
                if (App.workerscountapp != null && App.balanceunpaid != null && App.sharesvalid != null)
                {
                    Workers.Content = App.workerscountapp;
                    UnpaidBalance.Content = App.balanceunpaid;
                    Share.Content = App.sharesvalid;
                    Hashrates.Content = App.hashratesfromminer + " " + "Mh/s";
                }
                else
                {
                    //if gpu details null

                    Workers.Content = "Loading.";
                    UnpaidBalance.Content = "Loading.";
                    Share.Content = "Loading.";
                    Hashrates.Content = "Loading.";
                    await Task.Delay(500);
                    Workers.Content = "Loading..";
                    UnpaidBalance.Content = "Loading..";
                    Share.Content = "Loading..";
                    Hashrates.Content = "Loading..";
                    await Task.Delay(500);
                    Workers.Content = "Loading...";
                    UnpaidBalance.Content = "Loading...";
                    Share.Content = "Loading...";
                    Hashrates.Content = "Loading...";
                    await Task.Delay(500);
                }

                RestClient<UserHash.Root> userhash = new RestClient<UserHash.Root>();
                hash = await userhash.GetAsync("http://localhost:4789");

                Debug.WriteLine("MinerLauncher.xaml.cs UserHash");
            }
            catch
            {
            }
        }

        private void data(object sender, ElapsedEventArgs e)
        {
            //reload this class every 1500ms from startStatusBarTimer
            try
            {
                App.Current.Dispatcher.Invoke((Action)async delegate
                {
                    if (App.fanlive2 == null && App.templive2 == null && App.loadlive2 == null)
                    {
                        //if gpu details null
                        fan.Content = "Loading.";
                        temp.Content = "Loading.";
                        gpuload.Content = "Loading.";
                        await Task.Delay(500);
                        fan.Content = "Loading..";
                        temp.Content = "Loading..";
                        gpuload.Content = "Loading..";
                        await Task.Delay(500);
                        fan.Content = "Loading...";
                        temp.Content = "Loading...";
                        gpuload.Content = "Loading...";
                        await Task.Delay(500);
                    }
                    else
                    {
                        //not null and show details
                        fan.Content = App.fanlive2;
                        temp.Content = App.templive2;
                        gpuload.Content = App.loadlive2;
                    }

                    //check if process of lolMiner is only 1
                    try
                    {
                        if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") == 1 && App.BL != null && App.eth != null)
                        {
                            //check if data null and get them again
                            if (App.BL.currentStats.difficulty == 0 && App.BL.currentStats.block_time == 0 && App.BL.currentStats.block_reward == 0 && App.eth.ethereum.usd == 0)
                            {
                                RestClient<UserHash.Root> userhash = new RestClient<UserHash.Root>();
                                hash = await userhash.GetAsync("http://localhost:4789");

                                Debug.WriteLine("MinerLauncher.xaml.cs UserHash 2");
                                if (hash != null)
                                {
                                    RestClient<CryptoEco.Root> ethh = new RestClient<CryptoEco.Root>();
                                    eth = await ethh.GetAsync("https://api.coingecko.com/api/v3/simple/price?ids=ethereum&vs_currencies=USD");

                                    Debug.WriteLine("MinerLauncher.xaml.cs CryptoEco");

                                    RestClient<BlockM.Root> blocks = new RestClient<BlockM.Root>();
                                    BL = await blocks.GetAsync("https://etherchain.org/api/basic_stats");

                                    Debug.WriteLine("MinerLauncher.xaml.cs BlockM");

                                    per24.Content = Math.Round((hash.Session.Performance_Summary * 1e6 / ((BL.currentStats.difficulty * 0.000000000001 / BL.currentStats.block_time) * 1000 * 1e9)) * ((60 / BL.currentStats.block_time) * BL.currentStats.block_reward) * 60 * (eth.ethereum.usd) * 24, 2) + " " + "$/24 hours";
                                    perweek.Content = Math.Round((hash.Session.Performance_Summary * 1e6 / ((BL.currentStats.difficulty * 0.000000000001 / BL.currentStats.block_time) * 1000 * 1e9)) * ((60 / BL.currentStats.block_time) * BL.currentStats.block_reward) * 60 * (eth.ethereum.usd) * 168, 2) + " " + "$/1 week";
                                    permonth.Content = Math.Round(hash.Session.Performance_Summary * 1e6 / (BL.currentStats.difficulty * 0.000000000001 / BL.currentStats.block_time * 1000 * 1e9) * (60 / BL.currentStats.block_time * BL.currentStats.block_reward) * 60 * eth.ethereum.usd * 730.001, 2) + " " + "$/1 month";
                                }
                            }
                            //data not null
                            else
                            {
                                //get hashrate again
                                RestClient<UserHash.Root> userhash2 = new RestClient<UserHash.Root>();
                                hash = await userhash2.GetAsync("http://localhost:4789");

                                Debug.WriteLine("MinerLauncher.xaml.cs UserHash 3");
                                if (hash != null)
                                {
                                    //profit per 24

                                    per24.Content = Math.Round((hash.Session.Performance_Summary * 1e6 / ((App.BL.currentStats.difficulty * 0.000000000001 / App.BL.currentStats.block_time) * 1000 * 1e9)) * ((60 / App.BL.currentStats.block_time) * App.BL.currentStats.block_reward) * 60 * (App.eth.ethereum.usd) * 24, 2) + " " + "$/24 hours";

                                    //profit per week

                                    perweek.Content = Math.Round((hash.Session.Performance_Summary * 1e6 / ((App.BL.currentStats.difficulty * 0.000000000001 / App.BL.currentStats.block_time) * 1000 * 1e9)) * ((60 / App.BL.currentStats.block_time) * App.BL.currentStats.block_reward) * 60 * (App.eth.ethereum.usd) * 168, 2) + " " + "$/1 week";

                                    //profit per month

                                    permonth.Content = Math.Round((hash.Session.Performance_Summary * 1e6 / ((App.BL.currentStats.difficulty * 0.000000000001 / App.BL.currentStats.block_time) * 1000 * 1e9)) * ((60 / App.BL.currentStats.block_time) * App.BL.currentStats.block_reward) * 60 * (App.eth.ethereum.usd) * 730.001, 2) + " " + "$/1 month";
                                }
                            }
                        }

                        //if process of lolminer was 0
                        else if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") == 0)
                        {
                            per24.Content = "Loading PerHour.";
                            perweek.Content = "Loading PerWeek.";
                            permonth.Content = "Loading PerMonth.";
                            await Task.Delay(500);
                            per24.Content = "Loading PerHour..";
                            perweek.Content = "Loading PerWeek..";
                            permonth.Content = "Loading PerMonth..";
                            await Task.Delay(500);
                            per24.Content = "Loading PerHour...";
                            perweek.Content = "Loading PerWeek...";
                            permonth.Content = "Loading PerMonth...";
                            await Task.Delay(500);
                        }
                        minerapi();
                    }
                    catch (Exception eee)
                    {
                        Debug.WriteLine("error1" + eee);
                    }
                });
            }
            catch (Exception ee)
            {
                Debug.WriteLine("error2" + ee);
            }
        }

        public void Data()
        {
            try
            {
                //get gpu name
                ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");
                foreach (ManagementObject obj in myVideoObject.Get())
                {
                    gpuname.Content = (obj["Name"]);
                }

                //load from savemanager workername
                user = SaveManager.ReadFromXmlFile<Users>("data", "account");
                if (Accounts.emailuser == null)
                {
                    workername.Content = "Worker Name:" + " " + user.username;
                }
                else if (user.username == null)
                {
                    workername.Content = "Worker Name:" + " " + Accounts.user.username;
                }
                else
                {
                    workername.Content = "Worker Name:" + " " + Accounts.emailuser.Username;
                }

                //check if Process lolMiner is 0 so it say startmining
                if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") == 0)
                {
                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                    onoff.Text = "Start Mining";
                }
                else
                {
                    //mining
                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;
                    onoff.Text = "Mining";
                }

                user = SaveManager.ReadFromXmlFile<Users>("data", "account");
                if (user.address != null)
                {
                    addressstackpanel.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
            }
        }

        private void miner()
        {
            try
            {
                user = SaveManager.ReadFromXmlFile<Users>("data", "account");
                if (user.address != null)
                {
                    if (addressstackpanel.Visibility != Visibility.Collapsed)
                    {
                        addressstackpanel.Visibility = Visibility.Collapsed;
                    }

                    //miner args
                    genArgs = $"--algo ETHASH --pool stratum+ssl://eth-de.flexpool.io:5555 --user {user.address}.{user.username} --apiport 4789";

                    //path to lolminer.exe
                    string pathToFile = @"lolMiner\lolMiner.exe";
                    Process runProg = new Process();
                    runProg.StartInfo.FileName = pathToFile;
                    runProg.StartInfo.Arguments = genArgs;
                    runProg.StartInfo.CreateNoWindow = true;
                    runProg.Start();

                    //check if Process lolminer is bigger then 1 to kill others
                    if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") > 1)
                    {
                        foreach (Process process in Process.GetProcessesByName("lolMiner"))
                        {
                            process.Kill();
                        }
                    }
                }
                else
                {
                }
            }
            catch
            {
            }
        }

        //miner button pressed for both its on and off at same time
        private async void onoffbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") == 0)
                {
                    if (user.address != null)
                    {
                        //mining starting

                        onoff.Text = "Mining Starting";
                        icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;

                        //mining started
                        Thread t = new Thread(miner);
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();
                        onoff.Text = "Mining";
                    }
                    else
                    {
                        if (user.address == null)
                        {
                            var notificationManager = new NotificationManager(NotificationPosition.TopRight);
                            //chech if textbox and passwordbox empty or not

                            await notificationManager.ShowAsync(
                            new NotificationContent { Title = "Mner", Message = "Add pool address", Type = NotificationType.Warning },
                            areaName: "WindowArea");
                        }
                        else if (hash.Stratum.Current_Pool == "")
                        {
                            var notificationManager = new NotificationManager(NotificationPosition.TopRight);
                            //chech if textbox and passwordbox empty or not

                            await notificationManager.ShowAsync(
                            new NotificationContent { Title = "Mner", Message = "pool address Wrong", Type = NotificationType.Error },
                            areaName: "WindowArea");
                        }
                    }
                }

                //stop mining on click
                else if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") >= 1)
                {
                    foreach (Process process in Process.GetProcessesByName("lolMiner"))
                    {
                        process.Kill();
                    }
                    icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                    onoff.Text = "Start Mining";
                }
            }
            catch
            {
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            user.address = textboxaddress.Text;
            //save
            SaveManager.WriteToXmlFile<Users>(user, "data", "account");
            addressstackpanel.Visibility = Visibility.Collapsed;
        }
    }
}