import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class TravelingSalesman {

    private JFrame frame;
	private DefaultTableModel tableModel;
    private final List<City> cities;

    public TravelingSalesman() {
        cities = new ArrayList<>();
        initializeGUI();
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(TravelingSalesman::new);
    }

    private void initializeGUI() {
        frame = new JFrame("Traveling Salesman Problem");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setLayout(new BorderLayout());

        tableModel = new DefaultTableModel(new Object[]{"City Name", "X Coordinate", "Y Coordinate"}, 0);
		JTable table = new JTable(tableModel);

        JPanel inputPanel = getjPanel();

        frame.add(new JScrollPane(table), BorderLayout.CENTER);
        frame.add(inputPanel, BorderLayout.SOUTH);
        frame.setSize(500, 400);
        frame.setVisible(true);
    }

    private JPanel getjPanel() {
        JButton addButton = new JButton("Add City");
        addButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                addCity();
            }
        });

        JButton calculateButton = new JButton("Calculate Shortest Route");
        calculateButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                calculateShortestRoute();
            }
        });

        JPanel inputPanel = new JPanel();
        inputPanel.add(addButton);
        inputPanel.add(calculateButton);
        return inputPanel;
    }

    private void addCity() {
        String name = JOptionPane.showInputDialog(frame, "Enter City Name:");
        String xCoord = JOptionPane.showInputDialog(frame, "Enter X Coordinate:");
        String yCoord = JOptionPane.showInputDialog(frame, "Enter Y Coordinate:");

        try {
            double x = Double.parseDouble(xCoord);
            double y = Double.parseDouble(yCoord);
            City city = new City(name, x, y);
            cities.add(city);
            tableModel.addRow(new Object[]{name, x, y});
        } catch (NumberFormatException ex) {
            JOptionPane.showMessageDialog(frame, "Invalid coordinates. Please enter numeric values.");
        }
    }

    private void calculateShortestRoute() {
        if (cities.size() < 2) {
            JOptionPane.showMessageDialog(frame, "Please add at least two cities.");
            return;
        }

        List<City> bestRoute = null;
        double bestDistance = Double.MAX_VALUE;

        for (List<City> route : permute(cities)) {
            double distance = calculateTotalDistance(route);
            if (distance < bestDistance) {
                bestDistance = distance;
                bestRoute = new ArrayList<>(route);
            }
        }

		JOptionPane.showMessageDialog(frame, "Shortest Route: " + routeToString(bestRoute) + "\nTotal Distance: " + bestDistance);
    }

    private List<List<City>> permute(List<City> cities) {
        List<List<City>> permutations = new ArrayList<>();
        generatePermutations(cities, 0, permutations);
        return permutations;
    }

    private void generatePermutations(List<City> cities, int index, List<List<City>> permutations) {
        if (index == cities.size() - 1) {
            permutations.add(new ArrayList<>(cities));
            return;
        }

        for (int i = index; i < cities.size(); i++) {
            Collections.swap(cities, i, index);
            generatePermutations(cities, index + 1, permutations);
            Collections.swap(cities, i, index);
        }
    }

    private double calculateTotalDistance(List<City> route) {
        double distance = 0.0;
        for (int i = 0; i < route.size() - 1; i++) {
            distance += route.get(i).distanceTo(route.get(i + 1));
        }
        distance += route.getLast().distanceTo(route.getFirst());
        return distance;
    }

    private String routeToString(List<City> route) {
        StringBuilder sb = new StringBuilder();
        for (City city : route) {
            sb.append(city.getName()).append(" -> ");
        }
        sb.append(route.getFirst().getName()); // Back to the start
        return sb.toString();
    }

    static class City {
        private String name;
        private double x;
        private double y;

        public City(String name, double x, double y) {
            this.name = name;
            this.x = x;
            this.y = y;
        }

        public String getName() {
            return name;
        }

        public double distanceTo(City other) {
            return Math.sqrt(Math.pow(this.x - other.x, 2) + Math.pow(this.y - other.y, 2));
        }
    }
}
