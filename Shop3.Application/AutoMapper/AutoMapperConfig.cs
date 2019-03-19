using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            // regiter mapping giữa model và viewmodel
            // nhớ regiter tron startup trên tầng web
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });
        }
    }
}
