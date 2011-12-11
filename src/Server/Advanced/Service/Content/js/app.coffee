$(document).ajaxError (ev, xhr, settings, errorThrown) ->
    alert(xhr.responseText)

viewModel =
    selectedWorkId: ko.observable()
    selectedWork: ko.observable()
    currentWorks: ko.observableArray([])
    stats: ko.observable()	
    pendingJobsCount: ko.observable()
    scheduledJobsCount: ko.observable()
    runningJobsCount: ko.observable()

    start: -> $.post "Start", -> viewModel.refresh()
    stop: -> $.post "Stop", -> viewModel.refresh()
    refresh: -> $.get("Stats?v=1", null, viewModel.loadStats)

    loadStats: (s) ->
        viewModel.pendingJobsCount(s.PendingJobs)
        viewModel.scheduledJobsCount(s.ScheduledJobs)
        viewModel.runningJobsCount(s.RunningJobs)
        viewModel.currentWorks(s.CurrentJobs)

ko.dependentObservable (-> 
	workIdFind = viewModel.selectedWorkId()
	$.get("Stats/Work/#{workIdFind}/?v=1", null, viewModel.selectedWork)
	), viewModel

window.worksViewModel = viewModel
ko.applyBindings(viewModel)

ko.linkObservableToUrl(viewModel.selectedWorkId, "workId")

#ko.dependentObservable (-> viewModel.refresh()), viewModel
viewModel.refresh()