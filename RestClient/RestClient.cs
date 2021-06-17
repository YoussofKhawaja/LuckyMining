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

using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LuckyMining.RestClient
{
    public enum AuthType
    {
        Basic,
        Token,
        None
    }

    /// <summary>
    /// RestClient implements methods for calling CRUD operations
    /// using HTTP.
    /// </summary>
    public class RestClient<T>
    {
        private string ResponseTxt = string.Empty;
        //public Users users = new Users();

        public string GetResponse()
        {
            string res = ResponseTxt;
            //ResponseTxt = string.Empty;
            return res;
        }

        public Task<T> GetResponseModel()
        {
            var taskModels = JsonConvert.DeserializeObject<T>(GetResponse());
            return Task.FromResult(taskModels);
        }

        public async Task<T> GetAsync(string WebServiceUrl)
        {
            try
            {
                var httpClient = new HttpClient();

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var json = await httpClient.GetStringAsync(WebServiceUrl);

                ResponseTxt = json;

                var taskModels = JsonConvert.DeserializeObject<T>(json);

                return taskModels;
            }
            catch
            {
                return default(T);
            }
        }

        public async Task<bool> PostAsync(T t, string WebServiceUrl, string authKey = null, AuthType authType = AuthType.None)
        {
            var httpClient = new HttpClient();
            byte[] authToken;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            switch (authType)
            {
                case AuthType.None:
                    break;

                case AuthType.Basic:
                    authToken = Encoding.ASCII.GetBytes($"{authKey}:");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(authToken));
                    break;

                case AuthType.Token:
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                        authKey);
                    break;

                default:
                    break;
            }

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(WebServiceUrl, httpContent);
            ResponseTxt = await result.Content.ReadAsStringAsync();

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> PostHTTPSAsync(T t, string WebServiceUrl)
        {
            var httpClient = new HttpClient();

            //https://stackoverflow.com/questions/22251689/make-https-call-using-httpclient
            //specify to use TLS 1.2 as default connection
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(WebServiceUrl, httpContent);

            return result.IsSuccessStatusCode;
        }

        public Task<bool> PostHTTPSAsyncRaw(string json, string WebServiceUrl)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebServiceUrl);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}