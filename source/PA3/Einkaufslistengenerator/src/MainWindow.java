import javax.swing.*;
import java.awt.*;
import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.*;
import java.util.stream.Collectors;

public class MainWindow extends JFrame {
    private JComboBox languageBox;
    private JComboBox typeBox;
    private JComboBox foodBox;
    private JPanel main;
    private JSlider quantity;
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

        createMenuBar();
        updateUI();
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