
using System.Web.Http;
using DataRepository;

using DataModel;


namespace API.Controllers
{
    public class UserBetController : ApiController
    {

        [HttpGet]
        [Route("api/betpoints/user/{userID}")]
        public dynamic GetUserPointsDetails(int userID)
        {
            UserBetRepository _repo = new UserBetRepository();

            return _repo.GetUserBetPointsDetails(userID);
        }

        [HttpPost]
        [Route("api/match/tournament/{tournamentID}")]
        public dynamic GetTeamList(UserLoginModel user, int tournamentID)
        {
            UserBetRepository _repo = new UserBetRepository();

            return _repo.GetUserMatchList(user, tournamentID);
        }

        [HttpPost]
        [Route("api/place/bet")]
        public dynamic AddMatchBetBet(UserBetModel userbet)
        {
            UserBetRepository _repo = new UserBetRepository();

            return _repo.PlaceMatchBet(userbet);
        }
   
        [HttpPost]
        [Route("api/registerbet")]
        public dynamic RegistertBet(TournamentRegistrationModel tournamentregister)
        {
            UserBetRepository _repo = new UserBetRepository();

            return _repo.RegisterTournamentBet(tournamentregister);
        }
    }
}