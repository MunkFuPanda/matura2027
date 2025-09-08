package formen;

import java.awt.*;

public class Dreieck extends Rechteck {
	protected int width;
	protected int height;
	public Dreieck(int x, int y, int width, int height, Color color) {
		super(x, y, width, height, color);
		this.width = width;
		this.height = height;
	}

	@Override
	public void paint(Graphics g) {
		g.setColor(color);
		this.drawLine(g, x, y, x + width, y);
		this.drawLine(g, x + width, y, x + width, y + height);
		this.drawLine(g, x + width, y + height, x, y);
	}
}
