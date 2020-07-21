using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleAppChallenge
{
    class Program
    {
        //static void Main(string[] args)
        static async Task Main(string[] args)
        {
            
            var numberList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var evenNumberSum = SumEvenNumbers(numberList.ToList());
            Console.WriteLine($"Sum of even numbers from 1 thru 10 is {evenNumberSum}");
            var uri = "http://localhost:5000/getList";
            await RetrieveAndWrite(uri);
        }

        /// <summary>
        /// Sum up all the even numbers in a supplied List<int> parameter and return the result.
        /// </summary>
        /// <param name="numberList"></param>
        /// <returns></returns>
        static int SumEvenNumbers(List<int> numberList)
        {
            var result = numberList.Where(i => i % 2 == 0).Sum();
            return result;
        }

        static async Task RetrieveAndWrite(string uri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth","xyz");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Console.WriteLine("Get");
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {

                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }

            }
        }
    }
}
