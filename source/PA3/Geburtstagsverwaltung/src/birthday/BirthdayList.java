package birthday;

import jakarta.xml.bind.JAXB;
import jakarta.xml.bind.JAXBContext;
import jakarta.xml.bind.JAXBException;
import jakarta.xml.bind.Marshaller;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;

import java.io.File;
import java.util.ArrayList;

@XmlRootElement(name="Birthdaylist")
public class BirthdayList {


    ArrayList<Birthday> birthdaylist = new ArrayList<Birthday>();

    public BirthdayList() {}
    public BirthdayList(ArrayList<Birthday> list) { this.birthdaylist = list; }


    @XmlElement(name = "Product")
    public ArrayList<Birthday> getListe() { return birthdaylist; }

    public void writeListe(BirthdayList e, File f) throws JAXBException {
        JAXBContext jc = JAXBContext.newInstance(BirthdayList.class);
        Marshaller m = jc.createMarshaller();
        m.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, Boolean.TRUE);
        m.marshal(e, f);
    }

    public void readListe(File f) throws JAXBException {
        birthdaylist.clear();
        birthdaylist = JAXB.unmarshal(f, BirthdayList.class).getListe();
        for (Birthday p : birthdaylist) {
            System.out.println("Gelesen: " + p.getName() + ", " + p.getDate().toString());
        }
    }

}