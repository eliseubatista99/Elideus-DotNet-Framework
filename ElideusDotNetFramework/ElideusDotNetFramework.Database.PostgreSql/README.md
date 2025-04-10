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

The provider needs us to specify a type, that type is the table model. Lets image that we have a table of users, then our UserTable class could be something like:


```csharp
public class UserTable
{
	public required string Username { get; set; }
	
	public required string Password { get; set; }
}
```

Then we create the IDatabaseUsersProvider that extends from the IDatabaseProvider with the type UserTable:

```csharp
public interface IDatabaseUserProvider : IDatabaseProvider<UserTable>
{
}
```

The IDatabaseProvider interface already defines a few [methods](https://github.com/eliseubatista99/Elideus-DotNet-Framework/blob/feat/documentation/ElideusDotNetFramework/ElideusDotNetFramework.Database/README.md),
so in the IDatabaseUserProvider we can define any new method we like.


Then, we create the implementation of the IDatabaseProvider:

```csharp
public class DatabaseUserProvider : NpgsqlDatabaseProvider<UserTable>, IDatabaseUserProvider
{
}
```

Now we inject the dependency and it is ready to use.

# Database interaction examples:

### CreateTableIfNotExists

```csharp
public override bool CreateTableIfNotExists()
{
	var command = $"CREATE TABLE IF NOT EXISTS {UserTable.TABLE_NAME}" +
				$"(" +
				$"{UserTable.COLUMN_ID} VARCHAR(64) NOT NULL," +
				$"{UserTable.COLUMN_PASSWORD} VARCHAR(28) NOT NULL," +
				$"PRIMARY KEY ({UserTable.COLUMN_ID} )" +
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
public override List<UserTable> GetAll()
{
	var command = $"SELECT * FROM {UserTable.TABLE_NAME} WHERE {UserTable.COLUMN_ID} = '{id}'";

	return ExecuteRead(connectionString, command);
}
```

### Add

```csharp
public override bool Add(UserTable user)
{
	var result = $"INSERT INTO {UserTable.TABLE_NAME} " +
					$"({UserTable.COLUMN_ID}, {UserTable.COLUMN_PASSWORD}) VALUES " +
					$"('{user.Id}', '{user.Passwor}');";
					
	return ExecuteWrite(connectionString, command);

```

