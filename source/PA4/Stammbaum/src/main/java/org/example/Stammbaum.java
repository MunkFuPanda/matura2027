package org.example;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.List;

public class Stammbaum extends JFrame {
    private JPanel main;
    private JPanel panel_par;
    private JPanel panel_cur;
    private JPanel panel_child;
    private JMenuBar menuBar;
    private JMenu menu;
    private JMenuItem add;
    private JMenuItem export;

    private List<Person> personList = new ArrayList<>();

    // Singleton INSTANCE
    private static Stammbaum INSTANCE;


    public Stammbaum() {
        setTitle("Stammbaum");
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setSize(800, 600);
        setContentPane(main);

        // set Menu
        menuBar = new JMenuBar();
        menu = new JMenu("Options");
        add = new JMenuItem("Add");
        export = new JMenuItem("Export");
        menu.add(add);
        menu.add(export);
        menuBar.add(menu);
        setJMenuBar(menuBar);

        // Action Listener
        add.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                new AddPerson();
                loadPersons();
            }
        });


        // activate Visible
        setVisible(true);
    }

    public static Stammbaum getInstance() {
        if (INSTANCE == null) {
            INSTANCE = new Stammbaum();
        }
        return INSTANCE;
    }

    public void addPerson(Person person) {
        personList.add(person);
    }



    // private Methods
    private void loadPersons() {
        if (!personList.isEmpty()) {
            for (Person person : personList) {
                panel_cur.add(person.getMain());
                System.out.println(person.getName());
            }
        }
    }

    public static void main(String[] args) {
        Stammbaum.getInstance();
    }
}
