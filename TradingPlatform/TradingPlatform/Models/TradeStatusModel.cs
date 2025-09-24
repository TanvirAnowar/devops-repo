using System;

public class ApiOrderResponse
{
    public List<Order> Orders { get; set; } = new();
    public string LastTransactionID { get; set; }
}

public class Order
{
    // Define properties of your order object
    // Example:
    public int Id { get; set; }
    public string Product { get; set; }
}