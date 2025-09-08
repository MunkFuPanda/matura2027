import javax.swing.*;

public class MemoryButton extends JButton {
	private ImageIcon value = null;
	private boolean found = false;

	public MemoryButton(ImageIcon value) {
		this.value = value;
	}

	public boolean isFound() {
		return found;
	}

	public void setFound(boolean found) {
		this.found = found;
	}

	public ImageIcon getValue() {
		return value;
	}
}
