# Elideus | DotNet Framework Database | PostgreSql

**PostgreSql** is a subpackage of the *Database Package*, and provides the basic functionalities required to interact with a PostgreSQL database.

This package includes a class called `NpgsqlDatabaseProvider` which implements the `IDatabaseProvider` interface.

It also includes four methods that can be overridden by any project using this package:

- `ExecuteReadMultiple`: Reads multiple entries from the database.
- `ExecuteRead`: Reads a single entry from the database.
- `ExecuteWrite`: Executes an update against the database.
- `MapDataReader`: Used to map the `NpgsqlDataReader` to the desired type.

---

## Database Provider

To use this package, you need to create a provider for each table. That provider must inherit from `NpgsqlDatabaseProvider`.

The provider must specify a type parameter, representing the table model. For example, suppose we have a `users` table:

```csharp
public class UserTable
{
    public required string Username { get; set; }

    public required string Password { get; set; }
}
```

You then define an interface for the provider extending `IDatabaseProvider<UserTable>`:

```csharp
public interface IDatabaseUserProvider : IDatabaseProvider<UserTable>
{
}
```

The `IDatabaseProvider` interface already defines several [methods](https://github.com/eliseubatista99/Elideus-DotNet-Framework/blob/feat/documentation/ElideusDotNetFramework/ElideusDotNetFramework.Database/README.md), 
so your custom interface can define any new methods specific to your application.

Finally, implement the interface by extending `NpgsqlDatabaseProvider`:

```csharp
public class DatabaseUserProvider : NpgsqlDatabaseProvider<UserTable>, IDatabaseUserProvider
{
}
```

Now the provider is ready to be registered and used as a dependency in your application.

---

## Database Interaction Examples

### CreateTableIfNotExists

```csharp
public override bool CreateTableIfNotExists()
{
    var command = $"CREATE TABLE IF NOT EXISTS {UserTable.TABLE_NAME}" +
                  $"(" +
                  $"{UserTable.COLUMN_ID} VARCHAR(64) NOT NULL," +
                  $"{UserTable.COLUMN_PASSWORD} VARCHAR(28) NOT NULL," +
                  $"PRIMARY KEY ({UserTable.COLUMN_ID})" +
                  $")";

    return ExecuteWrite(connectionString, command);
}
```

### GetAll

```csharp
public override List<UserTable> GetAll()
{
    var command = $"SELECT * FROM {UserTable.TABLE_NAME}";

    return ExecuteReadMultiple(connectionString, command);
}
```

### GetById

```csharp
public override UserTable? GetById(string id)
{
    var command = $"SELECT * FROM {UserTable.TABLE_NAME} WHERE {UserTable.COLUMN_ID} = '{id}'";

    return ExecuteRead(connectionString, command);
}
```

### Add

```csharp
public override bool Add(UserTable user)
{
    var command = $"INSERT INTO {UserTable.TABLE_NAME} " +
                  $"({UserTable.COLUMN_ID}, {UserTable.COLUMN_PASSWORD}) VALUES " +
                  $"('{user.Id}', '{user.Password}');";

    return ExecuteWrite(connectionString, command);
}
```