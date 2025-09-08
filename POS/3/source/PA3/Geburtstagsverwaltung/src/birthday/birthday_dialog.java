package birthday;

import javax.swing.*;
import com.toedter.calendar.JDateChooser;


public class birthday_dialog extends JDialog {
    private JPanel panel1;
    private JTextField textField1;
    private JDateChooser JDateChooser1;
    private JButton addButton;

    public birthday_dialog() {
        super();
        setContentPane(panel1);
        setVisible(true);
        setSize(400, 300);
        setDefaultCloseOperation(DISPOSE_ON_CLOSE);

        addButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent e) {
                JDateChooser1.setSize(300, 300);
                try {
                    Birthday birthday = new Birthday(textField1.getText(), JDateChooser1.getDate());
                    mainWindow.addBirthday(birthday);
                    mainWindow.filltable();
                    dispose();
                }
                catch (Exception ex) {
                    System.out.println(ex.getMessage());
                }
            }
        });
    }

    private void createUIComponents() {
        JDateChooser1 = new JDateChooser();
    }
}
