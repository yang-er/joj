/*!
  * Powered by 小羊实验室
  */

function getSchoolDetail(a) {
    htmlobj=jQuery.ajax({url:"ajax.php?school="+a,async:false});
    jQuery("#schoolDetail").html(htmlobj.responseText);
}

function ajaxget(geturl, dom) {
    return $.ajax({
        type: "GET",
        url: geturl + "&inajax=1",
        dataType: "html",
        complete: function (jqXHR) {
            if (jqXHR.status == 200) {
                $(dom).html(jqXHR.responseText);
            }
        },
        error: function (jqXHR) {
            notice("请联系管理员。" + jqXHR.status + ' ' + jqXHR.statusText + "<br><pre>" + jqXHR.responseText, "danger", "内部错误");
            $(dom).html('<span color="red">内部错误，无法显示</span>');
        },
        beforeSend: function (jqXHR) {
            $(dom).html('<span class="octicons-sync"></span>加载中');
        }
    });
}

function showWindow(handlekey, geturl) {
    return $.ajax({
        type: "GET",
        url: geturl + "&handlekey=" + handlekey + "&inajax=1",
        dataType: "html",
        complete: function (jqXHR) {
            if (jqXHR.status == 200) {
                $("#append-parent").append('<div id="ajax_result_' + handlekey + '">' + jqXHR.responseText + '</div>');
                $("#modal-" + handlekey).on(
                    "hidden.bs.modal",
                    function (e) {
                        $('#ajax_result_' + handlekey).remove()
                    }
                ).modal("show");
            }
        },
        error: function (jqXHR) {
            notice("请联系管理员。" + jqXHR.status + ' ' + jqXHR.statusText + "<br><pre>" + jqXHR.responseText, "danger", "内部错误");
        }
    });
}

function notice(text, type, header) {
    type = type || "info";
    if (header) {
        $("#notification-box").append('<div class="alert alert-' + type + ' new-add fade" role="alert"><h4 class="alert-heading">' + header + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></h4><p class="mb-0">' + text + '</p></div>');
    } else {
        $("#notification-box").append('<div class="alert alert-' + type + ' new-add fade" role="alert">' + text + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div>');
    }
    $("#notification-box .new-add").removeClass("new-add").addClass("show");
}

function ajaxpost(Form, handlekey, parent, arg2) {
    var form = new FormData(Form);
    form.append("inajax", "1");
    form.append("handlekey", handlekey);
    $.ajax({
        url: $(Form).prop("action"),
        type: "post",
        data: form,
        processData: false,
        contentType: false,
        success: function (data) {
            parent && $("#modal-" + parent).modal("hide");
            $("#append-parent").append('<div id="ajax_result_' + handlekey + '">' + data + '</div>');
            $("#modal-" + handlekey).on(
                "hidden.bs.modal",
                function (e) {
                    $('#ajax_result_' + handlekey).remove()
                }
            ).modal("show");
        },
        error: function (jqXHR) {
            notice("请联系管理员。" + jqXHR.status + ' ' + jqXHR.statusText + "<br><pre>" + jqXHR.responseText, "danger", "内部错误");
        }
    });
    return false;
}

$(function () {
    $('[data-toggle="ajaxWindow"]').on("click", function () {
        showWindow($(this).data("target"), $(this).prop("href"));
        return false;
    });
    $('.extendable-area').on("dblclick", function () {
        $(this).toggleClass('extended');
    });
});
