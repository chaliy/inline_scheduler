/*jslint onevar: true, undef: true, newcap: true, regexp: true, plusplus: true, bitwise: true, devel: true, maxerr: 50 */
/*global window: true, jQuery:true, $:true, document:true*/
/// <reference path="vendor/jquery/1.5.1/jquery.js"/>
$(function () {

    // Error
    $(document).ajaxError(function (ev, xhr, settings, errorThrown) {
        alert(xhr.responseText);
    });

    var mainPanel = $("#main");
    var body = $("body");
    function showProgress() {
        mainPanel.addClass("loading");        
    }

    function hideProgress() {
        mainPanel.removeClass("loading");        
    }

    function refreshStats() {
        showProgress();
        $.ajax({
            url: "Stats?v=1",
            context: document.body,
            dataType: "json",
            cache: false,
            success: function (data, status, xhr) {

                hideProgress();
                var content = $("#job-list-tmpl").tmpl(data);
                mainPanel.empty();
                mainPanel.append(content);

                $(".force-work-btn").click(function (btn) {
                    var workKey = $(btn.srcElement).data("work-key");
                    $.post("Work/" + workKey + "/Force", function () {
                        refreshStats();
                    });
                });
            }
        });
    };

    $("#refresh-stats-btn").click(function () {
        refreshStats();
    });

    $("#stop-btn").click(function () {
        $.post("Stop", function () {
            refreshStats();
        });
    });

    $("#start-btn").click(function() {
        $.post("Start", function() {
            refreshStats();
        });
    });

    refreshStats();
}); 