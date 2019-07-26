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
using Shop3.Data.Enums;

namespace Shop3.Application.Implementation
{
    public class CommonService : ICommonService
    {
        IRepository<Footer, string> _footerRepository;
        IRepository<SystemConfig, string> _systemConfigRepository;
        IUnitOfWork _unitOfWork;
        IRepository<Slide, int> _slideRepository;
        private IRepository<PageDefault, string> _pageDefaultRepository;
        private readonly IMapper _mapper;
        public CommonService(IRepository<Footer, string> footerRepository,
            IRepository<SystemConfig, string> systemConfigRepository,
            IUnitOfWork unitOfWork,
            IRepository<Slide, int> slideRepository, IMapper mapper,
            IRepository<PageDefault, string> pageDefaultRepository)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _slideRepository = slideRepository;
            _pageDefaultRepository = pageDefaultRepository;
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

        public PageDefaultViewModel GetPageDefault(string pageDefaultId)
        {
            var model = _pageDefaultRepository.FindSingle(x=>x.Id == pageDefaultId && x.Status == Status.Active);
            return _mapper.Map<PageDefault, PageDefaultViewModel>(model);
        }
    }
}
