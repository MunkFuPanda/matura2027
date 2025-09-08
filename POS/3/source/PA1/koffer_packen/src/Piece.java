public class Piece {
	private String name;
	private Piece next;

	public Piece(String name) {
		this.name = name;
	}

	public String getName() {
		return name;
	}

	public Piece getNext() {
		return next;
	}

	public int size() {
		if (next != null) {
			return 1 + next.size();
		} else {
			return 1;
		}
	}

	public void addNewPiece(String name) {
		if (next != null) {
			next.addNewPiece(name);
		} else {
			next = new Piece(name);
		}
	}

	@Override
	public String toString() {
		return name + (next == null ? "" : " " + next);
	}
}
