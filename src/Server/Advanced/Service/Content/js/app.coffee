$(document).ajaxError (ev, xhr, settings, errorThrown) ->
    alert(xhr.responseText)

mainPanel = $("#main")
body = $("body")

showProgress = -> mainPanel.addClass("loading")
hideProgress = -> mainPanel.removeClass("loading")

refreshStats = ->
    showProgress()
    $.ajax
        url: "Stats?v=1"
        context: document.body
        dataType: "json"
        cache: false
        success: (data, status, xhr) ->

            hideProgress()
            content = $("#job-list-tmpl").tmpl(data)
            mainPanel.empty()
            mainPanel.append(content)

            $(".force-work-btn").click (btn) ->
                workKey = $(btn.srcElement).data("work-key")
                $.post("Work/#{workKey}/Force", () -> refreshStats())    

$("#refresh-stats-btn").click -> refreshStats()

$("#stop-btn").click ->
    $.post "Stop", -> refreshStats()    

$("#start-btn").click ->
    $.post "Start", -> refreshStats()    

refreshStats()