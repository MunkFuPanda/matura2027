package sudoku;

import jakarta.xml.bind.JAXB;
import jakarta.xml.bind.JAXBContext;
import jakarta.xml.bind.JAXBException;
import jakarta.xml.bind.Marshaller;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;

import java.io.File;
import java.util.ArrayList;

@XmlRootElement(name="sudoku")
public class SudokuList {


    ArrayList<Feld> productlist = new ArrayList<Feld>();

    public SudokuList() {}
    public SudokuList(ArrayList<Feld> list) { this.productlist = list; }


    @XmlElement(name = "feld")
    public ArrayList<Feld> getListe() { return productlist; }

    public void readListe(File f) throws JAXBException {
        productlist.clear();
        productlist = JAXB.unmarshal(f, SudokuList.class).getListe();
    }
}