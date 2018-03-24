using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using oMySQLData = MySql.Data.MySqlClient;
using DataModel;

namespace DataRepository
{
    public class TeamRepository
    {

        public int AddNewUser(UserModel newuser)
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
            oCon.Open();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand("sp_addnewuser", oCon);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            oMySQLData.MySqlParameter userEmail = new oMySQLData.MySqlParameter("userEmail", newuser.EmailAddress);
            oMySQLData.MySqlParameter firstName = new oMySQLData.MySqlParameter("firstName", newuser.FirstName);
            oMySQLData.MySqlParameter middleName = new oMySQLData.MySqlParameter("middleName", newuser.MiddleName);
            oMySQLData.MySqlParameter lastName = new oMySQLData.MySqlParameter("lastName", newuser.LastName);
            oMySQLData.MySqlParameter pswd = new oMySQLData.MySqlParameter("pswd", newuser.Password);

            cmd.Parameters.Add(userEmail);
            cmd.Parameters.Add(firstName);
            cmd.Parameters.Add(middleName);
            cmd.Parameters.Add(lastName);
            cmd.Parameters.Add(pswd);

            int result = 0;
            result = cmd.ExecuteNonQuery();

            oCon.Close();

            return result;

        }


        public dynamic LogInUser(UserModel loginuser)
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
            oCon.Open();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand("sp_loginuser", oCon);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            oMySQLData.MySqlParameter loginEmail = new oMySQLData.MySqlParameter("loginEmail", loginuser.EmailAddress);
            oMySQLData.MySqlParameter loginPassword = new oMySQLData.MySqlParameter("loginPassword", loginuser.Password);

            cmd.Parameters.Add(loginEmail);
            cmd.Parameters.Add(loginPassword);

            oMySQLData.MySqlDataReader reader = cmd.ExecuteReader();

            UserModel user = new UserModel();
            while (reader.Read())
            {
                user.UserID = Convert.ToInt32(reader["flduserid"]);
                user.EmailAddress = reader["fldUserEmail"].ToString();
                user.FirstName = reader["fldFirstName"].ToString();
            }

            oCon.Close();

            return user;
        }
        public dynamic GetUserBetPointsDetails(int userID)
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
            oCon.Open();

            string fetchQuery = "SELECT * FROM view_userdetails WHERE UserID = " + userID;

            List<UserPointsModel> userbets = new List<UserPointsModel>();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand(fetchQuery, oCon);
            cmd.ExecuteNonQuery();

            oMySQLData.MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                UserPointsModel userbet = new UserPointsModel();
                userbet.UserID = Convert.ToInt32(reader["UserID"]);
                userbet.TournamentID = Convert.ToInt32(reader["TournamentID"]);
                userbet.TotalPoints = Convert.ToInt32(reader["TotalBetPoints"]);
                userbet.TournamentPoints = Convert.ToInt32(reader["TournamentPoints"]);

                userbets.Add(userbet);
            }

            return userbets;
        }

        public dynamic GetUserMatchList(UserLoginModel user, int tournamentID)
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
            oCon.Open();

            string fetchQuery = "SELECT * FROM view_matchlistdetails WHERE TournamentID = " + tournamentID;

            List<TeamModel> teams = new List<TeamModel>();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand(fetchQuery, oCon);
            cmd.ExecuteNonQuery();

            oMySQLData.MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                TeamModel team = new TeamModel();
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
            return teams;
        }

        public int PlaceNewBet(UserBetModel userbet)
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
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

        public dynamic GetAvailableTournament()
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

        public int RegisterTournamentBet(TournamentRegistrationModel register)
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
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

    }
}
