using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using System.ServiceModel.Web;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "match")]
    public class Match
    {
        double timeElapsed;

        [DataMember(Name = "map", Order = 1, IsRequired = true)]
        public string Map { get; set; }
        [DataMember(Name = "gameMode", Order = 2, IsRequired = true)]
        public string GameMode { get; set; }
        [DataMember(Name = "fragLimit", Order = 3, IsRequired = true)]
        public int FragLimit { get; set; }
        [DataMember(Name = "timeLimit", Order = 4, IsRequired = true)]
        public int TimeLimit { get; set; }
        [DataMember(Name = "timeElapsed", Order = 5, IsRequired = true)]
        public double TimeElapsed
        {
            get { return timeElapsed; }
            set { timeElapsed = Math.Round(value, 6, MidpointRounding.AwayFromZero); }
        }
        [DataMember(Name = "scoreboard", Order = 6, IsRequired = true)]
        public List<Player> Scoreboard { get; set; }

        public string Endpoint { get; set; }
        public string StringTimestamp { get; set; }
        public DateTime DateTimeTimestamp { get; set; }
        public string JustDateFromTimestamp { get; set; }

        public static void AddMatchInfo(LiteCollection<Match> matchesCollection, LiteCollection<Server> serversCollection, Match match, string endpoint, string timestamp)
        {
            if (!(serversCollection.FindOne(x => x.Endpoint == endpoint) == null))
            {
                match.Endpoint = endpoint;
                match.StringTimestamp = timestamp;
                match.DateTimeTimestamp = Convert.ToDateTime(timestamp).ToUniversalTime();
                match.JustDateFromTimestamp = timestamp.Substring(0, 10);
                int playersCount = match.Scoreboard.Count();

                if (playersCount == 1)
                {
                    match.Scoreboard[0].NameInUpperCase = match.Scoreboard[0].Name.ToUpper();
                    match.Scoreboard[0].ScoreboardPercent = 100;
                }
                else
                {
                    for (int i = 0; i < playersCount; i++)
                    {
                        match.Scoreboard[i].NameInUpperCase = match.Scoreboard[i].Name.ToUpper();
                        match.Scoreboard[i].ScoreboardPercent = (double)(playersCount - (i + 1)) / (playersCount - 1) * 100;
                    }
                }
                matchesCollection.Insert(match);
            }
            else
                throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
        }
        public static Match GetMatchInfo(LiteCollection<Match> matchesCollection, string endpoint, string timestamp)
        {
            if (!(matchesCollection.FindOne(x => x.Endpoint == endpoint && x.StringTimestamp == timestamp) == null))
                return matchesCollection.FindOne(x => x.Endpoint == endpoint && x.StringTimestamp == timestamp);
            else
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
        }
    }
}
