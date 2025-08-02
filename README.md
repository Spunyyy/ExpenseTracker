# Expense tracker API
REST API for Expense tracker, created with ASP.NET, EF Core, using JWT Authorization

# Start
1. Clone the repository.
2. Configure connection string in `appsettings.json`
3. Apply EF Core migrations.

# Features
 - User registration and login with JWT & refresh tokens.
 - Expense statistics by month and category.
 - User profile endpoint.
 - All endpoints require authentication except registration and login.

# Endpoints
## Auth
- **POST** api/Auth/register
  - Registers new user.
- **POST** api/Auth/login
  - Authenticates user and returns JWT with refresh token.
- **POST** api/Auth/refresh-token
  - Issues new JWT & refresh token.

## Expense
- **POST** api/Expense
  - Adds new expense for authenticated user.
- **DELETE** api/Expense/id
  - Deletes user's expense by ID.
- **PUT** api/Expense/id
  - Edits user's expense by ID.
- **GET** api/Expense/
  - Returns all expenses for user, also can be filtered by year and month.
- **GET** api/Expense/id
  - Returns details of specific expense by ID.

## Stats
- **GET** api/Stats/monthly
  - Returns summarized expenses by month.
- **GET** api/Stats/category
  - Returns summarized expenses by category.

## User
- **GET** api/User/me 
  - Returns details about user.
