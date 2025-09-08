public class Node {
	private int value = 0;
	private Node left = null;
	private Node right = null;

	public Node(int value) {
		this.value = value;
	}

	public void set(int val) {
		if (val < value) {
			if (left != null) {
				left.set(val);
			} else {
				left = new Node(val);
			}
		} else if (val > value) {
			if (right != null) {
				right.set(val);
			} else {
				right = new Node(val);
			}
		}
	}

	public boolean get(int val) {
		if (val < value) {
			return left != null && left.get(val);
		} else if (val > value) {
			return right != null && right.get(val);
		} else {
			return true;
		}
	}

	public int size() {
		int result = 1;
		if (left != null) {
			result += left.size();
		}
		if (right != null) {
			result += right.size();
		}

		return result;
	}

	public Node remove(int val) {
		if (val == value) {
			if (left == null && right == null) return null;
			if (left == null) return right;
			if (right == null) return left;
			left.set(right);
			return left;
		} else if (val < value) {
			if (left != null) left = left.remove(val);
		} else {
			if (right != null) right = right.remove(val);
		}

		return this;
	}

	private void set(Node n) {
		if (n.value < value) {
			if (left != null) {
				left.set(n.value);
			} else {
				left = n;
			}
		} else if (n.value > value) {
			if (right != null) {
				right.set(n.value);
			} else {
				right = n;
			}
		}
	}

	public void clone(Set res) {
		res.set(value);

		if (left != null) {
			left.clone(res);
		}

		if (right != null) {
			right.clone(res);
		}
	}

	public void intersect(Set res, Set s) {
		if (s.get(value)) {
			res.set(value);
		}

		if (left != null) {
			left.intersect(res, s);
		}

		if (right != null) {
			right.intersect(res, s);
		}
	}

	public void diff(Set res, Set s) {
		if (!s.get(value)) {
			res.set(value);
		}

		if (left != null) {
			left.diff(res, s);
		}

		if (right != null) {
			right.diff(res, s);
		}
	}

	public void range(Set res, int from, int to) {
		if (value >= from && value <= to) {
			res.set(value);
		}

		if (left != null) {
			left.range(res, from, to);
		}

		if (right != null) {
			right.range(res, from, to);
		}
	}

	public int total() {
		int result = value;
		if (left != null) {
			result += left.total();
		}

		if (right != null) {
			result += right.total();
		}

		return result;
	}

	public double average() {
		return (double) total() / (double) size();
	}

	public int min() {
		if (left != null) return left.min();
		return value;
	}

	public int max() {
		if (right != null) return right.max();
		return value;
	}

	@Override
	public String toString() {
		return (left == null ? "" : (left + " ")) + value + (right == null ? "" : (" " + right));
	}
}