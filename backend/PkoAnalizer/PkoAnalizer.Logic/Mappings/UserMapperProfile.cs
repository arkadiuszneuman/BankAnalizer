using AutoMapper;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Users.Models.User;

namespace PkoAnalizer.Logic.Mappings
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
