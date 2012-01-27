(function() {
  var app, self;

  $(document).ajaxError(function(ev, xhr, settings, errorThrown) {
    return alert(xhr.responseText);
  });

  self = this;

  self.selectedJobId = ko.observable();

  self.selectedJob = ko.observable();

  self.currentJobs = ko.observableArray([]);

  self.filter = ko.observable();

  self.pendingJobsCount = ko.observable();

  self.scheduledJobsCount = ko.observable();

  self.runningJobsCount = ko.observable();

  self.isStopped = ko.observable();

  self.inProgress = ko.observable();

  self.currentPage = ko.observable();

  self.start = function() {
    return self.post("Start");
  };

  self.stop = function() {
    return self.post("Stop");
  };

  self.force = function(jobId) {
    return self.post("Work/" + jobId + "/Force");
  };

  self.post = function(c) {
    self.inProgress(true);
    return $.post(c, function() {
      return self.refresh();
    });
  };

  self.get = function(u, c) {
    self.inProgress(true);
    return $.ajax({
      url: u,
      context: document.body,
      dataType: "json",
      cache: false,
      success: function(data, status, xhr) {
        c(data);
        return self.inProgress(false);
      }
    });
  };

  ko.dependentObservable(function() {
    var jobIdFind;
    jobIdFind = self.selectedJobId();
    if (jobIdFind) {
      return self.get("Stats/Job/" + jobIdFind + "/?v=1", function(s) {
        s.force = function() {
          return self.force(s.WorkKey);
        };
        self.currentPage("job");
        return self.selectedJob(s);
      });
    } else {
      return self.selectedJob(null);
    }
  });

  ko.dependentObservable(function() {
    var filter;
    filter = self.filter();
    if (filter) {
      self.selectedJob(null);
      return self.get("Stats/List/" + filter + "/?v=1", function(s) {
        self.pendingJobsCount(s.Overal.PendingJobs);
        self.scheduledJobsCount(s.Overal.ScheduledJobs);
        self.runningJobsCount(s.Overal.RunningJobs);
        self.isStopped(s.Overal.IsStopped);
        self.currentPage("list");
        return self.currentJobs($.map(s.CurrentJobs, function(j) {
          j.force = function() {
            return self.force(j.WorkKey);
          };
          j.Report = j.Report.replace(/\r\n/g, "<br/>");
          return j;
        }));
      });
    }
  });

  window.worksViewModel = self;

  ko.applyBindings(self);

  app = $.sammy(function() {
    this.get("#filters/:filter", function() {
      return self.filter(this.params.filter);
    });
    this.get("#jobs/:jobId", function() {
      return self.selectedJobId(this.params.jobId);
    });
    return this.get("", function() {
      return self.filter("all");
    });
  });

  app.run();

  $("#main").show();

}).call(this);
