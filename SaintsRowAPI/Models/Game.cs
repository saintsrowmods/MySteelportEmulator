using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Models
{
    public class Game
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }

        public static Game GetFromApiKey(string apiKey)
        {
            switch (apiKey)
            {
                case "7713307274052196224":
                    return new Game()
                    {
                        ID = 3,
                        Title = "Saints Row: The Third",
                        ShortTitle = "SRTT",
                    };

                case "4099492307594627576":
                    return new Game()
                    {
                        ID = 4,
                        Title = "Saints Row IV",
                        ShortTitle = "SRIV",
                    };

                default:
                    throw new Exception("Unexpected API key? " + apiKey);
            }
        }
    }
}
