namespace TestProject3;

using System;
using LinqToDB.Mapping;

public class Product
{
    public int    ProductID { get; set; }

    public string Name      { get; set; }

    public int    VendorID  { get; set; }

    // public Vendor Vendor    { get; set; }

    // ... other columns ...
}