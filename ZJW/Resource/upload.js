
    $(function () {
        $('#file_upload').uploadifive({
            'auto': false,
            'fileSizeLimit': '10000KB',
            'buttonText': '浏  览',
            'multi': true,
            'queueID': 'queue',
            'uploadScript': '/File/Upload',
            'formData': { 'ID': $('#item_ID').val() },
            'onUploadComplete': function (file, data) { refeshGrid(); }
        });
        $('#grid_table').datagrid({
            url: '/File/GetList?ID='+$('#item_ID').val(),
            sortName: 'ID',
            idField: 'ID',

            fit: false,
            fitColumns: true,
            frozenColumns: [[
            { field: 'ck', checkbox: true },
            {
                field: 'opt', title: '操作', width: 150, align: 'center',
                formatter: function (value, r) {
                    var optBtn = '';
                    if (r.isSys != '1') {
                        optBtn = ' <a class="button blue small" style="color: white;text-decoration: none;" href="/File/Download?ID=' + r.ID + '" title=\"下载\">下载</a> ';
                        optBtn += ' <a class="button blue small" style="color: white;text-decoration: none;" href="javascript:delFile(\'' + r.ID + '\')" title=\"删除\">删除</a> ';
                    }
                    return optBtn;
                }
            }
            ]],
            columns: [[
            { field: 'FileName', title: '文件名', width: 2000, sortable: true, align: 'center' }

            ]]


        });
    });
function checkQueue() {
    if ($("#queue").children().length == 0) {
        $.messager.alert("操作提示", "请先点击浏览选择要上传的文件！", "info");
    } else {
        $('#file_upload').uploadifive('upload');
    }
}
function checkCancel() {
    if ($("#queue").children().length == 0) {
        $.messager.alert("操作提示", "没有待上传的文件！", "info");
    } else {
        $('#file_upload').uploadifive('clearQueue');
    }
}


function delFile(ID) {
    removeObjWithConfirm(ID, "ID", '/File/Delete', "funAfterDel")
}
function funAfterDel() {
    refeshGrid();
}
function refeshGrid() {
    $('#grid_table').datagrid({});
}
