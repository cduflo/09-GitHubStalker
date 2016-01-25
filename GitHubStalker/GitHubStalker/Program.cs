using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GitHubStalker
{
    class Program
    {
        static void Main(string[] args)
        {
            /*  ---To use file data instead ----
            string user = "user";
            File.WriteAllText("user.json", user);
            string repost = "repos";
            File.WriteAllText("repos.json", repost);
            string user = File.ReadAllText("user.json");
            var o = JObject.Parse(user);

            string repos = File.ReadAllText("repos.json");
            var re = JArray.Parse(repos);
            */

            string username = "";

            while (true)
            {
                try {
                    //get username input (upgrade to method with try/catch)
                    Console.WriteLine("Please enter a username");
                    username = Console.ReadLine();

                    //download content from github, 
                    WebClient wcu = new WebClient();
                    wcu.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    string json = wcu.DownloadString("https://api.github.com/users/" + username);

                    WebClient wcc = new WebClient();
                    wcc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    string repos = wcc.DownloadString("https://api.github.com/users/" + username + "/repos");

                    //parse json
                    var o = JObject.Parse(json);
                    var re = JArray.Parse(repos);

                    //display data gathered from api
                    Console.WriteLine("Name: " + o["name"].ToString());
                    Console.WriteLine("Url: " + o["url"].ToString());
                    Console.WriteLine("Followers: " + o["followers"].ToString());
                    Console.WriteLine();
                    Console.WriteLine("Repositories: " + o["public_repos"].ToString());

                    for (var i = 0; i < re.Count; i++)
                    {
                        var obj = re[i];
                        //rep title, stars, watchers
                        Console.WriteLine("---- " + obj["name"].ToString() + ", " + obj["stargazers_count"].ToString() + " stars, " + obj["watchers_count"].ToString() + " watchers.");
                    }
                    break;
                }
                catch
                {
                    Console.WriteLine("Please enter a valid username.");
                    Console.WriteLine();
                }

            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Enter the name of the repository you would like additional information on. Enter 'quit' to exit.");
                string addrep = Console.ReadLine();

                if (addrep == "quit")
                {
                    break;
                }
                else
                {
                    try {
                        WebClient wc = new WebClient();
                        wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                        string cmt = wc.DownloadString("https://api.github.com/repos/" + username + "/" + addrep + "/commits");
                        var c = JArray.Parse(cmt);

                        WebClient wci = new WebClient();
                        wci.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                        string iss = wci.DownloadString("https://api.github.com/repos/" + username + "/" + addrep + "/issues");
                        var issue = JArray.Parse(iss);

                        Console.WriteLine();
                        Console.WriteLine(addrep);
                        Console.WriteLine("Commits: " + c.Count.ToString());
                        Console.WriteLine("Issues " + issue.Count.ToString());
                    }
                    catch
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please enter a valid repository name.");
                    }
                }
            }


            Console.ReadLine();


        }
    }
}
