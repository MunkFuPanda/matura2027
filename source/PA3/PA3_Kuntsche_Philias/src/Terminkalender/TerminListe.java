package Terminkalender;

import jakarta.xml.bind.JAXB;
import jakarta.xml.bind.JAXBContext;
import jakarta.xml.bind.JAXBException;
import jakarta.xml.bind.Marshaller;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;

import java.io.File;
import java.util.ArrayList;

@XmlRootElement(name="Terminliste")
public class TerminListe {


    ArrayList<Termin> birthdaylist = new ArrayList<Termin>();

    public TerminListe() {}
    public TerminListe(ArrayList<Termin> list) { this.birthdaylist = list; }


    @XmlElement(name = "Termin")
    public ArrayList<Termin> getListe() { return birthdaylist; }

    public void writeListe(TerminListe e, File f) throws JAXBException {
        JAXBContext jc = JAXBContext.newInstance(TerminListe.class);
        Marshaller m = jc.createMarshaller();
        m.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, Boolean.TRUE);
        m.marshal(e, f);
    }

}