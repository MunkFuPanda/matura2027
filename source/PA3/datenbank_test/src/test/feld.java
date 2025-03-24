package test;


import jakarta.xml.bind.annotation.XmlAttribute;
import jakarta.xml.bind.annotation.XmlElement;

public class feld {
    private int zahl;
    private String farbe;


    public feld() {}

    public feld(int zahl, String farbe) {
        this.zahl = zahl;
        this.farbe = farbe;

    }

    @XmlAttribute(name="zahl")
    public int getZahl() {
        return zahl;
    }
    public void setZahl(int zahl) {
        this.zahl = zahl;
    }

    @XmlElement(name="farbe")
    public String getFarbe() {
        return farbe;
    }
    public void setFarbe(String farbe) {
        this.farbe = farbe;
    }
}
