

var productCategoryController = function () {

    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {

        // validate form
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtNameM: { required: true },
                txtOrderM: { number: true },
                txtHomeOrderM: { number: true }
            }
        });

        // bật modal
        $('#btnCreate').off('click').on('click', function () {
            initTreeDropDownCategory();
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val(); // lấy id của node sau khi loaddata từ  _AddEditModal
            $.ajax({
                type: "GET",
                url: "/Admin/ProductCategory/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    common.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);

                    initTreeDropDownCategory(data.CategoryId);  // xử lý combotree bên view

                    $('#txtDescM').val(data.Description);

                    $('#txtImageM').val(data.Image);

                    $('#txtSeoKeywordM').val(data.SeoKeywords);
                    $('#txtSeoDescriptionM').val(data.SeoDescription);
                    $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                    $('#txtSeoAliasM').val(data.SeoAlias);

                    $('#ckStatusM').prop('checked', data.Status == 1);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);
                    $('#txtOrderM').val(data.SortOrder);
                    $('#txtHomeOrderM').val(data.HomeOrder);

                    $('#modal-add-edit').modal('show');

                   

                    common.stopLoading();
                    

                },
                error: function (status) {
                    common.notify('Có lỗi xảy ra', 'error');
                    common.stopLoading();
                }
            });
            resetFormMaintainance();
        });

        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val();
            common.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/ProductCategory/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        common.startLoading();
                    },
                    success: function (response) {
                        if (response.status = "error") {
                            common.notify('No deleteCategory', 'error');
                            common.stopLoading();
                        }
                        else {
                            common.notify('Deleted success', 'success');
                            common.stopLoading();
                            loadData(); 
                        }
                    },
                    error: function (status) {
                        common.notify('Has an error in deleting progress', 'error');
                        common.stopLoading();
                    }
                });
            });
        });

        $('#btnSave').on('click', function (e) {
            // kiểm tra tt sau đó ms save
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = parseInt($('#hidIdM').val()); // parse ra int nếu không nhận được
                var name = $('#txtNameM').val();
                var parentId = $('#ddlCategoryIdM').combotree('getValue');
                var description = $('#txtDescM').val();

                var image = $('#txtImageM').val();
                var order = parseInt($('#txtOrderM').val()); // parse ra int nếu không nhận được
                var homeOrder = $('#txtHomeOrderM').val();

                var seoKeyword = $('#txtSeoKeywordM').val();
                var seoMetaDescription = $('#txtSeoDescriptionM').val();
                var seoPageTitle = $('#txtSeoPageTitleM').val();
                var seoAlias = $('#txtSeoAliasM').val();
                var status = $('#ckStatusM').prop('checked') == true ? 1 : 0;
                var showHome = $('#ckShowHomeM').prop('checked');

                $.ajax({
                    type: "POST",
                    url: "/Admin/ProductCategory/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                        ParentId: parentId,
                        HomeOrder: homeOrder,
                        SortOrder: order,
                        HomeFlag: showHome,
                        Image: image,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyword,
                        SeoDescription: seoMetaDescription
                    },
                    dataType: "json",
                    beforeSend: function () {
                        common.startLoading();
                    },
                    success: function (response) {
                        common.notify('Update success', 'success');
                        $('#modal-add-edit').modal('hide');

                        resetFormMaintainance();

                        common.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        common.notify('Has an error in update progress', 'error');
                        common.stopLoading();
                    }
                });
            }
            return false;

        });

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImageM').click();
        });
        $("#fileInputImageM").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $('#txtImageM').val(path);
                    common.notify('Upload image succesful!', 'success');

                },
                error: function () {
                    common.notify('There was error uploading files!', 'error');
                }
            });
        });


    }




    // load dữ liệu productcategory dạng tree  https://www.jeasyui.com/
    function loadData() {
        $.ajax({
            url: '/Admin/ProductCategory/GetAll',
            dataType: 'json',
            success: function (response) {

                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });

                });

                var treeArr = common.unflattern(data); // tạo tree
                treeArr.sort(function (a, b) { // sắp xếp lai vị trí theo order
                    return a.sortOrder - b.sortOrder;
                });
                //var $tree = $('#treeProductCategory');

                // sử dụng easyui hiển thị tree
                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        //$('#tt').tree('select', node.target);
                        $('#hidIdM').val(node.id); // gán id của node bằng hidIdm trong _AddEditModal
                        // hiển thị context menu
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onDrop: function (target, source, point) { // phẩn tử đích ,nguồn và vị trí
                        //console.log(target);
                        //console.log(source);
                        //console.log(point);
                        var targetNode = $(this).tree('getNode', target); // lấy id của target

                        // thả phần tử vào node khác
                        if (point === 'append' ) {
                            var children = []; // mảng để chứa tất cả các phần tử con của node đó
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });

                            //Update to database
                            $.ajax({
                                url: '/Admin/ProductCategory/UpdateParentId',
                                type: 'post',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function (res) {
                                    loadData();
                                }
                            });
                        }

                        // kéo phẩn tử lên trên hoặc xuống dưới
                        else if (point === 'top' || point === 'bottom') {
                            $.ajax({
                                url: '/Admin/ProductCategory/ReOrder',
                                type: 'post',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function (res) {
                                    loadData();
                                }
                            });
                        }
         
                    }
                });

            }
        });
    }

    // xử lý combo tree bên view
    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = common.unflattern(data);
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });
                if (selectedId != undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }

    // rest thông tin form về null
    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        $('#txtAliasM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');
        $('#txtOrderM').val('');
        $('#txtHomeOrderM').val('');
        $('#txtImageM').val('');


        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');
        $('#txtSeoKeywordM').val('');
        $('#txtSeoDescriptionM').val('');

        $('#ckStatusM').prop('checked', true);
        $('#ckShowHomeM').prop('checked', false);
    }

}