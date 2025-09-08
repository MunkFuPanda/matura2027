package stammbaum;

import jakarta.xml.bind.annotation.XmlAttribute;
import jakarta.xml.bind.annotation.XmlElement;

import java.awt.*;
import java.util.ArrayList;
import java.util.Date;

public class person {
    Date date_b;
    String name;
    Character gender;
    Date date_d;
    person father;
    person mother;
    ArrayList<person> children;

    public person() {}

    public person(String name, Date date_b, Date date_d, Character gender) {
        this.date_b = date_b;
        this.date_d = date_d;
        this.gender = gender;
        this.name = name;

    }
    @XmlAttribute(name="Name")
    public String getName() { return name; }
    public void setName(String name) {
        this.name = name;
    }
    @XmlElement
    public Date getDate_b() { return date_b; }
    public void setDate_b(Date date) {this.date_b = date; }
    @XmlElement
    public Date getDate_d() { return date_d; }
    public void setDate_d(Date date) {this.date_d = date; }
    @XmlElement
    public Character getGender() { return gender; }
    public void setGender(Character gender) {this.gender = gender; }
    @XmlElement
    public person getFather() {return father;}
    public void setFather(person p) {this.father = p;}
    @XmlElement
    public person getMother() {return mother;}
    public void setMother(person p) {this.mother = p;}
    @XmlElement
    public ArrayList<person> getChildren() {return children;}
    public void setChildren(ArrayList<person> p) {this.children = p;}

    public String toString() {
        return name + " " + date_b + " " + date_d + " " + gender;
    }
}




