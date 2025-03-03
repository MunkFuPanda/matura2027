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
    private JPanel panel1;
    private JTextField passwordField;
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
        String password = passwordField.getText();
        if (validateUser(username, password)) {
            this.currentUser = username;
            return username;
        } else {
            JOptionPane.showMessageDialog(null, "Falsche Anmeldeinformationen!", "Fehler", JOptionPane.ERROR_MESSAGE);
        }
        return getCurrentUser();
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
