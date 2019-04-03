

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
                treeArr.sort(function (a, b) { // sắp xếp vị trí theo order
                    return a.sortOrder - b.sortOrder;
                });
                //var $tree = $('#treeProductCategory');
                // sử dụng easyui hiển thị tree
                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onDrop: function (target, source, point) { // phẩn tử đích ,nguồn và vị trí
                        //console.log(target);
                        //console.log(source);
                        //console.log(point);
                        var targetNode = $(this).tree('getNode', target); // lấy id của target

                        // thả phần tử vào node khác
                        if (point === 'append') { 
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
}