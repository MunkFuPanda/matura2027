import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;

public class InputListener implements DocumentListener {

	private final KlammerChecker klammerChecker;

	public boolean supress_events = false;

	public InputListener(KlammerChecker klammerChecker) {
		this.klammerChecker = klammerChecker;
	}

	@Override
	public void insertUpdate(DocumentEvent e) {
		if (!supress_events) {
			klammerChecker.check();
		}
	}

	@Override
	public void removeUpdate(DocumentEvent e) {
		if (!supress_events) {
			klammerChecker.check();
		}
	}

	@Override
	public void changedUpdate(DocumentEvent e) {
		if (!supress_events) {
			klammerChecker.check();
		}
	}
}
