package formen;

import java.awt.*;

public class Raute extends Rechteck{
	protected int width;
	protected int height;
	public Raute(int x, int y, int width, int height, Color color) {
		super(x, y, width, height, color);
		this.width = width;
		this.height = height;
	}

	@Override
	public void paint(Graphics g) {
		g.setColor(color);
		this.drawLine(g, x  + width, y, x + 2 * width   , y + height/2 );
		this.drawLine(g, x + 2 * width, y + height/2, x + width, y + 2 * height/2);
		this.drawLine(g, x + width, y + 2 * height/2, x, y + height/2);
		this.drawLine(g, x, y + height/2, x + width, y);
	}
}
