InlineScheduler
=============

__IMPORTANT : For now I am assuming that I am the only user of this lib, so contracts are changing without any notice or backward compatibility. If you are intersting in using this lib, let me know.__

Main goal is to provide in-proc scheduling facility. Originally designed to run in ASP.NET application, but probably in future this will expand to other types of hosts.

Features
========

1. Run any code on simple interval based schedule
2. Allows to force executing work
3. Gather some statistics and provide it with [simple UI](https://github.com/chaliy/inline_scheduler/wiki/UI)

What is planned
===============
1. More statistics, I believe this is key when you run background tasks
2. Other then interval schedules. Single execution
3. More reliable during worker process restart
4. Better work management, now it's quite supid and just limits to 40 workers
5. Distributed coordinator

Interanals
==========
1. TPL to run jobs
2. WCF Web API to build REST like API
3. CoffeeScript to code UI, jQuery, [Bootstrap from Twitter](http://twitter.github.com/bootstrap/)
4. NUnit for testing

Example
=======

	var scheduler = new Scheduler();

	scheduler.Schedule("Foo", () =>
	{    
	    Console.WriteLine("Foo is now working");    
	}, TimeSpan.FromMinutes(3));

	scheduler.Start();

Installation
============

[TBD]

Upgrade
=======

Instead of

    ProcessingScheduler.Init(scheduler);

Use

    InlineSchedulerWebHost.Init(scheduler);

	
License
=======

Licensed under the MIT