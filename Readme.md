InlineScheduler
=============

IMPORTANT : For now I am assuming that I am the only user of this lib, so contracts are changing without any notice or backward compatibility. If you are intersting in using this lib, let me know.

Main goal is to provide in-proc scheduling facility. Originally designed to run in ASP.NET application, but probably in future this will expand to other types of hosts.

Features
========

1. Run any code on simple interval based schedule;
2. Allows to force executing work;
3. Gather some statistics and provide it with [simple UI](https://github.com/chaliy/inline_scheduler/wiki/UI).

Interanals
==========
1. TPL to run jobs;
2. WCF Web API to build REST like API;
3. CoffeeScript to code UI;
4. NUnit for testing.

Example
=======

	var scheduler = new Scheduler();

	scheduler.Schedule("Foo", () =>
	{    
	    Console.WriteLine("Foo is now working");    
	}, TimeSpan.FromMinutes(3));

Installation
============
	
[TBD]
	
License
=======

Licensed under the MIT