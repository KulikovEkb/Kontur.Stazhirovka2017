using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Service
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "servers/{endpoint}/info",
            RequestFormat = WebMessageFormat.Json)]
        void AddServerInfo(Classes.Server server, string endpoint);

        [OperationContract]
        [WebGet(UriTemplate = "servers/{endpoint}/info",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Classes.Server GetServerInfo(string endpoint);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "servers/{endpoint}/matches/{timestamp}",
            RequestFormat = WebMessageFormat.Json)]
        void AddMatchInfo(Classes.Match match, string endpoint, string timestamp);

        [OperationContract]
        [WebGet(UriTemplate = "servers/{endpoint}/matches/{timestamp}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Classes.Match GetMatchInfo(string endpoint, string timestamp);

        [OperationContract]
        [WebGet(UriTemplate = "servers/info",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        List<Classes.ServerInfo> GetServersInfo();

        [OperationContract]
        [WebGet(UriTemplate = "servers/{endpoint}/stats",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Classes.ServerStatistics GetServerStats(string endpoint);

        [OperationContract]
        [WebGet(UriTemplate = "players/{name}/stats",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Classes.PlayerStatistics GetPlayerStats(string name);

        [OperationContract]
        [WebGet(UriTemplate = "reports/recent-matches/{count=5}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        List<Classes.MatchReport> GetRecentMatches(string count);

        [OperationContract]
        [WebGet(UriTemplate = "reports/best-players/{count=5}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        List<Classes.PlayerReport> GetBestPlayers(string count);

        [OperationContract]
        [WebGet(UriTemplate = "reports/popular-servers/{count=5}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        List<Classes.ServerReport> GetPopularServers(string count);
    }
}
