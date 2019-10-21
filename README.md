# Solution Details

This solution is related to the following link:
https://github.com/andreariano/interview-tests/blob/master/INTERMEDIATE_TEST.md

## Tools

* Visual Studion 2017 Community
* Postman
* Docker

## Framework

* .NET Core 2.2/ Asp.Net Core 2.2

## Third-party Nuget Packages

* Presentation And Application Layer 
  * MediatR: 7.0.0
  * FluentValidator: 8.5.0
  * RavenDB.Client: 4.2.4

* Repository / Implementation
  * RavenDB.Client: 4.2.4

## Configuration

* Development
  * appsettings.Development.json
    *RavenDB Settings example:   
		"Raven": {
				"Url": "http://localhost:8080",
				"Database": "TheatricalEventChargerServerDB"
			 }
* Production
  * appsettings.json

* RavenDB Docker
  * Reference: https://ravendb.net/docs/article-page/4.1/csharp/start/installation/running-in-docker-container

## Architecture

* Presentation Layer
  * Register all Dependencies (Startup.cs)
  * Manage Http Requests and Responses

* Application Layer
  * Orchestrate Repository and Domain Objects and determines how they relate with each other.

* Domain Layer
  * Here sits all Business rules concerned with Theatrical Event Charge
  * Composed by: 
    * Abstract (TheatricalEventChargerDomain.Abstracts);
    * Implentation (TheatricalEventChargerDomain);
	* Unit Tests (TheatricalEventChargerDomain.UnitTests).

* Repository Layer
  * Responsible only for Persisting data.
  * Composed by:
    * Abstract (TheatricalEventChargerRepository.Abstracts);
	  * IReadOnlyRepository was created to allowing Database sharing respecting the boundaries between contexts, which is 
	    the case of TheatricalPlayCatalogItem, that could be concerd to information that can't be changed by the Context 
		related to the Theatrical Event Charge (which is something more relates better with Finance context - just a guess).
    * Implentations (TheatricalEventChargerRepository.DocumentStore).

## Design Patterns

* Mediator
  * Dependency Package: MediatR;
  * This pattern is used to hide methods implementation from explicit calls, decoupling the Presentation Layer from Application Layer.

* Strategy
  * Dependency Package: Standard2.0;
  * It allows creating classes with diferent Bill Calculation Strategies, which can be switched over at runtime depending on the kind of play being calculated.

* Service Factory
  * Dependency Package: Standard2.0;
  * The Service Factory pattern provides an instance of a Bill Calculation Strategy taking into account the kind of play and instance availability, in case no instance is found a default one is provided. It's also possible define customized default bill Calculation strategy since the set of stragegies are initialized outside the factory.
  
## Concepts

* SOLID
  * Segregation can be seen all around the layers;
  * Open-Closed principle can be seen:
    * By varying the Calculation strategy;
	* If a calculation strategy changes for a kind of play, it requires only a new strategy class creation + its registeration;
  * Liskov: There are no implemention breaking Liskov restrictions about classes and super classes or sub classes.
  * Interface Segregation can be seen:
    * Calculation Strategy Abstractions are examples of the complaince with this principle;
  * DIP can be seen specificaly in the Native Dependency Injection provided by Asp.Net Core.

* DDD
  * Entity and Aggregate Root
    * TheatricalPlayPerformance and CustomerCharge;
  * Layer Separation according to each caracteristic;
  * Ubiquitous language: I tried as much as I could to bring the language used in the document into the code.

* Clean Code
  * Classes, Interfaces, Properties, Methods, etc were designed to be self-explained withou the needs for comments all of the code.

## Database

* Raven DB

  * I hard-coded RavenDB's Database creation and seed for Theatrical Play Catalog information for the sake of simplicity. 

## Support

* Few free to reach me out at ycmachado@gmail.com in case of facing any issue while running the project.
