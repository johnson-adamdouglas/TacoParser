using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;
using System.Runtime.ExceptionServices;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();

        //This is our .csv file stored in a variable named csvPath
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            logger.LogInfo("Log initialized");

            // using File.ReadAllLines(path) to grab all the lines from the .csv file and storing each one in a string array named lines
            var lines = File.ReadAllLines(csvPath);

            // This will log an error if zero lines are returned and will log a warning if only one line is returned
            if (lines.Length == 0)
            {
                logger.LogError("No data recieved");
            }
            if (lines.Length == 1)
            {
                logger.LogWarning("Only 1 line of data recieved");
            }
            logger.LogInfo($"Lines: {lines[0]}");

            // Instantiating a new object of type TacoParser that will be used to parse the .csv file
            var parser = new TacoParser();

            // Using parser's Parse method and Linq Select command to parse each line in lines and convert to array 
            var locations = lines.Select(parser.Parse).ToArray();

            // These 2 ITrackable objects will hold the locations of the 2 points furthest from eachother as we iterate through them
            ITrackable tacoBell1 = null;
            ITrackable tacoBell2 = null;

            //This variavle will store the distance between locations
            double distance = 0;

            // This for loop will iterate through each location in locations and temporarily store them in variable locA
            for (int i = 0; i < locations.Length; i++)
            {
                var locA = locations[i];
                var corA = new GeoCoordinate();
                corA.Latitude = locA.Location.Latitude;
                corA.Longitude = locA.Location.Longitude;

                // This for loop will iterate through every other location and temporarily store them in variable locB while using
                // GeoCoordinate.GetDistanceTo to get the distance between locA and locB
                for (int j = 0; j < locations.Length; j++)
                {
                    var locB = locations[j];
                    var corB = new GeoCoordinate();
                    corB.Latitude = locB.Location.Latitude;
                    corB.Longitude = locB.Location.Longitude;

                    //If distance between locA and locB is greater than the current distance, distance is updated and locA and locB are stored in
                    //the ITrackable variables tacoBell1 and tacoBell2
                    if (corA.GetDistanceTo(corB) > distance)
                    {
                        distance = corA.GetDistanceTo(corB);
                        tacoBell1 = locA;
                        tacoBell2 = locB;
                    }
                }
            }

            // Now we will log the two locations furthest from eachother and display the distance between them
            logger.LogInfo($"{tacoBell1.Name} and {tacoBell2.Name} are the farthest apart at {distance * .000621371192} miles");

            
        }
    }
}
