package test;

import javax.swing.*;
import java.sql.*;
import java.util.ArrayList;

public class testDatenbank {
    private static Connection conn;

    public static Connection verbindenMitDatenbank() {
        try {
            Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
            conn = DriverManager.getConnection("jdbc:derby:C:/Users/Philias/Desktop/POS3/source/PA3/datenbank_test/datenbank;create=false");
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
    public static void insert(String name) {
        try {
            PreparedStatement statement = conn.prepareStatement("INSERT INTO TEST (name) VALUES (?)");
            statement.setString(1, name);
            statement.executeUpdate();
        } catch (SQLException e) {
            System.err.println("Fehler beim Einfügen der Daten!");
            e.printStackTrace();
        }
    }

    public static ArrayList<String> get() {
        ArrayList<String> list = new ArrayList<>();
        try {
            Statement statement = conn.createStatement();
            ResultSet resultSet = statement.executeQuery("SELECT * FROM test");
            while (resultSet.next()) {
                list.add(resultSet.getString("name"));
            }
        } catch (SQLException e) {
            System.err.println("Fehler beim Abrufen der Daten!");
            e.printStackTrace();
        }
        return list;
    }
}
