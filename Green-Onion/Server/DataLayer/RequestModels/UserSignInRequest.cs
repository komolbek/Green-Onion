namespace GreenOnion.Server.DataLayer.RequestModels
{
    public class UserSignInRequest
    {
        public UserSignInRequest()
        {
        }

        public string username { get; set; }

        public string password { get; set; }
    }
}
