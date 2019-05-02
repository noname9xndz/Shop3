using System;
using System.Collections.Generic;
using System.Text;
using Shop3.Application.ViewModels.System;
using Shop3.Utilities.Dtos;

namespace Shop3.Application.Interfaces
{
    public interface IAnnouncementService
    {
        PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize);

        bool MarkAsRead(Guid userId, string id); // đã đọc hay chưa
    }
}
