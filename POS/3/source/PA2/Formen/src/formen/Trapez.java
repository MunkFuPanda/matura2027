package formen;

import java.awt.*;

public class Trapez extends Form {
    protected int weight;
    protected int height;
    public Trapez(int x, int y, int weight, int height, Color color) {
        super(x, y, color);
        this.weight = weight;
        this.height = height;
    }
    @Override
    public void paint(Graphics g) {
        g.setColor(color);
        this.drawLine(g, x - weight, y, x + 2*weight, y);
        this.drawLine(g, x + 2*weight, y, x + weight, y + height);
        this.drawLine(g, x, y + height, x + weight, y + height);
        this.drawLine(g, x - weight, y, x , y + height);

    }
}

