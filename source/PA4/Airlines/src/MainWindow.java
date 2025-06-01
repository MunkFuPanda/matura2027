import javax.swing.*;
import java.awt.*;
import java.sql.*;
import java.util.ArrayList;
import java.util.HashSet;

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
    private ArrayList<Airport> seenAirports = new ArrayList<>();

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
        startAirport.setParent(null);
        endAirport.setParent(null);

        try {
            PreparedStatement stmt = con.prepareStatement("select * from route where airport1 = ? and airport2 = ?");
            stmt.setInt(1, startAirport.getId());
            stmt.setInt(2, endAirport.getId());

            ResultSet rs = stmt.executeQuery();
            if (rs.next()) {
                endAirport.setParent(startAirport);
                seenAirports.clear();
                seenAirports.add(startAirport);
                return endAirport;
            } else { // no direct route found
                System.out.println("No direct route found from " + startAirport.getName() + " to " + endAirport.getName());
                if (findIndirectRoute(startAirport, endAirport)) {
                    return endAirport;
                } else {
                    JOptionPane.showMessageDialog(this, "No route found from " + startAirport.getName() + " to " + endAirport.getName(), "Error", JOptionPane.ERROR_MESSAGE);
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return null;
    }

    private boolean findIndirectRoute(Airport startAirport, Airport endAirport) {
        System.out.println("Searching for indirect route from " + startAirport.getName() + " to " + endAirport.getName());
        try {
            PreparedStatement stmt = con.prepareStatement("select airport.* from route left join airport on airport2 = airport.id where airport1 = ?");
            stmt.setInt(1, startAirport.getId());
            ResultSet rs = stmt.executeQuery();
            while (rs.next()) {
                Airport nextAirport = Airport.getAirport(rs.getInt("id"), rs.getDouble("latitude"), rs.getDouble("longitude"), rs.getString("IATA"), rs.getString("name"));
                nextAirport.setParent(startAirport);
                seenAirports.add(nextAirport);
            }
            rs.close();

            for (Airport airport : seenAirports) {
                if (airport.getIata().equals(endAirport.getIata())) {
                    System.out.println("Found indirect route from " + startAirport.getName() + " to " + endAirport.getName() + " via " + airport.getName());
                    return true;
                }
            }

            for (Airport e : seenAirports) {
                if (!seenAirports.contains(e)) {
                    if (findIndirectRoute(e, endAirport)) {
                        System.out.println("Found2 indirect route from " + startAirport.getName() + " to " + endAirport.getName() + " via " + e.getName());
                        return true;
                    }
                }
            }

        } catch (SQLException e) {
            throw new RuntimeException(e);
        }

        return false;
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
