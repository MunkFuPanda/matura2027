package mail;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.util.Objects;

import static mail.EmailDatenbank.createMail;

public class mainWindow extends JFrame {
    private JTabbedPane tabbedPane1;
    private JPanel panel1;
    private JPanel panel_p;
    private JPanel panel_g;
    private JPanel panel_n;
    private JTable table_p;
    private JTable table_g;
    private JTextField textField1;
    private JTextArea textArea1;
    private JComboBox comboBox1;
    private JButton sendenButton;
    private String User;


    public mainWindow() {
        super("Email");
        setSize(800, 600);
        setVisible(true);
        setContentPane(panel1);
        setDefaultCloseOperation(EXIT_ON_CLOSE);


        DefaultTableModel model1 = new DefaultTableModel();
        table_p.setModel(model1);


        model1.addColumn("Absender");
        model1.addColumn("Datum");
        model1.addColumn("Titel");

        DefaultTableModel model2 = new DefaultTableModel();
        table_g.setModel(model2);


        model2.addColumn("Empfänger");
        model2.addColumn("Datum");
        model2.addColumn("Titel");


        //name and password
        while (true) {


            JTextField usernameField = new JTextField();
            JTextField passwordField = new JTextField();
            Object[] message = {
                    "Username:", usernameField,
                    "Password:", passwordField
            };
            JOptionPane.showConfirmDialog(null, message, "Enter Details", JOptionPane.OK_CANCEL_OPTION);
            String name = usernameField.getText();
            String password = passwordField.getText();
            if (name.isEmpty() || password.isEmpty()) {
                continue;
            }
            String[] user = EmailDatenbank.getUser(name);


            System.out.println(user[0]);
            if (user[0] == null) {
                //create User
                EmailDatenbank.createUser(name, password);
                User = name;
                break;
            }


            if (user[0].equals(name) && user[1].equals(password)) {
                User = name;
                break;
            }
            else {
                JOptionPane.showMessageDialog(null, "Falscher Name oder Passwort", "Login", JOptionPane.ERROR_MESSAGE);
                continue;
            }
        }


        System.out.println(User);
        Mail[] mail = EmailDatenbank.getmail(User);
        for (Mail mail1 : mail) {
            model1.addRow(new Object[]{mail1.getAbsender(), mail1.getDatum(), mail1.getTitel()});
        }

        Mail[] mail2 = EmailDatenbank.getmailfrom(User);
        for (Mail mail1 : mail2) {
            System.out.println(mail1.empfaenger);
            model2.addRow(new Object[]{mail1.getEmpfaenger(), mail1.getDatum(), mail1.getTitel()});
        }

        String[] users = EmailDatenbank.getAllUser();
        for (String user : users) {
            if (Objects.equals(user, User))
                continue;
            comboBox1.addItem(user);
        }


        sendenButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if (textField1.getText().isEmpty() || textArea1.getText().isEmpty()) {
                    JOptionPane.showMessageDialog(null, "Bitte alle Felder ausfüllen", "Fehler", JOptionPane.ERROR_MESSAGE);
                    return;
                }
                Mail mail = new Mail(User, comboBox1.getSelectedItem().toString(), textField1.getText(), textArea1.getText());
                createMail(mail);
                textField1.setText(null);
                textArea1.setText(null);
                JOptionPane.showMessageDialog(null, "E-Mail erfolgreich versendet", "Erfolg", JOptionPane.INFORMATION_MESSAGE);
                Mail[] mail2 = EmailDatenbank.getmailfrom(User);
                model2.setRowCount(0);
                for (Mail mail1 : mail2) {
                    model2.addRow(new Object[]{mail1.getEmpfaenger(), mail1.getDatum(), mail1.getTitel()});
                }
            }
        });

        table_p.addMouseListener(new MouseAdapter() {
            public void mouseClicked(MouseEvent e) {
                if (e.getClickCount() == 1) {
                    JTable target = (JTable) e.getSource();
                    int row = target.getSelectedRow();
                    Mail clicked_mail = mail[row];
                    mainDialog mainDialog = new mainDialog(clicked_mail, 1);

                }
            }

        });

        table_g.addMouseListener(new MouseAdapter() {
            public void mouseClicked(MouseEvent e) {
                if (e.getClickCount() == 1) {
                    JTable target = (JTable) e.getSource();
                    int row = target.getSelectedRow();
                    Mail clicked_mail = mail2[row];
                    mainDialog mainDialog = new mainDialog(clicked_mail, 2);

                }
            }

        });
    }




    public static void main(String[] args) {
        EmailDatenbank.verbindenMitDatenbank();
        mainWindow mainWindow = new mainWindow();
    }
}
