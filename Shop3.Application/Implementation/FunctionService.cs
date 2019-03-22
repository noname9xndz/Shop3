using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.System;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Data.IRepositories;
using Shop3.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop3.Application.Implementation
{
    //public class FunctionService : IFunctionService
    //{
    //    private IFunctionRepository _functionRepository;

    //    public FunctionService(IFunctionRepository functionRepository)
    //    {
    //        _functionRepository = functionRepository;
    //    }

    //    public void Dispose()
    //    {
    //        GC.SuppressFinalize(this);
    //    }

    //    public Task<List<FunctionViewModel>> GetAll()
    //    {
    //        return _functionRepository.FindAll().ProjectTo<FunctionViewModel>().ToListAsync();
    //    }


    //    public List<FunctionViewModel> GetAllByPermisssion(Guid userId)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    public class FunctionService : IFunctionService
    {
        private readonly IRepository<Function, string> _functionRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FunctionService(IMapper mapper,IRepository<Function, string> functionRepository, IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

       
        public Task<List<FunctionViewModel>> GetAll()
        {
            return _functionRepository.FindAll().ProjectTo<FunctionViewModel>().ToListAsync();
        }

       
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
