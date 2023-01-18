using System.Data.Common;

namespace LoggingKata
{
    public class TacoParser
    {
        readonly ILog logger = new TacoLogger();
        
        //This method will take a line as its argument and return a TacoBell location
        public ITrackable Parse(string line)
        {
            logger.LogInfo("Begin parsing");

            // This will take each line and split them into an array of strings separated by a comma
            var cells = line.Split(',');

            // If the array length is less than 3 we will return a log warning
            if (cells.Length < 3)
            {
                logger.LogWarning("Not enough data");
                return null;
            }

            // Index 0 is stored as a double named latitude
            var latitude = double.Parse(cells[0]);
            // Index 1 is stored as a double named longitude
            var longitude = double.Parse(cells[1]);
            // Index 2 is stored as a string named name
            var name = cells[2];

            // New instance of Point is created to store the location's latitude and longitude
            var point = new Point() { Latitude = latitude, Longitude = longitude};
            // New tacoBell is created with this location's point and name
            var tacoBell = new TacoBell { Location = point, Name = name };

            return tacoBell;
        }
    }
}