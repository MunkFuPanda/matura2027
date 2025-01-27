import javax.swing.*;
import java.awt.*;
import java.util.Locale;
import java.util.MissingResourceException;
import java.util.ResourceBundle;

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

    private static ResourceBundle bundle;

    public MainWindow() throws HeadlessException {
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);
        setTitle("Einkaufsliste");
        setSize(1200, 800);

        try {
            bundle = ResourceBundle.getBundle("einkaufsliste");
        } catch (MissingResourceException e) {
            System.err.println(e);
        }

        createMenuBar();

        languageBox.addItem(Locale.GERMAN);
        languageBox.addItem(Locale.ENGLISH);
        languageBox.addActionListener(e -> {
            Locale locale = (Locale) languageBox.getSelectedItem();
            assert locale != null;
            bundle = ResourceBundle.getBundle("einkaufsliste", locale);
            updateUI();
        });
    }

    private void updateUI() {
        setTitle(bundle.getString("titleText"));
        addButton.setText(bundle.getString("addButtonText"));
        deleteButton.setText(bundle.getString("deleteButtonText"));
        newItem.setText(bundle.getString("menuNewText"));
        loadItem.setText(bundle.getString("menuLoadText"));
        saveItem.setText(bundle.getString("menuSaveText"));
        menu.setText(bundle.getString("menuText"));
    }

    public static void main(String[] args) {
        Locale.setDefault(Locale.GERMANY);

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