import javax.swing.*;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;

public class mainwindow extends JFrame {
    private JPanel panel2;
    private JPanel panel_tree;
    private JPanel panel_ui;
    private JComboBox comboBox;
    private JButton choose_button;
    private JTree tree;

    public mainwindow() {
        super("Dateianzeige");
        setSize(500, 400);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setVisible(true);
        setContentPane(panel2);
        final File[] dir = new File[1];

        ArrayList<String> filetypes = new ArrayList<>();
        filetypes.add("Java Dateien");
        filetypes.add("Python Dateien");
        comboBox.setModel(new DefaultComboBoxModel(filetypes.toArray()));
        tree.setModel(new DefaultTreeModel(new DefaultMutableTreeNode()));

        choose_button.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                JFileChooser fc = new JFileChooser();
                fc.setFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
                int result = fc.showOpenDialog(null);
                if (result == JFileChooser.APPROVE_OPTION) {
                    File selectedDir = fc.getSelectedFile();
                    dir[0] = fc.getSelectedFile();
                    DefaultMutableTreeNode root = new DefaultMutableTreeNode(selectedDir.getName());
                    DefaultTreeModel model = new DefaultTreeModel(root);
                    tree.setModel(model);

                    ArrayList<String> selectedFiletypes = new ArrayList<>();
                    if (comboBox.getSelectedIndex() == 0) {
                        selectedFiletypes.add("java");
                    } else if (comboBox.getSelectedIndex() == 1) {
                        selectedFiletypes.add("py");
                    }

                    add_to_tree(selectedDir, root, selectedFiletypes);
                    model.reload();
                }
            }
        });

        comboBox.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                DefaultMutableTreeNode root = new DefaultMutableTreeNode(dir[0].getName());
                DefaultTreeModel model = new DefaultTreeModel(root);
                tree.setModel(model);

                ArrayList<String> selectedFiletypes = new ArrayList<>();
                if (comboBox.getSelectedIndex() == 0) {
                    selectedFiletypes.add("java");
                } else if (comboBox.getSelectedIndex() == 1) {
                    selectedFiletypes.add("py");
                }

                add_to_tree(dir[0], root, selectedFiletypes);
                model.reload();
            }
        });
    }

    private List<File> getSubDirectories(File directory) {
        LinkedList<File> res = new LinkedList<>();
        for (File f : directory.listFiles()) {
            if (f.isDirectory()) {
                res.add(f);
            }
        }
        return res;
    }

    private List<File> getFiles(File directory, List<String> fileTypes) {
        LinkedList<File> res = new LinkedList<>();
        if (directory.listFiles() != null) {
            for (File f : directory.listFiles()) {
                if (f.isFile()) {
                    for (String ext : fileTypes) {
                        if (f.getName().toLowerCase().endsWith("." + ext)) {
                            res.add(f);
                        }
                    }
                }
            }
        }
        return res;
    }

    private boolean hasMatchingFiles(File directory, List<String> fileTypes) {
        if (directory.listFiles() == null) return false;

        for (File f : directory.listFiles()) {
            if (f.isFile()) {
                for (String ext : fileTypes) {
                    if (f.getName().toLowerCase().endsWith("." + ext)) {
                        return true;
                    }
                }
            } else if (f.isDirectory()) {
                if (hasMatchingFiles(f, fileTypes)) {
                    return true;
                }
            }
        }
        return false;
    }

    private void add_to_tree(File dir, DefaultMutableTreeNode node, List<String> fileTypes) {
        if (!dir.isDirectory()) return;

        for (File file : getFiles(dir, fileTypes)) {
            node.add(new DefaultMutableTreeNode(file.getName()));
        }

        for (File subDir : getSubDirectories(dir)) {
            if (hasMatchingFiles(subDir, fileTypes)) {
                DefaultMutableTreeNode childNode = new DefaultMutableTreeNode(subDir.getName());
                node.add(childNode);
                add_to_tree(subDir, childNode, fileTypes);
            }
        }
    }

    public static void main(String[] args) {
        mainwindow mainwindow = new mainwindow();
    }
}