package stammbaum;

import jakarta.xml.bind.JAXB;
import jakarta.xml.bind.JAXBContext;
import jakarta.xml.bind.JAXBException;
import jakarta.xml.bind.Marshaller;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;

import java.io.File;
import java.util.ArrayList;

@XmlRootElement(name="personlist")
public class personlist {


    static ArrayList<person> birthdaylist = new ArrayList<person>();

    public personlist() {}
    public personlist(ArrayList<person> list) { this.birthdaylist = list; }


    @XmlElement(name = "Person")
    public ArrayList<person> getListe() { return birthdaylist; }

    public void writeListe(personlist e) throws JAXBException {
        JAXBContext jc = JAXBContext.newInstance(personlist.class);
        Marshaller m = jc.createMarshaller();
        m.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, Boolean.TRUE);
        m.marshal(e, new File("resources/persons.xml"));
    }

    public void readListe(File file) throws JAXBException {
        birthdaylist.clear();
        birthdaylist = JAXB.unmarshal(file, personlist.class).getListe();
        for (person p : birthdaylist) {
            System.out.println("Gelesen: " + p.getName() + ", " + p.getGender().toString());
        }
    }
}