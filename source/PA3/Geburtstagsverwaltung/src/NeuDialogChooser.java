import com.toedter.calendar.DateUtil;
import com.toedter.calendar.JDateChooser;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.ZoneId;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.HashSet;

public class NeuDialogChooser extends JDialog {
    private JDateChooser JDateChooser1;
    private JPanel panel1;
    private JTextField NameInput;
    private JLabel NameLabel;
    private JLabel DateLabel;
    private JButton addButton;

    public NeuDialogChooser(Geburtagsverwaltung geburtagsverwaltung) {
        setSize(800, 400);
        setContentPane(panel1);
        pack();
        setVisible(true);

        addButton.addActionListener(e -> {
            String name = NameInput.getText();
            Date date = JDateChooser1.getDate();

            LocalDate localDate = date.toInstant()
                    .atZone(ZoneId.systemDefault())
                    .toLocalDate();

            date = Date.from(localDate.atStartOfDay(ZoneId.systemDefault()).toInstant());

            geburtagsverwaltung.birthdaysList.computeIfAbsent(date, k -> new HashSet<>()).add(name);


            NameInput.setText("");
            JDateChooser1.setDate(null);
            geburtagsverwaltung.updateTable();

        });
    }



    private void createUIComponents() {
        JDateChooser1 = new JDateChooser();
    }
}
