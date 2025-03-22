package einkaufsliste;

import jakarta.xml.bind.JAXBException;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableRowSorter;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.*;

public class mainWindow extends JFrame{
    private JComboBox comboBox1;
    private JComboBox comboBox2;
    private JButton button1;
    private JButton button2;
    private JTable table1;
    private JSlider slider1;
    private JPanel panel1;
    private JButton button3;
    private JButton button4;

    public mainWindow() {
        super("Einkaufsliste");
        setSize(1000, 800);
        setVisible(true);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setResizable(true);
        setContentPane(panel1);

        Locale.setDefault( new Locale("en") );
        ResourceBundle bundle = ResourceBundle.getBundle("product");

        DefaultTableModel model = new DefaultTableModel();
        table1.setModel(model);


        model.addColumn(bundle.getString("Produkt"));
        model.addColumn(bundle.getString("Anzahl"));



        JMenuBar mb = new JMenuBar();
        JMenu m = new JMenu(bundle.getString("Datei"));
        JMenuItem mi1 = new JMenuItem(bundle.getString("Neu"));
        JMenuItem mi2 = new JMenuItem(bundle.getString("Speichern"));
        JMenuItem mi3 = new JMenuItem(bundle.getString("Öffnen"));
        JMenuItem mi4 = new JMenuItem(bundle.getString("drucken"));

        button1.setText(bundle.getString("Hinzufügen"));
        button2.setText(bundle.getString("Entfernen"));
        button3.setText(bundle.getString("Zusammenfassen"));
        button4.setText(bundle.getString("Sortieren"));

        m.add(mi1);
        m.add(mi2);
        m.add(mi3);
        m.add(mi4);
        mb.add(m);
        setJMenuBar(mb);

        String dateipfad = bundle.getString("Resources/Produkte.csv");
        HashMap<String, ArrayList<String>> produkte = new HashMap<>();



        try (BufferedReader br = new BufferedReader(new FileReader(dateipfad))) {
            String zeile;

            while ((zeile = br.readLine()) != null) {
                String[] werte = zeile.split("; ");
                if (!produkte.containsKey(werte[0])) {
                    produkte.put(werte[0], new ArrayList<>());
                }

                if (!produkte.get(werte[0]).contains(werte[1])) {
                    produkte.get(werte[0]).add(werte[1]);
                }

            }
        } catch (IOException e) {
            System.err.println("Fehler beim Lesen der Datei: " + e.getMessage());
        }


        for (Map.Entry<String, ArrayList<String>> entry : produkte.entrySet()) {
            String produkt = entry.getKey();
            comboBox1.addItem(produkt);
        }

        comboBox1.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                String selectedProdukt = (String) comboBox1.getSelectedItem();
                comboBox2.removeAllItems();
                if (produkte.containsKey(selectedProdukt)) {
                    for (String anzahl : produkte.get(selectedProdukt)) {
                        comboBox2.addItem(anzahl);
                    }
                }
            }
        });

        button3.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                //summarize same entry
                HashMap<String, Integer> produkte = new HashMap<>();
                for (int i = 0; i < model.getRowCount(); i++) {
                    if (!produkte.containsKey(model.getValueAt(i, 0).toString())) {
                        produkte.put(model.getValueAt(i, 0).toString(), Integer.parseInt(model.getValueAt(i, 1).toString()));
                    }
                    else {
                        produkte.put(model.getValueAt(i, 0).toString(), produkte.get(model.getValueAt(i, 0).toString()) + Integer.parseInt(model.getValueAt(i, 1).toString()));
                    }
                }
                model.setRowCount(0);
                for (Map.Entry<String, Integer> entry : produkte.entrySet()) {
                    model.addRow(new Object[]{entry.getKey(), entry.getValue()});
                }
            }
        });


        button4.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                //sort list
                TableRowSorter<DefaultTableModel> sorter = new TableRowSorter<>(model);
                table1.setRowSorter(sorter);
                sorter.setSortKeys(Collections.singletonList(new RowSorter.SortKey(1, SortOrder.DESCENDING)));
            }
        });

        button1.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                model.addRow(new Object[]{comboBox2.getSelectedItem(), slider1.getValue()});
            }
        });

        button2.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                model.addRow(new Object[]{comboBox2.getSelectedItem(), slider1.getValue()});
            }
        });

        mi1.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                model.setRowCount(0);

            }
        });

        mi2.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                ArrayList<Product> produkte = new ArrayList<>();
                for (int i = 0; i < model.getRowCount(); i++) {
                    String produkt = (String) model.getValueAt(i, 0);
                    int anzahl = (int) model.getValueAt(i, 1);
                    produkte.add(new Product(String.valueOf(Collections.singletonList(produkt)), anzahl));
                }
                ProductList productlist = new ProductList(produkte);


                JFileChooser fc = new JFileChooser();
                fc.showSaveDialog(mainWindow.this);
                File f = fc.getSelectedFile();
                try {
                    productlist.writeListe(productlist, f);
                } catch (JAXBException ex) {
                    throw new RuntimeException(ex);
                }
            }
        });

        mi3.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                ProductList productlist = new ProductList();


                JFileChooser fc = new JFileChooser();
                fc.showSaveDialog(mainWindow.this);
                File f = fc.getSelectedFile();
                try {
                    productlist.readListe(f);
                    for (Product p : productlist.getListe()) {
                        model.addRow(new Object[]{p.getName().replace("[", "").replace("]", ""), p.getCount()});
                    }

                } catch (JAXBException ex) {
                    throw new RuntimeException(ex);
                }
            }
        });

        mi4.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                JFileChooser fc = new JFileChooser();
                fc.showSaveDialog(mainWindow.this);
                File f = fc.getSelectedFile();
                    try {
                        f.createNewFile();
                        System.out.print("File created successfully!");
                        List<String> lines = new ArrayList<>();
                        for (int i = 0; i < model.getRowCount(); i++) {
                            String produkt = (String) model.getValueAt(i, 0);
                            int anzahl = (int) model.getValueAt(i, 1);
                            lines.add(produkt + " " + anzahl);
                        }
                        Path file = f.toPath();
                        Files.write(file, lines, StandardCharsets.UTF_8);
                    } catch (IOException ee) {
                        ee.printStackTrace();
                    }
            }
        });







    }

    public static void main(String[] args) {
        mainWindow mainWindow = new mainWindow();
    }
}
