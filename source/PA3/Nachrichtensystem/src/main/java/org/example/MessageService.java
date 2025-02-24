package org.example;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class MessageService {
    public static void loadInbox(JTable table, String currentUser) {
        DefaultTableModel model = new DefaultTableModel(new String[]{"Absender", "Datum", "Betreff"}, 0);
        try (Connection conn = DatabaseConnection.getConnection()) {
            String query = "SELECT u.username, m.send_date, m.subject " +
                    "FROM Messages m JOIN Users u ON m.sender_id = u.user_id " +
                    "WHERE m.receiver_id = (SELECT user_id FROM Users WHERE username = ?)";
            PreparedStatement stmt = conn.prepareStatement(query);
            stmt.setString(1, currentUser);
            ResultSet rs = stmt.executeQuery();

            while (rs.next()) {
                model.addRow(new Object[]{rs.getString("username"), rs.getString("send_date"), rs.getString("subject")});
            }
            table.setModel(model);
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public static void loadSentMessages(JTable table, String currentUser) {
        DefaultTableModel model = new DefaultTableModel(new String[]{"Empfänger", "Datum", "Betreff"}, 0);
        try (Connection conn = DatabaseConnection.getConnection()) {
            String query = "SELECT u.username, m.send_date, m.subject " +
                    "FROM Messages m JOIN Users u ON m.receiver_id = u.user_id " +
                    "WHERE m.sender_id = (SELECT user_id FROM Users WHERE username = ?)";
            PreparedStatement stmt = conn.prepareStatement(query);
            stmt.setString(1, currentUser);
            ResultSet rs = stmt.executeQuery();

            while (rs.next()) {
                model.addRow(new Object[]{rs.getString("username"), rs.getString("send_date"), rs.getString("subject")});
            }
            table.setModel(model);
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }
}
