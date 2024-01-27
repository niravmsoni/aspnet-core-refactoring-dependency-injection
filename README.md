# Offloading Composition Root responsibilities to a separate class library
	- Often times, we see the Program.cs overburdened with all the service registrations as well as the code to invoke/start the application.

	- Application background
		- This is a console application that imports products from csv and outputs them to csv or Database.

	- Problem
		- With this structure we have 2 different things tied together i.e. EntryPoint and CompositionRoot
		- Right now, this application has a single entry point i.e. Console App
		- In-case if tomorrow a requirement comes in to have a WebAPI as another entry point, this could cause issue and we would require a lot of refactoring since we would reuse all the core logic	

	- Solution
		- Offloading responsibilities of doing the service registrations in a separate class library project
		- All the registrations are done either in WebAPI.Program class or are done in respective project by writing extension methods on IServiceCollection
			- Before
				- If we follow clean architecture practices, we typically have this structure
					- WebAPI - Has reference of Domain, Application and Infrastructure projects
					- Domain
					- Application
					- Infrastructure
			- After
				- A better way to deal with such registrations is by creating a CompositionRoot(Separate project).
					- WebAPI - Has reference only of CompositionRoot project
					- CompositionRoot - Has reference of Domain, Application and Infrastructure projects
					- Domain
					- Application
					- Infrastructure

	- This will be especially beneficial if we have or foresee more than 1 entrypoints against an application

	- To further better this code, we could break down dependencies into their respective project types by writing extension methods on IServiceCollection
		- See ProductImporter.Logic project - DIRegistrations class
		- Same implemented for ProductImporter.Logic.Transformations - DiRegistrations class

		- How do we allow tweaking of behavior to this DI?
			- Introduce a model class (ProductTransformationOptions.cs)
			- Pass it as a Action delegate in the serviceCollection method and take decisions based on the value set in model
			- From CompositionRoot project, set option properties and then we should see code behaving per the behavior defined on the model

	- Application Configuration using Options Pattern
		- Require options model - Refer References of CsvProductTargetOptions
		- NuGet package - Microsoft.Extensions.Options and Microsoft.Extensions.Configuration.Binder
		- Setup done in ProductImporter.Logic.DIRegistrations class