package formen;

import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;
import java.util.List;

public class FormenPanel extends JPanel {
	int count = 0;
	private List<Form> formen = new ArrayList<>();
	private Color selectedColor;

	public void addForm(Form form) {
		formen.add(form);
	}

	public void addRechteck(Point p) {
		Rechteck rechteck = new Rechteck(p.x, p.y, 200, 100, selectedColor);
		addForm(rechteck);
		repaint();
	}

	public void addQuadrat(Point p) {
		Quadrat quadrat = new Quadrat(p.x, p.y, 200, 100, selectedColor);
		addForm(quadrat);
		repaint();
	}

	public void addKreis(Point p) {
		Kreis kreis = new Kreis(p.x, p.y, 100, 100, selectedColor);
		addForm(kreis);
		repaint();
	}

	public void addTrapez(Point p) {
		Trapez trapez = new Trapez(p.x, p.y, 100, 100, selectedColor);
		addForm(trapez);
		repaint();
	}

	public void addRaute(Point p) {
		Raute raute = new Raute(p.x, p.y, 100, 100, selectedColor);
		addForm(raute);
		repaint();
	}


	@Override
	public void paint(Graphics g) {
		super.paint(g);
		for(Form f : formen)
		{
			if(f != null)
				f.paint(g);
		}
	}

	public void setColor(Color color) {
		selectedColor = color;
	}

	public void addDreieck(Point point) {
	}

	public void addEllipse(Point point) {
	}

	public void addPolygon(Point point) {
	}

	public void addSechseck(Point point) {
	}
}
