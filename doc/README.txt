Web services framework for my Bachelor thesis.

//---------- Structure

Application
A simple console application with self-host functionality. Contains few controllers to test a basic behaviour.

Resttp
A class librabry, which contains most of the framework logic.

Resttp.ContentNegotiation
Handles Content Negotiation logic.

Resttp.Dependencies
Interfaces for frameworks dependencies module.

Resttp.IoC
A default framework Inversion of control container. Can be used as a stand alone IoC container.

Resttp.Routing
Interfaces for frameworks routing module.

Resttp.Tests
Unit tests to test some of the frameworks inner behaviour.


//---------- How to run an application?

Run Application console application. It will self-host and will be available at http://localhost:61111.

//---------- Example requests

GET http://localhost:61111/Main?sk=111&txt=labaslabas HTTP/1.1
User-Agent: Fiddler
Content-Type: application/json
Host: localhost:61111

POST http://localhost:61111/Home/Pagrindinis HTTP/1.1
User-Agent: Fiddler
Content-Type: application/json
Host: localhost:61111
Content-Length: 0

GET http://localhost:61111/Home/Get?nr=11111 HTTP/1.1
User-Agent: Fiddler
Content-Type: application/xml
Host: localhost:61111
Content-Length: 0

PUT http://localhost:61111/Home/PutPut HTTP/1.1
User-Agent: Fiddler
Content-Type: application/xml
Host: localhost:61111
Content-Length: 0

GET http://localhost:61111/First/Anon?nr=11&txt=hey HTTP/1.1
User-Agent: Fiddler
Content-Type: application/json
Host: localhost:61111
Content-Length: 0






