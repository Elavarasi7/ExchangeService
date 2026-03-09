Overview
=========
ExchangeRateApi is a .NET 8 Web API that provides currency exchange rates by integrating with the ExchangeRate-API. The API exposes endpoints for retrieving exchange rates between currencies. 
Features
=========
•	Fetches real-time exchange rates from an external provider.
•	RESTful endpoints for querying rates.
•	Configurable API key and base URL via appsettings.json.
•	Integrated Swagger UI for API documentation and testing.
Technologies Used
=================
•	.NET 8 (C# 12)
•	ASP.NET Core Web API
•	Swashbuckle.AspNetCore (Swagger/OpenAPI)
•	HttpClient for external API calls
•	xUnit, Moq for unit testing
How to Run
==========
1.	Clone the repository and open the solution in Visual Studio 2022.
2.	Configure API Key
Edit ExchangeRateApi/appsettings.json:
3.	Build and Run the API
•	Set ExchangeRateApi as the startup project.
•	Press F5 or use Debug > Start Debugging.
4.	Explore the API
•	Navigate to https://localhost:<port>/swagger in your browser to access Swagger UI.
5.	Run Tests
•	Right-click ExchangeRateTests project and select Run Tests.
