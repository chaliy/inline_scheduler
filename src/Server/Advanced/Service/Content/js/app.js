(function() {
  var body, currentPath, hideProgress, mainPanel, refresh, replaceMain, route, showHome, showProgress, showWorkDetails;
  $(document).ajaxError(function(ev, xhr, settings, errorThrown) {
    return alert(xhr.responseText);
  });
  mainPanel = $("#main");
  body = $("body");
  currentPath = window.location.hash;
  showProgress = function() {
    return body.addClass("loading");
  };
  hideProgress = function() {
    return body.removeClass("loading");
  };
  replaceMain = function(tpl, data) {
    var content;
    content = $(tpl).tmpl(data);
    mainPanel.empty();
    return mainPanel.append(content);
  };
  refresh = function() {
    var key;
    if (currentPath.indexOf("#work-details-") === 0) {
      key = currentPath.replace("#work-details-", "");
      return showWorkDetails(key);
    } else {
      return showHome();
    }
  };
  route = function(path) {
    currentPath = path;
    return refresh();
  };
  showHome = function() {
    showProgress();
    return $.ajax({
      url: "Stats?v=1",
      context: document.body,
      dataType: "json",
      cache: false,
      success: function(data, status, xhr) {
        replaceMain("#work-list-tmpl", data);
        $(".force-work-btn").click(function(btn) {
          var workKey;
          workKey = $(btn.srcElement).data("work-key");
          return $.post("Work/" + workKey + "/Force", function() {
            return refresh();
          });
        });
        return hideProgress();
      }
    });
  };
  showWorkDetails = function(key) {
    showProgress();
    return $.ajax({
      url: "Stats/Work/" + key + "/?v=1",
      context: document.body,
      dataType: "json",
      cache: false,
      success: function(data, status, xhr) {
        replaceMain("#work-details-tmpl", data);
        return hideProgress();
      }
    });
  };
  $("#refresh-stats-btn").click(function() {
    return refresh();
  });
  $("#stop-btn").click(function() {
    return $.post("Stop", function() {
      return refresh();
    });
  });
  $("#start-btn").click(function() {
    return $.post("Start", function() {
      return refresh();
    });
  });
  $(window).bind("hashchange", function() {
    currentPath = window.location.hash;
    return refresh();
  });
  refresh();
}).call(this);
