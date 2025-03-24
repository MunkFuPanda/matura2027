package Terminkalender;

import jakarta.xml.bind.JAXBException;

import javax.swing.*;
import javax.swing.table.DefaultTableColumnModel;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.*;
import java.io.File;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;


public class mainWindow extends JFrame {
    private JTable table1;
    public JList list1;
    private JPanel panel1;
    static DefaultTableModel model = new DefaultTableModel();
    private static ArrayList<Termin> list = new ArrayList<>();

    private static ArrayList<String> akutelleListe = new ArrayList<>();

    public mainWindow() {
        super("PA3C");
        setContentPane(panel1);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setSize(800, 600);
        setVisible(true);

        JMenuBar mb = new JMenuBar();
        JMenu m = new JMenu("Termine");
        JMenuItem mi1 = new JMenuItem("Neuer Termin");
        JMenuItem mi2 = new JMenuItem("Termine exportieren");
        JMenuItem mi3 = new JMenuItem("Termine löschen");
        m.setMnemonic('T');
        mi1.setMnemonic('N');
        mi2.setMnemonic('E');
        mi3.setMnemonic('L');



        KeyStroke menushortcut_2 = KeyStroke.getKeyStroke(KeyEvent.VK_E, InputEvent.CTRL_DOWN_MASK);
        KeyStroke menushortcut_3 = KeyStroke.getKeyStroke(KeyEvent.VK_L, InputEvent.CTRL_DOWN_MASK);
        KeyStroke menushortcut_4 = KeyStroke.getKeyStroke(KeyEvent.VK_T, InputEvent.CTRL_DOWN_MASK);



        mi1.setAccelerator(menushortcut_2);
        mi2.setAccelerator(menushortcut_3);
        mi3.setAccelerator(menushortcut_4);




        m.add(mi1);
        m.add(mi2);
        m.add(mi3);
        mb.add(m);
        setJMenuBar(mb);

        mi1.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                dialog dialog1 = new dialog();
                renewTable();
                list1.setListData(akutelleListe.toArray());
            }
        });



        table1.setModel(model);
        model.addColumn("Datum");
        model.addColumn("Zeit");
        model.addColumn("Dauer");
        model.addColumn("Titel");
        model.addColumn("Beschreibung");
        renewTable();

        mi2.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                ArrayList<Termin> produkte = new ArrayList<>();
                for (Termin termin : list) {
                    produkte.add(termin);
                }



                TerminListe productlist = new TerminListe(produkte);


                JFileChooser fc = new JFileChooser();
                fc.showSaveDialog(mainWindow.this);
                File f = fc.getSelectedFile();
                try {
                    productlist.writeListe(productlist, f);
                } catch (JAXBException ex) {
                    ex.printStackTrace();
                }
            }
        });

        mi3.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                ArrayList<Termin> produkte = new ArrayList<>();
                table1.getSelectedRows();
                for (int i : table1.getSelectedRows()) {
                    akutelleListe.remove(list.get(i).getName());
                    produkte.add(list.get(i));
                }
                TermineDatenbank.delete(produkte);
                renewTable();
                list1.setListData(akutelleListe.toArray());
            }
        });


        list1.setListData(akutelleListe.toArray());
        pack();

    }
    public static void renewTable() {
        Date heute = new Date();
        model.setRowCount(0);
        list = TermineDatenbank.getTermin();
        akutelleListe.clear();
        for (Termin t : list) {
            model.addRow(new Object[]{t.getDate(), t.getTime(), t.getDuration(), t.getName(), t.getDescription()});
            if (t.date.getDate() == heute.getDate() && t.date.getMonth() == heute.getMonth() && t.date.getYear() == heute.getYear()) {
                akutelleListe.add(t.getName());
            }
        }

    }




    public static void main(String[] args) {
        TermineDatenbank.verbindenMitDatenbank();
        mainWindow mainWindow = new mainWindow();
    }
}
