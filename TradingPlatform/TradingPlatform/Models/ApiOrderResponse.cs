using System;



public class ApiOrderResponse
{
    public List<Order> Orders { get; set; } = new();
    public string LastTransactionID { get; set; } = string.Empty;
}

public class Order
{
    public string Id { get; set; } = string.Empty;
    public string CreateTime { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string TradeID { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string TimeInForce { get; set; } = string.Empty;
    public string TriggerCondition { get; set; } = string.Empty;
    public string TriggerMode { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}