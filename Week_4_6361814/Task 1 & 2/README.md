# .NET Core Web API Project

This project is a basic ASP.NET Core Web API scaffolded using the API template. It includes a default controller (`ValuesController`) with CRUD (Create, Read, Update, Delete) action methods following RESTful conventions.

## How to Run

1. Open a terminal in the project root directory.
2. Build and run the project:
   ```powershell
   dotnet run
   ```
3. The API will be available at `http://localhost:5000` (or the port specified in the output).

## Testing the API
- Use a tool like Postman or your browser to test the GET endpoint:
  - `GET http://localhost:5000/api/values`
- You should receive the default response from the controller.

## Project Structure
- `Controllers/ValuesController.cs`: Contains the default CRUD action methods.

---

This project is ready for further development. Customize controllers and models as needed for your application.
