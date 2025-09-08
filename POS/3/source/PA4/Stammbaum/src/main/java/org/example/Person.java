package org.example;

import javax.swing.*;
import java.sql.Date;
import java.time.LocalDate;
import java.util.List;


public class Person extends JFrame{

    private JPanel main;
    private JLabel label_name;
    private JLabel label_birthday;

    public Person() {};
    public Person(String name, LocalDate birthDate) {
        this.name = name;
        this.birthDate = birthDate;

        setContentPane(main);
        label_name.setText(name);
        label_birthday.setText(birthDate.toString());
        setVisible(true);
    }

    private String name;
    private LocalDate birthDate;
    private List<Person> children;

    public Person getFather() {
        return father;
    }

    public void setFather(Person father) {
        this.father = father;
    }

    public Person getMother() {
        return mother;
    }

    public void setMother(Person mother) {
        this.mother = mother;
    }

    private Person father;
    private Person mother;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public LocalDate getBirthDate() {
        return birthDate;
    }

    public void setBirthDate(LocalDate birthDate) {
        this.birthDate = birthDate;
    }

    public List<Person> getChildren() {
        return children;
    }

    public void setChildren(List<Person> children) {
        this.children = children;
    }

    public JPanel getMain() {
        return main;
    }

    public void setMain(JPanel main) {
        this.main = main;
    }

}
