using System.Web.Http;
using DataRepository;
using DataModel;
using System.Collections.Generic;

namespace API.Controllers
{
    public class TournamentController : ApiController
    {

        [HttpGet]
        [Route("api/main")]
        public List<TournamentMatchModel> GetInitialTournamentMatchList()
        {
            TournamentRepository _repo = new TournamentRepository();
            return _repo.GetTournamentMatchList();
        }

        [HttpGet]
        [Route("api/tournaments")]
        public List<TournamentModel> GetTournamentList()
        {
            TournamentRepository _repo = new TournamentRepository();
            return _repo.GetAvailableTournament();
        }
    }
}