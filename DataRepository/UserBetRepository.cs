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
    public class UserBetRepository
    {
        private oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");

        public dynamic GetUserBetPointsDetails(int userID)
        {
            oCon.Open();

            string fetchQuery = "SELECT * FROM view_userdetails WHERE UserID = " + userID;

            List<UserPointsModel> userbets = new List<UserPointsModel>();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand(fetchQuery, oCon);
            cmd.ExecuteNonQuery();

            oMySQLData.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                UserPointsModel userbet = new UserPointsModel();
                userbet.UserID = Convert.ToInt32(reader["UserID"]);
                userbet.TournamentID = Convert.ToInt32(reader["TournamentID"]);
                userbet.TotalPoints = Convert.ToInt32(reader["TotalBetPoints"]);
                userbet.TournamentPoints = Convert.ToInt32(reader["TournamentPoints"]);

                userbets.Add(userbet);
            }

            oCon.Close();
            return userbets;
        }

        public dynamic GetUserMatchList(UserLoginModel user, int tournamentID)
        {
            oCon.Open();

            string fetchQuery = "SELECT * FROM view_matchlistdetails WHERE TournamentID = " + tournamentID;

            List<MatchBetModel> teams = new List<MatchBetModel>();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand(fetchQuery, oCon);
            cmd.ExecuteNonQuery();

            oMySQLData.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MatchBetModel team = new MatchBetModel();
                team.MatchID = Convert.ToInt32(reader["fldMatchID"]);
                team.MatchDate = Convert.ToDateTime(reader["fldMatchDate"]);
                team.TeamOne = reader["TeamOne"].ToString();
                team.TeamTwo = reader["TeamTwo"].ToString();
                team.TeamOneID = Convert.ToInt32(reader["TeamOneID"]);
                team.TeamTwoID = Convert.ToInt32(reader["TeamTwoID"]);

                team.BettorID = user.UserID;

                teams.Add(team);
            }

            oCon.Close();

            List<UserBetModel> usersbet = GetUserBetList(user.UserID, tournamentID);

            if (usersbet.Count > 0)
            {
                foreach (MatchBetModel item in teams)
                {
                    item.TeamOneBet = usersbet.Where(t => t.MatchID == item.MatchID && t.TeamID == item.TeamOneID).Select(t => t.PlaceBet).FirstOrDefault();
                    item.TeamTwoBet = usersbet.Where(t => t.MatchID == item.MatchID && t.TeamID == item.TeamTwoID).Select(t => t.PlaceBet).FirstOrDefault();
                }
            }

            return teams;
        }

        public int RegisterTournamentBet(TournamentRegistrationModel register)
        {
            oCon.Open();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand("sp_registerusertournamentbet", oCon);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            oMySQLData.MySqlParameter userID = new oMySQLData.MySqlParameter("userID", register.UserID);
            oMySQLData.MySqlParameter tournamentID = new oMySQLData.MySqlParameter("tournamentID", register.TournamentID);
            oMySQLData.MySqlParameter betPoints = new oMySQLData.MySqlParameter("betPoints", register.TournamentPoints);

            cmd.Parameters.Add(userID);
            cmd.Parameters.Add(tournamentID);
            cmd.Parameters.Add(betPoints);

            int result = 0;
            result = cmd.ExecuteNonQuery();
            oCon.Close();

            return result;
        }

        public int PlaceMatchBet(UserBetModel userbet)
        {
            oCon.Open();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand("sp_addmatchbet", oCon);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            oMySQLData.MySqlParameter matchID = new oMySQLData.MySqlParameter("matchID", userbet.MatchID);
            oMySQLData.MySqlParameter bettorID = new oMySQLData.MySqlParameter("bettorID", userbet.BettorID);
            oMySQLData.MySqlParameter betTeamID = new oMySQLData.MySqlParameter("betTeamID", userbet.TeamID);
            oMySQLData.MySqlParameter tournamentID = new oMySQLData.MySqlParameter("tournamentID", userbet.TournamentID);
            oMySQLData.MySqlParameter placeBetPoints = new oMySQLData.MySqlParameter("placeBetPoints", userbet.PlaceBet);

            cmd.Parameters.Add(matchID);
            cmd.Parameters.Add(bettorID);
            cmd.Parameters.Add(betTeamID);
            cmd.Parameters.Add(tournamentID);
            cmd.Parameters.Add(placeBetPoints);

            int result = 0;
            result = cmd.ExecuteNonQuery();
            oCon.Close();

            return result;
        }

        List<UserBetModel> GetUserBetList(int userID, int tournamentID)
        {
            oCon.Open();

            string fetchQuery = "SELECT * FROM view_userbetdetails WHERE fldBettorID = " + userID + " AND fldTournamentID = " + tournamentID;

            List<UserBetModel> bets = new List<UserBetModel>();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand(fetchQuery, oCon);
            cmd.ExecuteNonQuery();

            oMySQLData.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                UserBetModel bet = new UserBetModel();
                bet.BettorID = Convert.ToInt32(reader["fldBettorID"]);
                bet.TournamentID = Convert.ToInt32(reader["fldTournamentID"]);
                bet.MatchID = Convert.ToInt32(reader["fldMatchID"]);
                bet.TeamID = Convert.ToInt32(reader["fldTeamBetID"]);
                bet.PlaceBet = Convert.ToInt32(reader["fldPlaceBetPoints"]);

                bets.Add(bet);
            }

            oCon.Close();
            return bets;
        }

    }
}
