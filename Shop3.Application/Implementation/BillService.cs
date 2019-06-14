using AutoMapper;
using AutoMapper.QueryableExtensions;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Infrastructure.Interfaces;
using Shop3.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Shop3.Application.Implementation
{
    public class BillService : IBillService
    {
        private readonly IRepository<Bill, int> _orderRepository;
        private readonly IRepository<BillDetail, int> _orderDetailRepository;
        private readonly IRepository<Color, int> _colorRepository;
        private readonly IRepository<Size, int> _sizeRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public BillService(IRepository<Bill, int> orderRepository,
            IRepository<BillDetail, int> orderDetailRepository,
            IRepository<Color, int> colorRepository,
            IRepository<Product, int> productRepository,
            IRepository<Size, int> sizeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Create(BillViewModel billVm)
        {
           // billVm.DateCreated = billVm.DateModified = DateTime.Now;
            var order = _mapper.Map<BillViewModel, Bill>(billVm);
            var orderDetails = _mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
            foreach (var detail in orderDetails)
            {
                // lấy ra price mới nhất của product
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
            }
            order.BillDetails = orderDetails;
            _orderRepository.Add(order);
        }

        public void Update(BillViewModel billVm)
        {
            
            //Mapping to order domain
            var order = _mapper.Map<BillViewModel, Bill>(billVm);

            //Get order Detail
            var newDetails = order.BillDetails;

            //new details added , id = 0 => được thêm mới
            var addedDetails = newDetails.Where(x => x.Id == 0).ToList();

            //get updated details , id khac 0 => sửa
            var updatedDetails = newDetails.Where(x => x.Id != 0).ToList(); // tolist để clone ra biến updatedDetails, where là chưa lấy ra

            //Existed details : get bill tồn tại trong db
            var existedDetails = _orderDetailRepository.FindAll(x => x.BillId == billVm.Id);

            //Clear db
            order.BillDetails.Clear(); // clear sau khi clone đươc thông tin ra các biến

            foreach (var detail in updatedDetails)
            {  // lấy ra price mới nhất của product
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                //_orderDetailRepository.Update(detail);
                _orderDetailRepository.Update(detail.Id,detail);
            }

            foreach (var detail in addedDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _orderDetailRepository.Add(detail);
            }

            _orderDetailRepository.RemoveMultiple(existedDetails.Except(updatedDetails).ToList()); // xóa bill không tồn tại trừ bill được update

           // _orderRepository.Update(order);
            _orderRepository.Update(order.Id,order);
        }

        public void UpdateStatus(int billId, BillStatus status)
        {
            var order = _orderRepository.FindById(billId);
            order.BillStatus = status;
            //_orderRepository.Update(order);
            _orderRepository.Update(order.Id,order);
        }

        public List<SizeViewModel> GetSizes()
        {
            var  data =  _sizeRepository.FindAll();
           return _mapper.ProjectTo<SizeViewModel>(data).ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        

        public PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword
            , int pageIndex, int pageSize)
        {
            var query = _orderRepository.FindAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated >= start);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated <= end);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.CustomerName.Contains(keyword) || x.CustomerMobile.Contains(keyword));
            }
            // phân trang và thực thi query
            var totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
            
            return new PagedResult<BillViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Results = _mapper.ProjectTo<BillViewModel>(data).ToList(),
                RowCount = totalRow
            };
        }

        public BillViewModel GetDetail(int billId)
        {
            var bill = _orderRepository.FindSingle(x => x.Id == billId);
            var billVm = _mapper.Map<Bill, BillViewModel>(bill);
            var billDetailVm = _mapper.ProjectTo<BillDetailViewModel>(_orderDetailRepository.FindAll(x => x.BillId == billId)).ToList();
            billVm.BillDetails = billDetailVm;
            return billVm;
        }

        public List<BillDetailViewModel> GetBillDetails(int billId)
        {
            var  data = _orderDetailRepository
                .FindAll(x => x.BillId == billId, c => c.Bill, c => c.Color, c => c.Size, c => c.Product);
            return _mapper.ProjectTo<BillDetailViewModel>(data).ToList();

        }

        public List<ColorViewModel> GetColors()
        {
            return _mapper.ProjectTo<ColorViewModel>(_colorRepository.FindAll()).ToList();
        }

        public BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm)
        {
            var billDetail = _mapper.Map<BillDetailViewModel, BillDetail>(billDetailVm);
            _orderDetailRepository.Add(billDetail);
            return billDetailVm;
        }

        public void DeleteDetail(int productId, int billId, int colorId, int sizeId)
        {
            var detail = _orderDetailRepository.FindSingle(x => x.ProductId == productId
           && x.BillId == billId && x.ColorId == colorId && x.SizeId == sizeId);
            _orderDetailRepository.Remove(detail);
        }

        public ColorViewModel GetColor(int id)
        {
            return _mapper.Map<Color, ColorViewModel>(_colorRepository.FindById(id));
        }

        public SizeViewModel GetSize(int id)
        {
            return _mapper.Map<Size, SizeViewModel>(_sizeRepository.FindById(id));
        }

        public PagedResult<BillDetailViewModel> GetAllPagingByCustomerId(Guid id, int page, int pageSize)
        {
        
             var query = from bd in _orderDetailRepository.FindAll()
                    join b in _orderRepository.FindAll()
                    on bd.BillId equals b.Id
                    where b.CustomerId == id
                    orderby b.DateCreated descending
                    select bd;
       
            var totalRow = query.Count();
            var data = query.Skip((page - 1) * pageSize).Take(pageSize);

          return new PagedResult<BillDetailViewModel>()
           {
            CurrentPage = page,
            PageSize = pageSize,
            Results = _mapper.ProjectTo<BillDetailViewModel>(data).ToList(),
            RowCount = totalRow
           };
         
        }
        public BillViewModel GetBillByIdAndUserId(Guid id, int billId)
        {
            throw new NotImplementedException();
        }

        public bool ReOderByUser(Guid id, int biiId)
        {
            throw new NotImplementedException();
        }
    }
}
