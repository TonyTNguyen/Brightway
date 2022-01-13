using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Brightway
{
    
    class Program
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        class PizzaOrder
        {
            public IList<string> toppings;
        };
            
        static async Task Main(string[] args)
        {
            const int maxDisplay = 20;
            try
            {
                string rawData = await _httpClient.GetStringAsync("https://www.brightway.com/CodeTests/pizzas.json");
                var jsonData = JsonConvert.DeserializeObject<IList<PizzaOrder>>(rawData);

                if (jsonData != null)
                {
                    // fuse data for easy process later
                    var data = from o in jsonData select string.Join(", ", o.toppings);

                    // make final query
                    var result = data.GroupBy(x => x).Select(g => new { g.Key, Number = g.Count() }).OrderByDescending(p => p.Number).ToList();

                    // display result
                    int displayNumber = result.Count() > maxDisplay ? maxDisplay: result.Count();

                    Console.WriteLine("Most frequent order toppings:");

                    for (int i = 0; i < displayNumber; i++)
                    {
                        Console.WriteLine($"Toppings: {result[i].Key}. Number orders: {result[i].Number}");
                    }
                }
                else
                {
                    Console.WriteLine("There is no data from query");
                }
            }
            catch (HttpRequestException e) 
            {
                Console.WriteLine($"There is problem with http request {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to complete this project because -- {e.Message}");
            }
        }
    }
}
