namespace GreenOnion.Server.DataLayer.RequestModels
{
    public class UserSignUpRequest
    {
        public UserSignUpRequest()
        {
        }

        public string userId { get; set; }

        public string firstName { get; set; }

        public string username { get; set; }

        public string password { get; set; }
    }
}
