using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using oMySQLData = MySql.Data.MySqlClient;
using DataModel;
using Common;

namespace DataRepository
{
    public class TournamentRepository
    {

        public List<TournamentMatchModel> GetTournamentMatchList()
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
            oCon.Open();

            string fetchQuery = "SELECT * FROM tbltournaments WHERE fldisactive = 1 ORDER BY fldTournamentDate DESC";

            List<TournamentMatchModel> tournamentmatches = new List<TournamentMatchModel>();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand(fetchQuery, oCon);
            cmd.ExecuteNonQuery();

            oMySQLData.MySqlDataReader reader = cmd.ExecuteReader();

            string matchQuery;
            while (reader.Read())
            {
                TournamentMatchModel tournamentmatch = new TournamentMatchModel();

                tournamentmatch.TournamentID = Convert.ToInt32(reader["fldtournamentid"]);
                tournamentmatch.TournamentName = (reader["fldTournamentName"]).ToString();

                oMySQLData.MySqlConnection oConmatches = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
                oConmatches.Open();

                matchQuery = "SELECT * FROM view_matchlistdetails WHERE TournamentID = " + tournamentmatch.TournamentID + " ORDER BY fldmatchdate DESC LIMIT 3";

                oMySQLData.MySqlCommand cmdmatches = new oMySQLData.MySqlCommand(matchQuery, oConmatches);
                cmdmatches.ExecuteNonQuery();

                oMySQLData.MySqlDataReader matchreader = cmdmatches.ExecuteReader();

                List<MatchModel> matches = new List<MatchModel>();

                while (matchreader.Read())
                {
                    MatchModel match = new MatchModel();
                    match.TeamOne = matchreader["TeamOne"].ToString();
                    match.TeamTwo = matchreader["TeamTwo"].ToString();
                    match.MatchDate = Convert.ToDateTime(matchreader["fldMatchDate"]);
                    matches.Add(match);
                }

                tournamentmatch.Matches = matches;
                matchreader.Close();

                tournamentmatches.Add(tournamentmatch);
            }

            oCon.Close();

            return tournamentmatches;
        }
       
        public List<TournamentModel> GetAvailableTournament()
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
            oCon.Open();

            string fetchQuery = "SELECT * FROM tbltournaments";

            List<TournamentModel> tournaments = new List<TournamentModel>();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand(fetchQuery, oCon);
            cmd.ExecuteNonQuery();

            oMySQLData.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                TournamentModel tournament = new TournamentModel();
                tournament.TournamentID = Convert.ToInt32(reader["fldTournamentID"]);
                tournament.TournamentName = reader["fldTournamentName"].ToString();
                tournament.TournamentDate = Convert.ToDateTime(reader["fldTournamentDate"]);

                tournaments.Add(tournament);
            }
            oCon.Close();
            return tournaments;
        }

    }
}
