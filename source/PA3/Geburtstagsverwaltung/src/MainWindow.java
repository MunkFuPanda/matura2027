import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.event.ActionEvent;
import org.jdatepicker.JDatePicker;

public class MainWindow extends JFrame {
    private JPanel contentPane;
    private JTable birthdayTable;
    private DefaultTableModel tableModel;

    public MainWindow() {
        setTitle("Geburtstagsverwaltung");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setContentPane(contentPane);
        setSize(600, 400);

        initMenuBar();
        initTable();

        setVisible(true);
    }

    private void initMenuBar() {
        JMenuBar menuBar = new JMenuBar();
        JMenu fileMenu = new JMenu("File");

        JMenuItem addBirthday = new JMenuItem("Geburtstag hinzufügen");
        addBirthday.addActionListener(this::showAddBirthdayDialog);

        JMenuItem deleteBirthday = new JMenuItem("Geburtstag löschen");
        deleteBirthday.addActionListener(e -> deleteSelectedBirthday());

        JMenuItem exitMenuItem = new JMenuItem("Exit");
        exitMenuItem.addActionListener(e -> System.exit(0));

        fileMenu.add(addBirthday);
        fileMenu.add(deleteBirthday);
        fileMenu.addSeparator();
        fileMenu.add(exitMenuItem);
        menuBar.add(fileMenu);
        setJMenuBar(menuBar);
    }

    private void initTable() {
        String[] columnNames = {"Name", "Geburtstag"};
        tableModel = new DefaultTableModel(columnNames, 0);
        birthdayTable.setModel(tableModel);
    }

    private void showAddBirthdayDialog(ActionEvent e) {
        AddBirthdayDialog dialog = new AddBirthdayDialog(this, tableModel);
        dialog.setVisible(true);
    }

    private void deleteSelectedBirthday() {
        int selectedRow = birthdayTable.getSelectedRow();
        if (selectedRow >= 0) {
            tableModel.removeRow(selectedRow);
        } else {
            JOptionPane.showMessageDialog(this, "Bitte einen Eintrag auswählen.");
        }
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(MainWindow::new);
    }
}
