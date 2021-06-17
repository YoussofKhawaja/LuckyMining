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

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LuckyMining.Models
{
    public class UserHash
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Mining
        {
            public string Algorithm { get; set; }
        }

        public class Stratum
        {
            public string Current_Pool { get; set; }
            public string Current_User { get; set; }
            public double Average_Latency { get; set; }
        }

        public class Session
        {
            public int Startup { get; set; }
            public string Startup_String { get; set; }
            public int Uptime { get; set; }
            public int Last_Update { get; set; }
            public int Active_GPUs { get; set; }
            public double Performance_Summary { get; set; }
            public string Performance_Unit { get; set; }
            public int Accepted { get; set; }
            public int Submitted { get; set; }
            public double TotalPower { get; set; }
        }

        public class GPU
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public double Performance { get; set; }

            [JsonPropertyName("Consumption(W)")]
            public double ConsumptionW { get; set; }

            [JsonPropertyName("FanSpeed(%)")]
            public int FanSpeed { get; set; }

            [JsonPropertyName("Temp(degC)")]
            public int TempDegC { get; set; }

            [JsonPropertyName("MemTemp(degC)")]
            public int MemTempDegC { get; set; }

            public int Session_Accepted { get; set; }
            public int Session_Submitted { get; set; }
            public int Session_HWErr { get; set; }
            public int Session_BestShare { get; set; }
            public string PCIE_Address { get; set; }
        }

        public class Root
        {
            public string Software { get; set; }
            public Mining Mining { get; set; }
            public Stratum Stratum { get; set; }
            public Session Session { get; set; }
            public List<GPU> GPUs { get; set; }
        }
    }
}