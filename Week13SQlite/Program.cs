using System.Data.SQLite;

//ReadData(CreateConnection());
//InsertCustomer(CreateConnection());
//RemoveCustomer(CreateConnection());
FindCustomer(CreateConnection());

static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source=mydb.db; Version = 3; New = True; Compress = True;");
    try
    {
        connection.Open();
        //Console.WriteLine("DB found");
    }
    catch
    {
        Console.WriteLine("DB notfound");
    }
    return connection;
}

static void ReadData(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command;
    command= myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, * FROM customer";

    reader = command.ExecuteReader();
    while (reader.Read())
    {
        string readerRowID = reader["rowid"].ToString();
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);
        Console.WriteLine($"{readerRowID}. Full name: {readerStringFirstName} {readerStringLastName}; Date of birth {readerStringDoB}");
    }
    myConnection.Close();
}

static void InsertCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string fName, lName, Dob;
    Console.WriteLine("Ender first name:");
    fName= Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName= Console.ReadLine();
    Console.WriteLine("Enter date of birth(mm-dd-yyyy):");
    Dob= Console.ReadLine();

    command= myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer(firstName, lastName, dateOfBirth) " +
        $"VALUES ('{fName}', '{lName}', '{Dob}')";

    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Row inserted: {rowInserted}");
    ReadData(myConnection);
} 

static void RemoveCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string idToDelete;
    Console.WriteLine("Enter an id to delete a customer:");
    idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {idToDelete}";
    int rowDeleted = command.ExecuteNonQuery();
    Console.WriteLine($"{rowDeleted} was removed");
    ReadData(myConnection);
}

static void FindCustomer(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;
    string searchName;
    Console.WriteLine("Enter the first name to display customer data:");
    searchName= Console.ReadLine();
    command = myConnection.CreateCommand();
    command.CommandText = $"SELECT customer.rowid, customer.firstName, customer.lastName, status.statusType " +
         $"FROM customerStatus " +
         $"JOIN customer ON customer.rowid = customerStatus.customerId " +
         $"JOIN status ON status.rowid = customerStatus.statusId " +
         $"WHERE firstname LIKE '{searchName}'";
    reader = command.ExecuteReader();
    while (reader.Read())
    {
        string readerRowID = reader["rowid"].ToString();
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringStatus = reader.GetString(3);
        Console.WriteLine($"Search result: ID: {readerRowID}. {readerStringFirstName} {readerStringLastName}. Status: {readerStringStatus}");
    }
    myConnection.Close();
}