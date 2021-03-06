﻿using Shop3.Application.ViewModels.Products;
using Shop3.Data.Enums;
using Shop3.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Application.Interfaces
{
    public interface IBillService
    {
        void Create(BillViewModel billVm);

        void Update(BillViewModel billVm);

        PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword,
            int pageIndex, int pageSize);

        BillViewModel GetDetail(int billId);

        BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm);

        void DeleteDetail(int productId, int billId, int colorId, int sizeId);

        void UpdateStatus(int orderId, BillStatus status);

        List<BillDetailViewModel> GetBillDetails(int billId);

        List<ColorViewModel> GetColors();

        List<SizeViewModel> GetSizes();

        ColorViewModel GetColor(int id);

        SizeViewModel GetSize(int id);

        PagedResult<BillDetailViewModel> GetAllPagingByCustomerId(Guid id,int page, int pageSize);

        void Save();

        PagedResult<BillViewModel> GetBillByIdAndUserId(string keyword,Guid id,int page ,int pageSize);


        decimal GetOrderTotal(int billId);

        BillViewModel GetDetailByUser(int billId,Guid id);

        void ReOderByUser(int billId, Guid id,string message);

        bool CheckStatusBillWithUser(int billId,Guid id);


    }
}
