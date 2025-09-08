package formen;

import java.awt.*;

public class Sechseck extends Form{
	protected int width;
	protected int height;
	public Sechseck(int x, int y, int width, int height, Color color) {
		super(x, y, color);
		this.width = width;
		this.height = height;
	}

	@Override
	public void paint(Graphics g) {
		g.setColor(color);
		this.drawLine(g, x, y, x + width/2, y);
		this.drawLine(g, x + width/2, y, x + width/2 + width/2 , y - width/2);
		this.drawLine(g, x + width/2 + width/2, y - width/2, x + width/2 , y - width/2 - width/2);
		this.drawLine(g, x + width/2, y - width/2 - width/2, x , y - width/2 - width/2) ;
		this.drawLine(g, x, y - width/2 - width/2, x - width/2, y - width/2);
		this.drawLine(g, x - width/2, y - width/2, x , y);
	}
}
