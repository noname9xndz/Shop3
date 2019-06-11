using AutoMapper;
using AutoMapper.QueryableExtensions;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Common;
using Shop3.Data.Entities;
using Shop3.Infrastructure.Interfaces;
using Shop3.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop3.Application.Implementation
{
    public class CommonService : ICommonService
    {
        IRepository<Footer, string> _footerRepository;
        IRepository<SystemConfig, string> _systemConfigRepository;
        IUnitOfWork _unitOfWork;
        IRepository<Slide, int> _slideRepository;
        private readonly IMapper _mapper;
        public CommonService(IRepository<Footer, string> footerRepository,
            IRepository<SystemConfig, string> systemConfigRepository,
            IUnitOfWork unitOfWork,
            IRepository<Slide, int> slideRepository, IMapper mapper)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _slideRepository = slideRepository;
            _mapper = mapper;
        }

        public FooterViewModel GetFooter()
        {
            return _mapper.Map<Footer, FooterViewModel>(_footerRepository.FindSingle(x => x.Id ==
            CommonConstants.DefaultFooterId));
        }

        public List<SlideViewModel> GetSlides(string groupAlias)
        {
            var data = _slideRepository.FindAll(x => x.Status && x.GroupAlias == groupAlias);
           return _mapper.ProjectTo<SlideViewModel>(data).ToList();
        }

        public SystemConfigViewModel GetSystemConfig(string code)
        {
            return _mapper.Map<SystemConfig, SystemConfigViewModel>(_systemConfigRepository.FindSingle(x => x.Id == code));
        }
    }
}
