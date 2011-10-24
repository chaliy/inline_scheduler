(function() {
  var body, currentHash, hideProgress, key, mainPanel, refreshStats, replaceMain, showProgress, showWorkDetails;
  $(document).ajaxError(function(ev, xhr, settings, errorThrown) {
    return alert(xhr.responseText);
  });
  mainPanel = $("#main");
  body = $("body");
  showProgress = function() {
    return mainPanel.addClass("loading");
  };
  hideProgress = function() {
    return mainPanel.removeClass("loading");
  };
  replaceMain = function(tpl, data) {
    var content;
    content = $(tpl).tmpl(data);
    mainPanel.empty();
    return mainPanel.append(content);
  };
  refreshStats = function() {
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
            return refreshStats();
          });
        });
        $(".work-details-lnk").click(function(btn) {
          var workKey;
          workKey = $(btn.srcElement).data("work-key");
          return showWorkDetails(workKey);
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
    return refreshStats();
  });
  $("#stop-btn").click(function() {
    return $.post("Stop", function() {
      return refreshStats();
    });
  });
  $("#start-btn").click(function() {
    return $.post("Start", function() {
      return refreshStats();
    });
  });
  currentHash = window.location.hash;
  if (currentHash.indexOf("#work-details-") === 0) {
    key = currentHash.replace("#work-details-", "");
    showWorkDetails(key);
  } else {
    refreshStats();
  }
}).call(this);
