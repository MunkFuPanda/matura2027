public class Distance {
    public static double measure(double p1_longitude, double p1_latitude,  double p2_longitude, double p2_latitude)
    {
        var R = 6378.137; // Radius of earth in km
        var dLat = p2_latitude * Math.PI / 180 - p1_latitude * Math.PI / 180;
        var dLon = p2_longitude * Math.PI / 180 - p1_longitude * Math.PI / 180;
        var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) + Math.cos(p1_latitude * Math.PI / 180) *
                Math.cos(p2_latitude * Math.PI / 180) * Math.sin(dLon / 2) * Math.sin(dLon / 2);
        var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        return R * c; // km
    }
}
