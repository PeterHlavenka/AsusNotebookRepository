using LinqToDB;

namespace TestProject3;

public class DbNorthwind : LinqToDB.Data.DataConnection
{
    public DbNorthwind() : base("Northwind") { }

    public ITable<Product>  Product  => this.GetTable<Product>();
    // public ITable<Category> Category => this.GetTable<Category>();

    // ... other tables ...
}