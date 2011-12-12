# Application Pool 
Configuration in the application host file (C:\Windows\System32\inetsrv\config\applicationHost.config) 

	<add name="SchedulerApplicationPool" managedRuntimeVersion="v4.0" startMode="AlwaysRunning" />

# Site
 •If Raven runs in an application pool with other sites, modify the application host file (C:\Windows\System32\inetsrv\config\applicationHost.config) to: 

<application path="/Raven" serviceAutoStartEnabled="true" />

