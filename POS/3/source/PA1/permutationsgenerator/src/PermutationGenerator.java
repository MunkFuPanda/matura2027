import java.util.Iterator;
import java.util.NoSuchElementException;

public class PermutationGenerator implements Iterable<Integer[]>, Iterator<Integer[]> {
    private final int n;
    private final Integer[] current;
    private boolean first; //indicates if its the first call to next() ;)

    public PermutationGenerator(int n) {
        this.n = n;
        this.current = new Integer[n];
        for (int i = 0; i < n; i++) {
            current[i] = i;
        }
        this.first = true;
    }

    @Override
    public Iterator<Integer[]> iterator() {
        return this;
    }

    @Override
    public boolean hasNext() {
        if (first) {
            return true;
        }
        for (int i = n - 2; i >= 0; i--) {
            if (current[i] < current[i + 1]) {
                return true;
            }
        }
        return false;
    }

    @Override
    public Integer[] next() {
        if (first) {
            first = false;
            return current.clone();
        }

        int k = n - 2;
        while (k >= 0 && current[k] >= current[k + 1]) {
            k--;
        }
        if (k == -1) {
            throw new NoSuchElementException();
        }

        int l = n - 1;
        while (current[k] >= current[l]) {
            l--;
        }

        swap(k, l);
        reverse(k + 1, n - 1);
        return current.clone();
    }

    private void swap(int i, int j) {
        int temp = current[i];
        current[i] = current[j];
        current[j] = temp;
    }

    private void reverse(int start, int end) {
        while (start < end) {
            swap(start, end);
            start++;
            end--;
        }
    }
}