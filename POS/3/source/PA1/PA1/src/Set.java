public class Set {

    private Node root;

    public boolean contains(int number) {
        if (root != null) {
            return root.contains(number);
        } else {
            return false;
        }
    }

    public void insert(int number) {
        if (root != null) {
            root.insert(number);
        } else {
            root = new Node(number);
        }
    }

    @Override
    public String toString() {
        return root == null ? "[ ]" : "[" + root.toString() + "]";
    }

    private void traverse(Node current, Set out, int divisor) {
        if (current == null) {
            return;
        }
        if (current.getNumber() % divisor == 0)
            out.insert(current.getNumber());

        traverse(current.getRight(), out, divisor);
        traverse(current.getLeft(), out, divisor);
    }

    Set getDivisible(int divisor)
    {
        //Aufgabe 1
        Set set = new Set();
        traverse(root, set, divisor);
        return set;
    }
}