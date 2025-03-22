package birthday;

import jakarta.xml.bind.annotation.XmlAttribute;
import jakarta.xml.bind.annotation.XmlElement;

import java.awt.*;
import java.util.Date;

public class Birthday {
    Date date;
    String name;

    public Birthday() {}

    public Birthday(String name, Date date) {
        this.date = date;
        this.name = name;

    }
    @XmlAttribute(name="Name")
    public String getName() { return name; }
    public void setName(String name) {
        this.name = name;
    }
    @XmlElement
    public Date getDate() { return date; }
    public void setDate(Date date) {this.date = date; }

    public void print() {
        System.out.println(name + " " + date.toString());
    }

    public int compareTo(Birthday birthday) {
        return this.date.compareTo(birthday.date);
    }
}




