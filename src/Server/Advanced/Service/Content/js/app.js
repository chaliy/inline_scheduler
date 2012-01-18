(function() {
  var viewModel;

  $(document).ajaxError(function(ev, xhr, settings, errorThrown) {
    return alert(xhr.responseText);
  });

  viewModel = {
    selectedJobId: ko.observable(),
    selectedJob: ko.observable(),
    currentJobs: ko.observableArray([]),
    filter: ko.observable(),
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
    force: function(jobId) {
      return viewModel.post("Work/" + jobId + "/Force");
    },
    refresh: function() {
      viewModel.inProgress(true);
      return viewModel.get("Stats?v=1", function(s) {
        viewModel.pendingJobsCount(s.Overal.PendingJobs);
        viewModel.scheduledJobsCount(s.Overal.ScheduledJobs);
        viewModel.runningJobsCount(s.Overal.RunningJobs);
        viewModel.isStopped(s.Overal.IsStopped);
        viewModel.currentJobs($.map(s.CurrentJobs, function(j) {
          j.force = function() {
            return viewModel.force(j.WorkKey);
          };
          j.Report = j.Report.replace(/\r\n/g, "<br/>");
          return j;
        }));
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

  ko.dependentObservable(function() {
    var jobIdFind;
    jobIdFind = viewModel.selectedJobId();
    if (jobIdFind) {
      return viewModel.get("Stats/Work/" + jobIdFind + "/?v=1", function(s) {
        s.force = function() {
          return viewModel.force(s.WorkKey);
        };
        return viewModel.selectedJob(s);
      });
    } else {
      return viewModel.selectedJob(null);
    }
  });

  viewModel.currentJobsFilterd = ko.dependentObservable(function() {
    var currentJobs, filter;
    currentJobs = viewModel.currentJobs();
    filter = viewModel.filter();
    return currentJobs.filter(function(j) {
      if (filter === "running") {
        return j.CurrentStatus === "Running";
      } else if (filter === "failing") {
        return j.Health === "Bad";
      }
      return true;
    });
  });

  window.worksViewModel = viewModel;

  ko.applyBindings(viewModel);

  ko.linkObservableToUrl(viewModel.selectedJobId, "jobId");

  ko.linkObservableToUrl(viewModel.filter, "filter");

  viewModel.refresh();

}).call(this);
