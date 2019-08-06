using AutoMapper;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Common;
using Shop3.Data.Entities;
using Shop3.Infrastructure.Interfaces;
using Shop3.Utilities.Dtos;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml.FormulaParsing.Utilities;
using Shop3.Application.ViewModels.Custom;

namespace Shop3.Application.Implementation
{
    public class SlideService : ISlideService
    {
        private readonly IRepository<Slide, int> _slideRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SlideService
        (IRepository<Slide, int> slideRepository,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _slideRepository = slideRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public PagedResult<SlideViewModel> GetAllSlidesByGroupAlias(string keyword, int page, int pageSize)
        {
            var query = _slideRepository.FindAll().GroupBy(x => x.GroupAlias).Select(x => x.First());
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword) || x.GroupAlias == keyword);

            int totalRow = query.Count();
            var data = query.OrderBy(x => x.DisplayOrder)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var output = _mapper.ProjectTo<SlideViewModel>(data).ToList();
            var paginationSet = new PagedResult<SlideViewModel>()
            {
                Results = output,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };

            return paginationSet;


        }
       

        public PagedResult<CustomSlideViewModel> GetAllSlidesPagingByGroupAlias(string keyword, int page, int pageSize)
        {
            var slides = _slideRepository.FindAll();

            var query = from s in slides
                group s by s.GroupAlias into slide
                select new CustomSlideViewModel
                {
                    GroupAlias = slide.Key,
                    TotalSlide = (slide.Count() )
                };

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.GroupAlias.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.TotalSlide)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
            var paginationSet = new PagedResult<CustomSlideViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };

            return paginationSet;


        }

        public List<SlideViewModel> GetAllSlidesByGroupAlias(string alias)
        {
            var slides = _slideRepository.FindAll(x=>x.GroupAlias == alias);
            var model = _mapper.ProjectTo<SlideViewModel>(slides).ToList();
            return model;
        }


        public List<SlideViewModel> AddAndRemoveSlide(int[] slideIdRemove, List<SlideViewModel> slideAdd)
        {
            throw new System.NotImplementedException();
        }

        public SlideViewModel GetSlideById(int id)
        {
            return _mapper.Map<Slide, SlideViewModel>(_slideRepository.FindById(id));
        }

        public void AddSlide(SlideViewModel slideViewModel)
        {
            var slide = _mapper.Map<SlideViewModel, Slide>(slideViewModel);
            _slideRepository.Add(slide);
        }

        public void UpdateSlide(SlideViewModel slide)
        {
            var slideViewModel = _mapper.Map<SlideViewModel, Slide>(slide);
            _slideRepository.Update(slideViewModel.Id, slideViewModel);
        }

        public void DeleteSlideById(int id)
        {
            _slideRepository.RemoveById(id);
        }

        public void DeleteSlideByAlias(string alias)
        {
            var model = _slideRepository.FindAll(x => x.GroupAlias == alias).ToList();
            _slideRepository.RemoveMultiple(model);
        }


        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
