



var billCompeleteController = function () {
    var cachedObj = {

        paymentMethods: [],
        billStatuses: []
    }

    this.initialize = function () {
        $.when(loadBillStatus(), loadPaymentMethod())
            .done(function () {
                loadBillCompelete();
            });
        registerEvents();
    }

    function registerEvents() {
        $('#ddlShowPage').on('change',
            function () {
                common.configs.pageSize = $(this).val();
                common.configs.pageIndex = 1; 
                loadBillCompelete(true);

            });

    }


    function loadBillCompelete(isPageChanged) {

        var template = $('#table-billcompelete-template').html();
        var render = "";

        $.ajax({
            type: 'POST',
            data: {
                page: common.configs.pageIndex,
                pageSize: common.configs.pageSize
            },
            url: '/Cart/GetAllBillCompeleteByUserId',
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
                    $("#table-billcompelete").html(render);
                }

                wrapPaging(response.RowCount, function () {
                    loadBillCompelete();
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



}