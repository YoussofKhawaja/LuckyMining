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

using LibreHardwareMonitor.Hardware;
using LuckyMining.Models;
using LuckyMining.RestClient;
using LuckyMining.Saving;
using LuckyMining.Views;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace LuckyMining
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Users user;
        public static CryptoEco.Root eth;
        public static string fanlive2;
        public static string templive2;
        public static string loadlive2;
        public static BlockM.Root BL;
        public static UserHash.Root hash;
        public static Workers.Root workerscount;
        public static MinerBalance.Root minerbalance;
        public static SharesInfo.Root sharessinfo;
        public static string workerscountapp;
        public static string balanceunpaid;
        public static string sharesvalid;
        public static string hashratesfromminer;

        //gpu
        private Computer computer = new Computer
        {
            IsGpuEnabled = true
        };

        public async void ApplicationStart(object sender, StartupEventArgs e)
        {
            try
            {
                //here where we get saved data
                user = SaveManager.ReadFromXmlFile<Users>("data", "account");
                //if user is null
                if (user == null)
                {
                    Debug.WriteLine("does not exist");
                    Accounts accounts = new Accounts();
                    accounts.Show();
                }
                //username not found
                else if (user.username == null)
                {
                    Debug.WriteLine("username not found");
                    Accounts accounts = new Accounts();
                    accounts.Show();
                }
                //if user not null
                if (user != null && user.username != null)
                {
                    Debug.WriteLine("username=" + user.username);
                    MainWindow mainwindow = new MainWindow();
                    mainwindow.Show();
                }
                //get gpu details
                await Task.Run(gpu);
                await Task.Run(minerapi);
                await Task.Run(async () =>
                {
                    //get eth balance
                    RestClient<CryptoEco.Root> ethh = new RestClient<CryptoEco.Root>();
                    eth = await ethh.GetAsync("https://api.coingecko.com/api/v3/simple/price?ids=ethereum&vs_currencies=USD");

                    Debug.WriteLine("App.xaml.cs Crypto");
                    if (eth == null)
                    {
                        return;
                    }
                    //get blockchain details
                    RestClient<BlockM.Root> blocks = new RestClient<BlockM.Root>();
                    BL = await blocks.GetAsync("https://etherchain.org/api/basic_stats");

                    Debug.WriteLine("App.xaml.cs BlockM");
                    if (BL == null)
                    {
                        return;
                    }
                });
            }
            catch
            {
            }
        }

        private async void gpu()
        {
            try
            {
                while (true)
                {
                    //get gpu details by LibreHardwareMonitor
                    computer.Open();
                    {
                        foreach (IHardware hardware in computer.Hardware)
                        {
                            hardware.Update();
                            foreach (ISensor sensor in hardware.Sensors)
                            {
                                //get fan speed
                                if (sensor.Name.Equals("GPU Fan", StringComparison.OrdinalIgnoreCase))
                                {
                                    fanlive2 = ($"{sensor.Value}%");
                                }
                                //get temp
                                if (sensor.SensorType == SensorType.Temperature)
                                {
                                    templive2 = (sensor.Value + " " + "celsius (°C)");
                                }
                                //get gpu load
                                if (sensor.SensorType == SensorType.Load)
                                {
                                    loadlive2 = (sensor.Value + "%");
                                }
                            }
                        }
                    };
                    await Task.Delay(500);
                    computer.Close();
                }
            }
            catch
            {
            }
        }

        private async void minerapi()
        {
            try
            {
                while (true)
                {
                    //here where we get saved data
                    user = SaveManager.ReadFromXmlFile<Users>("data", "account");

                    //workers
                    RestClient<Workers.Root> workersonline = new RestClient<Workers.Root>();
                    workerscount = await workersonline.GetAsync("https://api.flexpool.io/v2/miner/workerCount?coin=ETH&address=0x0473E7Ade3C7cc6371aFBa073f0E918134F20205");

                    Debug.WriteLine("App.xaml.cs Workers");

                    if (workerscount != null)
                        workerscountapp = workerscount.result.workersOnline + "/" + workerscount.result.workersOffline;
                    else
                        workerscountapp = "0" + "/" + "0";
                    await Task.Delay(500);
                    //unpaid balance
                    RestClient<MinerBalance.Root> unpaid = new RestClient<MinerBalance.Root>();
                    minerbalance = await unpaid.GetAsync("https://api.flexpool.io/v2/miner/balance?coin=ETH&address=0x0473E7Ade3C7cc6371aFBa073f0E918134F20205");

                    Debug.WriteLine("App.xaml.cs MinerBalance");

                    if (minerbalance != null)
                        balanceunpaid = minerbalance.result.balanceCountervalue + " " + "USD";
                    else
                        balanceunpaid = "0" + " " + "USD";
                    await Task.Delay(500);

                    //Shares Info
                    RestClient<SharesInfo.Root> sharesinfo = new RestClient<SharesInfo.Root>();
                    sharessinfo = await sharesinfo.GetAsync("https://api.flexpool.io/v2/miner/workers?coin=ETH&address=0x0473E7Ade3C7cc6371aFBa073f0E918134F20205&worker=" + $"{user.username}");

                    Debug.WriteLine("App.xaml.cs SharesInfo");

                    if (sharessinfo.result != null)
                        sharesvalid = sharessinfo.result[0].validShares.ToString();
                    else
                        sharesvalid = "0";

                    if (Process.GetProcesses().Count(p => p.ProcessName == "lolMiner") == 1)
                    {
                        RestClient<UserHash.Root> userhash1 = new RestClient<UserHash.Root>();
                        hash = await userhash1.GetAsync("http://localhost:4789");

                        Debug.WriteLine("App.xaml.cs UserHash");

                        if (hash != null)
                            hashratesfromminer = hash.Session.Performance_Summary.ToString();
                        else
                            hashratesfromminer = "0";
                    }
                    else
                    {
                        hashratesfromminer = "0";
                    }
                }
            }
            catch
            {
            }
        }
    }
}