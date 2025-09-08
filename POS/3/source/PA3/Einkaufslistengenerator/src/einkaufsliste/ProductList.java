package einkaufsliste;

import jakarta.xml.bind.JAXB;
import jakarta.xml.bind.JAXBContext;
import jakarta.xml.bind.JAXBException;
import jakarta.xml.bind.Marshaller;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;

import java.io.File;
import java.util.ArrayList;

@XmlRootElement(name="Productlist")
public class ProductList {


    ArrayList<Product> productlist = new ArrayList<Product>();

    public ProductList() {}
    public ProductList(ArrayList<Product> list) { this.productlist = list; }


    @XmlElement(name = "Product")
    public ArrayList<Product> getListe() { return productlist; }

    public void writeListe(ProductList e, File f) throws JAXBException {
        JAXBContext jc = JAXBContext.newInstance(ProductList.class);
        Marshaller m = jc.createMarshaller();
        m.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, Boolean.TRUE);
        m.marshal(e, f);
    }

    public void readListe(File f) throws JAXBException {
        productlist.clear();
        productlist = JAXB.unmarshal(f, ProductList.class).getListe();
        for (Product p : productlist) {
            System.out.println("Gelesen: " + p.getName() + ", " + p.getCount());
        }
    }

}