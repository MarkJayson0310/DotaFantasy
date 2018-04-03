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

        //[EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [Route("api/user/add")]
        [HttpPost]
        public dynamic AddNewUser(UserModel newuser)
        {
            TeamRepository _repo = new TeamRepository();
            return _repo.AddNewUser(newuser);
        }

        //[EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [Route("api/user/login")]
        [HttpPost]
        public dynamic LoginUser(UserModel newuser)
        {
            TeamRepository _repo = new TeamRepository();
            return _repo.LogInUser(newuser);
        }

        [Route("api/user/activate")]
        [HttpPost]
        public dynamic ValidateUserActivation(ActivateUserModel activate)
        {
            TeamRepository _repo = new TeamRepository();
            return _repo.ValidateUserActivation(activate);
        }

        //[EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [HttpGet]
        [Route("api/betpoints/user/{userID}")]
        public dynamic GetUserPointsDetails(int userID)
        {
            TeamRepository _repo = new TeamRepository();

            return _repo.GetUserBetPointsDetails(userID);
        }

        [HttpGet]
        [Route("api/landing")]
        public dynamic GetInitialTournamentMatchList()
        {
            TeamRepository _repo = new TeamRepository();
            return _repo.GetInitialTournamentMatchList();
        }

        // GET: Team
        //[EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/match/tournament/{tournamentID}")]
        public dynamic GetTeamList(UserLoginModel user, int tournamentID)
        {
            TeamRepository _repo = new TeamRepository();

            return _repo.GetUserMatchList(user, tournamentID);
        }

        //[EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/add/bet")]
        public dynamic AddNewBet(UserBetModel userbet)
        {
            TeamRepository _repo = new TeamRepository();

            return _repo.PlaceNewBet(userbet);
        }

        //[EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [HttpGet]
        [Route("api/tournaments")]
        public dynamic GetTournamentList()
        {
            TeamRepository _repo = new TeamRepository();

            return _repo.GetAvailableTournament();
        }

        //[EnableCors(origins: "http://localhost:64057", headers: "*", methods: "*")]
        [HttpPost]
        [Route("api/registerbet")]
        public dynamic ReturnJSON(TournamentRegistrationModel tournamentregister)
        {
            TeamRepository _repo = new TeamRepository();

            return _repo.RegisterTournamentBet(tournamentregister);
        }
    }
}