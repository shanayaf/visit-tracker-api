# Visit Tracker API

 The system tracks store visits with support for user roles (Admin and Standard), JWT authentication, and includes CRUD operations for users, stores, products, visits, and photos.

I used .NET 7, Entity Framework Core, and MySQL. I also added bonus features like pagination, filtering, sorting, and unit tests to go the extra mile.

---

## Features

- User login with JWT tokens
- Role-based access (Admin, Standard)
- Admin can manage all data except posting photos and visits
- Standard users can create visits and upload photos
- Pagination, filtering, and sorting for visits
- Swagger UI for easy API testing
- Docker support for easy setup
- Unit testing 

---

## How to Run It with Docker

1. Make sure Docker Desktop is running.
2. In the root folder of the project, run:


docker-compose up --build


3. Open the browser and go to:


http://localhost:8080/swagger/index.html


From Swagger, you can test everything. First login and get the token, then authorize and call the endpoints.

---

## API Docs

While running, go to:

http://localhost:8080/swagger/index.html


You can register, login, authorize with Bearer token, and use all the API endpoints.

---

## Unit Tests

Run the tests with:

dotnet test VisitTracker.Tests


Tests are written for controllers like Photos and Visits.

---

## Tech Stack

- .NET 7
- Entity Framework Core
- MySQL (via Docker)
- xUnit for testing
- Swagger

---

## Project Structure
---
VisitTracker.API/         --> Main Web API
VisitTracker.Tests/       --> xUnit test project
docker-compose.yml        --> Docker setup
README.md

