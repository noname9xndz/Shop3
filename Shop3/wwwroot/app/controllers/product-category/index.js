

var productCategoryController = function () {

    this.initialize = function () {
        loadData();
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
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                //var $tree = $('#treeProductCategory');
                // sử dụng easyui hiển thị tree
                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true
                });

            }
        });
    }
}