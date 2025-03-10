package org.example;

import javax.swing.*;

public class MainWindow extends JFrame {
    private JTabbedPane tabbedPane;
    private JTable inboxTable;
    private JTable sentTable;
    private String currentUser;
    private JTabbedPane tabbedPane1;
    private JPanel panel1;
    private JPanel newMessagePanel;
    private JPanel sentMessagePanel;
    private JPanel inboxMessagePanel;
    private JButton dispatchMailButton;
    private JTextArea newMessageText;
    private JTextField recieverUserName;
    private JTextField newMessageTitel;
    private JTable inboxMessageTable;

    public MainWindow(String currentUser) {
        this.currentUser = currentUser;
        setTitle("MegaSystem - Eingeloggt als: " + currentUser);
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
        SwingUtilities.invokeLater(() -> {
            JFrame frame = new JFrame("Login");
            Login login = new Login();
            frame.setContentPane(login.panel1);
            frame.pack();
            frame.setLocationRelativeTo(null);
            frame.setVisible(true);

            // Benutzername abfragen, nachdem der Login erfolgreich war
            if (login.getCurrentUser() != null) {
                frame.dispose(); // Schließt das Login-Fenster
                MainWindow mv = new MainWindow(login.getCurrentUser());
                mv.setVisible(true);
            }
        });
    }

}

