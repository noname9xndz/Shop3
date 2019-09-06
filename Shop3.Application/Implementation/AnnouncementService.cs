using AutoMapper;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.System;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Infrastructure.Interfaces;
using Shop3.Utilities.Dtos;
using System;
using System.Linq;

namespace Shop3.Application.Implementation
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IRepository<Announcement, string> _announcementRepository;
        private readonly IRepository<AnnouncementUser, int> _announcementUserRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AnnouncementService(IRepository<Announcement, string> announcementRepository,
            IRepository<AnnouncementUser, int> announcementUserRepository,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _announcementUserRepository = announcementUserRepository;
            this._announcementRepository = announcementRepository;
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //public PagedResult<AnnouncementViewModel> GetAll(Guid userId, int pageIndex, int pageSize)
        public PagedResult<AnnouncementViewModel> GetAll(Guid userId, int pageIndex, int pageSize)
        {
            var query = from first in _announcementRepository.FindAll()
                        join last in _announcementUserRepository.FindAll()
                        on first.Id equals last.AnnouncementId into temp
                        from last in temp.DefaultIfEmpty()
                        where last.UserId == userId
                        select new AnnouncementViewModel
                        {
                            Id = first.Id,
                            Title = first.Title,
                            DateCreated = first.DateCreated,
                            UserId = first.UserId,
                            Content = first.Content,
                            FullName = first.AppUser.FullName,
                            //first.AnnouncementUsers,
                            Status = first.Status
                        };

            int totalRow = query.Count();

            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            var paginationSet = new PagedResult<AnnouncementViewModel>
            {
                Results = data,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize)
        {
            var query = from x in _announcementRepository.FindAll()
                        join y in _announcementUserRepository.FindAll()
                        on x.Id equals y.AnnouncementId into xy // left join
                        from annonUser in xy.DefaultIfEmpty()
                        where annonUser.HasRead == false && (annonUser.UserId == null || annonUser.UserId == userId)
                        select x;
            int totalRow = query.Count();

            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            var model = _mapper.ProjectTo<AnnouncementViewModel>(data).ToList();

            var paginationSet = new PagedResult<AnnouncementViewModel>
            {
                Results = model,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public bool MarkAsRead(Guid userId, string id) // đã đọc hay chưa
        {
            bool result = false;
            var announUser = _announcementUserRepository.FindSingle(x => x.AnnouncementId == id
                                                                               && x.UserId == userId);
            var announ = _announcementRepository.FindSingle(y => y.Id == id && y.UserId == userId);
            if (announUser == null)
            {
                _announcementUserRepository.Add(new AnnouncementUser
                {
                    AnnouncementId = id,
                    UserId = userId,
                    HasRead = true
                });
                result = true;
            }
            else
            {
                if (announUser.HasRead == false && announ.Status == Status.InActive)
                {
                    announUser.HasRead = true;
                    announ.Status = Status.Active;
                    _unitOfWork.Commit();
                    result = true;
                }

            }
            return result;
        }

        public bool MarkAsReadAll(Guid userId)
        {
            bool result = false;
            var announUser = _announcementUserRepository.FindAll(x => x.UserId == userId && x.HasRead == false);
            var announ = _announcementRepository.FindAll(x => x.UserId == userId && x.Status == Status.InActive);
            if (announ.Count() > 0 && announUser.Count() > 0)
            {
                announUser.ToList().ForEach(x => x.HasRead = true);
                announ.ToList().ForEach(y => y.Status = Status.Active);
                SaveChanges();
                result = true;

            }
            return result;

        }

        public bool Delete(Guid userId, string id)
        {
            bool result = false;
            var announ = _announcementRepository.FindSingle(y => y.Id == id && y.UserId == userId);
            var announUser = _announcementUserRepository.FindSingle(x => x.AnnouncementId == id
                                                                         && x.UserId == userId);
            if (announ.ToString() != null && announUser.ToString() != null)
            {
                _announcementRepository.RemoveById(id);
                _announcementUserRepository.Remove(announUser);
                SaveChanges();
                result = true;

            }
            return result;

        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public bool DeleteAll(Guid userId, string key)
        {
            bool result = false;
            if (key == "Seen")
            {
                var announUser = _announcementUserRepository.FindAll(x => x.UserId == userId && x.HasRead == true).ToList();
                var announ = _announcementRepository.FindAll(x => x.UserId == userId && x.Status == Status.Active).ToList();
                if (announ.Count > 0 && announUser.Count > 0)
                {
                    _announcementRepository.RemoveMultiple(announ);
                    _announcementUserRepository.RemoveMultiple(announUser);
                    result = true;
                }

            }
            else if (key == "UnRead")
            {
                var announUser = _announcementUserRepository.FindAll(x => x.UserId == userId && x.HasRead == false).ToList();
                var announ = _announcementRepository.FindAll(x => x.UserId == userId && x.Status == Status.InActive).ToList();
                if (announ.Count > 0 && announUser.Count > 0)
                {
                    _announcementRepository.RemoveMultiple(announ);
                    _announcementUserRepository.RemoveMultiple(announUser);

                }
                result = true;
            }
            else if (key == " ")
            {
                var announUser = _announcementUserRepository.FindAll(x => x.UserId == userId).ToList();
                var announ = _announcementRepository.FindAll(x => x.UserId == userId).ToList();
                if (announ.Count > 0 && announUser.Count > 0)
                {
                    _announcementRepository.RemoveMultiple(announ);
                    _announcementUserRepository.RemoveMultiple(announUser);

                }
                result = true;
            }
            else
            {
                result = false;
            }
            SaveChanges();
            return result;

        }
    }
}
