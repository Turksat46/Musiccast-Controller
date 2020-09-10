using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace MusicCast_Controller_V1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //Welcome-Screen
            Console.WriteLine("Welcome to MusicCast Controller Version 1 - Created by Turksat46");

            //Asking for the IP-Address of the MusicCast-Device
            Console.WriteLine("Please Type your MusicCast-Device-IP here");
            string ipadress = Console.ReadLine();
            Console.Clear();
            
            //Messaging User that we are attempting the pinging to the Device! 
            //We want to ping to check if the IP-Address exists and is reachable!
            Console.WriteLine("Pinging to the IP-Address! This can take a minute");

            //Initialize the ping
            Ping pingSender = new Ping();
            IPAddress address = IPAddress.Parse(ipadress);
            int timeout = 10000;

            //Ping to the IP
            PingReply reply = pingSender.Send(address, timeout);

            //Go to a loop for checking the result of the ping!
            while (true){
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine("Succesfully pinged to {0}", reply.Address.ToString());
                    //Console.WriteLine("Turning the MusicCast-System on!");
                    //WebRequest request = WebRequest.Create("http://" + address.ToString() + "/YamahaExtendedControl/v1/main/setPower?power=on");
                    //WebResponse response = request.GetResponse();
                    //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    //break;

                    //SecurityCheck
                    //Get Network Name
                    Console.WriteLine("Creating Security-Question!");
                    WebRequest request = WebRequest.Create("http://" + address.ToString() + "/YamahaExtendedControl/v1/system/getNetworkStatus");
                    WebResponse response = request.GetResponse();
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        Console.WriteLine(responseFromServer);


                    }
                    break;
                }
                    
            
            }

            //Start the Menu!
            for (; ; )
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("----------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("What would you like to do? Type Help for commands!");
                string input = Console.ReadLine();

                //Check the input!
                if(input == "Help" || input == "help")
                {
                    Console.WriteLine("There are: Status, Volume, Power, Source, Control, Experimental, Exit");
                }

                if(input == "Exit" || input == "exit")
                {
                    Environment.Exit(0);
                }

                if (input == "Status" || input == "status")
                {
                    WebRequest request = WebRequest.Create("http://" + address.ToString() + "/YamahaExtendedControl/v1/main/getStatus");
                    request.Credentials = CredentialCache.DefaultCredentials;
                    WebResponse response = request.GetResponse();
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        String responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        Console.WriteLine(responseFromServer);

                        //Testing
                        if (System.Text.RegularExpressions.Regex.IsMatch(responseFromServer, "power"))
                        {
                            Console.WriteLine("Found!");
                        }

                        //Testing 2
                        string powerstatus = responseFromServer.Substring(responseFromServer.IndexOf("power")+8, 12);
                        //string powerstatus = responseFromServer.LastIndexOf
                        Console.WriteLine(powerstatus);
                        
                    }

                    // Close the response.
                    response.Close();

                }

                if(input == "Volume" || input == "volume")
                {
                    Console.WriteLine("What do you want to change to the volume? up, down, (a number)?");
                    string vlm = Console.ReadLine();
                    WebRequest request = WebRequest.Create("http://" + address.ToString() + "/YamahaExtendedControl/v1/main/setVolume?volume="+vlm);
                    WebResponse response = request.GetResponse();
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        Console.WriteLine(responseFromServer);
                    }

                    // Close the response.
                    response.Close();
                }

                if(input == "Power" || input == "power")
                {
                    Console.WriteLine("Which State? on, standby, toggle?");
                    string state = Console.ReadLine();
                    WebRequest request = WebRequest.Create("http://" + address.ToString() + "/YamahaExtendedControl/v1/main/setPower?power="+ state);
                    WebResponse response = request.GetResponse();
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        Console.WriteLine(responseFromServer);
                    }
                }

                if (input == "Source" || input == "source")
                {
                    Console.WriteLine("Which Source should MusicCast open for?");
                    Console.WriteLine("net_radio, napster, spotify, juke, qobuz, tidal, deezer, server, bluetooth, airplay, mc_link");
                    string source = Console.ReadLine();
                    WebRequest request = WebRequest.Create("http://" + address.ToString() + "/YamahaExtendedControl/v1/main/setInput?input=" + source);
                    WebResponse response = request.GetResponse();
                }

                if(input == "Control" || input == "control")
                {
                    Console.WriteLine("What would you like to do? stop, play, previous, next, toggleShuffle, toggleRepeat ?");
                    string action = Console.ReadLine();
                    WebRequest request = WebRequest.Create("http://" + address.ToString() + "/YamahaExtendedControl/v1/netusb/setPlayback?playback=" + action);
                    WebResponse response = request.GetResponse();
                }

                if(input == "Experimental" || input == "experimental")
                {
                    Console.WriteLine("Write your own command here! For help, please consult the readme-file in the git!");
                    string command = Console.ReadLine();
                    WebRequest request = WebRequest.Create(command);
                    WebResponse response = request.GetResponse();
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        Console.WriteLine(responseFromServer);
                    }

                    // Close the response.
                    response.Close();
                }
            }
        }
    }
}
