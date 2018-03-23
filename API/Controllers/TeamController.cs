using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DataRepository;
using System.Web.Http.Cors;
using DataModel;
using System.Net.Http;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace API.Controllers
{
    public class TeamController : ApiController
    {

        [EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [Route("api/user/add")]
        [HttpPost]
        public dynamic AddNewUser(UserModel newuser)
        {
            TeamRepository _repo = new TeamRepository();
            return _repo.AddNewUser(newuser);
        }

        [EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [Route("api/user/login")]
        [HttpPost]
        public dynamic LoginUser(UserModel newuser)
        {
            TeamRepository _repo = new TeamRepository();
            return _repo.LogInUser(newuser);
        }

        [EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [HttpGet]
        [Route("api/betpoints/user/{userID}")]
        public dynamic GetUserPointsDetails(int userID)
        {
            TeamRepository _repo = new TeamRepository();

            return _repo.GetUserBetPointsDetails(userID);
        }

        // GET: Team
        [EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/match/tournament/{tournamentID}")]
        public dynamic GetTeamList(UserLoginModel user, int tournamentID)
        {
            TeamRepository _repo = new TeamRepository();

            return _repo.GetUserMatchList(user, tournamentID);
        }

        [EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/add/bet")]
        public dynamic AddNewBet(UserBetModel userbet)
        {
            TeamRepository _repo = new TeamRepository();

            return _repo.PlaceNewBet(userbet);
        }

        [EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [HttpGet]
        [Route("api/tournaments")]
        public dynamic GetTournamentList()
        {
            TeamRepository _repo = new TeamRepository();

            return _repo.GetAvailableTournament();
        }

        [EnableCors(origins: "http://localhost:55869", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/xml")]
        public dynamic ReturnJSON([FromBody]string xml)
        {
            XmlDocument doc = new XmlDocument();
            //doc.Load(@"C:\Users\mark.j.s.panopio\VS projects\WebApplication1\API\Controllers\XMLtoJSON.xml");
            doc.LoadXml(xml);
            var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);

            var transactObject1 = JsonConvert.DeserializeObject(json);

            return null;
        }
    }
}