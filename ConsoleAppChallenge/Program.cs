using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppChallenge
{
    class Program
    {
        static List<string> trackingList = new List<string>();
        // static void Main(string[] args)
        static async Task Main(string[] args)
        {
            Console.WriteLine("Main Thread Started");
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            var delay = config["delay"];
            Console.WriteLine($" delay is: {delay}ms");

            var numberList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var evenNumberSum = SumEvenNumbers(numberList.ToList());
            Console.WriteLine($"Sum of even numbers from 1 thru 10 is {evenNumberSum}");
            var uri = config["uri"];// "https://localhost:5001/api/values/";
            Console.WriteLine(uri);
            await RetrieveAndWrite200(uri);
            await RetrieveAndWrite500(uri);
            await GetTimeout(uri);

            //Creating Threads
            Thread t1 = new Thread(new ParameterizedThreadStart(Thread1Print))
            {
                Name = "Thread1"
            };
            Thread t2 = new Thread(Thread2Print)
            {
                Name = "Thread1"
            };
            Thread t3 = new Thread(Thread3Print)
            {
                Name = "Thread2"
            };
            //Executing the methods
            t1.Start(delay);
            t2.Start();
            t3.Start();
            //wait for t1 to fimish
            t1.Join();
            //wait for t2 to finish
            t2.Join();
            //wait for t3 to finish
            t3.Join();

            foreach (var tprint in trackingList)
            {
                Console.WriteLine(tprint);
            }
            Console.WriteLine("Main Thread Ended");
            Console.Read();

        }

        static void Thread1Print(object delay)
        {
            var numberList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Console.WriteLine("Thread1Print Started using " + Thread.CurrentThread.Name);
            for (int i = 0; i < numberList.Length; i++)
            {
                Console.WriteLine(numberList[i]);
                Thread.Sleep(Convert.ToInt32(delay));
            }
            Console.WriteLine("Thread1Print Ended using " + Thread.CurrentThread.Name);

        }

        static void Thread2Print()
        {
            var numberList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            for (int i = 0; i < numberList.Length; i++)
            {
                Console.WriteLine(numberList[i]);
                trackingList.Add($"{Thread.CurrentThread.Name}: {numberList[i]}");
                Thread.Sleep(500);
            }

        }

        static void Thread3Print()
        {
            var numberList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            for (int i = numberList.Length - 1; i >= 0; i--)
            {
                Console.WriteLine(numberList[i]);
                trackingList.Add($"{Thread.CurrentThread.Name}: {numberList[i]}");
                Thread.Sleep(1000);
            }

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


        static async Task RetrieveAndWrite500(string uri)
        {
            try
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        client.BaseAddress = new Uri(uri);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var uriMethod = uri + "getservertime/";
                        var responseLog = new ServerResponseLogDto();
                        responseLog.Starttime = DateTime.UtcNow;
                        HttpResponseMessage response = await client.GetAsync(uriMethod);
                        responseLog.Endtime = DateTime.UtcNow;
                        responseLog.HttpStatus = (int)response.StatusCode;
                        var responseText = await response.Content.ReadAsStringAsync();
                        
                        if (response.IsSuccessStatusCode)
                        {

                            //change to datetime local 
                            Console.WriteLine($"The WebAPI server date is: {responseText}");
                            responseLog.ErrorCode = 1;
                            responseLog.ResponseText = responseText;
                        }
                        else if (response.StatusCode == HttpStatusCode.InternalServerError)
                        {

                            responseLog.ErrorCode = 2;
                            responseLog.ResponseText = "Internal Service Error.";
                            Console.WriteLine("Internal Service Error.");
                        }
                        else if (response.StatusCode == HttpStatusCode.GatewayTimeout)
                        {
                            responseLog.ErrorCode = -999;
                            Console.WriteLine("Timeout Error.");
                        }
                        else
                        {
                            responseLog.ResponseText = "Bad Request";
                            responseLog.ErrorCode = 2;
                        }

                        await AddLogRecord(responseLog);

                    }
                }
            }
            catch
            {
                Console.WriteLine("Make sure to start the WebAPI");
            }

        }

        static async Task RetrieveAndWrite200(string uri)
        {
            try
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        client.BaseAddress = new Uri(uri);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var uriMethod = uri + "getservertime/1";
                        var responseLog = new ServerResponseLogDto();
                        responseLog.Starttime = DateTime.UtcNow;
                        HttpResponseMessage response = await client.GetAsync(uriMethod);
                        responseLog.Endtime = DateTime.UtcNow;
                        responseLog.HttpStatus = (int)response.StatusCode;
                        var responseText = await response.Content.ReadAsStringAsync();
                        
                        if (response.IsSuccessStatusCode)
                        {

                            //change to datetime local 
                            Console.WriteLine($"The WebAPI server date is: {responseText}");
                            responseLog.ResponseText = responseText;
                            responseLog.ErrorCode = 1;
                        }
                        else if (response.StatusCode == HttpStatusCode.InternalServerError)
                        {

                            responseLog.ErrorCode = 2;
                            responseText = "Internal Service Error";
                            Console.WriteLine("Internal Service Error.");
                        }
                        else
                        {
                            responseLog.ResponseText = "Bad Request";
                            responseLog.ErrorCode = 2;
                        }

                        await AddLogRecord(responseLog);

                    }
                }
            }
            catch
            {
                Console.WriteLine("Make sure to start the WebAPI");
            }

        }

        static async Task GetTimeout(string uri)
        {
            try
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        client.BaseAddress = new Uri(uri);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var uriMethod = uri + "gettimeout/";
                        var responseLog = new ServerResponseLogDto();
                        responseLog.Starttime = DateTime.UtcNow;
                        HttpResponseMessage response = await client.GetAsync(uriMethod);
                        responseLog.Endtime = DateTime.UtcNow;
                        responseLog.HttpStatus = (int)response.StatusCode;
                        var responseText = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {

                            //change to datetime local 
                            Console.WriteLine($"The WebAPI server date is: {responseText}");
                            responseLog.ResponseText = responseText;
                            responseLog.ErrorCode = 1;
                        }
                        else if (response.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            if (responseText.Contains("System.TimeoutException"))
                            {
                                responseLog.ResponseText = "System.TimeoutException";
                                responseLog.ErrorCode = -999;
                            }
                            else
                            {
                                responseLog.ResponseText = "Internal Service Error";
                                responseLog.ErrorCode = 2;
                            }
                                
                            Console.WriteLine("Internal Service Error.");
                        }
                        else
                        {
                            responseLog.ResponseText = "Bad Request";
                            responseLog.ErrorCode = 2;
                        }

                        await AddLogRecord(responseLog);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Make sure to start the WebAPI");
            }

        }



        static async Task AddLogRecord(ServerResponseLogDto logDto)
        {
            try
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        var uri = "https://localhost:5001/api/values/";
                        client.BaseAddress = new Uri(uri);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var json = JsonConvert.SerializeObject(logDto);
                        HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync(uri, contentPost);

                        //HttpResponseMessage response = await client.PostAsync(uri, contentPost);
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            //change to datetime local 
                            Console.WriteLine("Response log added");
                        }
                        else if (response.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            Console.WriteLine("Response log failed: Internal Service Error.");
                        }
                        else
                        {
                            Console.WriteLine("Response log failed: Bad Request.");
                        }

                    }
                }
            }
            catch
            {
                Console.WriteLine("Make sure to start the WebAPI");
            }

        }



    }
}
