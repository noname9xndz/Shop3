using AutoMapper;
using Shop3.Application.Interfaces;
using Shop3.Application.ViewModels.Products;
using Shop3.Data.Entities;
using Shop3.Data.Enums;
using Shop3.Infrastructure.Interfaces;
using Shop3.Utilities.Constants;
using Shop3.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
            //var x = billVm.OrderTotal; 
            var order = Mapper.Map<BillViewModel, Bill>(billVm);
            var orderDetails = Mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
            foreach (var detail in orderDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                billVm.OrderTotal = (billVm.OrderTotal + (detail.Price * detail.Quantity));
            }

            order.OrderTotal = billVm.OrderTotal;
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
            var updatedDetails = newDetails.Where(x => x.Id != 0).ToList();

            //Existed details : get bill tồn tại trong db
            var existedDetails = _orderDetailRepository.FindAll(x => x.BillId == billVm.Id);

            //Clear db
            order.BillDetails.Clear(); // clear sau khi clone đươc thông tin ra các biến

            foreach (var detail in updatedDetails)
            {  // lấy ra price mới nhất của product
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                billVm.OrderTotal = (billVm.OrderTotal + (detail.Price * detail.Quantity));
                _orderDetailRepository.Update(detail.Id, detail);
            }

            foreach (var detail in addedDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _orderDetailRepository.Add(detail);
                billVm.OrderTotal = (billVm.OrderTotal + (detail.Price * detail.Quantity));
                //_orderDetailRepository.Update(detail.Id,detail);
            }
            // xóa bill không tồn tại trừ bill được update
            // _orderDetailRepository.RemoveMultiple(existedDetails.Except(updatedDetails).ToList()); 

            order.OrderTotal = billVm.OrderTotal;
            if (updatedDetails.Count > 0 && addedDetails.Count > 0)
            {
                order.BillDetails = updatedDetails;
                order.BillDetails = addedDetails;
            }
            else if (updatedDetails.Count() > 0)
            {
                order.BillDetails = updatedDetails;
            }
            else
            {
                order.BillDetails = updatedDetails;

            }

            var orderTotal = billVm.OrderTotal;
            _orderRepository.Update(order.Id, order);
        }

        public void UpdateStatus(int billId, BillStatus status)
        {
            var order = _orderRepository.FindById(billId);
            order.BillStatus = status;
            //_orderRepository.Update(order);
            _orderRepository.Update(order.Id, order);
        }

        public List<SizeViewModel> GetSizes()
        {
            var data = _sizeRepository.FindAll();
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
            var data = _orderDetailRepository
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

        public PagedResult<BillViewModel> GetBillByIdAndUserId(string keyword, Guid id, int page, int pageSize)
        {
            if (keyword == CommonConstants.BillCompeleted)
            {
                var query = _orderRepository.FindAll(x => x.CustomerId == id && x.BillStatus == BillStatus.Completed)
                    .OrderByDescending(x => x.DateCreated);
                var totalRow = query.Count();
                var data = query.Skip((page - 1) * pageSize).Take(pageSize);

                return new PagedResult<BillViewModel>()
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    Results = _mapper.ProjectTo<BillViewModel>(data).ToList(),
                    RowCount = totalRow
                };
            }
            else
            {
                var query = _orderRepository.FindAll(x => x.CustomerId == id && x.BillStatus != BillStatus.Completed).OrderByDescending(x => x.DateCreated);
                var totalRow = query.Count();
                var data = query.Skip((page - 1) * pageSize).Take(pageSize);

                return new PagedResult<BillViewModel>()
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    Results = _mapper.ProjectTo<BillViewModel>(data).ToList(),
                    RowCount = totalRow
                };
            }

        }

        public decimal GetOrderTotal(int billId)
        {
            decimal orderTotal = _orderDetailRepository.FindAll(x => x.BillId == billId).Sum(x => x.Price * x.Quantity);
            return orderTotal;
        }

        public BillViewModel GetDetailByUser(int billId, Guid id)
        {
            var bill = _orderRepository.FindSingle(x => x.Id == billId && x.CustomerId == id);
            var billVm = _mapper.Map<Bill, BillViewModel>(bill);
            var billDetailVm = _mapper.ProjectTo<BillDetailViewModel>(_orderDetailRepository.FindAll(x => x.BillId == billId)).ToList();
            billVm.BillDetails = billDetailVm;
            return billVm;

        }



        public void ReOderByUser(int billId, Guid id, string message)
        {
            var bill = _orderRepository.FindSingle(x => x.Id == billId && x.CustomerId == id);
            bill.ReOrderMesssage = message;
            bill.BillStatus = BillStatus.WattingConfirm;
            bill.Status = Status.InActive;
            var billVm = _mapper.Map<Bill, BillViewModel>(bill);
            _orderRepository.Update(billVm.Id, _mapper.Map<BillViewModel, Bill>(billVm)); ;
        }

        public bool CheckStatusBillWithUser(int billId, Guid id)
        {
            bool check = false;
            if (_orderRepository.FindAll(x => x.CustomerId == id
                                              && x.Id == billId
                                              && x.BillStatus == BillStatus.New
                                              ).Count() > 0)
            {
                check = true;
            }

            return check;
        }
    }
}
