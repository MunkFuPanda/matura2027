package org.example;

import javax.swing.*;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.nio.file.FileSystems;
import java.nio.file.PathMatcher;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.LinkedList;
import java.util.List;

public class FileManager extends JFrame{
    private JPanel main;
    private JTree tree;
    private JLabel label;
    private JTextField input_field;
    private JButton button_submit;
    private File dir;

    private DefaultTreeModel treeModel;
    private DefaultMutableTreeNode treeRoot;
    private List<String> fileTypes;

    public FileManager() {
        setTitle("FileManager");
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);
        setSize(800, 400);
        setVisible(true);
        dir = chosenDir();

        button_submit.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                setFileTypes();
                setGUI();
            }
        });
    }

    private void setGUI() {
        DefaultMutableTreeNode root = new DefaultMutableTreeNode(dir.getName());
        buildTree(dir, root);

        treeModel = new DefaultTreeModel(root);
        tree.setModel(treeModel);
        setContentPane(main);
        setVisible(true);
    }

    private void buildTree(File directory, DefaultMutableTreeNode parentNode) {
        // Ordner hinzufügen
        for (File subDir : getSubDirectorys(directory)) {
            DefaultMutableTreeNode dirNode = new DefaultMutableTreeNode(subDir.getName());
            parentNode.add(dirNode);
            buildTree(subDir, dirNode); // rekursiv für Unterordner
        }

        // Dateien hinzufügen
        for (String fileName : getFiles(directory, fileTypes)) {
            DefaultMutableTreeNode fileNode = new DefaultMutableTreeNode(fileName);
            parentNode.add(fileNode);
        }
    }

    private List<File> getSubDirectorys(File directory) {
        LinkedList<File> res = new LinkedList<>();
        for (File f : directory.listFiles()) {
            if (f.isDirectory()) {
                res.add(f);
            }
        }
        return res;
    }

    private List<String> getFiles(File directory, List<String> fileTypes) {
        PathMatcher matcher = FileSystems.getDefault().getPathMatcher("glob:**.{" + String.join(",", fileTypes) + "}");
        LinkedList<String> res = new LinkedList<>();
        for (File f : directory.listFiles()) {
            if (f.isFile() && matcher.matches(f.toPath())) {
                res.add(f.getName());
            }
        }
        return res;
    }

    private File chosenDir() {
        JFileChooser chooser = new JFileChooser();
        chooser.setFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
        int result = chooser.showOpenDialog(this);

        if (result == JFileChooser.APPROVE_OPTION) {
            return chooser.getSelectedFile();
        } else {
            return null;
        }
    }

    private void setFileTypes() {
        String inputText = input_field.getText();
        String[] fileTypesArr = inputText.split(",");
        fileTypes = new ArrayList<>(Arrays.stream(fileTypesArr).toList());
    }


    public static void main(String[] args) {
        new FileManager();
    }
}
