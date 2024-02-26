// See https://aka.ms/new-console-template for more information
using System.Globalization;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

// Calais to Dover ferry
const string combinedTransportId = "VjEoMzM7MTA4NDc3MzY2NTs4MDs4MDswKSg0NDsxMDgxNjkzNDk2OzE0ODs2MjswKUNhbGFpcy1Eb3Zlcg==";

Console.WriteLine(combinedTransportId);
var waypoint = Id_2_waypoint(combinedTransportId);
Console.WriteLine(waypoint);

Console.WriteLine("Goodbey, World!");

string Id_2_waypoint(string combinedTransportId)
{
    byte[] data = Convert.FromBase64String(combinedTransportId);
    string decodedCti = System.Text.Encoding.UTF8.GetString(data);

    var match = Regex.Match(decodedCti, @"\(([0-9]*);([0-9]*);([0-9]*);([0-9]*);([0-9]*)\)\(([0-9]*);([0-9]*);([0-9]*);([0-9]*);([0-9]*)\)");

    var tileIdStart = long.Parse(match.Groups[2].Value);
    int xStart = int.Parse(match.Groups[3].Value);
    int yStart = int.Parse((match.Groups[4].Value));
    TransformCoord(tileIdStart, xStart, yStart, out double lonStart, out double latStart);

    var tileIdEnd = long.Parse(match.Groups[7].Value);
    int xEnd = int.Parse(match.Groups[8].Value);
    int yEnd = int.Parse((match.Groups[9].Value));
    TransformCoord(tileIdEnd, xEnd, yEnd, out double lonEnd, out double latEnd);

    return string.Create(CultureInfo.InvariantCulture, $"combinedTransport={latStart},{lonStart},{latEnd},{lonEnd}");
}

void TransformCoord(long tileId, int x, int y, out double longitude, out double latitide)
{
    var xSmart = ((tileId >> 16) & 0xffff) * 254 + x;
    var ySmart = ((tileId) & 0xffff) * 254 + y;

    var xMercator = xSmart * 4.809543 - 20015087;
    var yMercator = ySmart * 4.809543 - 20015087;

    longitude = (180.0 / Math.PI) * (xMercator / 6371000.0);
    latitide = (180.0 / Math.PI) * (Math.Atan(Math.Exp(yMercator / 6371000.0)) - (Math.PI / 4.0)) / 0.5;
}

