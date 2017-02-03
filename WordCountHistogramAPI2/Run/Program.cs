using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

namespace WordCountHistogramAPI2 
{ 
    public class Program 
    { 
        static void Main() 
        { 
            string baseAddress = "http://localhost:9000/"; 

            using (WebApp.Start<Startup>(url: baseAddress)) 
            { 
                Console.ReadLine(); 
            } 
        } 
    } 
 } 
