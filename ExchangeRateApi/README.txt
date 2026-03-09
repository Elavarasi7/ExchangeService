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

Note: 
=====
I have generated my APIkey after signing to the Free ExchangeServiceAPI page and the key is placed in the appsetting.Json file. It is generally not advisable to keep the kep in appsettings.json, still for testing purpose , I have place here. This can be moved to Azure Key vaults when deploying to cloud. 

To Run Curl request in Windows Terminal , Please use the below : 
============================================================
curl -X POST "https://localhost:7068/ExchangeService" ^ -H "accept: text/plain" ^ -H "Content-Type: application/json" ^ -d "{\"amount\":4,\"inputCurrency\":\"USD\",\"outputCurrency\":\"AUD\"}"

MONITORING TOOLS CONFIGURATION: 
===============================
We can add the PROMETHEUS OR GRAFANA Nuget package for obeserving the API performance and metrics. This will help us to monitor the API usage and performance in real time. We can also set up alerts based on certain thresholds to get notified when the API is under heavy load or when there are any issues.