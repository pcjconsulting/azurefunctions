Azure Functions

• ProductEntityGetAllCreate and ProductEntityGetByIdUpdateDelete 
		use httpTrigger 
		bind to Azure Storage Account Table (Product) 
		perform CRUD on Product entities

• ProductGetAllCreate and ProductGetByIdUpdateDelete 
		use httpTrigger 
		bind to SQL Server using Entity Framework
		perform CRUD on Product table

Decided to write the SQL Server code but not to deploy MS SQL Server.
The Entity Frameworks IOC wire-up c# code is not executed.
The http endpoints for SQL Server, while coded, don't work.


To Publish
 In VS Code Create PcjFunctionApp1 and pcjstorageaccount1 in my Azure tenant
 Publish in Visual Studio.
 Postman:
	 https://pcjfunctionapp1.azurewebsites.net/api/productentity/42
