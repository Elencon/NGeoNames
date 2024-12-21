using NGeoNames.Entities;
using System;

namespace NGeoNames;

/// <summary>
/// Utility class.
/// </summary>
public static class GeoUtil
{
    private const double RADIUS_OF_EARTH = 6371000;   //Radius of the earth, in METERS
    private const double METERS_PER_MILE = 1609.344;  //Number of kilometers in an "international" mile (http://en.wikipedia.org/wiki/Mile)
    private const double METERS_PER_YARD = 0.9144;    //Number of meters in an "international" yard (http://en.wikipedia.org/wiki/Yard)

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    /// <param name="deg">Degrees.</param>
    /// <returns>Radians.</returns>
    internal static double Deg2Rad(double deg) => (Math.PI / 180.0) * deg;

    /// <summary>
    /// Calculates the distance from the source to destination location (in meters).
    /// </summary>
    /// <param name="src">The source location.</param>
    /// <param name="dest">The destination location.</param>
    /// <param name="radiusofearth">The radius of the earth in meters (default: 6371000).</param>
    /// <returns>Returns the distance, in meters, from source to destination.</returns>
    /// <remarks>
    /// Note that we use the <a href="http://en.wikipedia.org/wiki/International_System_of_Units">International
    /// System of Units (SI)</a>; units of distance are specified in meters. If you want to use imperial system (e.g.
    /// miles, nautical miles, yards, foot and whathaveyou's) you need to convert from/to meters. You can use the
    /// helper methods <see cref="MilesToMeters"/> / <see cref="MetersToMiles"/> and 
    /// <see cref="YardsToMeters"/> / <see cref="MetersToYards"/> for quick conversion.
    /// </remarks>
    internal static double DistanceTo(IGeoLocation src, IGeoLocation dest, double radiusofearth = RADIUS_OF_EARTH)
    {
        var dLat = Deg2Rad(dest.Latitude - src.Latitude);
        var dLon = Deg2Rad(dest.Longitude - src.Longitude);
        return radiusofearth * (2 * Math.Asin(Math.Min(1, Math.Sqrt(Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(Deg2Rad(src.Latitude)) * Math.Cos(Deg2Rad(dest.Latitude)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2)))));
    }

    /// <summary>
    /// Calculates a coordinate from a GeoName (latitude, longitude, altitude)
    /// </summary>
    /// <param name="n">The GeoName to determine the coordinate for.</param>
    /// <param name="radiusofearth">Radius of the earth, in meters.</param>
    /// <returns>Returns a coordinate (represented as an array of 3 doubles).</returns>
    /// <remarks>This method is for internal use (for the KdTree) only.</remarks>
    internal static double[] GetCoord(IGeoLocation n, double radiusofearth = RADIUS_OF_EARTH)
    {
        return [
            radiusofearth * Math.Cos(Deg2Rad(n.Latitude)) * Math.Cos(Deg2Rad(n.Longitude)),
            radiusofearth * Math.Cos(Deg2Rad(n.Latitude)) * Math.Sin(Deg2Rad(n.Longitude)),
            radiusofearth * Math.Sin(Deg2Rad(n.Latitude))
        ];
    }

    /// <summary>
    /// Converts meters to miles
    /// </summary>
    /// <param name="meters">The number of meters to convert to miles.</param>
    /// <returns>Returns the number of miles.</returns>
    public static double MetersToMiles(double meters) => meters / METERS_PER_MILE;

    /// <summary>
    /// Converts miles to meters
    /// </summary>
    /// <param name="miles">The number of miles to convert to meters.</param>
    /// <returns>Returns the number of meters.</returns>
    public static double MilesToMeters(double miles) => miles * METERS_PER_MILE;

    /// <summary>
    /// Converts meters to yards
    /// </summary>
    /// <param name="meters">The number of meters to convert to yards.</param>
    /// <returns>Returns the number of yards.</returns>
    public static double MetersToYards(double meters) => meters / METERS_PER_YARD;

    /// <summary>
    /// Converts yards to meters
    /// </summary>
    /// <param name="yards">The number of yards to convert to meters.</param>
    /// <returns>Returns the number of meters.</returns>
    public static double YardsToMeters(double yards) => yards * METERS_PER_YARD;
}
