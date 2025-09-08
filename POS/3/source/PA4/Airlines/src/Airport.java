import java.util.HashMap;

public class Airport {
    private Airport parent;
    private int id;
    private double lat;
    private double lng;
    private String iata;
    private String name;

    private static HashMap<Integer, Airport> airports = new HashMap<>();

    public static Airport getAirport(int id, double lat, double lng, String iata, String name) {
        Airport found = airports.get(id);
        if (found != null) {
            return found;
        }

        Airport airport = new Airport(id, lat, lng, iata, name);
        airports.put(id, airport);
        return airport;
    }

    private Airport(int id, double lat, double lng, String iata, String name) {
        this.id = id;
        this.lat = lat;
        this.lng = lng;
        this.iata = iata;
        this.name = name;
    }

    public Airport getParent() {
        return parent;
    }

    public void setParent(Airport parent) {
        this.parent = parent;
    }

    public int getId() {
        return id;
    }

    public double getLat() {
        return lat;
    }

    public double getLng() {
        return lng;
    }

    public String getIata() {
        return iata;
    }

    public String getName() {
        return name;
    }

    @Override
    public String toString() {
        return name + " (" + iata + ")";
    }
}
