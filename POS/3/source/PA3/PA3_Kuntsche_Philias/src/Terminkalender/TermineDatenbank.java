package Terminkalender;

import java.sql.*;
import java.util.ArrayList;

public class TermineDatenbank {
    private static Connection conn;

    public static Connection verbindenMitDatenbank() {
        try {
            Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
            conn = DriverManager.getConnection("jdbc:derby:C:/Users/Philias/Desktop/POS3/source/PA3/PA3_Kuntsche_Philias/datenbank;create=false");
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

    public static void insertTermin(Termin termin) {
        try {
            PreparedStatement stmt = conn.prepareStatement("INSERT INTO Termine (name, date, time, duration, description) VALUES (?, ?, ?, ?, ?)");
            stmt.setString(1, termin.getName());
            stmt.setDate(2, new java.sql.Date(termin.getDate().getTime()));
            stmt.setString(3, termin.getTime().toString());
            stmt.setString(4, termin.getDuration());
            stmt.setString(5, termin.getDescription());
            stmt.executeUpdate();
            System.out.println("Termin wurde eingefügt.");
        } catch (SQLException e) {
            System.err.println("Fehler beim Einfügen des Termins!");
            e.printStackTrace();
        }
    }

    public static ArrayList<Termin> getTermin() {
        try {
            ArrayList<Termin> terminliste = new ArrayList<Termin>();
            PreparedStatement stmt = conn.prepareStatement("SELECT * FROM Termine");
            ResultSet rs = stmt.executeQuery();
            while(rs.next()) {
                terminliste.add(new Termin(rs.getTime("time"), rs.getDate("date"), rs.getString("name"), rs.getString("duration"), rs.getString("description")));

                System.out.println("Termin wurde abgerufen.");
                System.out.println(terminliste.toString());
            }
            return terminliste;
        } catch (SQLException e) {
            System.err.println("Fehler beim Abrufen des Termins!");
            e.printStackTrace();
            return null;
        }

    }

    public static void createTable() {
        try {
            Statement stmt = TermineDatenbank.conn.createStatement();
            stmt.executeUpdate("CREATE TABLE Termine (t_id INTEGER PRIMARY KEY GENERATED ALWAYS AS IDENTITY, name VARCHAR(255), date DATE, time TIME, duration VARCHAR(255), description VARCHAR(255))");
            System.out.println("Tabelle Termine wurde erstellt.");
        } catch (SQLException e) {
            System.err.println("Fehler beim Erstellen der Tabelle Termine!");
            e.printStackTrace();
        }
    }

    public static void select() {
        try {
            Statement stmt = TermineDatenbank.conn.createStatement();
            ResultSet rs = stmt.executeQuery("SELECT * FROM Termine");
            while (rs.next()) {
                System.out.println(rs.getString("name") + ", " + rs.getDate("date") + ", " + rs.getTime("time") + ", " + rs.getString("duration") + ", " + rs.getString("description"));
            }
        } catch (SQLException e) {
            System.err.println("Fehler beim Abrufen der Termine!");
            e.printStackTrace();
        }
    }

    public static void delete(ArrayList<Termin> list) {
        try {
            PreparedStatement stmt = conn.prepareStatement("DELETE FROM Termine WHERE name = ?");
            for (Termin t : list) {
                stmt.setString(1, t.getName());
                stmt.executeUpdate();
            }
            System.out.println("Termin wurde gelöscht.");
        } catch (SQLException e) {
            System.err.println("Fehler beim Löschen des Termins!");
            e.printStackTrace();
        }

    }
}
