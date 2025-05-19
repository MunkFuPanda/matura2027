public class Airport {
    int id;
    double lat;
    double lng;
    String name;
    String IATA;

    public Airport(int id, double lat, double lng, String name, String IATA) {
        this.id = id;
        this.lat = lat;
        this.lng = lng;
        this.name = name;
        this.IATA = IATA;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public double getLat() {
        return lat;
    }

    public void setLat(double lat) {
        this.lat = lat;
    }

    public double getLng() {
        return lng;
    }

    public void setLng(double lng) {
        this.lng = lng;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getIATA() {
        return IATA;
    }

    public void setIATA(String IATA) {
        this.IATA = IATA;
    }

    @Override
    public String toString() {
        return id + " " + lat + " " + lng + " " + name + " " + IATA;
    }
}
