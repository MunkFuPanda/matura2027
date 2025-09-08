package mail;

import java.sql.Date;

public class Mail {
    String absender, empfaenger, titel, text;
    Date datum=new Date(new java.util.Date().getTime());



    public Mail() {
    }

    public Mail(String absender, String empfaenger, String titel, String text) {
        this.absender = absender;
        this.empfaenger = empfaenger;
        this.titel = titel;
        this.text = text;
        this.datum = datum;
    }

    public void setAbsender(String absender) {
        this.absender = absender;
    }

    public void setDatum(Date datum) {
        this.datum = datum;
    }

    public void setTitel(String titel) {
        this.titel = titel;
    }

    public void setInhalt(String inhalt) {
        this.text = inhalt;
    }

    public String getAbsender() {
        return absender;
    }

    public String getEmpfaenger() {
        return empfaenger;
    }

    public String getTitel() {
        return titel;
    }

    public String getText() {
        return text;
    }

    public Date getDatum() {
        return datum;
    }

    public void setEmpfaenger(String empfaenger) {
        this.empfaenger = empfaenger;
    }
}
