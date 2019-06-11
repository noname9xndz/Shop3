﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.System;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Data.IRepositories;
using Shop3.Infrastructure.Interfaces;
using Shop3.Utilities.Dtos;
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
        private IRepository<Function, string> _functionRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FunctionService(IMapper mapper,IRepository<Function, string> functionRepository,
                        IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CheckExistedId(string id)
        {
            return _functionRepository.FindById(id) != null;
        }

        public void Add(FunctionViewModel functionVm)
        {
            var function = _mapper.Map<Function>(functionVm);
            
            _functionRepository.Add(function);
        }

        public void Delete(string id)
        {
            _functionRepository.Remove(id);
        }

        public FunctionViewModel GetById(string id)
        {
            var function = _functionRepository.FindSingle(x => x.Id == id);

            return _mapper.Map<Function, FunctionViewModel>(function);
        }

        public Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);

            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));

            return _mapper.ProjectTo <FunctionViewModel>(query.OrderBy(x => x.ParentId)).ToListAsync();
        }

        //public PagedResult<FunctionViewModel> GetAll2(string filter,int pageIndex, int pageSize)
        //{
        //    var query = _functionRepository.FindAll(x => x.Status == Status.Active);

        //    if (!string.IsNullOrEmpty(filter))
        //        query = query.Where(x => x.Name.Contains(filter));

        //    int totalRow = query.Count();
        //    var model = query.OrderBy(x => x.SortOrder).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ProjectTo<FunctionViewModel>().ToList();

        //    var paginationSet = new PagedResult<FunctionViewModel>
        //    {
        //        Results = model,
        //        CurrentPage = pageIndex,
        //        RowCount = totalRow,
        //        PageSize = pageSize
        //    };

        //    return paginationSet;
        //}

        public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
        {
            //return _functionRepository.FindAll(x => x.ParentId == parentId).ProjectTo<FunctionViewModel>();
            var data = _functionRepository.FindAll(x => x.ParentId == parentId);
            return _mapper.ProjectTo<FunctionViewModel>(data);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(FunctionViewModel functionVm)
        {
            var functionDb = _functionRepository.FindById(functionVm.Id);
            var function = _mapper.Map<Function>(functionVm);
        }

        public void ReOrder(string sourceId, string targetId)
        {
            var source = _functionRepository.FindById(sourceId);
            var target = _functionRepository.FindById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            //_functionRepository.Update(source);
            //_functionRepository.Update(target);

            _functionRepository.Update(source.Id,source);
            _functionRepository.Update(target.Id,target);
        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            //Update parent id for source
            var category = _functionRepository.FindById(sourceId);
            category.ParentId = targetId;
            //_functionRepository.Update(category);
            _functionRepository.Update(category.Id,category);

            //Get all sibling
            var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
               // _functionRepository.Update(child);
                _functionRepository.Update(child.Id,child);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
