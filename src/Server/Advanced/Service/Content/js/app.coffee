$(document).ajaxError (ev, xhr, settings, errorThrown) ->
    alert(xhr.responseText)

self = this

self.selectedJobId = ko.observable()
self.selectedJob = ko.observable()
self.currentJobs = ko.observableArray([])  
self.filter = ko.observable()    
    
# Stats
self.pendingJobsCount = ko.observable()
self.scheduledJobsCount = ko.observable()
self.runningJobsCount = ko.observable()
self.isStopped = ko.observable()

# UI
self.inProgress = ko.observable()   
self.currentPage = ko.observable()

# Actions
self.start = -> self.post "Start"
self.stop = -> self.post "Stop"
self.force = (jobId) -> self.post "Work/#{jobId}/Force"

# Routes
#self.refresh = -> 
#   if self.selectedJobId() != ""
#       self.refreshList()            

# Utils
self.post = (c) -> 
    self.inProgress(true)
    $.post c, -> self.refresh()

self.get = (u, c) ->
    self.inProgress(true)
    $.ajax
        url: u
        context: document.body            
        dataType: "json"
        cache: false
        success: (data, status, xhr) -> 
            c(data)
            self.inProgress(false)

ko.dependentObservable -> 
    jobIdFind = self.selectedJobId()
    if (jobIdFind)
	    self.get("Stats/Job/#{jobIdFind}/?v=1", (s) -> 
            s.force = -> self.force(s.WorkKey)
            self.currentPage("job")
            self.selectedJob(s))
    else
        self.selectedJob(null)

ko.dependentObservable ->         
        filter = self.filter()
        if filter
            self.selectedJob(null)
            self.get("Stats/List/#{filter}/?v=1", (s) -> 
                # Stats
                self.pendingJobsCount(s.Overal.PendingJobs)
                self.scheduledJobsCount(s.Overal.ScheduledJobs)
                self.runningJobsCount(s.Overal.RunningJobs)
                self.isStopped(s.Overal.IsStopped)
                # Data
                self.currentPage("list")
                self.currentJobs($.map(s.CurrentJobs, (j) ->
                                        j.force = -> self.force(j.WorkKey)
                                        j.Report = j.Report.replace(/\r\n/g, "<br/>")
                                        j ))
            )

window.worksViewModel = self
ko.applyBindings(self)

app = $.sammy ->    

    this.get "#filters/:filter", ->
        self.filter(this.params.filter)

    this.get "#jobs/:jobId", ->        
        self.selectedJobId(this.params.jobId)

    this.get "", ->
        self.filter("all")    
app.run()

$("#main").show()