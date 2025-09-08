package formen;

import java.awt.*;

public class Rechteck extends Form {
	protected int width;
	protected int height;

	/**
	 *
	 * @param x
	 * @param y
	 * @param width
	 * @param height
	 */

	public Rechteck(int x, int y, int width, int height, Color color)
	{
		super(x, y, color);
		this.width = width;
		this.height = height;
	}

	@Override
	public void paint(Graphics g) {
		g.setColor(color);
		this.drawLine(g, x, y, x + width, y);
		this.drawLine(g, x + width, y, x + width, y + height);
		this.drawLine(g, x, y + height, x + width, y + height);
		this.drawLine(g, x, y, x, y + height);
	}



}
