import javax.swing.*;
import java.awt.*;

/*
    aus irgendeinem Grund updated die JList nie
    das heißt, es hat Elemente enthalten,
    aber zeigt diese nie an.
 */

public class personPanel extends JPanel{
    private JPanel panel1;
    private JList<String> list1;
    private JScrollPane scrollPane;

    public personPanel() {
        panel1 = new JPanel(new BorderLayout());
        list1 = new JList();
        scrollPane = new JScrollPane(list1);

        panel1.add(scrollPane, BorderLayout.CENTER);
        setLayout(new BorderLayout());
        add(panel1, BorderLayout.CENTER);
    }

    public void setPersonPanel(person person) {
        DefaultListModel<String> dlm = new DefaultListModel<>();

        dlm.addElement("First name: " + person.getFirstName());
        dlm.addElement("Last name: " + person.getLastName());
        dlm.addElement("Age: " + person.getAge());
        dlm.addElement("DOB: " + person.getDateOfBirth());
        dlm.addElement("Gender: " + person.getGender());

        if (person.getMother() != null) {
            dlm.addElement("Mother: " + person.getMother().getFullName());
        }

        if (person.getFather() != null) {
            dlm.addElement("Father: " + person.getFather().getFullName());
        }

        if (person.getChildren() != null) {
            for (person child : person.getChildren()) {
                dlm.addElement("Child: " + child.getFullName());
            }
        }
        list1.setModel(dlm);
    }

    public void setPersonPicture() {
        //ToDo: allow for adding pictures
    }
}