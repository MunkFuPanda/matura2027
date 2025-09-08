import javax.swing.*;
import java.awt.*;
import java.sql.*;
import java.util.*;

public class MainWindow extends JFrame {
    private static MainWindow instance;

    private JTextField startFlughafenField;
    private JTextField endFlughafenField;
    private JButton suchenButton;
    private JLabel endFlughafenLabel;
    private JLabel startFlughafenLabel;
    private Map map;
    private JPanel mainPanel;
    private JPanel mapPanel;

    private final Connection con;

    private MainWindow() {
        // Initialize the main window
        setVisible(true);
        setTitle("Airlines");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(800, 600);
        setContentPane(mainPanel);
        instance = this;

        mapPanel.setLayout(new GridLayout(1, 1));

        con = connectToDatabase();
        if (con == null) {
            System.err.println("Failed to connect to the database.");
            return;
        }

        suchenButton.addActionListener(e -> {
            Airport startAirport = selectAirport(startFlughafenField.getText().trim());
            Airport endAirport = selectAirport(endFlughafenField.getText().trim());
            if (startAirport != null && endAirport != null) {
                startFlughafenLabel.setText(startAirport.getName());
                endFlughafenLabel.setText(endAirport.getName());
                startFlughafenField.setText(startAirport.getIata());
                endFlughafenField.setText(endAirport.getIata());

                if (startAirport.equals(endAirport)) {
                    JOptionPane.showMessageDialog(this, "Start and end airports cannot be the same.", "Error", JOptionPane.ERROR_MESSAGE);
                    return;
                }

                map.setDestination(createRoute(startAirport, endAirport));
            } else {
                JOptionPane.showMessageDialog(this, "Please select valid airports.", "Error", JOptionPane.ERROR_MESSAGE);
            }
        });
    }

    private Airport createRoute(Airport startAirport, Airport endAirport) {
        // Reset parent pointers
        startAirport.setParent(null);
        endAirport.setParent(null);

        // Dijkstra's algorithm
        HashMap<Integer, Airport> airportMap = new HashMap<>();
        HashMap<Integer, Double> distances = new HashMap<>();
        HashMap<Integer, Airport> parents = new HashMap<>();
        PriorityQueue<AirportDistance> queue = new PriorityQueue<>(Comparator.comparingDouble(ad -> ad.distance));

        // Initialize
        airportMap.put(startAirport.getId(), startAirport);
        distances.put(startAirport.getId(), 0.0);
        queue.add(new AirportDistance(startAirport, 0.0));

        while (!queue.isEmpty()) {
            AirportDistance current = queue.poll();
            Airport currAirport = current.airport;
            double currDist = current.distance;

            if (currAirport.getId() == endAirport.getId()) {
                // Reconstruct path by setting parents
                Airport step = endAirport;
                while (parents.containsKey(step.getId())) {
                    step.setParent(parents.get(step.getId()));
                    step = step.getParent();
                }
                return endAirport;
            }

            // Get all neighbors (direct connections)
            try {
                PreparedStatement stmt = con.prepareStatement("select airport2, a.latitude, a.longitude, a.iata, a.name, a.id " + "from route r join airport a on r.airport2 = a.id where r.airport1 = ?");
                stmt.setInt(1, currAirport.getId());
                ResultSet rs = stmt.executeQuery();
                while (rs.next()) {
                    int neighborId = rs.getInt("airport2");
                    if (!airportMap.containsKey(neighborId)) {
                        Airport neighbor = Airport.getAirport(rs.getInt("id"), rs.getDouble("latitude"), rs.getDouble("longitude"), rs.getString("IATA"), rs.getString("name"));
                        airportMap.put(neighborId, neighbor);
                    }
                    Airport neighbor = airportMap.get(neighborId);

                    // Calculate distance (use Haversine or simple Euclidean)
                    double edgeWeight = distanceBetween(currAirport, neighbor);
                    double newDist = currDist + edgeWeight;

                    if (!distances.containsKey(neighborId) || newDist < distances.get(neighborId)) {
                        distances.put(neighborId, newDist);
                        parents.put(neighborId, currAirport);
                        queue.add(new AirportDistance(neighbor, newDist));
                    }
                }
                rs.close();
            } catch (SQLException e) {
                e.printStackTrace();
            }
        }

        JOptionPane.showMessageDialog(this, "No route found from " + startAirport.getName() + " to " + endAirport.getName(), "Error", JOptionPane.ERROR_MESSAGE);
        return null;
    }

    // Helper class for priority queue
    private static class AirportDistance {
        Airport airport;
        double distance;

        AirportDistance(Airport airport, double distance) {
            this.airport = airport;
            this.distance = distance;
        }
    }

    // Calculate distance between two airports (Haversine formula)
    private double distanceBetween(Airport a1, Airport a2) {
        double lat1 = Math.toRadians(a1.getLat());
        double lon1 = Math.toRadians(a1.getLng());
        double lat2 = Math.toRadians(a2.getLat());
        double lon2 = Math.toRadians(a2.getLng());
        double dlat = lat2 - lat1;
        double dlon = lon2 - lon1;
        double aa = Math.sin(dlat / 2) * Math.sin(dlat / 2) + Math.cos(lat1) * Math.cos(lat2) * Math.sin(dlon / 2) * Math.sin(dlon / 2);
        double c = 2 * Math.atan2(Math.sqrt(aa), Math.sqrt(1 - aa));
        double R = 6371.0; // Earth radius in km
        return R * c;
    }

    private Connection connectToDatabase() {
        try {
            // Schritt 1: JDBC-Treiber registrieren
            Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
            // Schritt 2: Connection zum Datenbanksystem herstellen
            return DriverManager.getConnection("jdbc:derby:Airlines;create=true");
        } catch (Exception e) {
            System.out.println(e.toString());
        }

        return null;
    }

    public static MainWindow getInstance() {
        return instance;
    }

    public static void main(String[] args) {
        new MainWindow();
    }

    private void createUIComponents() {
        // This method can be used to initialize UI components if needed
        map = Map.getInstance();
    }

    private Airport selectAirport(String name) {
        HashSet<Airport> airports = new HashSet<>();

        try {
            PreparedStatement stmt = con.prepareStatement("select * from airport where upper(iata) = ?");
            stmt.setString(1, name.toUpperCase());
            ResultSet rs = stmt.executeQuery();
            while (rs.next()) {
                airports.add(Airport.getAirport(rs.getInt("id"), rs.getDouble("latitude"), rs.getDouble("longitude"), rs.getString("IATA"), rs.getString("name")));
            }

            rs.close();
        } catch (Exception e) {
            e.printStackTrace();
        }

        if (airports.size() == 1) {
            return airports.iterator().next();
        } else {
            airports.clear();

            try {
                PreparedStatement stmt = con.prepareStatement("select * from airport where upper(name) like ?");
                stmt.setString(1, "%" + name.toUpperCase() + "%");
                ResultSet rs = stmt.executeQuery();
                while (rs.next()) {
                    System.out.println("Found airport: " + rs.getString("name"));
                    airports.add(Airport.getAirport(rs.getInt("id"), rs.getDouble("latitude"), rs.getDouble("longitude"), rs.getString("IATA"), rs.getString("name")));
                }

                rs.close();
            } catch (Exception e) {
                e.printStackTrace();
            }

            if (airports.size() == 1) {
                return airports.iterator().next();
            } else if (airports.isEmpty()) {
                JOptionPane.showMessageDialog(this, "No airport found with the name: " + name, "Error", JOptionPane.ERROR_MESSAGE);
                return null;
            }

            System.out.println("Found multiple airports, showing chooser dialog.");
            // Show a dialog to choose the airport
            Chooser chooser = new Chooser(airports);
            return chooser.getSelectedAirport();
        }
    }
}
