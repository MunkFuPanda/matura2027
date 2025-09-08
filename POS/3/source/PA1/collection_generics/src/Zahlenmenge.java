import java.util.Collection;
import java.util.HashSet;
import java.util.Random;
import java.util.stream.Collectors;

public class Zahlenmenge<T extends Number & Comparable<T>> {
	private final Collection<T> menge;

	public Zahlenmenge() {
		this.menge = new HashSet<>();
	}

	public void set(T val) {
		menge.add(val);
	}

	public boolean get(T val) {
		return menge.contains(val);
	}

	public int size() {
		return menge.size();
	}

	public void remove(T val) {
		menge.remove(val);
	}

	public Zahlenmenge<T> clone() {
		Zahlenmenge<T> cloned = new Zahlenmenge<>();
		cloned.menge.addAll(this.menge);
		return cloned;
	}

	public void print() {
		System.out.println(menge);
	}

	public Zahlenmenge<T> intersect(Zahlenmenge<T> s) {
		Zahlenmenge<T> result = new Zahlenmenge<>();
		result.menge.addAll(menge.stream().filter(s.menge::contains).collect(Collectors.toSet()));
		return result;
	}

	public Zahlenmenge<T> union(Zahlenmenge<T> s) {
		Zahlenmenge<T> result = this.clone();
		result.menge.addAll(s.menge);
		return result;
	}

	public Zahlenmenge<T> diff(Zahlenmenge<T> s) {
		Zahlenmenge<T> result = new Zahlenmenge<>();
		result.menge.addAll(menge.stream().filter(e -> !s.menge.contains(e)).collect(Collectors.toSet()));
		return result;
	}

	public Zahlenmenge<T> range(T from, T to) {
		Zahlenmenge<T> result = new Zahlenmenge<>();
		result.menge.addAll(menge.stream().filter(e -> e.compareTo(from) >= 0 && e.compareTo(to) <= 0).collect(Collectors.toSet()));
		return result;
	}

	public Double total() {
		return menge.stream().mapToDouble(Number::doubleValue).sum();
	}

	public Double average() {
		return menge.stream().mapToDouble(Number::doubleValue).average().orElse(0.0);
	}

	public T min() {
		return menge.stream().min(T::compareTo).orElse(null);
	}

	public T max() {
		return menge.stream().max(T::compareTo).orElse(null);
	}

	public boolean equals(Zahlenmenge<T> s) {
		return menge.equals(s.menge);
	}

	public T zufallsZahl() {
		int size = menge.size();
		if (size == 0) return null;
		int item = new Random().nextInt(size);
		return menge.stream().skip(item).findFirst().orElse(null);
	}
}
