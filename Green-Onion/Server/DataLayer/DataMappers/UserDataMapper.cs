using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.DataLayer.RequestModels;
using GreenOnion.Server.DataLayer.DTOs;

namespace GreenOnion.Server.DataLayer.DataMappers
{
    // Class used for mapping User related objects from Request, DTO model to Entity and vice versa.
    public class UserDataMapper
    {
        public UserDataMapper()
        {
        }

        public static UserDto MapEntityToDto(User userEntity)
        {
            UserDto userDto = new UserDto()
            {
                userId = userEntity.userId,
                firstName = userEntity.firstName,
                lastName = userEntity.lastName,
                aboutMe = userEntity.aboutMeInfo,
                jobPosition = userEntity.jobPosition
            };

            return userDto;
        }

        public static User MapSignUpRequestToUser(UserSignUpRequest signUpRequest)
        {
            var user = new User();
            user.firstName = signUpRequest.firstName;
            user.userId = signUpRequest.userId;

            return user;
        }

        public static UserAccount MapSignUpRequestToUserAccount(UserSignUpRequest signUpRequest)
        {
            var userAccount = new UserAccount();
            userAccount.username = signUpRequest.username;
            userAccount.password = signUpRequest.password;
            userAccount.userId = signUpRequest.userId;

            return userAccount;
        }
    }
}
