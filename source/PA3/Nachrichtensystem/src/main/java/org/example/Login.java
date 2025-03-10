package org.example;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class Login {
    private String currentUser;
    private JTextField usernameField;
    JPanel panel1;
    private JPasswordField passwordField;
    private JButton loginButton;

    public Login() {
        loginButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                authenticateUser();
            }
        });
    }

    public String authenticateUser() {
        String username = usernameField.getText();
        String password = new String(passwordField.getPassword());

        if (userExists(username)) {
            if (validateUser(username, password)) {
                this.currentUser = username;
                JOptionPane.showMessageDialog(null, "Login erfolgreich!", "Erfolg", JOptionPane.INFORMATION_MESSAGE);
                return username;
            } else {
                JOptionPane.showMessageDialog(null, "Falsches Passwort! Bitte erneut eingeben.", "Fehler", JOptionPane.ERROR_MESSAGE);
            }
        } else {
            int option = JOptionPane.showConfirmDialog(null,
                    "Benutzer existiert nicht. Möchtest du ihn erstellen?", "Neuer Benutzer", JOptionPane.YES_NO_OPTION);

            if (option == JOptionPane.YES_OPTION) {
                createUser(username, password);
                this.currentUser = username;
                return username;
            }
        }
        return null;
    }

    private boolean validateUser(String username, String password) {
        try (Connection conn = DatabaseConnection.getConnection()) {
            String query = "SELECT USER_ID FROM Users WHERE USERNAME = ? AND PASSWORD = ? LIMIT 1";
            PreparedStatement stmt = conn.prepareStatement(query);
            stmt.setString(1, username);
            stmt.setString(2, password);
            ResultSet rs = stmt.executeQuery();
            return rs.next();
        } catch (SQLException e) {
            e.printStackTrace();
            return false;
        }
    }

    private boolean userExists(String username) {
        try (Connection conn = DatabaseConnection.getConnection()) {
            String query = "SELECT USER_ID FROM Users WHERE USERNAME = ? LIMIT 1";
            PreparedStatement stmt = conn.prepareStatement(query);
            stmt.setString(1, username);
            ResultSet rs = stmt.executeQuery();
            return rs.next();
        } catch (SQLException e) {
            e.printStackTrace();
            return false;
        }
    }

    private void createUser(String username, String password) {
        try (Connection conn = DatabaseConnection.getConnection()) {
            String query = "INSERT INTO Users (USERNAME, PASSWORD) VALUES (?, ?)";
            PreparedStatement stmt = conn.prepareStatement(query);
            stmt.setString(1, username);
            stmt.setString(2, password);
            stmt.executeUpdate();
            JOptionPane.showMessageDialog(null, "Benutzer erfolgreich erstellt!", "Erfolg", JOptionPane.INFORMATION_MESSAGE);
        } catch (SQLException e) {
            e.printStackTrace();
            JOptionPane.showMessageDialog(null, "Fehler beim Erstellen des Benutzers!", "Fehler", JOptionPane.ERROR_MESSAGE);
        }
    }

    public String getCurrentUser() {
        return currentUser;
    }
}
