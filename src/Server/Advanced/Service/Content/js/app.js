/*jslint onevar: true, undef: true, newcap: true, regexp: true, plusplus: true, bitwise: true, devel: true, maxerr: 50 */
/*global window: true, jQuery:true, $:true, document:true*/
/// <reference path="vendor/jquery/1.5.1/jquery.js"/>
$(function () {

    // Error
    $(document).ajaxError(function (ev, xhr, settings, errorThrown) {
        alert(xhr.responseText);
    });

    var mainPanel = $("#main");
    function refreshStats() {
        $.ajax({
            url: "Stats?v=1",
            context: document.body,
            dataType: "json",
            cache: false,
            progress: true,
            success: function (data, status, xhr) {
                var content = $("#command-list-template").tmpl(data);
                mainPanel.empty();
                mainPanel.append(content);
            }
        });
    };

    $('#refresh-stats-btn').click(function () {
        refreshStats();
    })

    refreshStats();
}); 