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
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace LuckyMining.Models
{
    [Serializable]
    public class Users
    {
        [JsonPropertyName("username")]
        public string username { get; set; }

        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("avatar")]
        public string avatar { get; set; }

        [JsonPropertyName("discriminator")]
        public string discriminator { get; set; }

        [JsonPropertyName("password")]
        public string password { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("address")]
        public string address { get; set; }

        [JsonPropertyName("error")]
        public string Error;

        [JsonPropertyName("code")]
        public string Code;

        public Users()
        {
        }

        public Users(string u, string p, string e, string i, string a, string d)
        {
            username = u;
            password = p;
            Email = e;
            id = i;
            avatar = a;
            discriminator = d;
        }

        public static implicit operator ImageSource(Users v)
        {
            throw new NotImplementedException();
        }
    }
}