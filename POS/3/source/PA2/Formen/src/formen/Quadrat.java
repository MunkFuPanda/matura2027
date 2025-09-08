package formen;

import java.awt.*;

public class Quadrat extends Rechteck{
	protected int width;
	protected int height;
	public Quadrat(int x, int y, int width, int height, Color color) {
		super(x, y, width, height, color);
		this.width = width;
		this.height = height;
	}

	@Override
	public void paint(Graphics g) {
		g.setColor(color);
		this.drawLine(g, x, y, x + width, y);
		this.drawLine(g, x + width, y, x + width, y + width);
		this.drawLine(g, x, y + width, x + width, y + width);
		this.drawLine(g, x, y, x, y + width);
	}
}
