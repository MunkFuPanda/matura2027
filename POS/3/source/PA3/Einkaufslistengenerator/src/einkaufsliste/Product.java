package einkaufsliste;


import jakarta.xml.bind.annotation.XmlAttribute;
import jakarta.xml.bind.annotation.XmlElement;

public class Product {
    private String name;
    private int count;

    public Product() {}

    public Product(String name, int count) {
        this.name = name;
        this.count = count;
    }
    @XmlAttribute(name="Count")
    public int getCount() {
        return count;
    }
    public void setCount(int count) {
        this.count = count;
    }
    @XmlElement
    public String getName() {
        return name;
    }
    public void setName(String name) {
        this.name = name;
    }
}
