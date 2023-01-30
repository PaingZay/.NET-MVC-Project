namespace ASPSession.Security
{
    public interface ISecurity
    {
        public bool LoginAuthentication(string username, string password, out string? customerID);

        public bool Authenticate();

        public bool RemoveAuthentication();
        public void ProvideSession(string username, string customerID);
    }
}
