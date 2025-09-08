package Anagramme;

import java.util.Arrays;
import java.util.Iterator;

public class PermutationenGenerator implements Iterator<int[]>, Iterable<int[]> {
    private boolean more;
    private int[] first;
    private int[] second;

    public PermutationenGenerator(int n) {
        first = new int[n];
        for (int i = 0; i < n; i++) {
            first[i] = i;
        }
        more = true;
    }

    public void nextPermutation() {
        int i = first.length - 2;
        while (i >= 0 && first[i] >= first[i + 1]) {
            i--;
        }
        if (i >= 0) {
            int j = first.length - 1;
            while (j >= 0 && first[j] <= first[i]) {
                j--;
            }
            swap(i, j);
            reverse(i + 1);
        } else {
            more = false;
        }
    }

    private void swap(int i, int j) {
        int tmp = first[i];
        first[i] = first[j];
        first[j] = tmp;
    }

    private void reverse(int i) {
        int j = first.length - 1;
        while (i < j) {
            swap(i, j);
            i++;
            j--;
        }
    }

    @Override
    public Iterator<int[]> iterator() {
        return this;
    }

    @Override
    public boolean hasNext() {
        return more;
    }

    @Override
    public int[] next() {
        if (more) {
            second = first.clone();
            nextPermutation();
            return second;
        }
        return null;
    }
}