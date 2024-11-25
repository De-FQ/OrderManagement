using AutoMapper;
using Data.UserManagement;
using Utility.Models.Admin.DTO;
using Utility.Models.Base;
using Utility.Models.Frontend.Activity;
using Utility.Models.Frontend.Attraction;
using Utility.Models.Frontend.Careers;
using Utility.Models.Frontend.Events;
using Utility.Models.Frontend.Faisal;
using Utility.Models.Frontend.HomePage;
using Utility.Models.Frontend.MediaGallery;
using Utility.Models.Frontend.News;
using Utility.Models.Frontend.Project;

namespace API.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>().AddTransform<string>(s => string.IsNullOrEmpty(s) ? string.Empty : s);
            CreateMap<UserModel, User>();

        }
    }
}
