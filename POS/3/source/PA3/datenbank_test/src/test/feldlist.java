package test;


import jakarta.xml.bind.JAXB;
import jakarta.xml.bind.JAXBContext;
import jakarta.xml.bind.JAXBException;
import jakarta.xml.bind.Marshaller;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;

import java.io.File;
import java.util.ArrayList;

@XmlRootElement(name="finley")
public class feldlist {


    ArrayList<feld> productlist = new ArrayList<feld>();

    public feldlist() {}
    public feldlist(ArrayList<feld> list) { this.productlist = list; }


    @XmlElement(name = "feld")
    public ArrayList<feld> getListe() { return productlist; }

    public void writeListe(feldlist e, File f) throws JAXBException {
        JAXBContext jc = JAXBContext.newInstance(feldlist.class);
        Marshaller m = jc.createMarshaller();
        m.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, Boolean.TRUE);
        m.marshal(e, f);
    }

    public void readListe(File f) throws JAXBException {
        productlist.clear();
        productlist = JAXB.unmarshal(f, feldlist.class).getListe();
    }
}