using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Text.Json.Nodes;
using System;

namespace ConsoleClient
{
    class Program
    {
        //private static readonly HttpClient client = new HttpClient();

        private static async Task ProcessText()
        {
            Boolean putanja = false;

            while (putanja == false)
            {
                Console.WriteLine("Write full path to the file: ");
                string val = Console.ReadLine();
                if(val == null)
                {
                    Console.WriteLine("Please type valid path");
                    continue;
                }
                string path = @"" + val;
                if (File.Exists(path) == false)
                {
                    Console.WriteLine("Please type valid path");
                    continue;
                }

                putanja = true;
                string text = System.IO.File.ReadAllText(@"" + val);

                Console.WriteLine(text);
                ToDoText prom = new ToDoText();
                prom.Text = text;
                prom.isComplete = false;
                var json = JsonSerializer.Serialize(prom);
                //Console.WriteLine(json);

                var url = "https://localhost:7033/api/todotexts";
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(url, data);
                    if(response.IsSuccessStatusCode)
                    {
                        
                        var result = await response.Content.ReadAsStringAsync();
                        var obj = JsonObject.Parse(result);
                        Console.WriteLine("Total number of words in this file is: " + obj["numWords"].ToString());
                    }
                    else
                    {
                        Console.WriteLine($"Request failed. Error status code: {(int)response.StatusCode}");
                    }

                    
                }

            }
        }
        static async Task Main(string[] args)
        {
            await ProcessText();

        }
    }
}
