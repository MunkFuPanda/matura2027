public class City {
    private int id;
    private String name;
    private String region;
    private String country;
    private double latitude;
    private double longitude;

    public City(int id, String name, String region, String country, double latitude, double longitude) {
        this.id = id;
        this.name = name;
        this.region = region;
        this.country = country;
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getRegion() {
        return region;
    }

    public String getCountry() {
        return country;
    }

    public double getLatitude() {
        return latitude;
    }

    public double getLongitude() {
        return longitude;
    }

    @Override
    public String toString() {
        return name;
    }
}
