import java.util.Iterator;
import java.util.Random;
import java.util.TreeSet;

public class Zahlenmenge<T extends Comparable<T>> {
	private TreeSet<T> data = new TreeSet<>();
	private Calculator<T> calculator;

	public Zahlenmenge() {
	}


	public Zahlenmenge(Calculator<T> calculator) {
		this.calculator = calculator;
	}


	public void set(T val) {
		data.add(val);
	}

	public boolean get(T val) {
		return data.contains(val);
	}

	public int size() {
		return data.size();
	}

	public void remove(T val) {
		data.remove(val);
	}

	@Override
	public Zahlenmenge<T> clone() {
		Zahlenmenge<T> clone = new Zahlenmenge<>();
		clone.data.addAll(data);
		return clone;
	}

	void print() {
		if (data.isEmpty())
			return;
		System.out.print("{ ");
		data.forEach(e -> System.out.printf("%s ", e));
		System.out.println("}");
	}

	Zahlenmenge<T> intersect(Zahlenmenge<T> s) {
		Zahlenmenge<T> intersect = clone();
		intersect.data.retainAll(s.data);
		return intersect;
	}

	Zahlenmenge<T> union(Zahlenmenge<T> s) {
		Zahlenmenge<T> union = clone();
		union.data.addAll(s.data);
		return union;
	}

	Zahlenmenge<T> diff(Zahlenmenge<T> s) {
		Zahlenmenge<T> diff = clone();
		diff.data.removeAll(s.data);
		return diff;
	}

	Zahlenmenge<T> range(T from, T to) {
		Zahlenmenge<T> range = new Zahlenmenge<>();
		range.data.addAll(data.subSet(from, true, to, true));
		return range;
	}

	T total() {
		if (calculator != null) {
			T sum = calculator.zero();
			for (T t : data) {
				sum = calculator.add(sum, t);
			}
			return sum;
		}
		if (data.isEmpty()) {
			return null;
		}

		if (max() instanceof Integer) {
			int sum = 0;
			for (T t : data) {
				sum += ((Integer) t);
			}
			return (T) Integer.valueOf(sum);
		}
		if (max() instanceof Double) {
			double sum = 0;
			for (T t : data) {
				sum += ((Integer) t);
			}
			return (T) Double.valueOf(sum);
		}
		return null;
	}

	T average() {
		return calculator.divide(total(), (T) Integer.valueOf(data.size()));
	}

	T min() {
		return data.first();
	}

	T max() {
		return data.last();
	}

	boolean equals(Zahlenmenge<T> s) {
		return data.equals(s.data);
	}

	T zufallsZahl() {
		return data.stream()
				.skip(new Random().nextInt(data.size()))
				.findFirst()
				.orElse(null);
	}

	public static void main(String[] args) {
		Zahlenmenge<Integer> s1 = new Zahlenmenge<>();
		s1.set(-9);
		s1.set(-5);
		s1.set(-4);
		s1.set(-3);
		s1.set(0);
		s1.set(2);
		s1.set(4);
		s1.set(10);
		s1.print();

		Zahlenmenge<Integer> s2 = new Zahlenmenge<>();
		s2.set(-5);
		s2.set(-3);
		s2.set(0);
		s2.set(1);
		s2.set(2);
		s2.set(7);
		s2.set(9);
		s1.print();

		s1.union(s2).print();
		s1.intersect(s2).print();
		s1.range(0, 10).print();
		s2.range(-10, 0).print();
		s2.remove(0);
		s2.print();
		s2.remove(2);
		s2.print();
		s1.intersect(s2).print();
		s1.diff(s2).print();
	}
}
