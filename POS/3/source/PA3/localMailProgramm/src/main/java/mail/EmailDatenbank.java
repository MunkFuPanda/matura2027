package mail;

import java.sql.*;
import java.util.ArrayList;
import java.util.Collections;

public class EmailDatenbank {
    private static Connection conn;

    public static Connection verbindenMitDatenbank() {
        try {
            Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
            conn = DriverManager.getConnection("jdbc:derby:C:/Users/Philias/Desktop/POS3/source/PA3/localMailProgramm/mail_db;create=false");
            System.out.println("Datenbankverbindung erfolgreich!");
        } catch (ClassNotFoundException e) {
            System.err.println("DERBY JDBC-Treiber nicht gefunden!");
            e.printStackTrace();
        } catch (SQLException e) {
            System.err.println("Fehler bei der Datenbankverbindung!");
            e.printStackTrace();
        }
        return conn;
    }


    public static String[] getUser(String name) {
        String[] user = new String[2];
        try {
            PreparedStatement stmt = conn.prepareStatement("SELECT * FROM PERSON WHERE NAME = ?");
            stmt.setString(1, name);
            ResultSet rs = stmt.executeQuery();
            while (rs.next()) {
                user[0] = rs.getString("name");
                user[1] = rs.getString("password");
            }
        } catch (SQLException e) {
            System.err.println("Fehler beim Abfragen der Datenbank!");
            e.printStackTrace();
        }
        return user;
    }

    public static void createUser(String name, String password) {
        try {
            PreparedStatement stmt = conn.prepareStatement("INSERT INTO PERSON (NAME, PASSWORD) VALUES (?, ?)");
            stmt.setString(1, name);
            stmt.setString(2, password);
            stmt.executeUpdate();
        } catch (SQLException e) {
            System.err.println("Fehler beim Erstellen der Datenbank!");
            e.printStackTrace();
        }
    }

    public static Mail[] getmail(String user) {
        ArrayList<Mail> mails = new ArrayList<Mail>();
        try {
            PreparedStatement stmt = conn.prepareStatement("SELECT * FROM MAIL WHERE EMPFAENGER = ?");
            stmt.setString(1, user);
            ResultSet rs = stmt.executeQuery();
            while (rs.next()) {
                Mail mail = new Mail();
                mail.setAbsender(rs.getString("sender"));
                mail.setDatum(rs.getDate("date"));
                mail.setTitel(rs.getString("titel"));
                mail.setInhalt(rs.getString("text"));
                mails.add(mail);
            }
        } catch (SQLException e) {
            System.err.println("Fehler beim Abfragen der Datenbank!");
            e.printStackTrace();
        }
        return mails.toArray(new Mail[mails.size()]);
    }

    public static Mail[] getmailfrom(String user) {
        ArrayList<Mail> mails = new ArrayList<Mail>();
        try {
            PreparedStatement stmt = conn.prepareStatement("SELECT * FROM MAIL WHERE SENDER = ?");
            stmt.setString(1, user);
            ResultSet rs = stmt.executeQuery();
            while (rs.next()) {
                Mail mail = new Mail();
                mail.setEmpfaenger(rs.getString("empfaenger"));
                mail.setDatum(rs.getDate("date"));
                mail.setTitel(rs.getString("titel"));
                mail.setInhalt(rs.getString("text"));
                mails.add(mail);
            }
        } catch (SQLException e) {
            System.err.println("Fehler beim Abfragen der Datenbank!");
            e.printStackTrace();
        }
        return mails.toArray(new Mail[mails.size()]);
    }

    public static void createMail(Mail mail) {
        try {
            PreparedStatement stmt = conn.prepareStatement("INSERT INTO MAIL (SENDER, EMPFAENGER, TITEL, TEXT, DATE) VALUES (?, ?, ?, ?, ?)");
            stmt.setString(1, mail.getAbsender());
            stmt.setString(2, mail.getEmpfaenger());
            stmt.setString(3, mail.getTitel());
            stmt.setString(4, mail.getText());
            stmt.setDate(5, mail.getDatum());
            stmt.executeUpdate();
            System.out.println("Erfolgreich erstellt");
        } catch (SQLException e) {
            System.err.println("Fehler beim Erstellen der Datenbank!");
            e.printStackTrace();
        }
    }

    public static String[] getAllUser() {
        ArrayList<String> user = new ArrayList<String>();
        try {
            PreparedStatement stmt = conn.prepareStatement("SELECT * FROM PERSON");
            ResultSet rs = stmt.executeQuery();
            while (rs.next()) {
                user.add(rs.getString("name"));
            }
        } catch (SQLException e) {
            System.err.println("Fehler beim Abfragen der Datenbank!");
            e.printStackTrace();
        }
        return user.toArray(new String[user.size()]);
    }
}
