# Elideus | DotNet Framework Database

**Database** is a subpackage of the *Elideus-DotNet-Framework*, and defines the basic operations required for implementing a database provider.

---

## Core Operations

The following methods are specified by this package and should be implemented by any custom database provider:

- `CreateTableIfNotExists`  
  Creates the database table if it doesn't already exist.

- `GetAll`  
  Retrieves all records of the specified type.

- `GetById`  
  Retrieves a specific record by its ID.

- `Add`  
  Inserts a new record into the database.

- `Edit`  
  Updates an existing record.

- `Delete`  
  Removes a record by its ID.

- `DeleteAll`  
  Removes all records from the table.

---

This interface standardizes the structure of database operations, making it easier to manage and extend database providers consistently throughout the framework.
