using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Common;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Infrastructure.Interfaces;

namespace Shop3.Application.Implementation
{
    public class PageDefaultService : IPageDefaultService
    {
        private IRepository<PageDefault, string> _pageDefaultRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PageDefaultService(IRepository<PageDefault, string> pageDefaultRepository,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _pageDefaultRepository = pageDefaultRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public PageDefaultViewModel GetPageDefault(string pageDefaultId)
        {
            var model = _pageDefaultRepository.FindSingle(x => x.Id == pageDefaultId && x.Status == Status.Active);
            return _mapper.Map<PageDefault, PageDefaultViewModel>(model);
        }

        public List<PageDefaultViewModel> GetAllPageDefault()
        {
            var model = _pageDefaultRepository.FindAll();
            return _mapper.ProjectTo<PageDefaultViewModel>(model).ToList();
        }

        public void Update(PageDefaultViewModel pageVm)
        {
            var page = _mapper.Map<PageDefaultViewModel, PageDefault>(pageVm);
            _pageDefaultRepository.Update(page.Id, page);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }
    }
}
