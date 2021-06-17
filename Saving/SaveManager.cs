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

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace LuckyMining.Saving
{
    public static class SaveManager
    {
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/LuckyMining/";

        //Custom
        public static void WriteToXmlFile<T>(T objectToWrite, string folderName, string fileName, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                Debug.WriteLine($"Saving Path: {path + folderName}");
                CreateDir(path + folderName);
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(path + folderName + "\\" + RemoveSpecialCharacters(fileName) + ".xml", append);
                serializer.Serialize(writer, objectToWrite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Saving Error: {ex.Message}");
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromXmlFile<T>(string folderName, string fileName) where T : new()
        {
            if (CheckFileExist(path + folderName + "\\" + RemoveSpecialCharacters(fileName) + ".xml"))
            {
                TextReader reader = null;
                try
                {
                    var serializer = new XmlSerializer(typeof(T));
                    reader = new StreamReader(path + folderName + "\\" + RemoveSpecialCharacters(fileName) + ".xml");
                    return (T)serializer.Deserialize(reader);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            else
            {
                return new T();
            }
        }

        public static int key = 666;

        public static string EncryptDecrypt(string textToEncrypt)
        {
            StringBuilder inSb = new StringBuilder(textToEncrypt);
            StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
            char c;
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                c = inSb[i];
                c = (char)(c ^ key);
                outSb.Append(c);
            }
            return outSb.ToString();
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static bool CheckFileExist(string userID, string folderName, string _fileExt)
        {
            if (File.Exists(path + folderName + "\\" + RemoveSpecialCharacters(userID) + _fileExt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckFileExist(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DeleteFile(string filepath)
        {
            if (Directory.Exists(filepath))
            {
                Directory.Delete(filepath);
            }
            else
            {
                Console.WriteLine(filepath + " File doesn't exist!");
            }
        }

        public static void CreateDir(string path)
        {
            bool exists = Directory.Exists(path);

            if (!exists)
                Directory.CreateDirectory(path);
        }
    }
}