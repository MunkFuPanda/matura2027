package org.example;

import javax.swing.*;

public class MainWindow extends JFrame {
    private JTabbedPane tabbedPane;
    private JTable inboxTable;
    private JTable sentTable;
    private String currentUser;

    public MainWindow(String currentUser) {
        this.currentUser = currentUser;
        setTitle("NegaSystem - Eingeloggt als: " + currentUser);
        setSize(600, 400);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLocationRelativeTo(null);

        inboxTable = new JTable();
        JScrollPane inboxScrollPane = new JScrollPane(inboxTable);
        tabbedPane.addTab("Posteingang", inboxScrollPane);

        sentTable = new JTable();
        JScrollPane sentScrollPane = new JScrollPane(sentTable);
        tabbedPane.addTab("Gesendet", sentScrollPane);

        JPanel newMessagePanel = new JPanel();
        JComboBox<String> recipientBox = new JComboBox<>();
        JTextField subjectField = new JTextField(20);
        JTextArea messageArea = new JTextArea(5, 20);
        JButton sendButton = new JButton("Senden");

        newMessagePanel.add(new JLabel("Empfänger:"));
        newMessagePanel.add(recipientBox);
        newMessagePanel.add(new JLabel("Betreff:"));
        newMessagePanel.add(subjectField);
        newMessagePanel.add(new JLabel("Nachricht:"));
        newMessagePanel.add(new JScrollPane(messageArea));
        newMessagePanel.add(sendButton);

        tabbedPane.addTab("Neue Nachricht", newMessagePanel);
        add(tabbedPane);

        MessageService.loadInbox(inboxTable, currentUser);
        MessageService.loadSentMessages(sentTable, currentUser);
    }

    public static void main(String[] args) {
        Login login = new Login();
        String user = login.authenticateUser();

        SwingUtilities.invokeLater(() -> {
            MainWindow mv = new MainWindow(user);
            mv.setVisible(true);
        });
    }
}
