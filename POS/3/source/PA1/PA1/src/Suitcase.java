public class Suitcase {
	private Piece root;

	public Piece getRoot() {
		return root;
	}

	public int size() {
		if (root != null) {
			return root.size();
		} else {
			return 0;
		}
	}

	public void addNewPiece(String name) {
		if (root != null) {
			root.addNewPiece(name);
		} else {
			root = new Piece(name);
		}
	}

	@Override
	public String toString() {
		if (root != null) {
			return root.toString();
		} else {
			return "";
		}
	}
}
