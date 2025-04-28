package stammbaum;

import jakarta.xml.bind.JAXBException;

import javax.swing.*;
import java.awt.*;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.io.File;
import java.util.ArrayList;
import java.util.Date;
import java.util.jar.JarException;

public class mainWindow extends JFrame {
    private JPanel panel1;
    private JPanel person;
    private JPanel eltern_1;
    private JPanel eltern_2;
    private JPanel kinder;
    private JButton button1;
    private JButton button2;
    private JButton button3;
    private JButton button4;
    private ArrayList<person> persons = new ArrayList<>();



    public mainWindow() throws JAXBException {
        super("Stammbaum");
        setContentPane(panel1);
        setDefaultCloseOperation(DO_NOTHING_ON_CLOSE);
        setSize(500, 400);
        setVisible(true);
        JButton father = new JButton();
        father.setText("Vater hinzugügen");
        JButton mother = new JButton();
        mother.setText("Mutter hinzufügen");
        JButton child = new JButton();
        child.setText("Kind hinzufügen");
        JLabel jl = new JLabel();
        jl.setText("Keine Person gefunden");
        personlist pl = new personlist();

        ArrayList<person> personen = new ArrayList<>();
        personen.add(new person("test", new Date(), new Date(), 't'));
        personlist personenliste = new personlist(personen);
        personenliste.writeListe(personenliste);



        pl.readListe(new File("resources/persons.xml"));
        for (person p : pl.getListe()) {
            persons.add(p);
        }
        if (persons.isEmpty()) {
            LayoutManager mgr = new GridLayout();
            person.setLayout(mgr);
            person.add(jl);


            
        }
        else {
            jl.setText(persons.getFirst().toString());
            LayoutManager mgr = new GridLayout();
            button3.setText(persons.getFirst().toString());
            button3.setEnabled(false);


        }






        this.addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {
                personlist perlist = new personlist(persons);
                try {
                    perlist.writeListe(perlist);
                } catch (JAXBException ex) {
                    throw new RuntimeException(ex);
                }
                System.exit(0);
            }
        });
    }


    public static void main(String[] args) throws JAXBException {
        mainWindow mainWindow = new mainWindow();
    }

    public void createUIComponents() {

    }
}
