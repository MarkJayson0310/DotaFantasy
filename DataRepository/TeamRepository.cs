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
    public class TeamRepository
    {
        private static Random random = new Random();

        public dynamic AddNewUser(UserModel newuser)
        {

            if (IsRegisterEmailAddExists(newuser.EmailAddress) >= 1)
            {
                newuser.IsUserExist = true;
                return newuser;
            }

            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
            oCon.Open();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand("sp_addnewuser", oCon);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            oMySQLData.MySqlParameter userEmail = new oMySQLData.MySqlParameter("userEmail", newuser.EmailAddress);
            oMySQLData.MySqlParameter firstName = new oMySQLData.MySqlParameter("firstName", newuser.FirstName);
            oMySQLData.MySqlParameter middleName = new oMySQLData.MySqlParameter("middleName", newuser.MiddleName);
            oMySQLData.MySqlParameter lastName = new oMySQLData.MySqlParameter("lastName", newuser.LastName);
            oMySQLData.MySqlParameter pswd = new oMySQLData.MySqlParameter("pswd", newuser.Password);

            string code = RandomString(6);

            oMySQLData.MySqlParameter activationCode = new oMySQLData.MySqlParameter("activationCode", code);

            cmd.Parameters.Add(userEmail);
            cmd.Parameters.Add(firstName);
            cmd.Parameters.Add(middleName);
            cmd.Parameters.Add(lastName);
            cmd.Parameters.Add(pswd);
            cmd.Parameters.Add(activationCode);

            int result = 0;
            result = cmd.ExecuteNonQuery();

            if (result > 0)
            {

                EmailNotification notif = new EmailNotification();
                notif.NotifyNewUsserForActivation(newuser.EmailAddress, code);

                return newuser;
            }

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
                user.IsVerified = Convert.ToBoolean(reader["fldIsVerified"]);
                user.IsUserExist = true;
            }

            oCon.Close();

            if (user == null)
            {
                user.EmailAddress = loginuser.EmailAddress;
                user.IsUserExist = false;
            }

            return user;
        }

        public dynamic ValidateUserActivation(ActivateUserModel activate)
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
            oCon.Open();

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand("sp_activateuser", oCon);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            oMySQLData.MySqlParameter userEmail = new oMySQLData.MySqlParameter("userEmail", activate.UserEmail);
            oMySQLData.MySqlParameter activationCode = new oMySQLData.MySqlParameter("activationCode", activate.ActivationCode);

            cmd.Parameters.Add(userEmail);
            cmd.Parameters.Add(activationCode);

            int result = 0;
            result = cmd.ExecuteNonQuery();

            oCon.Close();

            return result;
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

            while (reader.Read())
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

        public dynamic GetInitialTournamentMatchList()
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

        public dynamic GetUserMatchList(UserLoginModel user, int tournamentID)
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
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

                ///add teamone, two user bet
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

        List<UserBetModel> GetUserBetList(int userID, int tournamentID)
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
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

        int IsRegisterEmailAddExists(string emailID)
        {
            oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");
            oCon.Open();

            string fetchQuery = "SELECT * FROM tbluserlist WHERE fldUserEmail = " + "'" + emailID + "'";

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand(fetchQuery, oCon);
            int result = 0;

            result =  Convert.ToInt32(cmd.ExecuteScalar());
            return result;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
