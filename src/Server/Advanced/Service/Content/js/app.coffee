$(document).ajaxError (ev, xhr, settings, errorThrown) ->
    alert(xhr.responseText)

viewModel =
    selectedJobId: ko.observable()
    selectedJob: ko.observable()
    currentJobs: ko.observableArray([])
    
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
    force: (jobId) -> viewModel.post "Work/#{jobId}/Force"

    refresh: -> 
        viewModel.inProgress(true)        
        viewModel.get("Stats?v=1", (s) -> 
            # Stats
            viewModel.pendingJobsCount(s.Overal.PendingJobs)
            viewModel.scheduledJobsCount(s.Overal.ScheduledJobs)
            viewModel.runningJobsCount(s.Overal.RunningJobs)
            viewModel.isStopped(s.Overal.IsStopped)
            # Data            
            viewModel.currentJobs($.map(s.CurrentJobs, (j) ->
                                    j.force = -> viewModel.force(j.WorkKey)
                                    j.Report = j.Report.replace(/\r\n/g, "<br/>")
                                    j ))

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
    jobIdFind = viewModel.selectedJobId()
    if (jobIdFind)
	    viewModel.get("Stats/Work/#{jobIdFind}/?v=1", viewModel.selectedJob)
    else
        viewModel.selectedJob null            
	), viewModel

window.worksViewModel = viewModel
ko.applyBindings(viewModel)

ko.linkObservableToUrl(viewModel.selectedJobId, "jobId")

#ko.dependentObservable (-> viewModel.refresh()), viewModel
viewModel.refresh()