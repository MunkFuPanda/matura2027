package sudoku;


import jakarta.xml.bind.annotation.XmlAttribute;

public class Feld {
    private int zahl;
    private int positionY;
    private int positionX;

    public Feld() {}

    public Feld(int zahl, int positionY, int positionX) {
        this.zahl = zahl;
        this.positionY = positionY;
        this.positionX = positionX;
    }

    @XmlAttribute(name="zahl")
    public int getZahl() {
        return zahl;
    }
    public void setZahl(int zahl) {
        this.zahl = zahl;
    }

    @XmlAttribute(name="positionY")
    public int getPositionY() {
        return positionY;
    }
    public void setPositionY(int positionY) {
        this.positionY = positionY;
    }

    @XmlAttribute(name="positionX")
    public int getPositionX() {
        return positionX;
    }
    public void setPositionX(int positionX) {
        this.positionX = positionX;
    }
}
