package mail;

import javax.swing.*;
import java.util.Date;

public class mainDialog extends JDialog {
    private JLabel from;
    private JLabel titel;
    private JLabel date;
    private JLabel text;
    private JPanel panel1;
    private JLabel label12;

    public mainDialog(Mail mail, int i) {
        super();
        setTitle("Email");
        setContentPane(panel1);
        setSize(500, 500);
        setVisible(true);
        setDefaultCloseOperation(DISPOSE_ON_CLOSE);

        if (i == 1) {
            from.setText(mail.getAbsender());
            label12.setText("Absender");

        }
        else {
            from.setText(mail.getEmpfaenger());
            label12.setText("Empfaenger");
        }
        titel.setText(mail.getTitel());
        Date d = mail.getDatum();
        date.setText(d.toString());
        text.setText(mail.getText());
    }

}
