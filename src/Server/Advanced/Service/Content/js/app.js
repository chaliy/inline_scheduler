(function() {
  var body, hideProgress, mainPanel, refreshStats, showProgress;
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
  refreshStats = function() {
    showProgress();
    return $.ajax({
      url: "Stats?v=1",
      context: document.body,
      dataType: "json",
      cache: false,
      success: function(data, status, xhr) {
        var content;
        hideProgress();
        content = $("#job-list-tmpl").tmpl(data);
        mainPanel.empty();
        mainPanel.append(content);
        return $(".force-work-btn").click(function(btn) {
          var workKey;
          workKey = $(btn.srcElement).data("work-key");
          return $.post("Work/" + workKey + "/Force", function() {
            return refreshStats();
          });
        });
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
  refreshStats();
}).call(this);
