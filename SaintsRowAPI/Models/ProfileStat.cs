using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintsRowAPI.Models
{
    public class ProfileStat
    {
        public int UserID { get; set; }
        public Game Game { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public ProfileStat()
        {
        }

        public ProfileStat(int userId, Game game, string name, string value)
        {
            UserID = userId;
            Game = game;
            Name = name;
            Value = value;
        }
    }
}
