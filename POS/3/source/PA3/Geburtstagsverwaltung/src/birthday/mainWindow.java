package birthday;

import jakarta.xml.bind.JAXBException;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.awt.print.PrinterException;
import java.io.File;
import java.text.SimpleDateFormat;
import java.util.*;

public class mainWindow extends JFrame {
    private JPanel panel1;
    private JTable table1;
    private JList list1;
    private JLabel field1;
    private ResourceBundle bundle = ResourceBundle.getBundle("language");
    private JMenuItem m1 = new JMenuItem(bundle.getString("Geburtstag_hinzufügen"));
    private JMenuItem m2 = new JMenuItem(bundle.getString("Geburtstag_entfernen"));
    private JMenu fmg = new JMenu(bundle.getString("Sprache_wechseln"));
    private static DefaultTableModel model = new DefaultTableModel();
    private static ArrayList<Birthday> birthdays = new ArrayList<>();

    public mainWindow() {
        super("Geburtstagsverwaltung");
        setContentPane(panel1);
        setSize(1200, 1000);
        setVisible(true);
        setDefaultCloseOperation(DO_NOTHING_ON_CLOSE);
        Locale.setDefault( new Locale("de") );
        table1.setModel(model);
        model.addColumn(bundle.getString("Datum"));
        model.addColumn(bundle.getString("Geburtstage"));
        JMenu fm = new JMenu("Menü");
        JMenuBar mb = new JMenuBar();
        mb.add(fm);
        setJMenuBar(mb);
        fm.add(m1);
        fm.add(m2);
        mb.add(fmg);
        setJMenuBar(mb);
        JMenuItem mi1 = new JMenuItem("Deutsch");
        JMenuItem mi2 = new JMenuItem("English");
        JMenuItem mi3 = new JMenuItem("Монгол");
        fmg.add(mi1);
        fmg.add(mi2);
        fmg.add(mi3);
        mi1.addActionListener(e -> {
            updateUI(Locale.forLanguageTag("de"));
        });
        mi2.addActionListener(e -> {
            updateUI(Locale.forLanguageTag("en"));
        });
        mi3.addActionListener(e -> {
            updateUI(Locale.forLanguageTag("mn"));

        });
        field1.setText(bundle.getString("Heute_haben_Geburtstag"));


        m1.addActionListener(e -> newBirthday());
        m2.addActionListener(e -> removeBirthday());

        JMenuItem m3 = new JMenuItem("print");
        m3.addActionListener(e -> {
            try {
                table1.print();
            } catch (PrinterException ex) {
                throw new RuntimeException(ex);
            }
        });
        fm.add(m3);

        this.addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {
                save();
                System.exit(0);
            }
        });


        load();



        ArrayList<String> list = new ArrayList<>();
        Date date1 = new Date();
        SimpleDateFormat datum = new SimpleDateFormat("dd MMM yyyy");
        String formattedDate = datum.format(date1);
        for (Birthday b : birthdays) {
            SimpleDateFormat datum_bd = new SimpleDateFormat("dd MMM yyyy");
            String formattedDateBD = datum_bd.format(b.date);
            if (formattedDate.equals(formattedDateBD)) {
                list.add(b.getName());
                System.out.println(b.getName());
            }

        }
        list1.setListData(list.toArray());



    }

    private void save() {
        BirthdayList productlist = new BirthdayList(birthdays);
        File f = new File("Resources/birthdays.xml");
        try {
            productlist.writeListe(productlist, f);
        } catch (JAXBException ex) {
            throw new RuntimeException(ex);
        }
    }

    private void load() {
        BirthdayList productlist = new BirthdayList();
        try {
            File f = new File("Resources/birthdays.xml");
            try {
                productlist.readListe(f);
                for (Birthday p : productlist.getListe()) {
                    addBirthday(p);
                }
                filltable();

            } catch (JAXBException ex) {
                throw new RuntimeException(ex);
            }
        }
        catch (Exception ex) {
            return;
        }


    }

    private void updateUI (Locale locale) {
        bundle = ResourceBundle.getBundle("language", locale);
        field1.setText(bundle.getString("Heute_haben_Geburtstag"));
        m1.setText(bundle.getString("Geburtstag_hinzufügen"));
        m2.setText(bundle.getString("Geburtstag_entfernen"));
        field1.setText(bundle.getString("Heute_haben_Geburtstag"));
    }

    private void newBirthday() {
        birthday_dialog birthday_dialog = new birthday_dialog();
    }

    public static void addBirthday(Birthday birthday) {
        birthdays.add(birthday);
    }

    public static void filltable() {
        ArrayList<String> dates = new ArrayList<>();
        model.setRowCount(0);
        birthdays.sort(Birthday::compareTo);
        for (Birthday birthday : birthdays) {
            SimpleDateFormat sdf = new SimpleDateFormat("dd MMM yyyy");
            String formattedDate = sdf.format(birthday.date);
            if (dates.contains(formattedDate)) {
                model.addRow(new Object[]{"", birthday.name});
            }
            else {
                dates.add(formattedDate);
                model.addRow(new Object[]{formattedDate, birthday.name});
            }
        }
    }

    private void removeBirthday() {
        int row = table1.getSelectedRow();
        if (row == -1) return;
        birthdays.remove(row);
        birthdays.sort(Birthday::compareTo);
        filltable();
    }

    public static void main(String[] args) {
        mainWindow mainWindow = new mainWindow();
    }
}
