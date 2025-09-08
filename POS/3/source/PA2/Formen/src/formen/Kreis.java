package formen;

import java.awt.*;

public class Kreis extends Ellipse {
    protected int w;
    protected int h;
    public Kreis(int x, int y, int w, int h, Color color) {
        super(x, y, w, h, color);
        this.w = w;
        this.h = h;
    }

    @Override
    public void paint(Graphics g) {
        g.setColor(color);
        this.drawOval(g, x, y, w, h);
    }
}