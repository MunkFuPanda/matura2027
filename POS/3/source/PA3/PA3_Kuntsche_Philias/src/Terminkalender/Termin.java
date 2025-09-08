package Terminkalender;

import jakarta.xml.bind.annotation.XmlAttribute;
import jakarta.xml.bind.annotation.XmlElement;

import java.sql.Time;
import java.time.LocalTime;
import java.util.Date;

public class Termin {
        String time;
        Date date;
        String name;
        String duration;
        String description;

        public Termin() {}

        public Termin(Time time, Date date, String name, String duration, String description) {
            this.time = time.toString();
            this.date = date;
            this.name = name;
            this.duration = duration;
            this.description = description;

        }

    public Termin(LocalTime time, Date date, String name, String duration, String description) {
        this.time = Time.valueOf(time).toString();
        this.date = date;
        this.name = name;
        this.duration = duration;
        this.description = description;
    }

    public Termin(String name, Date date, String time, String duration, String description) {
        this.date = date;
        this.time = time.toString();
        this.duration = duration;
        this.name = name;
        this.description = description;
    }



    @XmlAttribute(name="Datum")
    public Date getDate() { return date; }
    public void setDate(Date date) {this.date = date; }
    @XmlAttribute(name="Zeit")
    public String getTime() { return time; }
    public void setTime(Time time) {this.time = time.toString(); }
    @XmlAttribute(name="Dauer")
    public String getDuration() { return duration; }
    public void setDuration(String duration) {this.duration = duration; }
        @XmlElement
        public String getName() { return name; }
        public void setName(String name) {
            this.name = name;
        }

        @XmlElement
        public String getDescription() { return description; }
        public void setDescription(String description) {this.description = description; }

}
