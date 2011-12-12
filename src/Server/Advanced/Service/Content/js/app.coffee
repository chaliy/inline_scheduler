$(document).ajaxError (ev, xhr, settings, errorThrown) ->
    alert(xhr.responseText)

viewModel =
    selectedWorkId: ko.observable()
    selectedWork: ko.observable()
    currentWorks: ko.observableArray([])
    
    # Stats
    pendingJobsCount: ko.observable()
    scheduledJobsCount: ko.observable()
    runningJobsCount: ko.observable()
    isStopped: ko.observable()

    # UI
    inProgress: ko.observable()    

    # Actions
    start: -> viewModel.post "Start"
    stop: -> viewModel.post "Stop"
    force: (workId) -> viewModel.post "Work/#{workId}/Force"

    refresh: -> 
        viewModel.inProgress(true)        
        viewModel.get("Stats?v=1", (s) -> 
            # Stats
            viewModel.pendingJobsCount(s.PendingJobs)
            viewModel.scheduledJobsCount(s.ScheduledJobs)
            viewModel.runningJobsCount(s.RunningJobs)
            viewModel.isStopped(s.IsStopped)
            # Data
            currentWorks = $.map(s.CurrentJobs, (j) ->
                    j.force = -> viewModel.force(j.WorkKey)
                    j )
            viewModel.currentWorks(currentWorks)

            viewModel.inProgress(false))

    # Utils
    post: (c) -> 
        viewModel.inProgress(true)
        $.post c, -> viewModel.refresh()

    get: (u, c) ->
        $.ajax
            url: u
            context: document.body
            dataType: "json"
            cache: false
            success: (data, status, xhr) -> c(data)

ko.dependentObservable (-> 
    workIdFind = viewModel.selectedWorkId()
    if (workIdFind)
	    viewModel.get("Stats/Work/#{workIdFind}/?v=1", viewModel.selectedWork)
    else
        viewModel.selectedWork null            
	), viewModel

window.worksViewModel = viewModel
ko.applyBindings(viewModel)

ko.linkObservableToUrl(viewModel.selectedWorkId, "workId")

#ko.dependentObservable (-> viewModel.refresh()), viewModel
viewModel.refresh()