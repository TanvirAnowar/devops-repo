using System;

public class TradeStatus
{
    public int Id { get; set; }
    public string StatusKey { get; set; } = string.Empty;
    public string StatusValue { get; set; } = string.Empty;
    public DateTime UpdatedTime { get; set; }
}