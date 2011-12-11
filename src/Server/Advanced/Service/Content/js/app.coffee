$(document).ajaxError (ev, xhr, settings, errorThrown) ->
    alert(xhr.responseText)

viewModel =    
    folders: ['Inbox', 'Archive', 'Sent', 'Spam']
    selectedFolder: ko.observable 'Inbox'
    selectedMailId: ko.observable()
    currentWorks: ko.observableArray([])

    loadStats: (s) =>
        viewModel.currentWorks(s.CurrentJobs)		

window.worksViewModel = viewModel
ko.applyBindings(viewModel)

ko.dependentObservable (-> $.get("Stats?v=1", null, viewModel.loadStats)), viewModel