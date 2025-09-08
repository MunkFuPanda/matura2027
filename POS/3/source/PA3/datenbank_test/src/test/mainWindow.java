package test;

import jakarta.xml.bind.JAXBException;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.util.ArrayList;

import static test.testDatenbank.insert;
import static test.testDatenbank.verbindenMitDatenbank;

public class mainWindow extends JFrame {
    private JTextArea textArea1;
    private JPanel panel1;
    private JButton button1;
    private JButton button2;
    private JTextField textField1;
    private JButton button3;

    public mainWindow() {
        this.setContentPane(this.panel1);
        this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        this.setSize(500, 400);
        this.setVisible(true);





        button1.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                ArrayList<String> finley = new ArrayList<>();
                finley = testDatenbank.get();
                for (int i = 0; i < finley.size(); i++) {
                    textArea1.append(finley.get(i) + "\n");
                }
            }
        });



        button2.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                feld feld = new feld();
                feld.setZahl(1);
                feld.setFarbe("rot");
                ArrayList<feld> list = new ArrayList<>();
                list.add(feld);

                feldlist productlist = new feldlist(list);


                JFileChooser fc = new JFileChooser();
                fc.showSaveDialog(mainWindow.this);
                File f = fc.getSelectedFile();
                try {
                    productlist.writeListe(productlist, f);
                } catch (JAXBException ex) {
                    throw new RuntimeException(ex);
                }
            }
        });


        button3.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                feldlist productlist = new feldlist();
                JFileChooser fc = new JFileChooser();
                fc.showOpenDialog(mainWindow.this);
                File f = fc.getSelectedFile();
                try {
                    productlist.readListe(f);
                } catch (JAXBException ex) {
                    throw new RuntimeException(ex);
                }
                textArea1.setText("");
                for (feld feld : productlist.getListe()) {
                    textArea1.append(feld.getZahl() + " " + feld.getFarbe() + "\n");
                }
            }
        });
    }



    public static void main(String[] args) {
        verbindenMitDatenbank();
        mainWindow mainWindow = new mainWindow();
    }
}
