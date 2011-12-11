(function() {
  var viewModel;
  $(document).ajaxError(function(ev, xhr, settings, errorThrown) {
    return alert(xhr.responseText);
  });
  viewModel = {
    selectedWorkId: ko.observable(),
    selectedWork: ko.observable(),
    currentWorks: ko.observableArray([]),
    stats: ko.observable(),
    pendingJobsCount: ko.observable(),
    scheduledJobsCount: ko.observable(),
    runningJobsCount: ko.observable(),
    start: function() {
      return $.post("Start", function() {
        return viewModel.refresh();
      });
    },
    stop: function() {
      return $.post("Stop", function() {
        return viewModel.refresh();
      });
    },
    refresh: function() {
      return $.get("Stats?v=1", null, viewModel.loadStats);
    },
    loadStats: function(s) {
      viewModel.pendingJobsCount(s.PendingJobs);
      viewModel.scheduledJobsCount(s.ScheduledJobs);
      viewModel.runningJobsCount(s.RunningJobs);
      return viewModel.currentWorks(s.CurrentJobs);
    }
  };
  ko.dependentObservable((function() {
    var workIdFind;
    workIdFind = viewModel.selectedWorkId();
    return $.get("Stats/Work/" + workIdFind + "/?v=1", null, viewModel.selectedWork);
  }), viewModel);
  window.worksViewModel = viewModel;
  ko.applyBindings(viewModel);
  ko.linkObservableToUrl(viewModel.selectedWorkId, "workId");
  viewModel.refresh();
}).call(this);
