using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RoyalCrypto
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebInvoke(Method = "POST",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      UriTemplate = "irelease/{ord_id}/{utfee}/{utamount}/{uobitamount}/{uoamount}/{ut_id}")]
        Verification irelease(string ord_id, string utfee, string utamount, string uobitamount, string uoamount, string ut_id);


        [OperationContract]
        [WebInvoke(Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      UriTemplate = "Select_SupportTicket/{st_id}")]
        SupportTicket Select_SupportTicket(string st_id);



        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "Select_UserDocument/{ud_id}")]
        List<UserDocument> Select_UserDocument(string ud_id);


        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "Select_UserAccount/{uac_id}")]
        UserAccount Select_UserAccount(string uac_id);

        [OperationContract]
        [WebInvoke(Method = "POST",
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json,
       UriTemplate = "forgot_pass/{Email}")]
        Verification forgot_pass(string Email);

        [OperationContract]
        [WebInvoke(Method = "GET",
  RequestFormat = WebMessageFormat.Json,
  ResponseFormat = WebMessageFormat.Json,
  UriTemplate = "Select_UserPaymentDetail/{up_id}")]
        List<UserPaymentDetail> Select_UserPaymentDetail(string up_id);


        [OperationContract]
        [WebInvoke(Method = "GET",
  RequestFormat = WebMessageFormat.Json,
  ResponseFormat = WebMessageFormat.Json,
  UriTemplate = "Select_UserPaymentDetailSingle/{up_id}")]
        UserPaymentDetail Select_UserPaymentDetailSingle(string up_id);


        [OperationContract]
        [WebInvoke(Method = "GET",
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json,
       UriTemplate = "Select_UserOrder/{ord_id}")]
        List<UserOrder> Select_UserOrder(string ord_id);

        [OperationContract]
        [WebInvoke(Method = "GET",
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json,
       UriTemplate = "Select_UserOrderSingle/{ord}")]
        UserOrder Select_UserOrderSingle(string ord);

        [OperationContract]
        [WebInvoke(Method = "GET",
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json,
       UriTemplate = "Select_TradeOrder/{fut_id}")]
        List<UserOrder> Select_TradeOrder(string Fut_id);


        [OperationContract]
        [WebInvoke(Method = "GET",
     RequestFormat = WebMessageFormat.Json,
     ResponseFormat = WebMessageFormat.Json,
     UriTemplate = "Select_UserChat/{ucht_id}")]
        UserChat Select_UserChat(string ucht_id);


        [OperationContract]
        [WebInvoke(Method = "GET",
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json,
       UriTemplate = "Select_TradeDetail/{uac_id}/{up_id}")]
        TradeDetail Select_TradeDetail(string uac_id, string up_id);




        [OperationContract]
        [WebInvoke(Method = "GET",
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json,
       UriTemplate = "Select_UserTrades/{ordertype}/{ctype}/{Fuacid}")]
        List<UserTrades> Select_UserTrades(string ordertype, string ctype, string Fuacid);

        [OperationContract]
        [WebInvoke(Method = "GET",
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json,
       UriTemplate = "Select_DashBoard/{fuac_id}")]
        List<UserTrades> Select_DashBoard(string fuac_id);

        [OperationContract]
        [WebInvoke(Method = "GET",
     RequestFormat = WebMessageFormat.Json,
     ResponseFormat = WebMessageFormat.Json,
     UriTemplate = "getupid/{ut_id}")]
        string getupid(string ut_id);

        [OperationContract]
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "tradestatuschange/{ut_id}")]
        string tradestatuschange(string ut_id);


        [OperationContract]
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "pass_check")]
        string pass_check(UserDocument d);


        [OperationContract]
        [WebInvoke(Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "Delete_UserPaymentDetail/{upid}")]
        Verification Delete_UserPaymentDetail(string upid);
        /*
                [OperationContract]
                [WebInvoke(Method = "DELETE",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "Delete_UserOrder/{upid}")]
                void Delete_UserOrder(String upid);
                */



        [OperationContract]
        [WebInvoke(Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "Add_SupportTicket")]
        string Add_SupportTicket(SupportTicket st);

        [OperationContract]
        [WebInvoke(Method = "POST",
    RequestFormat = WebMessageFormat.Json,
  ResponseFormat = WebMessageFormat.Json,
  UriTemplate = "MobileFactor/{phno}")]
        Verification MobileFactor(string phno);

        [OperationContract]
        [WebInvoke(Method = "POST",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "Login/{email}/{pass}")]
        Verification Login(string email, string pass,Verification fcm);

        [OperationContract]
        [WebInvoke(Method = "POST",
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json,
       UriTemplate = "Logout/{email}")]
        Verification Logout(string email);



        [OperationContract]
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "Add_UserAccount/{fname}/{lname}/{email}/{phone}/{password}/{cnic}/{dob}")]
        Verification Add_UserAccount(string fname, string lname, string email, string phone, string password, string cnic, string dob, Verification fcm);

        [OperationContract]
        [WebInvoke(Method = "POST",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "VerifyEmail/{uacid}/{code}")]
        Verification VerifyEmail(string uacid, string code);





        [OperationContract]
        [WebInvoke(Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "Add_UserChat/{fut_id}/{fuac_id}/{message}")]
        void Add_UserChat(string fut_id, string fuac_id, string message);



        [OperationContract]
        [WebInvoke(Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "Add_UserTrades/{fuacid}/{ordertype}/{fupid}/{amnt}/{executed_amount}/{executed_fees}/{pric}/{fes}/{uplimit}/{lowlimit}/{currencytyp}")]
        Verification Add_UserTrades(string fuacid, string ordertype, string fupid, string amnt, string executed_amount, string executed_fees, string pric, string fes, string uplimit,
                             string lowlimit, string currencytyp);


        [OperationContract]
        [WebInvoke(Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "Add_UserOrder/{userid}/{ord_userid}/{fuac_id}/{fut_id}/{price}/{amount}/{payment_method}/{upperlimit}/{lowerlimit}/{bitamount}/{bitprice}/{status}/{description}/{notify_status}")]
        Verification Add_UserOrder(string userid, string ord_userid, string fuac_id, string fut_id, string price, string amount,
                        string payment_method, string upperlimit, string lowerlimit, string bitamount, string bitprice, string status, string description, string notify_status);

        [OperationContract]
        [WebInvoke(Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "Add_UserDocument/{FUAC_id}/{UserDocument}")]
        Verification Add_UserDocument(string FUAC_id, string UserDocument);

        [OperationContract]
        [WebInvoke(Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "Add_UserPaymentDetail/{fuac_id}/{type}/{account}/{account_title}/{bankname}/{Bankcode}")]
        UserPaymentDetail Add_UserPaymentDetail(string fuac_id, string type, string account, string account_title, string bankname, string Bankcode);



        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "Update_UserAccount/{uac_id}/{fname}/{lname}/{password}/{terms}")]
        Verification Update_UserAccount(string uac_id, string fname, string lname, string password, string terms);


        [OperationContract]
        [WebInvoke(Method = "POST",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "Update_UserTrades/{ut_id}/{fuac_id}/{order_type}/{fup_id}/{amount}/{price}/{fees}/{upperlimit}/{lowerlimit}/{deadline}/{currencytype}/{status}")]
        void Update_UserTrades(string ut_id, string fuac_id, string order_type, string fup_id, string amount, string price, string fees, string upperlimit, string lowerlimit, string deadline, string currencytype, string status);





        [OperationContract]
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "Update_UserOrder/{ord_id}/{status}")]
        Verification Update_UserOrder(string ord_id, string status);

        [OperationContract]
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "userorder_dispute")]
        string userorder_dispute(UserOrderDispute uod);

        [OperationContract]
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "UserOrder_Pay")]
        string UserOrder_Pay(UserOrderPay uop);

        [OperationContract]
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "UserCancel_Order/{amount}")]
        string UserCancel_Order(string amount, UserCancelOrder uco);


    }

    [DataContract]
    public class UserCancelOrder
    {
        public string uoc_id, ford_id, fuser_id, fut_id, ftrade_userid, message;

        [DataMember]
        public string UOC_Id
        {
            get { return uoc_id; }
            set { uoc_id = value; }
        }

        [DataMember]
        public string FORD_Id
        {
            get { return ford_id; }
            set { ford_id = value; }
        }

        [DataMember]
        public string FUserId
        {
            get { return fuser_id; }
            set { fuser_id = value; }
        }

        [DataMember]
        public string FUT_Id
        {
            get { return fut_id; }
            set { fut_id = value; }
        }

        [DataMember]
        public string FTrade_UserId
        {
            get { return ftrade_userid; }
            set { ftrade_userid = value; }
        }

        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public UserCancelOrder(string Uoc_id, string Ford_id, string Fuser_id, string Fut_id, string Ftrade_userid, string Msg)
        {

            uoc_id = Uoc_id;
            ford_id = Ford_id;
            fuser_id = Fuser_id;
            fut_id = Fut_id;
            ftrade_userid = Ftrade_userid;
            message = Msg;

        }
    }

    [DataContract]
    public class UserOrderPay
    {
        public string uop_id, user_id, fut_id, ford_id, fuac_id, image;

        [DataMember]
        public string UOP_Id
        {
            get { return uop_id; }
            set { uop_id = value; }
        }
        [DataMember]
        public string UserId
        {
            get { return user_id; }
            set { user_id = value; }
        }
        [DataMember]
        public string FUT_Id
        {
            get { return fut_id; }
            set { fut_id = value; }
        }
        [DataMember]
        public string FORD_Id
        {
            get { return ford_id; }
            set { ford_id = value; }
        }
        [DataMember]
        public string FUAC_Id
        {
            get { return fuac_id; }
            set { fuac_id = value; }
        }
        [DataMember]
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public UserOrderPay(string Uop_id, string User_id, string Fut_id, string Ford_id, string Fuac_id, string Image)
        {

            uop_id = Uop_id;
            user_id = User_id;
            fut_id = Fut_id;
            ford_id = Ford_id;
            fuac_id = Fuac_id;
            image = Image;

        }
    }

    [DataContract]
    public class UserOrderDispute
    {
        public string uod_id, user_id, fut_id, ford_id, fuac_id, message, image;

        [DataMember]
        public string UOD_Id
        {
            get { return uod_id; }
            set { uod_id = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return user_id; }
            set { user_id = value; }
        }

        [DataMember]
        public string FUT_Id
        {
            get { return fut_id; }
            set { fut_id = value; }
        }

        [DataMember]
        public string FORD_Id
        {
            get { return ford_id; }
            set { ford_id = value; }
        }

        [DataMember]
        public string FUAC_Id
        {
            get { return fuac_id; }
            set { fuac_id = value; }
        }

        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        [DataMember]
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public UserOrderDispute(string Uod_id, string User_id, string Fut_id, string Ford_id, string Fuac_id, string msg, string img)
        {
            uod_id = Uod_id;
            user_id = User_id;
            fut_id = Fut_id;
            ford_id = Ford_id;
            fuac_id = Fuac_id;
            message = msg;
            image = img;

        }
    }

    [DataContract]
    public class UserOrder
    {
        public string ord_id, user_id, Ord_userId, fuac_id, fut_id, price, amount, payment_method, upper_limit, lower_limit, bit_amount, bit_price, status, order_date, expire, desc, notify_stat;


        [DataMember]
        public string ORD_Id
        {
            get { return ord_id; }
            set { ord_id = value; }
        }

        [DataMember]
        public string User_Id
        {
            get { return user_id; }
            set { user_id = value; }
        }

        [DataMember]
        public string Ord_UserId
        {
            get { return Ord_userId; }
            set { Ord_userId = value; }
        }

        [DataMember]
        public string FUAC_Id
        {
            get { return fuac_id; }
            set { fuac_id = value; }
        }

        [DataMember]
        public string FUT_Id
        {
            get { return fut_id; }
            set { fut_id = value; }
        }
        [DataMember]
        public string Price
        {
            get { return price; }
            set { price = value; }
        }
        [DataMember]
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        [DataMember]
        public string PaymentMethod
        {
            get { return payment_method; }
            set { payment_method = value; }
        }
        [DataMember]
        public string UpperLimit
        {
            get { return upper_limit; }
            set { upper_limit = value; }
        }
        [DataMember]
        public string LowerLimit
        {
            get { return lower_limit; }
            set { lower_limit = value; }
        }

        [DataMember]
        public string BitAmount
        {
            get { return bit_amount; }
            set { bit_amount = value; }
        }

        [DataMember]
        public string BitPrice
        {
            get { return bit_price; }
            set { bit_price = value; }
        }

        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        [DataMember]
        public string Order_Date
        {
            get { return order_date; }
            set { order_date = value; }
        }

        [DataMember]
        public string Expire
        {
            get { return expire; }
            set { expire = value; }
        }

        [DataMember]
        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }

        [DataMember]
        public string Notify_Status
        {
            get { return notify_stat; }
            set { notify_stat = value; }
        }


        public UserOrder(string ordid, string userid, string ord_userI, string fuacId, string futid, string pric,
            string amt, string paym_meth, string upr_lim,
                           string lowr_lim, string bit_Amount, string bit_Price, string stat, string ord_dat,
            string Expire, string Description, string Notify_Status)
        {

            ord_id = ordid;
            user_id = userid;
            Ord_userId = ord_userI;
            fuac_id = fuacId;
            fut_id = futid;
            price = pric;
            amount = amt;
            payment_method = paym_meth;
            upper_limit = upr_lim;
            lower_limit = lowr_lim;
            bit_amount = bit_Amount;
            bit_price = bit_Price;
            status = stat;
            order_date = ord_dat;
            expire = Expire;
            desc = Description;
            notify_stat = Notify_Status;


        }

    }

    [DataContract]
    public class UserPaymentDetail
    {
        public string up_id, fuac_id, type, account, accounttitle, bankname, bankcode;

        [DataMember]
        public string UP_Id
        {
            get { return up_id; }
            set { up_id = value; }
        }
        [DataMember]
        public string FUAC_Id
        {
            get { return fuac_id; }
            set { fuac_id = value; }
        }
        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        [DataMember]
        public string Account
        {
            get { return account; }
            set { account = value; }
        }
        [DataMember]
        public string AccountTitle
        {
            get { return accounttitle; }
            set { accounttitle = value; }
        }
        [DataMember]
        public string BankName
        {
            get { return bankname; }
            set { bankname = value; }
        }
        [DataMember]
        public string BankCode
        {
            get { return bankcode; }
            set { bankcode = value; }
        }

        public UserPaymentDetail(String upid, String fuacid, String typ, String acnt, String acttitl, String bnknam, String bnkcod)
        {
            up_id = upid;
            fuac_id = fuacid;
            type = typ;
            account = acnt;
            accounttitle = acttitl;
            bankname = bnknam;
            bankcode = bnkcod;
        }
    }
    [DataContract]
    public class TradeDetail
    {
        public string terms;
        public UserPaymentDetail upd;
        public TradeDetail(string termss, UserPaymentDetail s)
        {
            terms = termss;
            upd = s;
        }
        [DataMember]
        public string Terms
        {
            get { return terms; }
            set { terms = value; }
        }
        [DataMember]
        public UserPaymentDetail PaymentMethod
        {
            get { return upd; }
            set { upd = value; }
        }


    }

    [DataContract]
    public class Verification
    {
        string Status, msg;
        public Verification(string id, string cod)
        {
            Status = id;
            msg = cod;
        }
        [DataMember]
        public string status
        {
            get { return Status; }
            set { Status = value; }
        }
        [DataMember]
        public string message
        {
            get { return msg; }
            set { msg = value; }
        }
    }

    [DataContract]
    public class UserTrades
    {
        public string utstatus, ut_id, fuac_id, order_type, fup_id, amount, exec_fee, exec_amm, price, fees, upperlimit, lowerlimit, date, currencytype, status;

        [DataMember]
        public string ut_status
        {
            get { return utstatus; }
            set { utstatus = value; }
        }

        [DataMember]
        public string UT_Id
        {
            get { return ut_id; }
            set { ut_id = value; }
        }
        [DataMember]
        public string FUAC_Id
        {
            get { return fuac_id; }
            set { fuac_id = value; }
        }
        [DataMember]
        public string OrderType
        {
            get { return order_type; }
            set { order_type = value; }
        }
        [DataMember]
        public string FUP_Id
        {
            get { return fup_id; }
            set { fup_id = value; }
        }
        [DataMember]
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        [DataMember]
        public string Price
        {
            get { return price; }
            set { price = value; }
        }
        [DataMember]
        public string Fees
        {
            get { return fees; }
            set { fees = value; }
        }
        [DataMember]
        public string ExecutedAmount
        {
            get { return exec_amm; }
            set { exec_amm = value; }
        }
        [DataMember]
        public string ExecutedFees
        {
            get { return exec_fee; }
            set { exec_fee = value; }
        }
        [DataMember]
        public string UpperLimit
        {
            get { return upperlimit; }
            set { upperlimit = value; }
        }
        [DataMember]
        public string LowerLimit
        {
            get { return lowerlimit; }
            set { lowerlimit = value; }
        }

        [DataMember]
        public string CurrencyType
        {
            get { return currencytype; }
            set { currencytype = value; }
        }
        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        [DataMember]
        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public UserTrades(string utid, string fuacid, string ordertype, string fupid, string amnt, string ea, string ef, string pric, string fes, string uplimit,
                          string lowlimit, string currencytyp, string stat, string dat, string Uttrades)
        {
            utstatus = Uttrades;
            ut_id = utid;
            fuac_id = fuacid;
            order_type = ordertype;
            fup_id = fupid;
            amount = amnt;
            exec_amm = ea;
            exec_fee = ef;
            price = pric;
            fees = fes;
            upperlimit = uplimit;
            lowerlimit = lowlimit;
            currencytype = currencytyp;
            status = stat;
            date = dat;
        }
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.

    [DataContract]
    public class SupportTicket
    {

        public string st_id, tittle, description, image, fuac_id;

        public SupportTicket(string ST_id, string Title, string Description, string Image, string Fuac_id)
        {

            st_id = ST_id;
            tittle = Title;
            description = Description;
            image = Image;
            fuac_id = Fuac_id;

        }
        [DataMember]
        public string ST_Id
        {
            get { return st_id; }
            set { st_id = value; }
        }

        [DataMember]
        public string Title
        {
            get { return tittle; }
            set { tittle = value; }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        [DataMember]
        public string FUAC_Id
        {
            get { return fuac_id; }
            set { fuac_id = value; }
        }
    }

    [DataContract]
    public class UserChat
    {

        public string ucht_id, fut_id, fuac_id, message;

        public UserChat(string Ucht_id, string Fut_id, string Fuac_id, string Message)
        {

            ucht_id = Ucht_id;
            fut_id = Fut_id;
            fuac_id = Fuac_id;
            message = Message;

        }

        [DataMember]
        public string UCHT_Id
        {
            get { return ucht_id; }
            set { ucht_id = value; }
        }

        [DataMember]
        public string FUT_Id
        {
            get { return fut_id; }
            set { fut_id = value; }
        }

        [DataMember]
        public string FUAC_Id
        {
            get { return fuac_id; }
            set { fuac_id = value; }
        }

        [DataMember]
        public string Message
        {
            get { return Message; }
            set { Message = value; }
        }

    }


    [DataContract]
    public class UserDocument
    {

        public string ud_id, fuac_id, userdocument;

        public UserDocument(string Ud_id, string Fuac_id, string Userdocument)
        {

            ud_id = Ud_id;
            fuac_id = Fuac_id;
            userdocument = Userdocument;

        }

        [DataMember]
        public string UD_ID
        {
            get { return ud_id; }
            set { ud_id = value; }
        }

        [DataMember]
        public string FUAC_id
        {
            get { return fuac_id; }
            set { fuac_id = value; }
        }

        [DataMember]
        public string User_Document
        {
            get { return userdocument; }
            set { userdocument = value; }
        }

    }



    [DataContract]
    public class UserAccount
    {
        public string uac_id, firstname, lastname, email, phonenum, isemailactive, isphonenumactive, password, createddate, logindate, logoutdate, isactive, cnic, dateofbirth, userid, terms, documentverification;

        public UserAccount(string id, string fname, string lname,
            string emai, string phno, string emailactive,
            string phnoactive, string pass, string cdate, string indate,
            string outdate, string active, string nic, string dob, string uid
            , string term, string docver)
        {
            uac_id = id;
            firstname = fname;
            lastname = lname;
            email = emai;
            phonenum = phno;
            isemailactive = emailactive;
            isphonenumactive = phnoactive;
            password = pass;
            createddate = cdate;
            logindate = indate;
            logoutdate = outdate;
            isactive = active;
            cnic = nic;
            dateofbirth = dob;
            userid = uid;
            terms = term;
            documentverification = docver;
        }

        [DataMember]
        public string UAC_Id
        {
            get { return uac_id; }
            set { uac_id = value; }
        }
        [DataMember]
        public string FirstName
        {
            get { return firstname; }
            set { firstname = value; }
        }
        [DataMember]
        public string LastName
        {
            get { return lastname; }
            set { lastname = value; }
        }
        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        [DataMember]
        public string PhoneNum
        {
            get { return phonenum; }
            set { phonenum = value; }
        }
        [DataMember]
        public string IsEmailActive
        {
            get { return isemailactive; }
            set { isemailactive = value; }
        }
        [DataMember]
        public string IsPhoneNumActive
        {
            get { return isphonenumactive; }
            set { isphonenumactive = value; }
        }
        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        [DataMember]
        public string CreatedDate
        {
            get { return createddate; }
            set { createddate = value; }
        }
        [DataMember]
        public string LoginDate
        {
            get { return logindate; }
            set { logindate = value; }
        }
        [DataMember]
        public string LogoutDate
        {
            get { return logoutdate; }
            set { logoutdate = value; }
        }
        [DataMember]
        public string IsActive
        {
            get { return isactive; }
            set { isactive = value; }
        }
        [DataMember]
        public string CNIC
        {
            get { return cnic; }
            set { cnic = value; }
        }
        [DataMember]
        public string DateOfBirth
        {
            get { return dateofbirth; }
            set { dateofbirth = value; }
        }
        [DataMember]
        public string UserId
        {
            get { return userid; }
            set { userid = value; }
        }
        [DataMember]
        public string Terms
        {
            get { return terms; }
            set { terms = value; }
        }
        [DataMember]
        public string DocumentVerification
        {
            get { return documentverification; }
            set { documentverification = value; }
        }
    }
}



