using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Text.Json.Nodes;
using System;
using System.IO;
using System.Net;
using ConsoleApp.DAL;
using ConsoleApp.Repository;
using System.Collections.Specialized;

namespace ConsoleApp
{
    public class Program
    {
        private static async Task ProcessText()
        {
            Boolean choice_val = false;
            int choice_int = -1;
            while (choice_val == false)
            {
                Console.WriteLine("Type 0 if you want to take file from local path or type 1 to take file from local DB");
                string choice = Console.ReadLine();
                if (string.IsNullOrEmpty(choice))
                {
                    Console.WriteLine("Please type valid choice");
                    continue;
                }
                if (int.TryParse(choice, out choice_int) && (choice_int == 0 || choice_int == 1))
                {
                    choice_val = true;

                }
                else
                {
                    Console.WriteLine("Please type valid choice");
                    continue;
                }
            }

            if (choice_int == 0)
            {
                Boolean putanja = false;

                while (putanja == false)
                {
                    Console.WriteLine("Write full path to the file: ");
                    string val = Console.ReadLine();
                    if (val == null)
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
                        if (response.IsSuccessStatusCode)
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
            else
            {
                ITextRepository _textRepository = new TextRepository(new TextDBEntities());
                var lista=_textRepository.GetAll();
                var listaid = new List<int>();
                Console.WriteLine("List of available ids:");
                foreach (var item in lista)
                {
                    Console.WriteLine("ID: " + item.TextID.ToString());
                    listaid.Add(item.TextID);
                }
                Console.WriteLine("Type TextID for which you want to count words");
                int choice_id = -1;
                Boolean choice_check = false;
                while (choice_check == false)
                {
                   
                    string textid = Console.ReadLine();
                    if (int.TryParse(textid, out choice_id) && listaid.Contains(choice_id))
                    {
                        choice_check = true;
                        var textDB = _textRepository.GetById(choice_id);
                        Console.WriteLine(textDB.Text);
                        ToDoText prom = new ToDoText();
                        prom.Text = textDB.Text;
                        prom.isComplete = false;
                        var json = JsonSerializer.Serialize(prom);
                        //Console.WriteLine(json);

                        var url = "https://localhost:7033/api/todotexts";
                        var data = new StringContent(json, Encoding.UTF8, "application/json");
                        using (var client = new HttpClient())
                        {
                            var response = await client.PostAsync(url, data);
                            if (response.IsSuccessStatusCode)
                            {

                                var result = await response.Content.ReadAsStringAsync();
                                var obj = JsonObject.Parse(result);
                                Console.WriteLine("Total number of words in this file is: " + obj["numWords"].ToString());
                                textDB.numWords = (int?)obj["numWords"];
                                textDB.isComplete = true;
                                _textRepository.Update(textDB);
                                _textRepository.Save();
                            }
                            else
                            {
                                Console.WriteLine($"Request failed. Error status code: {(int)response.StatusCode}");
                            }


                        }
                    }
                    else
                    {
                        Console.WriteLine("Type valid TextID");
                        continue;
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

