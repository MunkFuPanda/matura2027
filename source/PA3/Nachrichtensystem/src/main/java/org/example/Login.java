package org.example;

import org.example.DatabaseConnection;

import javax.swing.*;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class Login {
    private String currentUser;

    public String authenticateUser() {
        while (true) {
            JTextField usernameField = new JTextField();
            JPasswordField passwordField = new JPasswordField();
            Object[] message = {
                    "Benutzername:", usernameField,
                    "Passwort:", passwordField
            };

            int option = JOptionPane.showConfirmDialog(null, message, "Login", JOptionPane.OK_CANCEL_OPTION);
            if (option != JOptionPane.OK_OPTION) {
                System.exit(0);
            }

            String username = usernameField.getText();
            String password = new String(passwordField.getPassword());

            if (validateUser(username, password)) {
                this.currentUser = username;
                return username;
            } else {
                JOptionPane.showMessageDialog(null, "Falsche Anmeldeinformationen!", "Fehler", JOptionPane.ERROR_MESSAGE);
            }
        }
    }

    private boolean validateUser(String username, String password) {
        try (Connection conn = DatabaseConnection.getConnection()) {
            String query = "SELECT * FROM Users WHERE username = ? AND password = ?";
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

    public String getCurrentUser() {
        return currentUser;
    }
}
