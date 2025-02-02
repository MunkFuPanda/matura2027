import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableColumnModel;
import java.awt.*;
import java.io.*;
import java.util.*;
import java.util.stream.Collectors;

public class MainWindow extends JFrame {
    private JComboBox languageBox;
    private JComboBox typeBox;
    private JComboBox foodBox;
    private JPanel main;
    private JSlider quantitySlider;
    private JTable shoppingTable;
    private JButton addButton;
    private JTextArea foodText;
    private JButton deleteButton;

    private JMenu menu;
    private JMenuItem newItem;
    private JMenuItem loadItem;
    private JMenuItem saveItem;

    private HashSet<Locale> languages = new HashSet<>(Arrays.asList(Locale.ENGLISH, Locale.GERMAN));
    private final HashMap<Locale, HashMap<String, HashSet<String>>> categories = new HashMap<>();

    private final HashMap<String, Integer> shoppingList = new HashMap<>(); // <food, quantity>

    private DefaultTableModel tableModel;

    private static ResourceBundle bundle;

    public MainWindow() throws HeadlessException {
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);
        setTitle("Einkaufsliste");
        setSize(1200, 800);

        if (languages.stream().map(Locale::toString).collect(Collectors.toSet()).contains(getLocale().toString())) {
            setLocale(Locale.ENGLISH);
        }

        languages.forEach(language -> languageBox.addItem(language));
        languageBox.addActionListener(e -> updateUI());

        bundle = ResourceBundle.getBundle("einkaufsliste", getLocale());

        typeBox.addActionListener(e -> {
            if (!(typeBox.getItemCount() == 0)) {
                switchFoods();
            }
        });

        addButton.addActionListener(e -> {
            String food = foodBox.getSelectedItem().toString();
            int quantity = quantitySlider.getValue();

            if (!foodText.getText().isEmpty()) {
                food = foodText.getText().trim();
            }

            shoppingList.put(food, shoppingList.getOrDefault(food, 0) + quantity);
            foodText.setText("");
            quantitySlider.setValue(1);

            updateTable();
        });

        deleteButton.addActionListener(e -> {
            for (Integer x : shoppingTable.getSelectedRows()) {
                String food = tableModel.getValueAt(x, 0).toString();
                shoppingList.remove(food);
            }

            updateTable();
        });

        createMenuBar();
        newItem.addActionListener(e -> {
            shoppingList.clear();
            updateTable();
        });

        loadItem.addActionListener(e -> {
            JFileChooser chooser = new JFileChooser();
            int returnVal = chooser.showOpenDialog(main);
            if (returnVal == JFileChooser.APPROVE_OPTION) {
                File file = chooser.getSelectedFile();
                try {
                    BufferedReader reader = new BufferedReader(new FileReader(file));
                    String line;
                    while (true) {
                        if ((line = reader.readLine()) == null) break;
                        String[] words = line.split(";");
                        shoppingList.put(words[0], Integer.parseInt(words[1]));
                    }
                } catch (FileNotFoundException ex) {
                    ex.printStackTrace();
                } catch (IOException ex) {
                    ex.printStackTrace();
                }
            }

            updateTable();
        });

        saveItem.addActionListener(e -> {
            JFileChooser chooser = new JFileChooser();
            int returnVal = chooser.showSaveDialog(main);
            if (returnVal == JFileChooser.APPROVE_OPTION) {
                File file = chooser.getSelectedFile();
                try {
                    BufferedWriter writer = new BufferedWriter(new FileWriter(file));
                    for (Map.Entry<String, Integer> entry : shoppingList.entrySet()) {
                        writer.write(entry.getKey() + ";" + entry.getValue() + "\n");
                    }

                    writer.close();
                } catch (IOException ex) {
                    ex.printStackTrace();
                }
            }
        });

        updateUI();
    }

    private void updateTable() {
        tableModel.setRowCount(0);
        for (Map.Entry<String, Integer> entry : shoppingList.entrySet()) {
            tableModel.addRow(new Object[]{entry.getKey(), entry.getValue()});
        }
    }

    private void loadItems() {
        BufferedReader reader = null;
        try {
            reader = new BufferedReader(new FileReader("resources/" + bundle.getString("productsFile")));
        } catch (FileNotFoundException ex) {
            ex.printStackTrace();
        }

        String line;
        while (true) {
            try {
                if (!((line = reader.readLine()) != null)) break;
            } catch (IOException e) {
                throw new RuntimeException(e);
            }
            String[] words = line.split(";");
            categories.computeIfAbsent((Locale) languageBox.getSelectedItem(), k -> new HashMap<>()).computeIfAbsent(words[0], k -> new HashSet<>()).add(words[1]);
        }
    }

    private void switchFoods() {
        Locale locale = (Locale) languageBox.getSelectedItem();
        assert locale != null;

        HashSet<String> foods = categories.get(locale).get(typeBox.getSelectedItem().toString());
        foodBox.removeAllItems();
        foodBox.setModel(new DefaultComboBoxModel<>(foods.toArray(new String[0])));
    }

    private void updateUI() {
        Locale locale = (Locale) languageBox.getSelectedItem();
        assert locale != null;
        bundle = ResourceBundle.getBundle("einkaufsliste", locale);

        setTitle(bundle.getString("titleText"));
        addButton.setText(bundle.getString("addButtonText"));
        deleteButton.setText(bundle.getString("deleteButtonText"));
        newItem.setText(bundle.getString("menuNewText"));
        loadItem.setText(bundle.getString("menuLoadText"));
        saveItem.setText(bundle.getString("menuSaveText"));
        menu.setText(bundle.getString("menuText"));

        if (!categories.containsKey(locale)) {
            loadItems();
        }

        HashMap<String, HashSet<String>> category = categories.get(locale);
        typeBox.removeAllItems();
        category.keySet().forEach(k -> typeBox.addItem(k));
        typeBox.setSelectedIndex(0);

        tableModel = new DefaultTableModel(new String[]{bundle.getString("productColumnHeader"), bundle.getString("quantityColumnHeader")}, 0) {
            @Override
            public boolean isCellEditable(int row, int column) {
                return false;
            }
        };

        shoppingTable.setModel(tableModel);
        TableColumnModel columnModel = shoppingTable.getColumnModel();
        columnModel.getColumn(0).setPreferredWidth(200);
        columnModel.getColumn(1).setPreferredWidth(50);
        updateTable();
    }

    public static void main(String[] args) {
        new MainWindow().setVisible(true);
    }

    private void createMenuBar() {
        JMenuBar menuBar = new JMenuBar();
        setJMenuBar(menuBar);

        menu = new JMenu(bundle.getString("menuText"));
        menuBar.add(menu);

        newItem = new JMenuItem(bundle.getString("menuNewText"));
        menu.add(newItem);
        newItem.addActionListener(e -> {
            System.out.println("New");
        });

        loadItem = new JMenuItem(bundle.getString("menuLoadText"));
        menu.add(loadItem);
        loadItem.addActionListener(e -> {
            System.out.println("Open");
        });

        saveItem = new JMenuItem(bundle.getString("menuSaveText"));
        menu.add(saveItem);
        saveItem.addActionListener(e -> {
            System.out.println("Save");
        });
    }
}