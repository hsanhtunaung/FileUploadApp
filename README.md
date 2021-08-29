1.This project is built with Visual Stuido 2019  by using ASP.NET MVC and Web API  in ASP.NET Core 3.1.This project is to upload XML/CSV files and to retrieve data with WebAPI.
Please download FilUploadDB.sql script and execute script. Download the whoe project files too and change your db configuration in appsettings.json.Please get sample XML and CSV file for uploading files from this path FileUploadApp/FileUploadApp/Files/ . You can see logs for invalid records on this path FileUploadApp/FileUploadApp/Logs/ . For API Links, you can use below links with sample data to get related data.

API by Currency
api/Get/GetByCurrency?currency=USD

AP  by date range
api/Get/GetBydate?date=2019-01-23T13:45:10

AP  by status
api/Get/GetByStatus?Status=Approved
