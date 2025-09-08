import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;

import java.util.ArrayList;

@XmlRootElement
public class Liste {
    private ArrayList<Row> einkaufsliste = new ArrayList<>();

    public Liste() {}

    public Liste(ArrayList<Row> einkaufsliste) {
        this.einkaufsliste = einkaufsliste;
    }

    @XmlElement
    public ArrayList<Row> getEinkaufsliste() {
        return einkaufsliste;
    }

    public void setEinkaufsliste(ArrayList<Row> einkaufsliste) {
        this.einkaufsliste = einkaufsliste;
    }
}
