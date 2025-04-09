# Elideus | DotNet Framework Database | PostgreSql

PostgreSql is a subpackage of the Database Package, and contains the basic functionalities to interact with a PostgreSql database.

This package includes a class called NpgsqlDatabaseProvider which implements the IDatabaseProvider.

It all includes 4 methods that can be overriden by the project using this package:

- ExecuteReadMultiple: Reads multiple entries from the database.
- ExecuteRead: Reads one entry from the database
- ExecuteWrite: Executes an update to the database
- MapDataReader: Used to map the NpgsqlDataReader to the desired type

# Database Provider

To implement this in a project, we need to create a provider for every table, and that provider must inherit from the NpgsqlDatabaseProvider.

Lets imagine that we have a table of clients, then we create the DatabaseClientsProvider:
