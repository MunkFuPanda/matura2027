package formen;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.util.Vector;

public class MainWindow extends JFrame {
	private JPanel mainPanel;
	private JPanel formenPanel;

	private ButtonGroup colorsGroup = new ButtonGroup();
	private ButtonGroup formsGroup = new ButtonGroup();

	public MainWindow() throws HeadlessException {
		setTitle("Formen");
		setDefaultCloseOperation(EXIT_ON_CLOSE);
		setContentPane(mainPanel);
		setSize(800, 400);

		setJMenuBar(createMenus());

		formenPanel.addMouseListener(new MouseAdapter() {
			@Override
			public void mouseClicked(MouseEvent e) {
				switch (((JRadioButtonMenuItem) formsGroup.getSelection()).getText()) {
					case "Dreieck":
						((FormenPanel) formenPanel).addDreieck(e.getPoint());
						break;
					case "Ellipse":
						((FormenPanel) formenPanel).addEllipse(e.getPoint());
						break;
					case "Kreis":
						((FormenPanel) formenPanel).addKreis(e.getPoint());
						break;
					case "Polygon":
						((FormenPanel) formenPanel).addPolygon(e.getPoint());
						break;
					case "Quadrat":
						((FormenPanel) formenPanel).addQuadrat(e.getPoint());
						break;
					case "Raute":
						((FormenPanel) formenPanel).addRaute(e.getPoint());
						break;
					case "Rechteck":
						((FormenPanel) formenPanel).addRechteck(e.getPoint());
						break;
					case "Sechseck":
						((FormenPanel) formenPanel).addSechseck(e.getPoint());
						break;
					case "Trapez":
						((FormenPanel) formenPanel).addTrapez(e.getPoint());
						break;
				}
			}
		});

	}

	public JMenuBar createMenus() {
		JMenuBar menuBar = new JMenuBar();
		setJMenuBar(menuBar);
		JMenu filesMenu = new JMenu("Datei");
		menuBar.add(filesMenu);
		JMenu colorMenu = new JMenu("Farbe");
		menuBar.add(colorMenu);
		JMenu formMenu = new JMenu("Form");
		menuBar.add(formMenu);

		JMenuItem openItem = new JMenuItem("Öffnen");
		filesMenu.add(openItem);
		JMenuItem closeItem = new JMenuItem("Speichern");
		filesMenu.add(closeItem);

		for (String color : new String[]{"Rot", "Blau", "Grün", "Gelb", "Pink"}) {
			JRadioButtonMenuItem buttonMenuItem = new JRadioButtonMenuItem(color);
			colorMenu.add(buttonMenuItem);
			colorsGroup.add(buttonMenuItem);
		}

		for (String form : new String[]{"Dreieck", "Ellipse", "Kreis", "Polygon", "Quadrat", "Raute", "Rechteck", "Sechseck", "Trapez"}) {
			JRadioButtonMenuItem buttonMenuItem = new JRadioButtonMenuItem(form);
			formMenu.add(buttonMenuItem);
			formsGroup.add(buttonMenuItem);
		}

		openItem.addActionListener(e -> {
			JFileChooser fileChooser = new JFileChooser();
			fileChooser.showOpenDialog(null);
		});

		colorsGroup.setSelected(((JRadioButtonMenuItem) colorMenu.getMenuComponent(0)).getModel(), true);
		formsGroup.setSelected(((JRadioButtonMenuItem) formMenu.getMenuComponent(0)).getModel(), true);

		return menuBar;
	}

	public static void main(String[] args) {
		MainWindow m = new MainWindow();
		m.setVisible(true);
	}

	private void createUIComponents() {
		// TODO: place custom component creation code here
		formenPanel = new FormenPanel();
	}
}
