package sudoku;

import javax.swing.*;
import java.sql.*;

public class SudokuDatenbank {
    private static Connection conn;

    public static Connection verbindenMitDatenbank() {
        try {
            Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
            conn = DriverManager.getConnection("jdbc:derby:C:/Users/Philias/Desktop/POS3/source/PA3/Sudoku/sudoku_db;create=false");
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

    public static void speichern(String name, String sudoku) throws SQLException {
        if (conn == null) {
            System.err.println("Keine aktive Datenbankverbindung!");
            return;
        }

        String gameSql = "INSERT INTO GAME (name, field) VALUES (?, ?)";

        try (PreparedStatement pstmtGame = conn.prepareStatement(gameSql)) {
            pstmtGame.setString(1, name);
            pstmtGame.setString(2, sudoku);
            int rowsAffected = 0;
            try {
                rowsAffected = pstmtGame.executeUpdate();
            } catch (SQLException e) {
                JOptionPane.showMessageDialog(null, "Name bereits vorhanden!", "Fehler", JOptionPane.ERROR_MESSAGE);
            }


            if (rowsAffected > 0) {
                System.out.println("Sudoku gespeichert!");
            } else {
                System.out.println("Fehler: Keine Zeile wurde eingefügt.");
            }
        } catch (SQLException e) {
            System.err.println("Fehler beim Speichern des Sudokus!");
            e.printStackTrace();
        }
    }

    public static String laden(String name) throws SQLException {
        if (conn == null) {
            System.err.println("Keine aktive Datenbankverbindung!");
            return "-1";
        }

        String gameSql = "SELECT Field FROM GAME WHERE NAME = ?";
        try (PreparedStatement pstmtGame = conn.prepareStatement(gameSql)) {
            pstmtGame.setString(1, name);
            ResultSet rs = pstmtGame.executeQuery();
            if (rs.next()) {
                return rs.getString("Field");
            } else {
                return "-1";
            }
        } catch (SQLException e) {
            return "-1";
        }
    }

    public static void erstelleTabelle() {
        String createTableSQL = "CREATE TABLE GAME ("
                + "name VARCHAR(255) NOT NULL PRIMARY KEY, "
                + "field VARCHAR(255) NOT NULL)";

        try (Statement stmt = conn.createStatement()) {
            stmt.executeUpdate(createTableSQL);
            System.out.println("Tabelle 'GAME' wurde erstellt.");
        } catch (SQLException e) {
            if (e.getSQLState().equals("X0Y32")) {
                System.out.println("Tabelle 'GAME' existiert bereits.");
            } else {
                System.err.println("Fehler beim Erstellen der Tabelle!");
                e.printStackTrace();
            }
        }
    }
}
