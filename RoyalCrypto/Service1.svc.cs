using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace RoyalCrypto
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        public void updatetradestatus(string fuac_id,int status){

            
            if (status == 1)
            {
                //cmds = new SqlCommand("update useraccount set logindate = convert(datetime,'" + DateTime.UtcNow.Add(new TimeSpan(5,0,0)).ToString("dd-MM-yyyy HH:mm:ss") + "',103), StatusOnline = 1 where uac_id = '" + fuac_id + "' ", connect);
            }
            else
            {
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
                SqlCommand cmds;
                cmds = new SqlCommand("update useraccount set logoutdate = convert(datetime,'" + DateTime.UtcNow.Add(new TimeSpan(5, 0, 0)).ToString("dd-MM-yyyy HH:mm:ss") + "',103), StatusOnline = 0 where uac_id = '" + fuac_id + "' ", connect);
                cmds.ExecuteNonQuery();
                connect.Close();
            }
           

        }

        public void updatelogintime(string fuac_id)
        {
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
                if (connect.State != ConnectionState.Open)
                    connect.Open();
                SqlCommand cmds;
                cmds = new SqlCommand("update useraccount set LoginDate = convert(datetime,'" + DateTime.UtcNow.Add(new TimeSpan(5, 0, 0)).ToString("dd-MM-yyyy HH:mm:ss") + "',103), StatusOnline = 1 where uac_id = '" + fuac_id + "' ", connect);
                cmds.ExecuteNonQuery();
                connect.Close();
        }
        public Verification forgot_pass(string Email)
        {

            
       

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open) ;
            connect.Open();
            SqlCommand cmd = new SqlCommand("select Password from UserAccount where Email = '" + Email + "'", connect);
            try
            {
                cmd.CommandType = CommandType.Text;
                SqlDataReader s = cmd.ExecuteReader();
                s.Read();
                string pass = s["Password"].ToString();
                s.Close();



                Message(Email, "RoyalCrypto  Forgot Password", "Password:" + Decryptdata(pass));
                connect.Close();
                return new Verification("success", "Email Sent");

            }
            catch (Exception pas)
            {
                connect.Close();
                return new Verification("failed", "invalidEmail");
            }

        }

        public Verification irelease(string ord_id, string utfee, string utamount, string uobitamount, string uoamount, string ut_id, string user_uid)
        {

           
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open) ;
            connect.Open();
            try
            {

                SqlCommand sql = new SqlCommand("select FUT_Id from UserOrder where Ord_id = "+ord_id,connect);
           
                SqlDataReader s = sql.ExecuteReader();
                s.Read();
                 ut_id = s["FUT_Id"].ToString();
                s.Close();

                sql = new SqlCommand("select Fees,Amount from UserTrades where ut_id = " + ut_id, connect);
                s = sql.ExecuteReader();
                s.Read();
                utfee = s["Fees"].ToString();
                utamount = s["Amount"].ToString();
                s.Close();

                decimal damnt = Convert.ToDecimal(utfee);
                decimal dfees = Convert.ToDecimal(utamount);

                decimal dividevalue = dfees / damnt;
                updatelogintime(user_uid);

                decimal FBitAmount = Convert.ToDecimal(uobitamount);
                decimal ExFee = dividevalue * FBitAmount;



                SqlCommand query = new SqlCommand("select fuac_id from userorder where ord_id = '" + ord_id + "'  ", connect);
                query.ExecuteNonQuery();
                SqlDataReader sdr = query.ExecuteReader();
                sdr.Read();

                string fuacid = sdr["fuac_id"].ToString();
                sdr.Close();

                SqlCommand query2 = new SqlCommand("select fcm_token from useraccount where uac_id = '" + fuacid + "' ", connect);

                query2.ExecuteNonQuery();
                SqlDataReader sdt = query2.ExecuteReader();
                sdt.Read();

                string fcm = sdt["fcm_token"].ToString();
                sdt.Close();


                var webClient = new WebClient();
                string type = "release,"+ord_id;
                string msg = "Order Release";
                string key = fcm;
                if (key == "")
                    key = "a";
                webClient.DownloadString("http://royalcryptoexchange.com/pushapi.php?send_notification&msg=" + msg + "&type=" + type + "&token=" + key);


                SqlCommand cmd = new SqlCommand("UPDATE UserTrades SET ExecutedFees ='" + ExFee.ToString() + "' FROM UserTrades INNER JOIN UserOrder  ON UserTrades.UT_Id = UserOrder.FUT_Id where UserOrder.Status='in-process' and  UserOrder.ORD_Id=" + ord_id, connect);
                cmd.ExecuteNonQuery();

                decimal amt = Convert.ToDecimal(uoamount);
                decimal bitamt = Convert.ToDecimal(uobitamount);
                decimal minusvalue = amt - bitamt;

               
                cmd = new SqlCommand("update UserTrades set Amount='" + minusvalue + "',ExecutedAmount='" + bitamt + "' where UT_Id=" + ut_id, connect);
                cmd.ExecuteNonQuery();

                
                cmd = new SqlCommand("update UserOrder set Status='completed' where ORD_id=" + ord_id + " and FUT_Id=" + ut_id, connect);

                cmd.ExecuteNonQuery();

                if (minusvalue == 0)
                {
                    
                    cmd = new SqlCommand("update UserTrades set Status='2' from UserTrades inner join UserOrder on UserTrades.UT_Id=UserOrder.FUT_Id where UserOrder.FUT_Id=" + ut_id + " and UserOrder.ORD_Id=" + ord_id, connect);
                    cmd.ExecuteNonQuery();
                }

                connect.Close();
                return new Verification("success", "");
            }
            catch (Exception e)
            {
                connect.Close();
                return new Verification("failed", "");
            }
            return null;
        }



        public Verification Add_UserAccount(string fname, string lname, string email, string phone, string password, string cnic, string dob, Verification fcm)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(RandomNumber(10, 99));
            sb.Append(RandomString(7));
            string isemailactive = sb.ToString();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select uac_id,Email,isactive,isemailactive from UserAccount where email = '" + email + "'", connect);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            try
            {
                string checker = reader["Email"].ToString();
                bool isactive = Convert.ToBoolean(reader["isactive"].ToString());

                //   string isactive = reader["isactive"].ToString();
                string alreadyactive = reader["isemailactive"].ToString();
                string uac_id = reader["uac_id"].ToString();
                reader.Close();

                if (email.Equals(checker))
                {
                    connect.Close();
                    if (!isactive)
                    {
                        Message(email, "RoyalCrypto Email Verification", "Verify Code:" + alreadyactive);
                        return new Verification("InActive", uac_id + " " + alreadyactive);

                    }
                    else if (isactive)
                    {
                        return new Verification("Active", "User Already Added and Verified");
                    }
                }
            }
            catch (Exception e)
            {
                reader.Close();
                Console.Write(e);
            }

            cmd = new SqlCommand("insert into UserAccount"
            + "(FirstName,LastName,Email,PhoneNum,IsEmailActive,Password,CNIC,DateOfBirth,IsActive,DocumentVerification,CreatedDate,StatusOnline,IsPhoneNumActive,fcm_token) values"
           + "('" + fname + "','" + lname + "','" + email + "','" + phone + "','" + isemailactive + "','" + encryptpass(password) + "','" + cnic + "',convert(datetime,'" + dob + "',103),0,'Un-Verified',convert(datetime,'" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "',103),0,0,'"+fcm.message+"') select @@Identity as UAC_Id", connect);
            cmd.CommandType = CommandType.Text;
            SqlDataReader s = cmd.ExecuteReader();
            s.Read();
            string id = s["UAC_Id"].ToString();
            s.Close();
            Message(email, "RoyalCrypto  Email Verification", "Verify Code:" + isemailactive);
            connect.Close();

            return new Verification("success", id + " " + isemailactive);
        }

        public Verification Login(string email, string pass,Verification fcm)
        {
            

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select uac_id,phonenum,Email,Password,isactive,isemailactive,IsPhoneNumActive from UserAccount where email = '" + email + "'", connect);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            try
            {
                string checker = reader["Email"].ToString();
                string phone = reader["phonenum"].ToString();
                bool isactive = Convert.ToBoolean(reader["isactive"].ToString());
                string Pass = reader["Password"].ToString();
                bool isphonenumactive = Convert.ToBoolean(reader["IsPhoneNumActive"].ToString());
                string alreadyactive = reader["isemailactive"].ToString();
                string uac_id = reader["uac_id"].ToString();
                reader.Close();

                if (email.Equals(checker))
                {

                    if (!isactive)
                    {
                        Message(email, "RoyalCrypto Email Verification", "Verify Code:" + alreadyactive);
                        return new Verification("InActive", uac_id + " " + alreadyactive);

                    }
                    else if (isactive)
                    {
                        string enter = Decryptdata(Pass);

                        if (email == checker && pass == enter)
                        {
                            cmd = new SqlCommand("update UserAccount set LoginDate = convert(datetime,'" + DateTime.UtcNow.Add(new TimeSpan(5, 0, 0)).ToString("dd-MM-yyyy HH:mm:ss") + "',103), StatusOnline = 1, fcm_token = '" + fcm.message + "' where uac_id = '" + uac_id + "'", connect);
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            return new Verification("success", uac_id + " " + isphonenumactive.ToString() + " " + phone);
                        }
                        //return new Verification("Active", "User Already Added and Verified");
                    }
                    string a = "";
                    //Convert.ToBase64String();

                    connect.Close();
                }
            }
            catch (Exception e)
            {
                reader.Close();
                Console.Write(e);

            }
            return new Verification("error", "Invalid Email Address or Password");

        }

        public Verification Logout(string email)
        {

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open) ;

            if (connect.State != ConnectionState.Open)
                connect.Open();

            SqlCommand cmd = new SqlCommand("update UserAccount set LogoutDate = convert(datetime,'" + DateTime.UtcNow.Add(new TimeSpan(5, 0, 0)).ToString("dd-MM-yyyy HH:mm:ss") + "',103), StatusOnline = 0 where Email = '" + email + "'", connect);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            return new Verification("success", "Logout Seccessful".ToString());

        }
        public Verification VerifyEmail(string uacid, string code)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select IsEmailActive from UserAccount where UAC_Id = " + uacid, connect);
            cmd.CommandType = CommandType.Text;
            SqlDataReader s = cmd.ExecuteReader();
            s.Read();
            string email = s["IsEmailActive"].ToString();
            s.Close();
            if (code.Equals(email))
            {
                cmd = new SqlCommand("update UserAccount set IsActive = 1, UserId = 'U-" + uacid + "',DocumentVerification = 'Un-Verified' where UAC_Id = " + uacid, connect);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                return new Verification(uacid, "Verified");
            }
            else
                return new Verification(uacid, "Unable to Verify");

        }


        string uname;
        private SmtpClient readfromWebConfig()
        {
            try
            {
                SmtpClient smtp = new SmtpClient();

                string host = ConfigurationManager.AppSettings["Host"].ToString();
                int port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                uname = ConfigurationManager.AppSettings["Uname"].ToString();
                string pass = ConfigurationManager.AppSettings["Password"].ToString();
                bool defaultcredentials = bool.Parse(ConfigurationManager.AppSettings["DefaultCredentials"]);
                int timeout = int.Parse(ConfigurationManager.AppSettings["Timeout"]);
                bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);

                smtp.Host = host;
                smtp.Port = port;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate,
                             X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = enableSsl;
                smtp.UseDefaultCredentials = defaultcredentials;
                smtp.Timeout = timeout;

                smtp.Credentials = new System.Net.NetworkCredential(uname, pass);

                return smtp;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public void Message(string to, string subject, string body)
        {
            try
            {
                SmtpClient client = readfromWebConfig();



                MailMessage email = new MailMessage(uname, to, subject, body);
                email.BodyEncoding = UTF8Encoding.UTF8;
              
                email.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(email);

            }
            catch (Exception e)
            {

            }
        }

        private int RandomNumber(int min, int max)
        {
            Random rn = new Random();
            return rn.Next(min, max);
        }
        private string RandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            Random rd = new Random();
            char value;
            for (int i = 0; i < length; i++)
            {
                value = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rd.NextDouble() + 65)));
                sb.Append(value);

            }
            return sb.ToString();

        }


        public string encryptpass(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }

        public UserAccount Select_UserAccount(string uac_id)
        {
            UserAccount list;// = new List<UserAccount>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from UserAccount where uac_id = '" + uac_id + "'", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();

            string uacid = sdr["UAC_Id"].ToString();
            string fname = sdr["FirstName"].ToString();
            string lname = sdr["LastName"].ToString();
            string email = sdr["Email"].ToString();
            string phno = sdr["PhoneNum"].ToString();
            string isemailactive = sdr["IsEmailActive"].ToString();
            string isphnoactive = sdr["IsPhoneNumActive"].ToString();
            string password = sdr["Password"].ToString();
            string createddate = sdr["CreatedDate"].ToString();
            string logindate = sdr["LoginDate"].ToString();
            string logoutdate = sdr["LogoutDate"].ToString();
            string isactive = sdr["IsActive"].ToString();
            string cnic = sdr["CNIC"].ToString();
            string dob = sdr["DateOfBirth"].ToString();
            string userid = sdr["UserId"].ToString();
            string terms = sdr["Terms"].ToString();
            string docver = sdr["DocumentVerification"].ToString();

            list = new UserAccount(uacid, fname, lname, email, phno, isemailactive, isphnoactive,
                Decryptdata(password), createddate, logindate, logoutdate, isactive, cnic, dob, userid, terms, docver
                );

            sdr.Close();
            connect.Close();
            return list;
        }

        public string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        public Verification Update_UserAccount(string uac_id, string fname, string lname, string password, string terms)
        {
            updatelogintime(uac_id);

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("update UserAccount set FirstName = '" + fname + "', LastName = '" + lname + "',Password = '" + encryptpass(password) + "',Terms = '" + terms + "' where UAC_Id = '" + Convert.ToInt32(uac_id) + "'", connect);

            cmd.CommandType = CommandType.Text;
            if (connect.State != ConnectionState.Open)
            {
                connect.Open();

            }

            cmd.ExecuteNonQuery();
            connect.Close();

            return new Verification("success", "record updated");
        }
        public SupportTicket Select_SupportTicket(string st_id)
        {

            SupportTicket list;//= new List<SupportTicket>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from SupportTicket where ST_Id = '" + st_id + "'", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            {
                string st_iid = sdr["ST_Id"].ToString();
                string title = sdr["Title"].ToString();
                string description = sdr["Description"].ToString();
                string image = sdr["Image"].ToString();
                string fuac_id = sdr["FUAC_Id"].ToString();


                list = (new SupportTicket(st_iid, title, description, image, fuac_id));
            }
            sdr.Close();
            connect.Close();
            return list;
        }




        public string Add_SupportTicket(SupportTicket st)
        {
            updatelogintime(st.FUAC_Id);

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("insert into SupportTicket values ('" + st.Title + "','" + st.Description + "','" + st.Image + "','" + st.FUAC_Id + "')", connect);
            cmd.CommandType = CommandType.Text;
            if (connect.State != ConnectionState.Open)
            {
                connect.Open();

            }
            try
            {
                cmd.ExecuteNonQuery();
                connect.Close();
                return "success";
            }
            catch (Exception e)
            {
                connect.Close();
                return "fail";
            }
        }



        public TradeDetail Select_TradeDetail(string uac_id, string up_id)
        {
            //updatelogintime(uac_id);

            UserPaymentDetail s = Select_UserPaymentDetailSingle(up_id);
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select terms from Useraccount where Uac_Id = '" + uac_id + "'", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            string term = sdr["terms"].ToString();
            sdr.Close();
            connect.Close();

            return new TradeDetail(term, s);

        }


        public UserChat Select_UserChat(string ucht_id)
        {

            UserChat list;//= new List<UserChat>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from UserChat where UCHT_Id = '" + ucht_id + "'", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();

            string uchtid = sdr["UCHT_Id"].ToString();
            string fut_id = sdr["FUT_Id"].ToString();
            string fuac_id = sdr["FUAC_Id"].ToString();
            string message = sdr["Message"].ToString();


            list = new UserChat(uchtid, fut_id, fuac_id, message);

            sdr.Close();
            connect.Close();
            return list;
        }



        public List<UserPaymentDetail> Select_UserPaymentDetail(string upida)
        {
            updatelogintime(upida);

            List<UserPaymentDetail> list = new List<UserPaymentDetail>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from UserPaymentDetail where FUAC_Id = '" + upida + "'", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {

                String upid = sdr["UP_Id"].ToString();
                String fuacid = sdr["FUAC_Id"].ToString();
                String typ = sdr["Type"].ToString();
                String acnt = sdr["Account"].ToString();
                String acttitl = sdr["AccountTitle"].ToString();
                String bnknam = sdr["BankName"].ToString();
                String bnkcod = sdr["BankCode"].ToString();




                list.Add(new UserPaymentDetail(upid, fuacid, typ, acnt, acttitl, bnknam, bnkcod));
            }
            sdr.Close();
            connect.Close();
            return list;
        }


        public UserPaymentDetail Select_UserPaymentDetailSingle(string upida)
        {
            UserPaymentDetail list;//= new List<UserPaymentDetail>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from UserPaymentDetail where UP_Id = '" + upida + "'", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            {

                String upid = sdr["UP_Id"].ToString();
                String fuacid = sdr["FUAC_Id"].ToString();
                String typ = sdr["Type"].ToString();
                String acnt = sdr["Account"].ToString();
                String acttitl = sdr["AccountTitle"].ToString();
                String bnknam = sdr["BankName"].ToString();
                String bnkcod = sdr["BankCode"].ToString();




                list = (new UserPaymentDetail(upid, fuacid, typ, acnt, acttitl, bnknam, bnkcod));
            }
            sdr.Close();
            connect.Close();
            return list;
        }


        public Verification Add_UserTrades(string fuacid, string ordertype, string fupid, string amnt, string exec_amount, string exec_fees, string pric, string fes, string uplimit,
                             string lowlimit, string currencytyp)
        {
            updatelogintime(fuacid);
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("insert into UserTrades values ('" + Convert.ToInt32(fuacid) + "','" + ordertype + "','" + Convert.ToInt32(fupid) + "','" + amnt + "','" + exec_amount + "','" + exec_fees + "','" + pric + "','" + fes + "','" + uplimit + "','" + lowlimit + "','" + currencytyp + "', 1 ,convert(datetime,'" + DateTime.UtcNow.Add(new TimeSpan(5, 0, 0)).ToString("dd-MM-yyyy HH:mm:ss") + "',103))", connect);
            cmd.CommandType = CommandType.Text;
            if (connect.State != ConnectionState.Open)
            {
                connect.Open();
            }

            try
            {
                cmd.ExecuteNonQuery();
                connect.Close();

                return new Verification("success", "Trade added");
            }
            catch (Exception e)
            {
                connect.Close();
                return new Verification("error", "unable to add Trade");
            }
        }
        public List<UserTrades> Select_DashBoard(string fuac_id)
        {
            updatelogintime(fuac_id);

            List<UserTrades> list = new List<UserTrades>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from UserTrades where fuac_id = '" + fuac_id + "' and status !=0", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string utid = sdr["UT_Id"].ToString();
                string fuacid = sdr["FUAC_Id"].ToString();
                string Ordertype = sdr["OrderType"].ToString();
                string fupid = sdr["FUP_Id"].ToString();
                string exec_amount = sdr["ExecutedAmount"].ToString();
                string exec_fees = sdr["ExecutedFees"].ToString();
                string amnt = sdr["Amount"].ToString();
                string pric = sdr["Price"].ToString();
                string fes = sdr["Fees"].ToString();
                string uplimit = sdr["UpperLimit"].ToString();
                string lowlimit = sdr["LowerLimit"].ToString();
                string date = sdr["Date"].ToString();
                string currencytyp = sdr["CurrencyType"].ToString();
                string stat = sdr["Status"].ToString();
                list.Add(new UserTrades(utid, fuacid, Ordertype, fupid, amnt, exec_amount, exec_fees, pric, fes, uplimit, lowlimit, currencytyp, stat, date,"0"));
            }
            sdr.Close();
            connect.Close();
            return list;
        }

        public List<UserTrades> Select_UserTrades(string ordertype, string currencytype, string Fuacid)
        {
            List<string> fuac = new List<string>();
            List<string> status = new List<string>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();

            SqlCommand cmd = new SqlCommand("select FUAC_Id from UserTrades where ordertype = '" + ordertype + "' and currencytype = '" + currencytype + "' and status!=0 ", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                
                string fuacid = sdr["FUAC_Id"].ToString();
                fuac.Add(fuacid);
            }
            sdr.Close();

            foreach (string uacid in fuac)
            {
                cmd = new SqlCommand("select logindate from UserAccount where uac_id ='" + uacid + "' ", connect);
                sdr = cmd.ExecuteReader();
                sdr.Read();

            //    DateTime val = Convert.ToDateTime(DateTime.UtcNow.Add(new TimeSpan(4, 44, 0)).ToString("dd-MM-yyyy HH:mm:ss"));
                    DateTime logindate = Convert.ToDateTime(sdr["logindate"].ToString());
                    DateTime currenttime = Convert.ToDateTime(DateTime.UtcNow.Add(new TimeSpan(5, 0, 0)));
                        
                TimeSpan check = currenttime.Subtract(logindate);
                 
                TimeSpan com = new TimeSpan(0, 15, 0);
                    if (check <= com)
                    {
                        //status 1
                        status.Add("1");
                        updatetradestatus(uacid, 1);

                    }
                    else { 
                    //status 0
                        status.Add("0");
                        updatetradestatus(uacid, 0);
                    }          
                
                sdr.Close();
            }
            List<UserTrades> list = new List<UserTrades>();
            int i = 0;
             cmd = new SqlCommand("select * from UserTrades where ordertype = '" + ordertype + "' and currencytype = '" + currencytype + "' and status!=0 ", connect);
             sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string utid = sdr["UT_Id"].ToString();
                string fuacid = sdr["FUAC_Id"].ToString();
                string Ordertype = sdr["OrderType"].ToString();
                string fupid = sdr["FUP_Id"].ToString();
                string exec_amount = sdr["ExecutedAmount"].ToString();
                string exec_fees = sdr["ExecutedFees"].ToString();
                string amnt = sdr["Amount"].ToString();
                string pric = sdr["Price"].ToString();
                string fes = sdr["Fees"].ToString();
                string uplimit = sdr["UpperLimit"].ToString();
                string lowlimit = sdr["LowerLimit"].ToString();
                string date = sdr["Date"].ToString();
                string currencytyp = sdr["CurrencyType"].ToString();
                string stat = sdr["Status"].ToString();

          

                list.Add(new UserTrades(utid, fuacid, Ordertype, fupid, amnt, exec_amount, exec_fees, pric, fes, uplimit, lowlimit, currencytyp, stat, date, status[i++]));
            }
            sdr.Close();
            connect.Close();
            return list;
        }


        public string UserCancel_Order(string amo, UserCancelOrder uco)
        {
            string uid = uco.FUserId.Substring(2, uco.FUserId.Length - 2);

            updatelogintime(uid);
           // uco.FORD_Id;
           // UserOrder y;
           // y.FUAC_Id;
            
            //uco.FTrade_UserId


            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("insert into UserCancelOrder (FORD_Id, FUserId, FUT_Id, FTrade_UserId, Message) values('" + uco.FORD_Id + "','" + uco.FUserId + "','" + uco.FUT_Id + "','" + uco.FTrade_UserId + "','" + uco.Message + "') select amount from usertrades where ut_id = '" + uco.FUT_Id + "' ", connect);
            try
            {
                string tid = uco.FTrade_UserId.Substring(2, uco.FTrade_UserId.Length-2);

                if (uid == tid)
                {
                    SqlCommand query = new SqlCommand("select fuac_id from userorder where ord_id = '" + uco.FORD_Id + "'  ", connect);
                    query.ExecuteNonQuery();
                    SqlDataReader sdr = query.ExecuteReader();
                    sdr.Read();

                    string fuacid = sdr["fuac_id"].ToString();
                    sdr.Close();

                    SqlCommand query2 = new SqlCommand("select fcm_token from useraccount where uac_id = '" + fuacid + "' ", connect);

                    query2.ExecuteNonQuery();
                    SqlDataReader sdt = query2.ExecuteReader();
                    sdt.Read();

                    string fcm = sdt["fcm_token"].ToString();
                    sdt.Close();
                    

                    var webClient = new WebClient();
                    string type = "cancel,"+uco.FORD_Id;
                    string msg = "Order Canceld";
                    string key = fcm;
                    if (key == "")
                        key = "a";
                    webClient.DownloadString("http://royalcryptoexchange.com/pushapi.php?send_notification&msg=" + msg + "&type=" + type + "&token=" + key);


                }
                else 
                {
                 
                    //string fuacid = u.FUAC_Id;
                    SqlCommand cmds = new SqlCommand("select fcm_token from useraccount where uac_id = '" + tid+ "' ", connect);
                   
                    SqlDataReader sdr = cmds.ExecuteReader();
                    sdr.Read();

                    string fcm = sdr["fcm_token"].ToString();
                    sdr.Close();


                    var webClient = new WebClient();
                    string type = "cancel,"+uco.FORD_Id;
                    string msg = "Order cancelled";
                    string key = fcm;
                    if (key == "")
                        key = "a";
                    webClient.DownloadString("http://royalcryptoexchange.com/pushapi.php?send_notification&msg=" + msg + "&type=" + type + "&token=" + key);
                }
                

                SqlDataReader abc = cmd.ExecuteReader();
                abc.Read();
                decimal am = Convert.ToDecimal(abc["amount"].ToString());
                abc.Close();
                decimal res = am + Convert.ToDecimal(amo);
                cmd = new SqlCommand("update usertrades set amount = '" + res + "' where ut_id = " + uco.FUT_Id, connect);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("update userorder set  status = 'cancel' where ord_id = " + uco.FORD_Id, connect);
                cmd.ExecuteNonQuery();


                connect.Close();
                return "success";

            }
            catch (Exception e)
            {
                connect.Close();
                return "failed";
            }

        }

        public string UserOrder_Pay(string userid,UserOrderPay uop)
        {

           
            updatelogintime(uop.FUAC_Id);

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("insert into UserOrderPay (UserId, FUT_Id, FORD_Id, FUAC_Id, Image) values('" + uop.UserId + "','" + uop.FUT_Id + "','"+uop.FORD_Id+"','" + uop.FUAC_Id + "','" + uop.Image + "')", connect);
            try
            {
                cmd.ExecuteNonQuery();
                SqlCommand query = new SqlCommand("select fuac_id from usertrades where ut_id = '" + uop.FUT_Id + "' ", connect);
                query.ExecuteNonQuery();
                SqlDataReader sdr = query.ExecuteReader();
                sdr.Read();

                string fuacid = sdr["fuac_id"].ToString();
               sdr.Close();
                // fuacid === trade id
                // uop.fuac_id === order id

                if (userid != uop.FUAC_Id)
                {
                    
                    //uop.FUAC_Id;

                    query = new SqlCommand("select fcm_token from useraccount where uac_id = '" + fuacid + "' ", connect);
                    query.ExecuteNonQuery();
                    SqlDataReader sdrs = query.ExecuteReader();
                    sdrs.Read();

                    string fcm = sdrs["fcm_token"].ToString();
                    sdrs.Close();


                    var webClient = new WebClient();
                    string type = "paid," + uop.FORD_Id;
                    string msg = "Order paid";
                    string key = fcm;
                    if (key == "")
                        key = "a";
                    webClient.DownloadString("http://royalcryptoexchange.com/pushapi.php?send_notification&msg=" + msg + "&type=" + type + "&token=" + key);


                }
               
                connect.Close();
                return "success";

            }
            catch (Exception e)
            {
                connect.Close();
                return "failed";
            }
            //return uop.UserOrderPay;
        }

        public string userorder_dispute(UserOrderDispute uod)
        {
            string tid = uod.FUAC_Id.Substring(2, uod.FUAC_Id.Length - 2);
            updatelogintime(tid);

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("insert into UserOrderDispute (UserId, FUT_Id, FORD_Id, FUAC_Id, Message, Image) values('" + uod.UserId + "','" + uod.FUT_Id + "','" + uod.FORD_Id + "','" + uod.FUAC_Id + "','" + uod.Message + "','" + uod.Image + "')", connect);
            try
            {

               // string uid = uod.UserId;
             


                string uid = uod.UserId.Substring(2, uod.UserId.Length - 2);
                //string tid = uco.FTrade_UserId.Substring(2, uco.FTrade_UserId.Length - 2);

                if (uid == tid)
                {
                    SqlCommand query = new SqlCommand("select fuac_id from userorder where ord_id = '" + uod.FORD_Id + "'  ", connect);
                    query.ExecuteNonQuery();
                    SqlDataReader sdr = query.ExecuteReader();
                    sdr.Read();

                    string fuacid = sdr["fuac_id"].ToString();
                    sdr.Close();

                    SqlCommand query2 = new SqlCommand("select fcm_token from useraccount where uac_id = '" + fuacid + "' ", connect);

                    query2.ExecuteNonQuery();
                    SqlDataReader sdt = query2.ExecuteReader();
                    sdt.Read();

                    string fcm = sdt["fcm_token"].ToString();
                    sdt.Close();

                    
                    var webClient = new WebClient();
                    string type = "dispute," + uod.FORD_Id;
                    string msg = "Order Dispute";
                    string key = fcm;
                    if (key == "")
                        key = "a";
                    webClient.DownloadString("http://royalcryptoexchange.com/pushapi.php?send_notification&msg=" + msg + "&type=" + type + "&token=" + key);


                }
                else
                {

                    //string fuacid = u.FUAC_Id;
                    SqlCommand cmds = new SqlCommand("select fcm_token from useraccount where uac_id = '" + uid + "' ", connect);
                    cmds.ExecuteNonQuery();
                    SqlDataReader sdr = cmds.ExecuteReader();
                    sdr.Read();

                    string fcm = sdr["fcm_token"].ToString();
                    sdr.Close();


                    var webClient = new WebClient();
                    string type = "dispute, "+uod.FORD_Id;
                    string msg = "Order Dispute";
                    string key = fcm;
                    if (key == "")
                        key = "a";
                    webClient.DownloadString("http://royalcryptoexchange.com/pushapi.php?send_notification&msg=" + msg + "&type=" + type + "&token=" + key);
                }


                cmd.ExecuteNonQuery();
                connect.Close();
                return "success";
                //return uod.UserOrderDispute;
            }
            catch (Exception e)
            {
                connect.Close();
                return "failed";
            }
        }

        public string pass_check(UserDocument d)
        {

            return d.User_Document;

        }

        public string tradestatuschange(string ut_id)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select status from userorder where fut_id = " + ut_id, connect);
            try
            {
                SqlDataReader sdr = cmd.ExecuteReader();
                bool val = true;
                while (sdr.Read())
                {
                    if (sdr["status"].ToString() != "completed" && sdr["status"].ToString() != "expire")
                    {
                        val = false;
                        break;
                    }
                }
                sdr.Close();
                if (val)
                {
                    cmd = new SqlCommand("update usertrades set status = 0 where ut_id = " + ut_id, connect);
                    cmd.ExecuteNonQuery();
                    connect.Close();
                    return "success";
                }
                else
                {
                    connect.Close();
                    return "failed";
                }

            }
            catch (Exception e)
            {
                connect.Close();
                return "failed";
            }


        }

        public string getupid(string ut_id)
        {

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select Fup_id from usertrades where ut_id = " + ut_id, connect);
            try
            {
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();

                string id = sdr["Fup_id"].ToString();
                sdr.Close();
                connect.Close();
                return id;
            }
            catch (Exception e)
            {
                connect.Close();
                return "0";
            }
        }
        public UserOrder Select_UserOrderSingle(string ord)
        {
            UserOrder list;// = new List<UserOrder>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from UserOrder where ord_id = '" + ord + "'", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            {

                string ordid = sdr["ORD_Id"].ToString();
                string userid = sdr["User_Id"].ToString();
                string ord_userid = sdr["ORD_UserId"].ToString();
                string fuac_id = sdr["FUAC_Id"].ToString();
                string futid = sdr["FUT_Id"].ToString();
                string pric = sdr["Price"].ToString();
                string amt = sdr["Amount"].ToString();
                string paym_meth = sdr["PaymentMethod"].ToString();
                string upr_lim = sdr["UpperLimit"].ToString();
                string lowr_lim = sdr["Lowerlimit"].ToString();
                string bit_amount = sdr["BitAmount"].ToString();
                string bit_price = sdr["BitPrice"].ToString();
                string stat = sdr["Status"].ToString();
                string ord_dat = sdr["Order_Date"].ToString();
                string expire = sdr["Expire"].ToString();
                string desc = sdr["Description"].ToString();
                string notify_stat = sdr["Notify_Status"].ToString();



                list = (new UserOrder(ordid, userid, ord_userid, fuac_id, futid, pric, amt, paym_meth, upr_lim,
                         lowr_lim, bit_amount, bit_price, stat, ord_dat, expire, desc, notify_stat));
            }
            sdr.Close();
            connect.Close();
            return list;
        }


        public List<UserOrder> Select_UserOrder(string ord)
        {
            List<UserOrder> list = new List<UserOrder>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from UserOrder where fuac_id = '" + ord + "' order by ord_id desc", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {

                string ordid = sdr["ORD_Id"].ToString();
                string userid = sdr["User_Id"].ToString();
                string ord_userid = sdr["ORD_UserId"].ToString();
                string fuac_id = sdr["FUAC_Id"].ToString();
                string futid = sdr["FUT_Id"].ToString();
                string pric = sdr["Price"].ToString();
                string amt = sdr["Amount"].ToString();
                string paym_meth = sdr["PaymentMethod"].ToString();
                string upr_lim = sdr["UpperLimit"].ToString();
                string lowr_lim = sdr["Lowerlimit"].ToString();
                string bit_amount = sdr["BitAmount"].ToString();
                string bit_price = sdr["BitPrice"].ToString();
                string stat = sdr["Status"].ToString();
                string ord_dat = sdr["Order_Date"].ToString();
                string expire = sdr["Expire"].ToString();
                string desc = sdr["Description"].ToString();
                string notify_stat = sdr["Notify_Status"].ToString();



                list.Add(new UserOrder(ordid, userid, ord_userid, fuac_id, futid, pric, amt, paym_meth, upr_lim,
                         lowr_lim, bit_amount, bit_price, stat, ord_dat, expire, desc, notify_stat));
            }
            sdr.Close();
            connect.Close();
            return list;
        }

        public List<UserOrder> Select_TradeOrder(string Fut_id)
        {

            //   string val = DateTime.UtcNow.ToString();
            // string v = DateTime.Now.ToString();
            List<UserOrder> list = new List<UserOrder>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from UserOrder where FUT_Id = '" + Fut_id + "'order by ORD_Id desc", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {

                string ordid = sdr["ORD_Id"].ToString();
                string userid = sdr["User_Id"].ToString();
                string ord_userid = sdr["ORD_UserId"].ToString();
                string fuac_id = sdr["FUAC_Id"].ToString();
                string futid = sdr["FUT_Id"].ToString();
                string pric = sdr["Price"].ToString();
                string amt = sdr["Amount"].ToString();
                string paym_meth = sdr["PaymentMethod"].ToString();
                string upr_lim = sdr["UpperLimit"].ToString();
                string lowr_lim = sdr["Lowerlimit"].ToString();
                string bit_amount = sdr["BitAmount"].ToString();
                string bit_price = sdr["BitPrice"].ToString();
                string stat = sdr["Status"].ToString();
                string ord_dat = sdr["Order_Date"].ToString();
                string expire = sdr["Expire"].ToString();
                string desc = sdr["Description"].ToString();
                string notify_stat = sdr["Notify_Status"].ToString();



                list.Add(new UserOrder(ordid, userid, ord_userid, fuac_id, futid, pric, amt, paym_meth, upr_lim,
                         lowr_lim, bit_amount, bit_price, stat, ord_dat, expire, desc, notify_stat));
            }
            sdr.Close();
            connect.Close();
            return list;
        }

        public Verification Add_UserOrder(string userid, string ord_userid, string fuac_id, string fut_id, string price, string amount,
                        string payment_method, string upperlimit, string lowerlimit, string bitamount, string bitprice, string status, string description, string notify_status)
        {
        
            
            decimal amt = Convert.ToDecimal(amount);
            decimal bamt = Convert.ToDecimal(bitamount);

            decimal Amount = amt - bamt;

            updatelogintime(fuac_id);

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("insert into UserOrder values ('" + userid + "','" + ord_userid + "','" + fuac_id + "','" + Convert.ToInt32(fut_id) + "','" + price + "','" + amount + "','" + payment_method + "','" + upperlimit + "','" + lowerlimit + "','" + bitamount + "','" + bitprice + "','" + status + "',convert(datetime,'" + DateTime.UtcNow.Add(new TimeSpan(5, 0, 0)).ToString("dd-MM-yyyy HH:mm:ss") + "',103),convert(datetime,'" + DateTime.UtcNow.Add(new TimeSpan(11, 0, 0)).ToString("dd-MM-yyyy HH:mm:ss") + "',103),'" + description + "','" + notify_status + "') select @@identity as iden ", connect);
            if (connect.State != ConnectionState.Open)
            {
                connect.Open();

            }
            try
            {

                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                string latest_ordid= sdr["iden"].ToString();
              sdr.Close();
                cmd = new SqlCommand("update usertrades set amount =  '" + Amount + "' where ut_id = " + fut_id, connect);
                cmd.ExecuteNonQuery();


                string uid = userid.Substring(2, userid.Length - 2); 
                SqlCommand query = new SqlCommand("select fcm_token from useraccount where uac_id = '" + uid + "' ", connect);
                query.ExecuteNonQuery();
                sdr = query.ExecuteReader();
                sdr.Read();

                string fcm = sdr["fcm_token"].ToString();
                sdr.Close();

 
                var webClient = new WebClient();
                string type = "order,"+latest_ordid;
                string msg = "Order Placed ";
                string key = fcm;
                
                if (key == "")
                    key = "a";
                webClient.DownloadString("http://royalcryptoexchange.com/pushapi.php?send_notification&msg=" + msg + "&type=" + type + "&token=" + key);


                connect.Close();
                return new Verification("Success", latest_ordid);
            }
            catch (Exception e)
            {
                connect.Close();
                return new Verification("failed", "query error");
            }


        }

        public UserPaymentDetail Add_UserPaymentDetail(string fuac_id, string type, string account, string account_title, string bankname, string Bankcode)
        {
            updatelogintime(fuac_id);

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("insert into UserPaymentDetail values ('" + Convert.ToInt32(fuac_id) + "','" + type + "','" + account + "','" + account_title + "','" + bankname + "','" + Bankcode + "') select @@identity as iden", connect);
            cmd.CommandType = CommandType.Text;

            if (connect.State != ConnectionState.Open)
            {
                connect.Open();

            }
            try
            {
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                string id = sdr["iden"].ToString();
                sdr.Close();
                connect.Close();
                return new UserPaymentDetail(id, fuac_id, type, account, account_title, bankname, Bankcode);
            }
            catch (Exception e)
            {
                connect.Close();
                //      return new Verification("failed", "query error");
            }

            return null;


        }

        public Verification MobileFactor(string phno)
        {
            Random random = new Random();
            int value = random.Next(1001, 9999);

            string message = "Your OTP Number is " + value + "( Sent By : Rameez )";
            //Label3.Text = message;
            string message1 = HttpUtility.UrlEncode(message);

            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.txtlocal.com/send/", new NameValueCollection()
                {
                {"apikey" , "zfI6lg38ANU-MGWGm1zXG8V1CMXlQZ3xcx8tjKvx3u"},
                {"numbers" ,phno},
                {"message" , message1},
                {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
            }
            return new Verification("success", value.ToString());
        }
        // in mobile email will be provided and isphonenumactive will be change in db




        public void Add_UserChat(string fut_id, string fuac_id, string message)
        {

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("insert into UserChat values ('" + Convert.ToInt32(fut_id) + "','" + Convert.ToInt32(fuac_id) + "','" + message + "')", connect);
            cmd.CommandType = CommandType.Text;
            if (connect.State != ConnectionState.Open)
            {
                connect.Open();

            }

            cmd.ExecuteNonQuery();
            connect.Close();


        }

        public void Update_UserTrades(string utid, string fuacid, string ordertype, string fupid, string amnt, string pric, string fes, string uplimit,
                             string lowlimit, string deadlin, string currencytyp, string stat)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("update UserTrades set Order_Type = '" + ordertype + "', Amount = '" + amnt + "',Price = '" + pric + "',Fees = '" + fes + "',UpperLimit = '" + uplimit + "',LowerLimit = '" + lowlimit + "', DeadLine = '" + deadlin + "',CurrencyType = '" + currencytyp + "',Status = '" + stat + "' where UT_Id = '" + Convert.ToInt32(utid) + "' and FUAC_Id = '" + Convert.ToInt32(fuacid) + "' and FUP_Id = '" + Convert.ToInt32(fupid) + "' ", connect);

            cmd.CommandType = CommandType.Text;
            if (connect.State != ConnectionState.Open)
            {
                connect.Open();

            }

            cmd.ExecuteNonQuery();
            connect.Close();
        }

        public Verification Update_UserOrder(string ordid, string status)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("update UserOrder set Status = '" + status + "' where ORD_Id = '" + Convert.ToInt32(ordid) + "'", connect);

            cmd.CommandType = CommandType.Text;
            if (connect.State != ConnectionState.Open)
            {
                connect.Open();

            }

            cmd.ExecuteNonQuery();
            connect.Close();

            return new Verification("Success", "Updated UserOrder");
        }

        public List<UserDocument> Select_UserDocument(string ud_id)
        {

            List<UserDocument> list = new List<UserDocument>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from UserDocument where FUAC_Id = '" + ud_id + "'", connect);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                string ud_iid = sdr["UD_Id"].ToString();
                string fuac_id = sdr["FUAC_Id"].ToString();
                string userdocument = sdr["User_Document"].ToString();


                list.Add(new UserDocument(ud_iid, fuac_id, userdocument));
            }
            sdr.Close();
            connect.Close();
            return list;
        }

        public Verification Add_UserDocument(string FUAC_id, string UserDocument)
        {
            updatelogintime(FUAC_id);

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("insert into UserDocument values ('" + Convert.ToInt32(FUAC_id) + "','" + UserDocument + "')", connect);
            cmd.CommandType = CommandType.Text;
            if (connect.State != ConnectionState.Open)
            {
                connect.Open();

            }
            try
            {
                cmd.ExecuteNonQuery();
                connect.Close();

                return new Verification("success", "");
            }
            catch (Exception e)
            {
                connect.Close();
                return new Verification("failed", "");
            }
        }


        public Verification Delete_UserPaymentDetail(string upid)
        {
           

            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand cmd = new SqlCommand("delete from UserPaymentDetail where UP_Id = '" + Convert.ToInt32(upid) + "' ", connect);

            cmd.CommandType = CommandType.Text;
            if (connect.State != ConnectionState.Open)
            {
                connect.Open();

            }
            try
            {
                cmd.ExecuteNonQuery();
                connect.Close();
                return new Verification("success", "");
            }
            catch (Exception e)
            {
                connect.Close();
                return new Verification("failed", e.ToString());
            }

        }



    }

}
