using System.Security.Cryptography;
using System.Text;
using WebApi.Models;

namespace WebApi.Helpers;

public static class HashHelper
{
    private static string Encode(double latitude, double longitude, int precision)
    {
        var base32Chars = "0123456789bcdefghjkmnpqrstuvwxyz".ToCharArray();

        var bits = new[] { 16, 8, 4, 2, 1 };

        var latInterval = new[] { -90.0, 90.0 };
        var lonInterval = new[] { -180.0, 180.0 };
    
        var geoHash = new StringBuilder();
        var isEven = true;
        var bit = 0;
        var ch = 0;
    
        while (geoHash.Length < precision)
        {
            double mid;
    
            if (isEven)
            {
                mid = (lonInterval[0] + lonInterval[1]) / 2;
    
                if (longitude > mid)
                {
                    ch |= bits[bit];
                    lonInterval[0] = mid;
                }
                else
                {
                    lonInterval[1] = mid;
                }
    
            }
            else
            {
                mid = (latInterval[0] + latInterval[1]) / 2;
    
                if (latitude > mid)
                {
                    ch |= bits[bit];
                    latInterval[0] = mid;
                }
                else
                {
                    latInterval[1] = mid;
                }
            }
    
            isEven = !isEven;
    
            if (bit < 4)
            {
                bit++;
            }
            else
            {
                geoHash.Append(base32Chars[ch]);
                bit = 0;
                ch = 0;
            }
        }
    
        return geoHash.ToString();
    }

    public static string GetGeoHash(this LocationCords cords, int precision = 12) =>
        Encode(cords.Latitude, cords.Longitude, precision);
    
    public static byte[] GetReversedMd5Hash(this byte[] bytes)
    {
        using var md5 = MD5.Create();
        return md5.ComputeHash(bytes).Reverse().ToArray();
    }
    
    public static byte[] ToBytes(this string str) => 
        Encoding.UTF8.GetBytes(str);
    
    public static string ToBase64String(this byte[] bytes) => 
        Convert.ToBase64String(bytes);
    
    public static string ToBase64String(this string str) => 
        Convert.ToBase64String(str.ToBytes());
}