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
    public class UserRepository
    {
        private static Random random = new Random();

        private oMySQLData.MySqlConnection oCon = new oMySQLData.MySqlConnection("Server=localhost;Database=betting;Uid=root;Pwd=Mysqlm@rch101984;");

        public dynamic AddNewUser(UserModel newuser)
        {

            if (IsRegisterEmailAddExists(newuser.EmailAddress) >= 1)
            {
                newuser.IsUserExist = true;
                return newuser;
            }

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

        public dynamic ValidateUserActivation(ActivateUserModel activate)
        {

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

        public dynamic LogInUser(UserModel loginuser)
        {
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

        int IsRegisterEmailAddExists(string emailID)
        {

            oCon.Open();

            string fetchQuery = "SELECT * FROM tbluserlist WHERE fldUserEmail = " + "'" + emailID + "'";

            oMySQLData.MySqlCommand cmd = new oMySQLData.MySqlCommand(fetchQuery, oCon);
            int result = 0;

            result = Convert.ToInt32(cmd.ExecuteScalar());

            oCon.Close();
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
