(function() {
  var viewModel;
  var __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; };
  $(document).ajaxError(function(ev, xhr, settings, errorThrown) {
    return alert(xhr.responseText);
  });
  viewModel = {
    folders: ['Inbox', 'Archive', 'Sent', 'Spam'],
    selectedFolder: ko.observable('Inbox'),
    selectedMailId: ko.observable(),
    currentWorks: ko.observableArray([]),
    loadStats: __bind(function(s) {
      return viewModel.currentWorks(s.CurrentJobs);
    }, this)
  };
  window.worksViewModel = viewModel;
  ko.applyBindings(viewModel);
  ko.dependentObservable((function() {
    return $.get("Stats?v=1", null, viewModel.loadStats);
  }), viewModel);
}).call(this);
