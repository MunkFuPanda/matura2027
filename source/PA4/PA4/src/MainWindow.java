import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import java.awt.*;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.sql.Connection;
import java.sql.DriverManager;
import java.util.HashMap;
import java.util.HashSet;
import java.util.stream.Collectors;

public class MainWindow extends JFrame {

    private final Connection con;

    private static MainWindow instance;
    private JPanel mainPanel;
    private JComboBox countryBox;
    private JTextField searchBox;
    private JButton addButton;
    private JTable cityTable;
    private JTree treeView;
    private JSlider kmSlider;
    private JPanel mapPanel;
    private Map map;

    private HashSet<City> cities = new HashSet<>();
    private DefaultTableModel tableModel = new DefaultTableModel(new Object[]{"Stadt", "Region", "Land", "Latitude", "Longitude"}, 0) {
        @Override
        public boolean isCellEditable(int row, int column) {
            return false; // Make the table non-editable
        }
    };

    private DefaultMutableTreeNode rootNode = new DefaultMutableTreeNode("Welt");
    private DefaultTreeModel treeModel = new DefaultTreeModel(rootNode);

    public static MainWindow getInstance() {
        return instance;
    }

    private MainWindow() {
        // Initialize the main window
        setVisible(true);
        setTitle("PA4");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(800, 1000);
        setContentPane(mainPanel);

        mapPanel.setLayout(new GridLayout(1, 1));
        cityTable.setModel(tableModel);
        treeView.setModel(treeModel);
        tableModel.setRowCount(0); // Clear any existing rows

        try {
            UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
        } catch (Exception e) {
            e.printStackTrace();
        }

        con = connectToDatabase();
        if (con == null) {
            System.err.println("Failed to connect to the database.");
            return;
        }

        addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {
                try {
                    if (con != null && !con.isClosed()) {
                        con.close();
                    }
                } catch (Exception ex) {
                    System.err.println("Error closing database connection: " + ex.getMessage());
                }
                System.exit(0);
            }
        });

        loadCountries();

        addButton.addActionListener(e -> {
            String country = (String) countryBox.getSelectedItem();
            String searchText = searchBox.getText().trim();

            if (country == null || country.isEmpty()) {
                JOptionPane.showMessageDialog(this, "Please select a country.", "Error", JOptionPane.ERROR_MESSAGE);
                return;
            }
            if (searchText.isEmpty()) {
                JOptionPane.showMessageDialog(this, "Please enter a search term.", "Error", JOptionPane.ERROR_MESSAGE);
                return;
            }

            HashSet<City> loadedCities = new HashSet<>();

            try {
                var stmt = con.prepareStatement("select * from \"Places\" where country = ? and lower(name) like ?");
                stmt.setString(1, country);
                stmt.setString(2, "%" + searchText + "%");
                var rs = stmt.executeQuery();

                // Populate the table with results
                while (rs.next()) {
                    loadedCities.add(new City(rs.getInt("PK_UID"), rs.getString("name"), rs.getString("region"), rs.getString("country"), rs.getDouble("latitude"), rs.getDouble("longitude")));
                }

                rs.close();
                stmt.close();
            } catch (Exception ex) {
                System.err.println("Error searching for cities: " + ex.getMessage());
            }

            if (loadedCities.isEmpty()) {
                JOptionPane.showMessageDialog(this, "No cities found matching the criteria.", "Info", JOptionPane.INFORMATION_MESSAGE);
                return;
            } else if (loadedCities.size() > 1) {
                City city = new Chooser(loadedCities).getChosenCity();
                if (cities.stream().noneMatch(city1 -> city1.getId() == city.getId())) {
                    cities.add(city);
                }
            } else {
                City city = loadedCities.iterator().next();
                if (cities.stream().noneMatch(city1 -> city1.getId() == city.getId())) {
                    cities.add(city);
                }
            }

            reloadComponents();
        });

        kmSlider.addChangeListener(e -> {
            map.setCitiesAndDistance(cities, kmSlider.getValue());
        });
    }

    private void reloadComponents() {
        // Clear the existing rows in the table model
        tableModel.setRowCount(0);
        cities.forEach(city -> tableModel.addRow(new Object[]{city.getName(), city.getRegion(), city.getCountry(), city.getLatitude(), city.getLongitude()}));
        // Update the map with the new set of cities
        map.setCitiesAndDistance(cities, kmSlider.getValue());

        // Optionally, you can also update the tree view or any other components if needed
        rootNode.removeAllChildren();

        HashSet<String> countryStrings = cities.stream().map(City::getCountry).collect(Collectors.toCollection(HashSet::new));
        HashMap<String, DefaultMutableTreeNode> countryNodes = new HashMap<>();
        for (String country : countryStrings) {
            DefaultMutableTreeNode countryNode = new DefaultMutableTreeNode(country);
            countryNodes.put(country, countryNode);
            rootNode.add(countryNode);
        }

        HashMap<String, DefaultMutableTreeNode> regionNodes = new HashMap<>();
        for (City city : cities) {
            DefaultMutableTreeNode countryNode = countryNodes.get(city.getCountry());
            if (countryNode != null && !regionNodes.containsKey(city.getRegion())) {
                DefaultMutableTreeNode regionNode = new DefaultMutableTreeNode(city.getRegion());
                regionNodes.put(city.getRegion(), regionNode);
                countryNode.add(regionNode);
            }
        }

        for (City city : cities) {
            DefaultMutableTreeNode regionNode = regionNodes.get(city.getRegion());
            if (regionNode != null) {
                DefaultMutableTreeNode cityNode = new DefaultMutableTreeNode(city);
                regionNode.add(cityNode);
            }
        }

        treeModel.reload();
    }

    private void loadCountries() {
        // Load countries from the database and populate the countryBox
        try {
            var stmt = con.createStatement();
            var rs = stmt.executeQuery("select distinct country from \"Places\"");
            while (rs.next()) {
                countryBox.addItem(rs.getString("country"));
            }
            rs.close();
            stmt.close();
        } catch (Exception e) {
            System.err.println("Error loading countries: " + e.getMessage());
        }
    }

    public static void main(String[] args) {
        instance = new MainWindow();
    }

    private Connection connectToDatabase() {
        try {
            // Schritt 1: JDBC-Treiber registrieren
            Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
            // Schritt 2: Connection zum Datenbanksystem herstellen
            return DriverManager.getConnection("jdbc:derby:Places");
        } catch (Exception e) {
            System.out.println(e.toString());
        }

        return null;
    }

    private void createUIComponents() {
        // This method can be used to initialize UI components if needed
        map = Map.getInstance();
    }
}
