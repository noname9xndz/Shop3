using AutoMapper;

namespace Shop3.Application.AutoMapper
{
    public class AutoMapperConfig
    {//http://docs.automapper.org
        public static MapperConfiguration RegisterMappings()
        {
            // nếu mapping này ko hoạt động config trực tiếp trên start up ở web
            // regiter mapping giữa model và viewmodel
            // nhớ regiter tron startup trên tầng startup trên tầng web
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });
        }
    }
}
