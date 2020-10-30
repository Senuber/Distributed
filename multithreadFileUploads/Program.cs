using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace multithreadFileUploads
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declare a list parameter for save colletion of files for download
            List< KeyValuePair<string, string> > fileLinksList = new List<KeyValuePair<string, string>>();

            //adding links and their names into List fileLinksList 
            fileLinksList.Add(new KeyValuePair<string, string> ("http://www.ubicomp.org/ubicomp2003/adjunct_proceedings/proceedings.pdf", "file 1"));
            fileLinksList.Add(new KeyValuePair<string, string> ("https://www.hq.nasa.gov/alsj/a17/A17_FlightPlan.pdf", "file 2"));
            fileLinksList.Add(new KeyValuePair<string, string> ("https://ars.els-cdn.com/content/image/1-s2.0-S0140673617321293-mmc1.pdf", "file 3"));
            fileLinksList.Add(new KeyValuePair<string, string> ("http://www.visitgreece.gr/deployedFiles/StaticFiles/maps/Peloponnese_map.pdf", "file 4" ));

            //declare thread_mode parameter, to save the user input data for mod selecting 
            string thread_mode;

            // show welcome text and help for mode selecting
            Console.WriteLine("please, enter the tread mode, thread_more = 0 -> single threaded, thread_more = 1 -> multi-threaded");

            //read the mode selecting
            thread_mode = Console.ReadLine();

            //single thread mode
            if (thread_mode == "0")
            {
                Console.WriteLine("singe-thread mode activated");

                //in loop donwload all files in single mode, it means next file downloading will start when previus file finished
                foreach (var f in fileLinksList)
                {
                    //declare a parameter for calculating time spent for downloading file and start it
                    var watch = System.Diagnostics.Stopwatch.StartNew();

                    using (var wc = new WebClient())
                    {
                        Console.WriteLine(String.Format("{0} -> download started", f.Value));
                        wc.DownloadFile(f.Key, f.Value);
                        Console.WriteLine(String.Format("{0} -> done", f.Value));
                    };

                    //stop the watch parameter as the process of downloading finished, and calculate the time elpased fomr start in milliseconds
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;

                    Console.WriteLine(String.Format("elapsed time for {0} : {1}", f.Value, elapsedMs));
                }
            }

            //multy thread mode
            if (thread_mode == "1")
            {
                Console.WriteLine("multi-thread mode activated");

                //start all downloading processes simultaniously,in multithread mode. it means every file downlaing will start in new thread and in general it means all downlading processes will work in parallel
                foreach (var f in fileLinksList)
                {
                    //create new thread and run all code included in its braces
                    new Thread(() =>
                    {
                        //declare a parameter for calculating time spent for downloading file and start it
                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        using (var wc = new WebClient())
                        {
                            Console.WriteLine(String.Format("{0} -> download started", f.Value));
                            wc.DownloadFile(f.Key, f.Value);
                            Console.WriteLine(String.Format("{0} -> done", f.Value));
                        }

                        //stop the watch parameter as the process of downloading finished, and calculate the time elpased fomr start in milliseconds
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;

                        Console.WriteLine(String.Format("elapsed time for {0} : {1}", f.Value, elapsedMs));
                    }).Start();
                }
            }
        }

    }
}
