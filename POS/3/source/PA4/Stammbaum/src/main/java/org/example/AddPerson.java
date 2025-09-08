package org.example;

import com.github.lgooddatepicker.components.DatePicker;

import javax.swing.*;
import java.awt.event.*;

public class AddPerson extends JDialog {
    private JPanel contentPane;
    private JButton buttonOK;
    private JButton buttonCancel;
    private JLabel label_name;
    private JTextField text_field_name;
    private DatePicker datepicker_birthday;
    private Stammbaum stammbaum = Stammbaum.getInstance();

    public AddPerson() {
        setContentPane(contentPane);
        setModal(true);
        getRootPane().setDefaultButton(buttonOK);

        buttonOK.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                onOK();
            }
        });

        buttonCancel.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                onCancel();
            }
        });

        // call onCancel() when cross is clicked
        setDefaultCloseOperation(DO_NOTHING_ON_CLOSE);
        addWindowListener(new WindowAdapter() {
            public void windowClosing(WindowEvent e) {
                onCancel();
            }
        });

        // call onCancel() on ESCAPE
        contentPane.registerKeyboardAction(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                onCancel();
            }
        }, KeyStroke.getKeyStroke(KeyEvent.VK_ESCAPE, 0), JComponent.WHEN_ANCESTOR_OF_FOCUSED_COMPONENT);

        pack();
        setVisible(true);
    }

    private void onOK() {
        Person person = new Person(text_field_name.getText(), datepicker_birthday.getDate());
        stammbaum.addPerson(person);
        dispose();
    }

    private void onCancel() {
        // add your code here if necessary
        dispose();
    }
}
