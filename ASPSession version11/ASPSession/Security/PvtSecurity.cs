using ASPSession.Connectors;

namespace ASPSession.Security
{

    public class PvtSecurity : ISecurity //a class that deals with authenticating user and placing or removing session cookies during login and logout respectively. PS session cookies are set to clear off by itself after a certain duration or if the user wills, he could clear it off the browser

    {
        IHttpContextAccessor context;
        IConnector conn;
        public PvtSecurity(IConnector conn, IHttpContextAccessor context) 
        {
            this.conn = conn;
            this.context = context;
        }


        public bool LoginAuthentication(string username, string password, out string? customerID) //check form data against DB, if user session is valid, give session id
        {
            return conn.IdentifyUserDB(username, password, out customerID); 
        }

        public void ProvideSession(string username, string customerID) // calls a method from connector to record session in DB and provides a session cookie
        {
            string guidStr = Guid.NewGuid().ToString();
            if(conn.ProvideSessionDB(customerID, guidStr)) //returns true if manage to record customer session in DB
            {
                
                context.HttpContext!.Session.SetString("username", username);
                context.HttpContext!.Session.SetString("customerID", customerID);  //inserts session details in session cookie
                context.HttpContext!.Session.SetString("sessionID", guidStr);
            }
                                                        
        }

        public bool Authenticate() //placed in every controller methods that requires authentication
        {
            string? customerID;
            string? sessionID;
            return  HasSessionInCookie(out customerID,out sessionID) && conn.IsSessionValidDB(customerID!, sessionID!); //check if user has a session in cookie, skips checking session validity if theres no session details in cookie
        }

        private bool HasSessionInCookie(out string? customerID, out string? sessionID) //to check whether session contains cust details, if none it means user not logged in
        {
            
            customerID = context.HttpContext!.Session.GetString("customerID"); //if session cookie exist, provide details to the calling method
            sessionID = context.HttpContext!.Session.GetString("sessionID");
            return customerID != null && sessionID != null;
        }


        public bool RemoveAuthentication()
        {
            string? customerID = context.HttpContext.Session.GetString("customerID");
            context.HttpContext!.Session.Clear();
            if(customerID != null || customerID != "")
            {
                return conn.RemoveExpiredSessionDB(customerID!);
            }
            else return false;
        }

    }


}