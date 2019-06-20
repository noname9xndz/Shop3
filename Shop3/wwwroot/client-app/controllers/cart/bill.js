



var billController = function () {
    var cachedObj = {

        paymentMethods: [],
        billStatuses: []
    }

    this.initialize = function () {
        $.when(loadBillStatus(), loadPaymentMethod())
            .done(function ()
            {
                loadBill();
               
            });
        registerEvents();
    }

    function registerEvents() {
        $('#ddlShowPage').on('change',
            function() {
                common.configs.pageSize = $(this).val();
                common.configs.pageIndex = 1;
                loadBill(true);
             

            });


        $('body').on('click', '#reorder', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            checkStatus(id);
             
           


        });
        $('body').on('click', '#vieworder', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
             $('#vieworders').modal('show');
            
            

        });


        $('body').on('click', '.sendmessage', function (e) {
            e.preventDefault();
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var messagereoder = $('#messagereoder').val();
                var id = $('#hidId').val();
                reorder(id, messagereoder);

            }

        });

    }
   
    function loadBill(isPageChanged)  {

            var template = $('#table-template').html();
            var render = "";

            $.ajax({
                type: 'POST',
                data: {
                    page: common.configs.pageIndex,
                    pageSize: common.configs.pageSize
                },
                url: '/Cart/GetAllBillPagingByUserId',
                dataType: 'json',
                success: function (response) {
                    $.each(response.Results, function (i, item) { 
                        render += Mustache.render(template, {
                            CustomerName: item.CustomerName,
                            Id: item.Id,
                            PaymentMethod: getPaymentMethodName(item.PaymentMethod),
                            DateCreated: common.dateTimeFormatJson(item.DateCreated),
                            BillStatus: getBillStatusName(item.BillStatus)
                        });
                    });

                    $('#lblTotalRecords').text(response.RowCount); // tổng số bản ghi
                    if (render != '') {
                        $("#table-bill").html(render);
                    }

                    wrapPaging(response.RowCount, function () {
                        loadBill();
                    }, isPageChanged);


                },
                error: function (status) {
                    console.log(status);
                    common.notify('Cannot loading data', 'error');
                }
            });
    }

    function wrapPaging(recordCount, callBack, changePageSize) { // changePageSize : load ra phân trang hay đổi trang
        var totalsize = Math.ceil(recordCount / common.configs.pageSize);
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        if (recordCount > 10) {
            $('#paginationUL').twbsPagination({
                totalPages: totalsize,
                visiblePages: 7, //max page hiển thị
                first: 'Đầu',
                prev: 'Trước',
                next: 'Tiếp',
                last: 'Cuối',
                onPageClick: function (event, p) {
                    common.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            });
        }
        
    }

    function getPaymentMethodName(paymentMethod) {
        var method = $.grep(cachedObj.paymentMethods, function (element, index) {
            return element.Value == paymentMethod;
        });
        if (method.length > 0)
            return method[0].Name;
        else return '';
    }

    function getBillStatusName(status) {

        var status = $.grep(cachedObj.billStatuses, function (element, index) {
            return element.Value == status;
        });
        if (status.length > 0)
            return status[0].Name;
        else
            return '';
    }

    function loadBillStatus() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetBillStatus",
            dataType: "json",
            success: function (response) {
                cachedObj.billStatuses = response;
                var render = "";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Value + "'>" + item.Name + "</option>";
                });
                $('#ddlBillStatus').html(render);
            }
        });
    }

    function loadPaymentMethod() {
        return $.ajax({
            type: "GET",
            url: "/Cart/GetPaymentMethod",
            dataType: "json",
            success: function (response) {
                cachedObj.paymentMethods = response;
                var render = "";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Value + "'>" + item.Name + "</option>";
                });
                $('#ddlPaymentMethod').html(render);
            }
        });
    }

    function getBillById(id) {
        $.ajax({
            type: "GET",
            url: "/Cart/GetBillById",
                data: { id: id },
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    //$('#hidId').val(response.Id);
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtCustomerName').val(data.CustomerName);

                    $('#txtCustomerAddress').val(data.CustomerAddress);
                    $('#txtCustomerMobile').val(data.CustomerMobile);
                    $('#txtCustomerMessage').val(data.CustomerMessage);
                    $('#ddlPaymentMethod').val(data.PaymentMethod);
                    $('#ddlCustomerId').val(data.CustomerId);
                    $('#ddlBillStatus').val(data.BillStatus);
                    $('#messagereoder').val(data.ReOrderMesssage);
                    $('#reorders').modal('show');
                    common.stopLoading();

                },
                error: function (e) {
                    common.notify('Has an error in progress', 'error');
                    common.stopLoading();
                }
            });
       
    }

    function reorder(id,mes) {
        $.ajax({
            type: "POST",
            url: "/Cart/ReOrder",
            data: {
                Id: id,
                reOrderMesssage: mes
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {

                common.notify('Save order successful', 'success');
                $('#reorders').modal('hide');
                resetFormMaintainance();
                common.stopLoading();
                loadBill(true);
            },
            error: function () {
                common.notify('Has an error in progress', 'error');
                common.stopLoading();
            }
        });
    }
    function checkStatus(id) {
        $.ajax({
            type: "GET",
            url: "/Cart/CheckStatus",
            data: {
                Id: id
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                getBillById(id);
                resetFormMaintainance();
            },
            error: function () {
                $('#vieworders').modal('hide');
                common.stopLoading();
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#messagereoder').val('');
    }
    
}