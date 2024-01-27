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

	- Implementing Multiple Implementing types
		- In this use-case, there are 3 different implementing types which are executing the same task i.e. Transformation
			- ICurrencyNormalizer
			- INameDecapitaliser
			- IReferenceAdder

		- We could have them implement a single interface
		- Above interfaces were removed and replaced with IProductTransformation interface
		- Demonstrates polymorphism. Refer ProductImporter.Logic.Transformation.ProductTransformer class
		- While requesting IProductTransformation from DI, please note that we have requested IEnumerable<IProductTransformation>
			- This gives us all types implementing IProductTransformation
		- If we simply request IProductTransformation from DI,
			- This gives us the last registration done with this type i.e. ReferenceAdder. If we do not want this to happen, instead of Add, use TryAdd.
			- This will ensure we get first registration done with this type

	- Service Locator Pattern
		- So far, We have used 2 ways to get dependencies
			- Dependency Injection
				- Class declares its dependencies (Mostly in constructors) and they get provided somehow. DI container is a way
			- Service Locator
				- Class references central service repository and requests services it needs from there
					- For ex: scope.ServiceProvider.GetRequiredService<IEnumerable<IProductTransformation>>()
				- This is misusing the DI container as Service Locator. This is easily an Anti-pattern

		- Why we should not use Service Locator pattern?
			- Testability
				- Classes using Service Locator are harder to test/mock
			- Hides Dependencies
				- Dependencies are not visible (Either through constructor).They are implicit. Need to go through code to understand its dependencies

		- Golden rule
			- We should reference the container as less as possible

	- Optional Registrations
		- What to do when you do not have an implementation of an interface available
		- We will target IProductTransformer here
		- 3 ways to achieve this
			- Service locator Pattern (Not recommended since its antipattern)
				- Inject IServiceProvider as dependency
				- Use GetService or GetRequiredService for retrieving value from Service provider
				- Difference
					- GetRequiredService() - Throws InvalidOperationException() when required service is not present in Service provider
					- GetService() - Returns null
				- We could do a null check and handle it here