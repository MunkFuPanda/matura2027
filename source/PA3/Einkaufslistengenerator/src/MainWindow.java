import javax.swing.*;
import java.awt.*;
import java.util.MissingResourceException;
import java.util.ResourceBundle;

public class MainWindow extends JFrame {
    private JComboBox languageBox;
    private JComboBox typeBox;
    private JComboBox foodBox;
    private JPanel main;
    private JSlider quantity;
    private JTable shoppingTable;
    private JButton addToListButton;
    private JTextArea foodText;

    public MainWindow() throws HeadlessException {
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);
        setTitle("Einkaufsliste");
        setSize(1200, 800);

        createMenuBar();

        try {
            UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
        } catch (Exception e) {
            e.printStackTrace();
        }

        pack();
    }

    public static void main(String[] args) {
        MainWindow eklm = new MainWindow();
        eklm.setVisible(true);

        String baseName = "resources.Produkte";

        try {
            ResourceBundle bundle = ResourceBundle.getBundle(baseName);
            System.out.println(bundle.getString("Hello"));
        } catch (MissingResourceException e) {
            System.err.println(e);
        }
    }

    private void createMenuBar() {
        JMenuBar menuBar = new JMenuBar();
        setJMenuBar(menuBar);

        JMenu menu = new JMenu("Menu");
        menuBar.add(menu);

        JMenuItem newItem = new JMenuItem("Neu");
        menu.add(newItem);
        newItem.addActionListener(e -> {
            System.out.println("New");
        });

        JMenuItem openItem = new JMenuItem("Öffnen");
        menu.add(openItem);
        openItem.addActionListener(e -> {
            System.out.println("Open");
        });

        JMenuItem saveItem = new JMenuItem("Speichern");
        menu.add(saveItem);
        saveItem.addActionListener(e -> {
            System.out.println("Save");
        });
    }
}