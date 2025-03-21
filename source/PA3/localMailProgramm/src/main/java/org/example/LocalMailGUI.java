/*
database structure spinnt bissl nach git pull
eif einmal ein select oder create ausführen in der console
dann kriegts ihr einen Fehler und die structure müsst wirder laden
 */

package org.example;

import javax.swing.*;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import javax.swing.table.DefaultTableModel;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.sql.*;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.HashMap;
import java.util.Map;



public class LocalMailGUI extends JFrame {
    private JPanel main;
    private JTabbedPane tab_main;
    private JPanel tab_write_mail;
    private JPanel tab_inbox;
    private JTextArea text_area_message;
    private JPanel tab_sent_mails;
    private JTextField text_field_title;
    private JTable table_inbox;
    private JTable table_sent_mails;
    private JComboBox combo_box_users;
    private JButton button_send_mail;

    private Connection con;
    private String currentUser;
    private int currentUserId;

    private DefaultTableModel model_inbox;
    private DefaultTableModel model_sent_mails;

    private HashMap<String, Integer> username_id = new HashMap<>();

    public LocalMailGUI() {
        setTitle("Local Mail Program");
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);
        setSize(800, 400);
        setVisible(true);

        connectToDatabase();
        if (loginUser()) {
            System.out.println("User logged in successfully: " + currentUser);
        } else {
            System.exit(0);
        }
        loadUsersInCombobox();

        model_inbox = new DefaultTableModel(new Object[][]{}, new String[]{"Sender", "Title", "Message", "Sendingdate"});
        table_inbox.setModel(model_inbox);
        loadMailsIntoInbox();

        model_sent_mails = new DefaultTableModel(new Object[][]{}, new String[]{"Reciever", "Title", "Sendingdate"});
        table_sent_mails.setModel(model_sent_mails);
        loadMailsIntoSentMails();

        button_send_mail.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                sendMail();
                loadMailsIntoInbox();
            }
        });
    }

    private void connectToDatabase() {
        try {
            con = DriverManager.getConnection(
                    "jdbc:derby:C:/_projs/lernen/localMailProgramm/src/main/resources/derby");
        } catch (SQLException e) {
            JOptionPane.showMessageDialog(null, "Database connection failed!", "Error", JOptionPane.ERROR_MESSAGE);
            e.printStackTrace();
            System.exit(1);
        }
    }

    private void loadUsersInCombobox() {
        String getUsersQuery = "SELECT id, username FROM USERS";
        try {
            Statement stmt = con.createStatement();
            ResultSet rs = stmt.executeQuery(getUsersQuery);
            while (rs.next()) {
                int userId = rs.getInt("id");
                String username = rs.getString("username");
                if (!username.equals(currentUser)) {
                    combo_box_users.addItem(username);
                    username_id.put(username, userId);
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private void loadMailsIntoInbox() {
        String getUsersQuery = "SELECT Sender, Title, Message, Sendingdate FROM MAILS where Receiver = ?";
        try {
            PreparedStatement pstmt = con.prepareStatement(getUsersQuery);
            pstmt.setInt(1, currentUserId);
            ResultSet rs = pstmt.executeQuery();
            while (rs.next()) {
                int sender = rs.getInt("sender");
                String title = rs.getString("title");
                String message = rs.getString("message");
                String dateTime = rs.getString("sendingdate");
                model_inbox.addRow(new Object[]{sender, title, message, dateTime});
                table_inbox.setModel(model_inbox);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private void loadMailsIntoSentMails() {
        String getUsersQuery = "SELECT Receiver, Title, Message, Sendingdate FROM MAILS where Sender = ?";
        try {
            PreparedStatement pstmt = con.prepareStatement(getUsersQuery);
            pstmt.setInt(1, currentUserId);
            ResultSet rs = pstmt.executeQuery();
            while (rs.next()) {
                int receiver = rs.getInt("receiver");
                String title = rs.getString("title");
                String message = rs.getString("message");
                String dateTime = rs.getString("sendingdate");
                model_sent_mails.addRow(new Object[]{receiver, title, message, dateTime});
                table_sent_mails.setModel(model_sent_mails);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private void sendMail() {
        String recipient = (String) combo_box_users.getSelectedItem();
        String title = (String) text_field_title.getText();
        String message = (String) text_area_message.getText();
        LocalDateTime now = LocalDateTime.now();
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
        String dateTime = now.format(formatter);

        String insertMailQuery = "insert into MAILS (sender, receiver, title, message, sendingdate) values (?, ?, ?, ?, ?)";

        try (PreparedStatement pstmtInsertMailQuery = con.prepareStatement(insertMailQuery)){
            pstmtInsertMailQuery.setInt(1, currentUserId);
            pstmtInsertMailQuery.setInt(2, username_id.get(recipient));
            pstmtInsertMailQuery.setString(3, title);
            pstmtInsertMailQuery.setString(4, message);
            pstmtInsertMailQuery.setString(5, dateTime);
            pstmtInsertMailQuery.executeUpdate();

            System.out.println("Mail sent");
            clearInputs();

        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private void clearInputs() {
        text_field_title.setText("");
        text_area_message.setText("");
    }

    private boolean loginUser() {
        String getUserQuery = "SELECT id, username FROM USERS WHERE username = ?";
        String getUserPassQuery = "SELECT id FROM USERS WHERE username = ? AND password = ?";
        String insertUserQuery = "INSERT INTO USERS (username, password) VALUES (?, ?)";

        while (true) {
            JPanel panel = new JPanel();
            JTextField usernameField = new JTextField(10);
            JPasswordField passwordField = new JPasswordField(10);

            panel.add(new JLabel("Username:"));
            panel.add(usernameField);
            panel.add(new JLabel("Password:"));
            panel.add(passwordField);

            int option = JOptionPane.showConfirmDialog(null, panel, "Login",
                    JOptionPane.OK_CANCEL_OPTION, JOptionPane.PLAIN_MESSAGE);

            if (option == JOptionPane.CANCEL_OPTION) {
                return false;
            }

            String username = usernameField.getText().trim();
            String password = new String(passwordField.getPassword()).trim();

            if (username.isEmpty() || password.isEmpty()) {
                JOptionPane.showMessageDialog(null, "Please enter both username and password.", "Error", JOptionPane.ERROR_MESSAGE);
                continue;
            }

            try (PreparedStatement pstmtGetUserPass = con.prepareStatement(getUserPassQuery);
                 PreparedStatement pstmtGetUser = con.prepareStatement(getUserQuery);
                 PreparedStatement pstmtInsertUser = con.prepareStatement(insertUserQuery, Statement.RETURN_GENERATED_KEYS)) {

                pstmtGetUserPass.setString(1, username);
                pstmtGetUserPass.setString(2, password);
                ResultSet userPassSet = pstmtGetUserPass.executeQuery();

                if (userPassSet.next()) {
                    currentUserId = userPassSet.getInt("id");
                    currentUser = username;
                    JOptionPane.showMessageDialog(null, "Login Successful!", "Welcome", JOptionPane.INFORMATION_MESSAGE);
                    return true;
                }

                pstmtGetUser.setString(1, username);
                ResultSet userSet = pstmtGetUser.executeQuery();

                if (userSet.next()) {
                    JOptionPane.showMessageDialog(null, "Invalid password. Please try again.", "Error", JOptionPane.ERROR_MESSAGE);
                    continue;
                }

                pstmtInsertUser.setString(1, username);
                pstmtInsertUser.setString(2, password);
                pstmtInsertUser.executeUpdate();

                ResultSet generatedKeys = pstmtInsertUser.getGeneratedKeys();
                if (generatedKeys.next()) {
                    currentUserId = generatedKeys.getInt(1);
                    currentUser = username;
                    JOptionPane.showMessageDialog(null, "User registered and logged in!", "Success", JOptionPane.INFORMATION_MESSAGE);
                    return true;
                }

            } catch (SQLException e) {
                e.printStackTrace();
                JOptionPane.showMessageDialog(null, "Database error!", "Error", JOptionPane.ERROR_MESSAGE);
                return false;
            }
        }
    }

    public static void main(String[] args) {
        new LocalMailGUI();
    }
}
