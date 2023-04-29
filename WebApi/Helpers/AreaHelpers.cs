using Database.Models;
using WebApi.Models;

namespace WebApi.Helpers;

internal static class AreaHelpers
{
    public static bool IsIntersects(this List<Segment> areaLines)
    {
        for (var i = 0; i < areaLines.Count; i++)
        for (var j = 0; j < areaLines.Count; j++)
        {
            if (j == i) continue;
            if (IsCrossing(areaLines[i], areaLines[j], false))
                return true;
        }

        return false;
    }

    public static bool IsOnLine(this IReadOnlyList<AreaLocation> points)
    {
        for (var i = 0; i < points.Count - 2; i++)
            if (!IsOnLine(points[i], points[i + 1], points[i + 2]))
                return false;
        return true;
    }

    private static bool IsOnLine(ILocation l1, ILocation l2, ILocation l3)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        return l3.Longitude * (l2.Latitude - l1.Latitude) - l3.Latitude * (l2.Longitude - l1.Longitude)
               == l1.Longitude * l2.Latitude - l2.Longitude * l1.Latitude;
    }

    public static bool IsIntersects(this Area area, Area areaToCheck)
    {
        var areaPoints = area.AreaLocations.ToArray();
        var areaToCheckPoints = areaToCheck.AreaLocations.ToArray();
        var areaCenter = areaPoints.GetCenterPoint();
        var areaToCheckCenter = areaToCheckPoints.GetCenterPoint();

        // Получить границы для проверки
        var areaToCheckSegments = GetSegmentsToCheck(areaPoints, new Segment(areaCenter, areaToCheckCenter));

        foreach (var point in areaToCheckPoints)
        {
            var line = new Segment(areaCenter, point);
            if (!areaToCheckSegments.Any(x => line.IsCrossing(x)))
                return true;
        }

        return false;
    }

    public static bool IsInside(this AreaLocation[] areaLocations, ILocation location, bool isStrict = true)
    {
        var areaCenter = areaLocations.GetCenterPoint();

        // Границы для проверки
        var areaToCheckSegments = GetSegmentsToCheck(areaLocations, new Segment(areaCenter, location));

        var line = new Segment(areaCenter, location);
        if (!areaToCheckSegments.Any(x => line.IsCrossing(x, isStrict)))
            return true;

        return false;
    }

    private static Segment[] GetSegmentsToCheck(AreaLocation[] areaPoints, Segment line)
    {
        // ПОЛУЧИТЬ СЕРЕДИНУ ОТРЕЗКА
        var lineCenterPoint = line.GetCenter();

        // НАЙТИ БЛИЖАЙШУЮ ТОЧКУ
        var closestPoint = GetClosestPoint(areaPoints, lineCenterPoint);

        // Получить границы для проверки
        return GetSegmentsToCheck(areaPoints, closestPoint);
    }

    private static Segment[] GetSegmentsToCheck(AreaLocation[] areaPoints, ILocation closestPoint)
    {
        return areaPoints.GetSegments()
            .Where(x => x.End.IsEqual(closestPoint) || x.Start.IsEqual(closestPoint)).ToArray();
    }

    private static ILocation GetClosestPoint(IReadOnlyList<ILocation> areaPoints, ILocation lineCenterPoint)
    {
        var closestPoint = areaPoints[0];
        var distance = lineCenterPoint.GetDistance(closestPoint);
        for (var i = 1; i < areaPoints.Count; i++)
        {
            var p = areaPoints[i];
            var d = lineCenterPoint.GetDistance(p);
            if (!(d < distance)) continue;

            closestPoint = p;
            distance = d;
        }

        return closestPoint;
    }

    public static List<Segment> GetSegments(this AreaLocation[] points)
    {
        var lines = new List<Segment>();
        for (var i = 0; i < points.Length; i++)
            lines.Add(i == points.Length - 1
                ? new Segment(points[^1], points[0])
                : new Segment(points[i], points[i + 1]));
        return lines;
    }

    private static double GetDistance(this ILocation p1, ILocation p2)
    {
        return Math.Sqrt(Math.Pow(p2.Longitude - p1.Longitude, 2) + Math.Pow(p2.Latitude - p1.Latitude, 2));
    }

    private static Location GetCenter(this Segment segment)
    {
        return new((segment.Start.Longitude + segment.End.Longitude) / 2,
            (segment.Start.Latitude + segment.End.Latitude) / 2);
    }

    private static double VectorMultiple(double ax, double ay, double bx, double by)
    {
        return ax * by - bx * ay;
    }

    private static bool IsCrossing(ILocation start1, ILocation end1, ILocation start2, ILocation end2,
        bool isStrict = true)
    {
        var v1 = VectorMultiple(end2.Longitude - start2.Longitude, end2.Latitude - start2.Latitude,
            start1.Longitude - start2.Longitude, start1.Latitude - start2.Latitude);
        var v2 = VectorMultiple(end2.Longitude - start2.Longitude, end2.Latitude - start2.Latitude,
            end1.Longitude - start2.Longitude, end1.Latitude - start2.Latitude);
        var v3 = VectorMultiple(end1.Longitude - start1.Longitude, end1.Latitude - start1.Latitude,
            start2.Longitude - start1.Longitude, start2.Latitude - start1.Latitude);
        var v4 = VectorMultiple(end1.Longitude - start1.Longitude, end1.Latitude - start1.Latitude,
            end2.Longitude - start1.Longitude, end2.Latitude - start1.Latitude);
        return isStrict ? v1 * v2 <= 0 && v3 * v4 <= 0 : v1 * v2 < 0 && v3 * v4 < 0;
    }
    
    private static bool IsCrossing(this Segment segment1, Segment segment2, bool isStrict = true)
    {
        return IsCrossing(segment1.Start, segment1.End, segment2.Start, segment2.End, isStrict);
    }

    private static Location GetCenterPoint(this IReadOnlyCollection<ILocation> points)
    {
        double sumX = 0;
        double sumY = 0;
        foreach (var point in points)
        {
            sumX += point.Longitude;
            sumY += point.Latitude;
        }

        return new Location(
            sumX / points.Count,
            sumY / points.Count);
    }
}