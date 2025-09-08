package formen;

import java.awt.*;

public class Ellipse extends Form {
	protected int w;
	protected int h;
	public Ellipse(int x, int y, int w, int h, Color color) {
		super(x, y, color);
		this.w = w;
		this.h = h;
	}

	@Override
	public void paint(Graphics g) {
		g.setColor(color);
		this.drawOval(g, x, y, w, h);
	}
}

