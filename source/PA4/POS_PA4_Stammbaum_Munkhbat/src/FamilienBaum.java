import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Objects;

/*
    XML wurde hier nicht gemacht
    Ich war zu faul dafür.
    Einige Sachen schauen vielleicht komisch aus
    Das Layout ist basierend darauf, dass es schon eine ganze Familie
    inklusive Mutter Vater und Kinder gibt
 */

public class FamilienBaum extends JFrame {
    private JPanel panel1;
    private JButton addPerson;
    private JButton setChild;
    private JComboBox<String> selectPerson;
    private JButton setFather;
    private JButton setMother;
    private JScrollPane familyTree;
    private personPanel personPanel;
    private JPanel familyPanel;
    private person currentPerson;
    private HashMap<String, person> familyList;
    private static GridBagConstraints motherConstraints = new GridBagConstraints();
    private static GridBagConstraints fatherConstraints = new GridBagConstraints();
    private GridBagConstraints childConstraints = new GridBagConstraints();
    private static GridBagConstraints personConstraints = new GridBagConstraints();
    private HashMap<person, JButton> visiblePersons;

    public FamilienBaum() {
        setSize(600, 800);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setVisible(true);
        setContentPane(panel1);
        motherConstraints.gridx = 0;
        motherConstraints.gridy = 0;
        fatherConstraints.gridx = 2;
        fatherConstraints.gridy = 0;
        personConstraints.gridx = 1;
        personConstraints.gridy = 1;
        childConstraints.gridx = 0;
        childConstraints.gridy = 2;
        familyPanel.setLayout(new GridBagLayout());
        familyList = new HashMap<>();
        visiblePersons = new HashMap<>();
        personPanel = new personPanel();
        setNewPerson();
        setMother.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                String name = (String) JOptionPane.showInputDialog(null, "Select Person", "FamilienBaum", JOptionPane.QUESTION_MESSAGE, null, familyList.keySet().toArray(), familyList.keySet().toArray()[0]);
                setPersonAsMother(familyList.get(name));
            }
        });
        setFather.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                String name = (String) JOptionPane.showInputDialog(null, "Select Person", "FamilienBaum", JOptionPane.QUESTION_MESSAGE, null, familyList.keySet().toArray(), familyList.keySet().toArray()[0]);
                setPersonAsFather(familyList.get(name));
            }
        });
        addPerson.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                setNewPerson();
            }
        });
        setChild.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                String name = (String) JOptionPane.showInputDialog(null, "Select Person", "FamilienBaum", JOptionPane.QUESTION_MESSAGE, null, familyList.keySet().toArray(), familyList.keySet().toArray()[0]);
                setPersonAsChild(familyList.get(name));
            }
        });
        selectPerson.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                setCurrentActivePerson(familyList.get(Objects.requireNonNull(selectPerson.getSelectedItem()).toString()));
            }
        });
    }

    public static void main(String[] args) {
        FamilienBaum frame = new FamilienBaum();
    }

    public void setPersonAsMother(person person) {
        if (currentPerson == null) {
            JOptionPane.showMessageDialog(null, "Current Person has not been selected, select one first");
        }
        if (currentPerson.getDateOfBirth().isBefore(person.getDateOfBirth()) ||
                currentPerson.getDateOfBirth().isEqual(person.getDateOfBirth())) {
            JOptionPane.showMessageDialog(null, "Mother cannot be born aftere or at the same time as child");
        }
        else if(currentPerson.getMother() != null) {
            JOptionPane.showMessageDialog(null, "Cannot set Mother, Mother already exists");
        }
        else {
            currentPerson.setMother(person);
        }
        person.addChild(currentPerson);
    }

    public void setPersonAsFather(person person) {
        if (currentPerson == null) {
            JOptionPane.showMessageDialog(null, "Current Person has not been selected, select one first");
        }
        if (currentPerson.getDateOfBirth().isBefore(person.getDateOfBirth()) ||
                currentPerson.getDateOfBirth().isEqual(person.getDateOfBirth())) {
            JOptionPane.showMessageDialog(null, "Father cannot be born after or at the same time as child");
        }
        else if(currentPerson.getFather() != null) {
            JOptionPane.showMessageDialog(null, "Cannot set Father, Mother already exists");
        }
        else {
            currentPerson.setFather(person);
            System.out.println("Father set");
        }
        person.addChild(currentPerson);
    }

    public void setPersonAsChild(person person) {
        if (currentPerson == null) {
            JOptionPane.showMessageDialog(null, "Current Person has not been selected, select one first");
        }
        if (currentPerson.getDateOfBirth().isAfter(person.getDateOfBirth()) ||
        currentPerson.getDateOfBirth().isEqual(person.getDateOfBirth())) {
            JOptionPane.showMessageDialog(null, "Child cannot be born before or at the same time as parent");
        }
        if (currentPerson.getChildren().contains(person)) {
            JOptionPane.showMessageDialog(null, "Already Child of this person");
        }
        else {
            currentPerson.addChild(person);
        }
        if (currentPerson.getGender().equalsIgnoreCase("m") && (person.getFather() == null && person.getMother() == null)) {
            person.setFather(currentPerson);
        }
        else if((currentPerson.getGender().equalsIgnoreCase("w")) && (person.getFather() == null) && (person.getMother() == null)) {
            person.setMother(currentPerson);
        }
    }

    public void setCurrentActivePerson(person person) {
        if (currentPerson == person) {
            familyPanel.repaint();
            familyPanel.revalidate();
            return;
        }
        childConstraints.gridx = 0;
        childConstraints.gridy = 2;
        familyPanel.removeAll();
        currentPerson = person;
        if (person.getMother() != null) {
            JButton mother = new JButton(person.getMother().getFullName());
            visiblePersons.put(person.getMother(), mother);
            familyPanel.add(visiblePersons.get(person.getMother()), motherConstraints);
        }
        if (person.getFather() != null) {
            JButton father = new JButton(person.getFather().getFullName());
            visiblePersons.put(person.getFather(), father);
            familyPanel.add(visiblePersons.get(person.getFather()), fatherConstraints);
        }
        if (person.getChildren() != null) {
            for (person child : person.getChildren()) {
                visiblePersons.put(child, new JButton(child.getFullName()));
                familyPanel.add(visiblePersons.get(child), childConstraints);
                // this is mostly for better layout design
                if (childConstraints.gridx > 9) {
                    childConstraints.gridx = 0;
                    childConstraints.gridy += 2;
                } else {
                    childConstraints.gridx++;
                }
            }
        }
        JButton mainPerson = new JButton(person.getFullName());
        visiblePersons.put(person, new JButton(person.getFullName()));
        familyPanel.add(visiblePersons.get(person), personConstraints);

        for (JButton button : visiblePersons.values()) {
            button.addActionListener(new ActionListener() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    setCurrentActivePerson(familyList.get(button.getText()));
                }
            });
        }
        personPanel.setPersonPanel(person);
        familyPanel.repaint();
        familyPanel.revalidate();
    }

    public void setNewPerson() {
        String firstname = JOptionPane.showInputDialog("Enter first name");
        String lastname = JOptionPane.showInputDialog("Enter last name");
        String dateOfBirth = JOptionPane.showInputDialog("Enter date of birth in format YYYY-MM-DD");
        String gender = JOptionPane.showInputDialog("Enter gender as m for male or w for female");
        familyList.put(firstname + " " + lastname, new person(firstname, lastname, LocalDate.parse(dateOfBirth), gender));
        selectPerson.addItem(firstname + " " + lastname);
    }
}
