$(document).ajaxError (ev, xhr, settings, errorThrown) ->
    alert(xhr.responseText)

mainPanel = $("#main")
body = $("body")
currentPath = window.location.hash

# Utility

showProgress = -> body.addClass("loading")
hideProgress = -> body.removeClass("loading")

replaceMain = (tpl, data) ->
    content = $(tpl).tmpl(data)
    mainPanel.empty()
    mainPanel.append(content)
    
refresh = ->
    if (currentPath.indexOf("#work-details-") == 0)
        key = currentPath.replace("#work-details-", "");
        showWorkDetails key
    else
        showHome()

route = (path) ->
    currentPath = path
    refresh()

# Routes

showHome = ->
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
                $.post("Work/#{workKey}/Force", () -> refresh())
            
            hideProgress()

showWorkDetails = (key) ->
    showProgress()
    $.ajax
        url: "Stats/Work/#{key}/?v=1"
        context: document.body
        dataType: "json"
        cache: false
        success: (data, status, xhr) ->            
            replaceMain "#work-details-tmpl", data
                        
            hideProgress()    

# Handlers

$("#refresh-stats-btn").click -> refresh()

$("#stop-btn").click ->
    $.post "Stop", -> refresh()    

$("#start-btn").click ->
    $.post "Start", -> refresh()

$(window).bind "hashchange", ->
    currentPath = window.location.hash
    refresh()


refresh()