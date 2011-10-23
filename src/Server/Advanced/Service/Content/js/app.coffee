$(document).ajaxError (ev, xhr, settings, errorThrown) ->
    alert(xhr.responseText)

mainPanel = $("#main")
body = $("body")

showProgress = -> mainPanel.addClass("loading")
hideProgress = -> mainPanel.removeClass("loading")

replaceMain = (tpl, data) ->
    content = $(tpl).tmpl(data)
    mainPanel.empty()
    mainPanel.append(content)

refreshStats = ->
    showProgress()
    $.ajax
        url: "Stats?v=1"
        context: document.body
        dataType: "json"
        cache: false
        success: (data, status, xhr) ->            
            replaceMain "#work-list-tmpl", data

            $(".force-work-btn").click (btn) ->
                workKey = $(btn.srcElement).data("work-key")
                $.post("Work/#{workKey}/Force", () -> refreshStats())    
            
            $(".work-details-lnk").click (btn) ->
                workKey = $(btn.srcElement).data("work-key")
                showWorkDetails workKey		
            
            hideProgress()

showWorkDetails = (key) ->
    replaceMain "#work-details-tmpl", key    

$("#refresh-stats-btn").click -> refreshStats()

$("#stop-btn").click ->
    $.post "Stop", -> refreshStats()    

$("#start-btn").click ->
    $.post "Start", -> refreshStats()


currentHash = window.location.hash
if (currentHash.indexOf("#work-details-") == 0)
    key = currentHash.replace("#work-details-", "");
    showWorkDetails key
else
    refreshStats()