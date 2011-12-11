(function() {
  var viewModel;
  $(document).ajaxError(function(ev, xhr, settings, errorThrown) {
    return alert(xhr.responseText);
  });
  viewModel = {
    selectedWorkId: ko.observable(),
    selectedWork: ko.observable(),
    currentWorks: ko.observableArray([]),
    pendingJobsCount: ko.observable(),
    scheduledJobsCount: ko.observable(),
    runningJobsCount: ko.observable(),
    isStopped: ko.observable(),
    inProgress: ko.observable(),
    start: function() {
      return viewModel.post("Start");
    },
    stop: function() {
      return viewModel.post("Stop");
    },
    force: function(workId) {
      return viewModel.post("Work/" + workId + "/Force");
    },
    refresh: function() {
      viewModel.inProgress(true);
      return viewModel.get("Stats?v=1", function(s) {
        var currentWorks;
        viewModel.pendingJobsCount(s.PendingJobs);
        viewModel.scheduledJobsCount(s.ScheduledJobs);
        viewModel.runningJobsCount(s.RunningJobs);
        viewModel.isStopped(s.IsStopped);
        currentWorks = $.map(s.CurrentJobs, function(j) {
          j.force = function() {
            return viewModel.force(j.WorkKey);
          };
          return j;
        });
        viewModel.currentWorks(currentWorks);
        return viewModel.inProgress(false);
      });
    },
    post: function(c) {
      viewModel.inProgress(true);
      return $.post(c, function() {
        return viewModel.refresh();
      });
    },
    get: function(u, c) {
      return $.ajax({
        url: u,
        context: document.body,
        dataType: "json",
        cache: false,
        success: function(data, status, xhr) {
          return c(data);
        }
      });
    }
  };
  ko.dependentObservable((function() {
    var workIdFind;
    workIdFind = viewModel.selectedWorkId();
    if (workIdFind) {
      return viewModel.get("Stats/Work/" + workIdFind + "/?v=1", viewModel.selectedWork);
    } else {
      return viewModel.selectedWork(null);
    }
  }), viewModel);
  window.worksViewModel = viewModel;
  ko.applyBindings(viewModel);
  ko.linkObservableToUrl(viewModel.selectedWorkId, "workId");
  viewModel.refresh();
}).call(this);
