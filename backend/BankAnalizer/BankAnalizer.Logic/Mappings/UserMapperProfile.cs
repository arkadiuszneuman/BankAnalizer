using AutoMapper;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.Users.Models.User;

namespace BankAnalizer.Logic.Mappings
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}
