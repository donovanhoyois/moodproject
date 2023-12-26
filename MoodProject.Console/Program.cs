// See https://aka.ms/new-console-template for more information

var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
Console.WriteLine(offset.Hours);
