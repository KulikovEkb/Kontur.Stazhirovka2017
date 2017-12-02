using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "playerReport")]
    public class PlayerReport
    {
        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        double killToDeathRatio;
        [DataMember(Name = "killToDeathRatio", Order = 2)]
        public double KillToDeathRatio
        {
            get { return killToDeathRatio; }
            set { killToDeathRatio = Math.Round(value, 6, MidpointRounding.AwayFromZero); }
        }

        static public List<PlayerReport> GetBestPlayers(LiteCollection<Match> matchesCollection, int quantity)
        {
            var bestPlayers = matchesCollection.FindAll()
                    .SelectMany(x => x.Scoreboard.Select(y => new { Name = y.Name, Kills = y.Kills, Deaths = y.Deaths }))
                    .GroupBy(x => x.Name)
                    .Select(x => new { Name = x.Key, KillsSum = x.Sum(y => y.Kills), DeathsSum = x.Sum(y => y.Deaths), PlayedMatches = x.Count() })
                    .Where(x => x.DeathsSum > 0 && x.PlayedMatches >= 10)
                    .Select(x => new { Name = x.Name, KillToDeathRatio = (double)x.KillsSum / x.DeathsSum })
                    .OrderByDescending(x => x.KillToDeathRatio)
                    .Take(quantity);


            var result = new List<PlayerReport>();
            foreach (var item in bestPlayers)
            {
                result.Add(new PlayerReport { KillToDeathRatio = item.KillToDeathRatio, Name = item.Name });
            }
            return result;
        }
    }
}
