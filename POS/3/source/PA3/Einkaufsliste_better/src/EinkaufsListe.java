import javax.swing.*;
import javax.swing.filechooser.FileNameExtensionFilter;
import javax.swing.table.DefaultTableModel;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.*;

import jakarta.xml.bind.*;
import org.w3c.dom.Document;
import org.w3c.dom.Element;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import java.io.File;


public class EinkaufsListe extends JFrame {

    private JComboBox combo_box_food_groups;
    private JComboBox combo_box_food;
    private JPanel panel_main;
    private JPanel panel_food;
    private JButton button_add;
    private JButton buttdon_delete;
    private JTable table;
    private JTextField text_field_amount;
    private JTextField text_field_extra_item;
    private JLabel label_extra_item;
    private JLabel label_amount;
    private TreeMap<String, ArrayList<String>> products = new TreeMap<>();
    private TreeMap<String, Integer> selectedItems = new TreeMap<>();
    private DefaultTableModel model;
    static JMenuBar menuBar = new JMenuBar(); // Bar on top with the menues
    static JMenu optionMenu = new JMenu("Options"); // single menue in the bar
    static JMenu languageMenu = new JMenu("Language");
    static JMenuItem saveList;
    static JMenuItem loadList;
    static JMenuItem language_en;
    static JMenuItem language_de;
    private Locale currentLocale;
    private ResourceBundle messages;
    private String csvFile = "./resources/Produkte.csv";
    private String XML_FILE = "./resources/EinkaufsListe.xml";



    public EinkaufsListe() {
        setLanguage("de");

        // setup language options
        language_en = new JMenuItem(messages.getString("language_en"));
        language_de = new JMenuItem(messages.getString("language_de"));
        languageMenu.add(language_en);
        languageMenu.add(language_de);
        menuBar.add(languageMenu);

        // setup xml Options
        saveList = new JMenuItem("Save List");
        loadList = new JMenuItem("Load List");
        optionMenu.add(saveList); // action Listener auf saveList und file saven mit wirteXML
        optionMenu.add(loadList);
        menuBar.add(optionMenu);
        setJMenuBar(menuBar);


        combo_box_food_groups.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                updateComboBox();
            }
        });

        button_add.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                // get value of amount text field und gib ein 2. argument mit
                int amount = 1;
                if (!text_field_amount.getText().isEmpty()) {
                    amount = Integer.parseInt(text_field_amount.getText());
                }
                if (!text_field_extra_item.getText().isEmpty()) {
                    writeTableList(text_field_extra_item.getText().toString(), amount);
                } else {
                    writeTableList(combo_box_food.getSelectedItem().toString(), amount);
                }
            }
        });

        buttdon_delete.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                deleteSelectedItem();
            }
        });


        saveList.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                writeToXMLFile();
            }
        });

        loadList.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                loadFromXMLFile();
            }
        });

        model = new DefaultTableModel(new Object[][]{}, new String[]{"item", "quantity"});
        table.setModel(model);
        setVisible(true);
        initUI();
    }

    private void setLanguage(String language) {
        currentLocale = new Locale(language);
        messages = ResourceBundle.getBundle("einkauf", currentLocale);

        // CSV-Datei basierend auf Sprache setzen
        if (language.equals("de")) {
            csvFile = "./resources/Produkte.csv";
        } else {
            csvFile = "./resources/Products_en.csv";
        }
    }

    private void switchLanguage(String language) {
        setLanguage(language);
        initUI();
    }

    private void initUI() {
        setTitle(messages.getString("title"));
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(panel_main);
        setSize(800, 400);

        language_en.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                switchLanguage("en");
            }
        });

        language_de.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                switchLanguage("de");
            }
        });

        button_add.setText(messages.getString("addButton"));
        buttdon_delete.setText(messages.getString("deleteButton"));
        saveList.setText(messages.getString("saveList"));
        optionMenu.setText(messages.getString("options"));
        label_amount.setText(messages.getString("text_field_amount"));
        label_extra_item.setText(messages.getString("text_field_extra_item"));

        readCSV(csvFile);
        writeComboBox();
    }

    private void readCSV(String csvFile) {
        String line;
        String delimiter = ";"; // Trennzeichen (z. B. Komma, Semikolon)

        try (BufferedReader br = new BufferedReader(new FileReader(csvFile))) {
            while ((line = br.readLine()) != null) {
                // Zeile aufteilen in Spalten
                String[] data = line.split(delimiter);
                if (products.containsKey(data[0])) {
                    products.get(data[0]).add(data[1]);
                } else {
                    ArrayList<String> list = new ArrayList<>();
                    list.add(data[1]);
                    products.put(data[0], list);
                }

            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void writeComboBox() {
        combo_box_food_groups.setModel(new DefaultComboBoxModel(products.keySet().toArray()));
        combo_box_food.setModel(new DefaultComboBoxModel(products.get(combo_box_food_groups.getSelectedItem()).toArray()));
    }

    private void updateComboBox() {
        combo_box_food.setModel(new DefaultComboBoxModel(products.get(combo_box_food_groups.getSelectedItem()).toArray()));
    }

    private void writeTableList(String selectedItem, int amount) {
        selectedItem = selectedItem.trim().toLowerCase();
        if (selectedItems.containsKey(selectedItem)) {
            selectedItems.put(selectedItem, selectedItems.get(selectedItem) + amount);
        } else {
            selectedItems.put(selectedItem, amount);
        }
        writeTable();
    }

    private void writeTable() {
        model.setRowCount(0);
        selectedItems.forEach((key, value) -> {
            model.addRow(new Object[]{key, value});
        });
    }

    private void deleteSelectedItem() {
        int row = table.getSelectedRow();
        if (row >= 0) {
            String item = (String) model.getValueAt(row, 0);
            selectedItems.remove(item);
            writeTable();
        }
    }


    private void writeToXMLFile() {
        try {

            Liste einkaufsliste = new Liste();
            selectedItems.forEach((key, value) -> einkaufsliste.getEinkaufsliste().add(new Row(key, value)));


            JAXBContext context = JAXBContext.newInstance(Liste.class);
            Marshaller marshaller = context.createMarshaller();
            marshaller.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);
            marshaller.marshal(einkaufsliste, new File(XML_FILE));
            JOptionPane.showMessageDialog(this, "Daten gespeichert in " + XML_FILE);

        } catch (PropertyException e) {

            throw new RuntimeException(e);

        } catch (JAXBException e) {

            throw new RuntimeException(e);

        }
    }

    private void loadFromXMLFile() {
        JFileChooser fileChooser = new JFileChooser();
        fileChooser.setDialogTitle("Wähle eine XML-Datei");
        fileChooser.setFileFilter(new FileNameExtensionFilter("XML Dateien", "xml"));

        int userSelection = fileChooser.showOpenDialog(this);

        if (userSelection == JFileChooser.APPROVE_OPTION) {
            File selectedFile = fileChooser.getSelectedFile();
            try {

                JAXBContext context = JAXBContext.newInstance(Liste.class);
                Unmarshaller unmarshaller = context.createUnmarshaller();
                Liste einkaufsliste = (Liste) unmarshaller.unmarshal(selectedFile);

                for (Row row : einkaufsliste.getEinkaufsliste()) {
                    selectedItems.put(row.getItem(), row.getQuantity());
                    model.addRow(new Object[]{row.getItem(), row.getQuantity()});
                }

            } catch (JAXBException e) {

                e.printStackTrace();

            }
        }
    }

    public static void main(String[] args) {
        new EinkaufsListe();
    }
}
