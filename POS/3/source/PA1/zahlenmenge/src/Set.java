public class Set {
	private Node root;

	public void set(int val) {
		if (root == null) {
			root = new Node(val);
		} else {
			root.set(val);
		}
	}

	public boolean get(int val) {
		if (root == null) return false;
		return root.get(val);
	}

	public int size() {
		if (root == null) return 0;
		return root.size();
	}

	public void remove(int val) {
		if (root == null) return;
		root.remove(val);
	}

	public Set clone() {
		Set set = new Set();

		if (root != null) {
			root.clone(set);
		}

		return set;
	}

	public void print() {
		if (root != null) {
			System.out.println(root);
		}
	}

	public Set intersect(Set s) {
		Set set = new Set();

		if (root != null) {
			root.intersect(set, s);
		}

		return set;
	}

	public Set union(Set s) {
		Set set = s.clone();

		if (root != null) {
			root.clone(set);
		}

		return set;
	}

	public Set diff(Set s) {
		Set set = new Set();

		if (root != null) {
			root.diff(set, s);
		}

		return set;
	}

	public Set range(int from, int to) {
		Set set = new Set();

		if (root != null) {
			root.range(set, from, to);
		}

		return set;
	}


	/////////////////////////// ZUSATZ ////////////////////////////
	public int total() {
		if (root == null) return 0;
		return root.total();
	}

	public double average() {
		if (root == null) return 0;
		return root.average();
	}

	public Integer min() {
		if (root == null) return null;
		return root.min();
	}

	public Integer max() {
		if (root == null) return null;
		return root.max();
	}

	public boolean equals(Set other) {
		return union(other).size() == size();
	}

	// zufallszahl
	// 1. generiere random zahl zwischen min() und max()
	// 2. binary search und nächste zahl nehmen
}