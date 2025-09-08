package Terminkalender;

import javax.swing.*;

import com.github.lgooddatepicker.components.TimePicker;
import com.toedter.calendar.JCalendar;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class dialog extends javax.swing.JDialog {
    private JButton sendenButton;
    private JPanel panel1;
    private JCalendar JCalendar1;
    private TimePicker timepicker;
    private JTextField textField_d;
    private JTextField textField_t;
    private JTextField textField_b;

    public dialog() {
        super();
        setContentPane(panel1);
        setTitle("Neuer Termin");
        setSize(600, 600);
        setVisible(true);
        setDefaultCloseOperation(DISPOSE_ON_CLOSE);

        sendenButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if (JCalendar1.getDate() != null && timepicker.getTime() != null && textField_t.getText() != null && textField_d.getText() != null && textField_b.getText() != null) {
                    Termin termin = new Termin(timepicker.getTime(), JCalendar1.getDate(), textField_t.getText(), textField_d.getText(), textField_b.getText());
                    TermineDatenbank.insertTermin(termin);
                    mainWindow.renewTable();
                 dispose();
                }
            }
        });
    }

    private void createUIComponents() {
        JCalendar1 = new JCalendar();
        timepicker = new TimePicker();
    }
}
