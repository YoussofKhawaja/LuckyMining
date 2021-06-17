//.____ __                 _____    .__           .__              ____//.____ __                 _____    .__           .__              ____
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

using System;
using System.Collections.Generic;

namespace LuckyMining.Models
{
    public class BlockM
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class CurrentStats
        {
            public DateTime time { get; set; }
            public double price_usd { get; set; }
            public double price_btc { get; set; }
            public long difficulty { get; set; }
            public double block_time { get; set; }
            public double hashrate { get; set; }
            public double tps { get; set; }
            public double uncle_rate { get; set; }
            public double block_reward { get; set; }
        }

        public class PreviousStats
        {
            public DateTime time { get; set; }
            public double price_usd { get; set; }
            public double price_btc { get; set; }
            public long difficulty { get; set; }
            public double block_time { get; set; }
            public long hashrate { get; set; }
            public double tps { get; set; }
            public double uncle_rate { get; set; }
            public double block_reward { get; set; }
        }

        public class Block
        {
            public string hash { get; set; }
            public int number { get; set; }
            public string miner_address { get; set; }
            public string miner { get; set; }
            public DateTime time { get; set; }
            public int tx_count { get; set; }
            public int uncle_count { get; set; }
            public object mining_reward { get; set; }
            public int block_time { get; set; }
            public bool iscontract { get; set; }
        }

        public class Root
        {
            public CurrentStats currentStats { get; set; }
            public PreviousStats previousStats { get; set; }
            public List<Block> blocks { get; set; }
            public List<List<double>> price_chart_data { get; set; }
        }
    }
}