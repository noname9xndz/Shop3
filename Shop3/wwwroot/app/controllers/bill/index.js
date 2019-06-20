var BillController = function () {
    // object chứa mảng các dữ liệu
    var cachedObj = {
        products: [],
        colors: [],
        sizes: [],
        paymentMethods: [],
        billStatuses: []
    }
    this.initialize = function () {
        // when đảm bảo các đối tượng bên trong chạy xong sau đó mới chạy tiếp trong done
        $.when(loadBillStatus(),
            loadPaymentMethod(),
            loadColors(),
            loadSizes(),
            loadProducts())
            .done(function () {
                loadData();
            });

        registerEvents();
    }

    function registerEvents() {
        // dùng datepicker để hiển thị
        $('#txtFromDate, #txtToDate').datepicker({
            autoclose: true,
            format: 'dd/mm/yyyy'
        });
        //Init validation form
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtCustomerName: { required: true },
                txtCustomerAddress: { required: true },
                txtCustomerMobile: { required: true },
                txtCustomerMessage: { required: true },
                ddlBillStatus: { required: true }
            }
        });

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });

        $("#btn-search").on('click', function () {
            loadData();
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-detail').modal('show');
        });

        $("#ddl-show-page").on('change', function () {
            common.configs.pageSize = $(this).val();
            common.configs.pageIndex = 1;
            loadData(true);
        });

        $('body').on('click', '.btn-view', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Bill/GetById",
                data: { id: that },
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    var data = response;
                    //bindding dữ liệu ra form
                    $('#hidId').val(data.Id);
                    $('#txtCustomerName').val(data.CustomerName);

                    $('#txtCustomerAddress').val(data.CustomerAddress);
                    $('#txtCustomerMobile').val(data.CustomerMobile);
                    $('#txtCustomerMessage').val(data.CustomerMessage);
                    $('#ddlPaymentMethod').val(data.PaymentMethod);
                    $('#ddlCustomerId').val(data.CustomerId);
                    $('#ddlBillStatus').val(data.BillStatus);

                    var billDetails = data.BillDetails;
                    if (data.BillDetails != null && data.BillDetails.length > 0) {
                        var render = '';
                        var templateDetails = $('#template-table-bill-details').html();
                        
                        $.each(billDetails, function (i, item) {
                            var products = getProductOptions(item.ProductId);
                            var colors = getColorOptions(item.ColorId);
                            var sizes = getSizeOptions(item.SizeId);

                            render += Mustache.render(templateDetails,
                                {
                                    Id: item.Id,
                                    Products: products,
                                    Colors: colors,
                                    Sizes: sizes,
                                    Quantity: item.Quantity
                                });
                        });
                        $('#tbl-bill-details').html(render);
                    }
                    $('#modal-detail').modal('show');
                    common.stopLoading();

                },
                error: function (e) {
                    common.notify('Has an error in progress', 'error');
                    common.stopLoading();
                }
            });
        });
        
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var customerName = $('#txtCustomerName').val();
                var customerAddress = $('#txtCustomerAddress').val();
                var customerId = $('#ddlCustomerId').val();
                var customerMobile = $('#txtCustomerMobile').val();
                var customerMessage = $('#txtCustomerMessage').val();
                var paymentMethod = $('#ddlPaymentMethod').val();
                var billStatus = $('#ddlBillStatus').val();
               //var dateCreated = new Date().toLocaleString();

                //bill detail : get product in bill detail 
                var billDetails = [];
                $.each($('#tbl-bill-details tr'), function (i, item) {
                    billDetails.push({
                        Id: $(item).data('id'),
                        ProductId: $(item).find('select.ddlProductId').first().val(),
                        Quantity: $(item).find('input.txtQuantity').first().val(),
                        ColorId: $(item).find('select.ddlColorId').first().val(),
                        SizeId: $(item).find('select.ddlSizeId').first().val(),
                        BillId: id
                    });
                });

                $.ajax({
                    type: "POST",
                    url: "/Admin/Bill/SaveEntity",
                    data: {
                        Id: id,
                        BillStatus: billStatus,
                        CustomerAddress: customerAddress,
                        CustomerId: customerId,
                        CustomerMessage: customerMessage,
                        CustomerMobile: customerMobile,
                        CustomerName: customerName,
                        PaymentMethod: paymentMethod,
                        Status: 1,
                        BillDetails: billDetails
                        
                    },
                    dataType: "json",
                    beforeSend: function () {
                        common.startLoading();
                    },
                    success: function (response) {
                        common.notify('Save order successful', 'success');
                        $('#modal-detail').modal('hide');
                        resetFormMaintainance();

                        common.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        common.notify('Has an error in progress', 'error');
                        common.stopLoading();
                    }
                });
                return false;
            }

        });
        
        $('#btnConfirm').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var customerName = $('#txtCustomerName').val();
                var customerAddress = $('#txtCustomerAddress').val();
                var customerId = $('#ddlCustomerId').val();
                var customerMobile = $('#txtCustomerMobile').val();
                var customerMessage = $('#txtCustomerMessage').val();
                var paymentMethod = $('#ddlPaymentMethod').val();
                billStatus = 4;// bill Completed
                //bill detail
                var billDetails = [];
                $.each($('#tbl-bill-details tr'), function (i, item) {
                    billDetails.push({
                        Id: $(item).data('id'),
                        ProductId: $(item).find('select.ddlProductId').first().val(),
                        Quantity: $(item).find('input.txtQuantity').first().val(),
                        ColorId: $(item).find('select.ddlColorId').first().val(),
                        SizeId: $(item).find('select.ddlSizeId').first().val(),
                        BillId: id
                    });
                });

                BillCompleted(id, billStatus, customerAddress, customerId, customerMessage, customerMobile, customerName, paymentMethod, billDetails);
                return false;
            }

        });

        $('#btnPending').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var customerName = $('#txtCustomerName').val();
                var customerAddress = $('#txtCustomerAddress').val();
                var customerId = $('#ddlCustomerId').val();
                var customerMobile = $('#txtCustomerMobile').val();
                var customerMessage = $('#txtCustomerMessage').val();
                var paymentMethod = $('#ddlPaymentMethod').val();
                billStatus = 1; // bill pending
                //bill detail billStatus
                var billDetails = [];
                $.each($('#tbl-bill-details tr'), function (i, item) {
                    billDetails.push({
                        Id: $(item).data('id'),
                        ProductId: $(item).find('select.ddlProductId').first().val(),
                        Quantity: $(item).find('input.txtQuantity').first().val(),
                        ColorId: $(item).find('select.ddlColorId').first().val(),
                        SizeId: $(item).find('select.ddlSizeId').first().val(),
                        BillId: id
                    });
                });

                BillPending(id, billStatus, customerAddress, customerId, customerMessage, customerMobile, customerName, paymentMethod, billDetails);
                return false;
            }

        });

        $('#btnCancel').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var customerName = $('#txtCustomerName').val();
                var customerAddress = $('#txtCustomerAddress').val();
                var customerId = $('#ddlCustomerId').val();
                var customerMobile = $('#txtCustomerMobile').val();
                var customerMessage = $('#txtCustomerMessage').val();
                var paymentMethod = $('#ddlPaymentMethod').val();
                billStatus = 3; // bill Cancelled
                //bill detail billStatus
                var billDetails = [];
                $.each($('#tbl-bill-details tr'), function (i, item) {
                        billDetails.push({
                        Id: $(item).data('id'),
                        ProductId: $(item).find('select.ddlProductId').first().val(),
                        Quantity: $(item).find('input.txtQuantity').first().val(),
                        ColorId: $(item).find('select.ddlColorId').first().val(),
                        SizeId: $(item).find('select.ddlSizeId').first().val(),
                        BillId: id
                    });
                });

                BillCancelled(id, billStatus, customerAddress, customerId, customerMessage, customerMobile, customerName, paymentMethod, billDetails);
                return false;
            }

        });

        $('#btnAddDetail').on('click', function () {
            // thêm mới sp trong bill
            var template = $('#template-table-bill-details').html();
            var products = getProductOptions(null);
            var colors = getColorOptions(null);
            var sizes = getSizeOptions(null);
            var render = Mustache.render(template,
                {
                    Id: 0,
                    Products: products,
                    Colors: colors,
                    Sizes: sizes,
                    Quantity: 0,
                    Total: 0
                });
            $('#tbl-bill-details').append(render);
        });

        $('body').on('click', '.btn-delete-detail', function () {
            $(this).parent().parent().remove(); // lấy thằng ông remove thằng cháu
        });

        $("#btnExport").on('click', function () {
            var that = $('#hidId').val();
            $.ajax({
                type: "POST",
                url: "/Admin/Bill/ExportExcel",
                data: { billId: that },
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    window.location.href = response;

                    common.stopLoading();

                }
            });
        });

    };

    function loadBillStatus() {
        return $.ajax({
            type: "GET",
            url: "/admin/bill/GetBillStatus",
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
            url: "/admin/bill/GetPaymentMethod",
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

    function loadProducts() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Product/GetAll",
            dataType: "json",
            success: function (response) {
                cachedObj.products = response;
            },
            error: function () {
                common.notify('Has an error in progress', 'error');
            }
        });
    }

    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                common.notify('Has an error in progress', 'error');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                common.notify('Has an error in progress', 'error');
            }
        });
    }

    function getProductOptions(selectedId) {

        var products = "<select class='form-control ddlProductId'>";
        $.each(cachedObj.products, function (i, product) {
            if (selectedId === product.Id)
                products += '<option value="' + product.Id + '" selected="select">' + product.Name + '</option>';
            else
                products += '<option value="' + product.Id + '">' + product.Name + '</option>';
        });
        products += "</select>";
        return products;
    }

    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddlColorId'>";
        $.each(cachedObj.colors, function (i, color) {
            if (selectedId === color.Id)
                colors += '<option value="' + color.Id + '" selected="select">' + color.Name + '</option>';
            else
                colors += '<option value="' + color.Id + '">' + color.Name + '</option>';
        });
        colors += "</select>";
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = "<select class='form-control ddlSizeId'>";
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Name + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }

    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtCustomerName').val('');

        $('#txtCustomerAddress').val('');
        $('#txtCustomerMobile').val('');
        $('#txtCustomerMessage').val('');
        $('#ddlPaymentMethod').val('');
        $('#ddlCustomerId').val('');
        $('#ddlBillStatus').val('');
        $('#tbl-bill-details').html('');
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/bill/GetAllPaging",
            data: {
                startDate: $('#txtFromDate').val(),
                endDate: $('#txtToDate').val(),
                keyword: $('#txtSearchKeyword').val(),
                page: common.configs.pageIndex,
                pageSize: common.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {

                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            CustomerName: item.CustomerName,
                            Id: item.Id,
                            PaymentMethod: getPaymentMethodName(item.PaymentMethod),
                            DateCreated: common.dateTimeFormatJson(item.DateCreated),
                            BillStatus: getBillStatusName(item.BillStatus)
                        });
                    });

                    //$("#lbl-total-records").text(response.RowCount);
                    $("#lbl-total-records").text('0');
                    if (render != undefined) {
                        $('#tbl-content').html(render);

                    }
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);


                }
                else {
                    $("#lbl-total-records").text('0');
                    $('#tbl-content').html('');
                }
                common.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

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

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / common.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
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

    function BillCompleted(id, billStatus, customerAddress, customerId, customerMessage, customerMobile, customerName, paymentMethod, billDetails) {
        $.ajax({
            type: "POST",
            url: "/Admin/Bill/SaveEntity",
            data: {
                Id: id,
                BillStatus: billStatus,
                CustomerAddress: customerAddress,
                CustomerId: customerId,
                CustomerMessage: customerMessage,
                CustomerMobile: customerMobile,
                CustomerName: customerName,
                PaymentMethod: paymentMethod,
                Status: 1,
                BillDetails: billDetails
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                common.notify('Save order successful', 'success');
                $('#modal-detail').modal('hide');
                resetFormMaintainance();

                common.stopLoading();
                loadData(true);
            },
            error: function () {
                common.notify('Has an error in progress', 'error');
                common.stopLoading();
            }
        });
    }

    function BillPending(id, billStatus, customerAddress, customerId, customerMessage, customerMobile, customerName, paymentMethod, billDetails) {
        $.ajax({
            type: "POST",
            url: "/Admin/Bill/SaveEntity",
            data: {
                Id: id,
                BillStatus: billStatus,
                CustomerAddress: customerAddress,
                CustomerId: customerId,
                CustomerMessage: customerMessage,
                CustomerMobile: customerMobile,
                CustomerName: customerName,
                PaymentMethod: paymentMethod,
                Status: 1,
                BillDetails: billDetails
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                common.notify('Save order successful', 'success');
                $('#modal-detail').modal('hide');
                resetFormMaintainance();

                common.stopLoading();
                loadData(true);
            },
            error: function () {
                common.notify('Has an error in progress', 'error');
                common.stopLoading();
            }
        });
    }
    
    function BillCancelled(id, billStatus, customerAddress, customerId, customerMessage, customerMobile, customerName, paymentMethod, billDetails) {
        $.ajax({
            type: "POST",
            url: "/Admin/Bill/SaveEntity",
            data: {
                Id: id,
                BillStatus: billStatus,
                CustomerAddress: customerAddress,
                CustomerId: customerId,
                CustomerMessage: customerMessage,
                CustomerMobile: customerMobile,
                CustomerName: customerName,
                PaymentMethod: paymentMethod,
                Status: 1,
                BillDetails: billDetails
            },
            dataType: "json",
            beforeSend: function () {
                common.startLoading();
            },
            success: function (response) {
                common.notify('Save order successful', 'success');
                $('#modal-detail').modal('hide');
                resetFormMaintainance();

                common.stopLoading();
                loadData(true);
            },
            error: function () {
                common.notify('Has an error in progress', 'error');
                common.stopLoading();
            }
        });
    }

}