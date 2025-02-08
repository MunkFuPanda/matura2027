import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableModel;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.time.LocalDate;
import java.time.ZoneId;
import java.time.ZonedDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;

public class Geburtagsverwaltung extends JFrame {
    private JTable BirthdayTable;
    private JPanel mainPanel;
    private DefaultTableModel tm;

    private HashSet<Locale> languages = new HashSet<>(Arrays.asList(Locale.ENGLISH, Locale.GERMAN, new Locale("cs","CZ")));
    private final HashMap<Locale, HashMap<String, HashSet<String>>> categories = new HashMap<>();

    public HashMap<Date, HashSet<String>> birthdaysList = new HashMap<>();
    private JMenu sprachenMenu;

    private static ResourceBundle bundle;
    private JMenuItem gtHinzufuegen;
    private JMenuItem gtLoeschen;
    private JMenuItem german;
    private JMenuItem english;
    private JMenuItem czech;

    public Geburtagsverwaltung() throws HeadlessException {
        setTitle("Formen");
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(mainPanel);
        setSize(1600, 800);

        JMenu fileMenu = new JMenu("Menü");
        fileMenu.setMnemonic('M');
        JMenuBar menuBar = new JMenuBar();
        menuBar.add(fileMenu);
        setJMenuBar(menuBar);

        gtHinzufuegen = new JMenuItem("Geburtstag hinzufügen");
        gtHinzufuegen.setMnemonic('H');
        gtHinzufuegen.addActionListener(e -> {
                NeuDialogChooser neuDialogChooser = new NeuDialogChooser(this);
        });
        fileMenu.add(gtHinzufuegen);

        JMenuItem gtLoeschen = new JMenuItem("Geburtstag löschen");
        gtLoeschen.setMnemonic('L');
        gtLoeschen.addActionListener(e -> {
            for (Integer x : BirthdayTable.getSelectedRows()) {
                String birthday = tm.getValueAt(x, 0).toString();
                DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd");
                ZonedDateTime zonedDateTime = ZonedDateTime.parse(birthday, formatter);
                birthdaysList.remove(zonedDateTime);
            }

            updateTable();
        });
        fileMenu.add(gtLoeschen);


        JMenu sprachenMenu = new JMenu("Sprache wechseln");
        fileMenu.setMnemonic('W');
        JMenuBar SpracheBar = new JMenuBar();
        menuBar.add(sprachenMenu);
        setJMenuBar(menuBar);

        JMenuItem german = new JMenuItem("Deutsch");
        german.setMnemonic('D');
        german.addActionListener(e -> {
            updateUI(Locale.GERMAN);
        });
        sprachenMenu.add(german);

        JMenuItem english = new JMenuItem("English");
        english.setMnemonic('E');
        english.addActionListener(e -> {
            updateUI(Locale.ENGLISH);
        });
        sprachenMenu.add(english);

        JMenuItem czech = new JMenuItem("Czech");
        czech.setMnemonic('C');
        czech.addActionListener(e -> {
            updateUI(Locale.forLanguageTag("cs"));
        });
        sprachenMenu.add(czech);

        initTable();

    }

    private void updateUI(Locale locale) {
        bundle = ResourceBundle.getBundle("mainbundle", locale);

        setTitle(bundle.getString("titleText"));
        gtHinzufuegen.setText(bundle.getString("HinzufuegenText"));
        gtLoeschen.setText(bundle.getString("LoeschenText"));
        german.setText(bundle.getString("GermanText"));
        english.setText(bundle.getString("EnglischText"));
        czech.setText(bundle.getString("CzechText"));
    }

    public void updateTable() {
        tm.setRowCount(0);
        for (Date date : birthdaysList.keySet()) {
            LocalDate localDate = date.toInstant().atZone(ZoneId.systemDefault()).toLocalDate();
            DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd");
            tm.addRow(new String[]{localDate.format(formatter), birthdaysList.get(date).toString()});
        }
    }

    private void initTable() {
        String[] columnNames = {"Geburtstag", "Name"};
        tm = new DefaultTableModel(columnNames, 0);
        BirthdayTable.setModel(tm);
    }


    public static void main(String[] args) {
        Geburtagsverwaltung m = new Geburtagsverwaltung();
        m.setVisible(true);
    }
}
