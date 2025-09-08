public class Node {
    private final int number;
    private Node left;
    private Node right;
    public Node(int number) {
        this.number = number;
    }
    @Override
    public String toString() {

        String res = "";
        if (left != null) {
            res += left + " ";
        }
        res += number;
        if (right != null) {
            res += " " + right;
        }
        return res;
    }
    boolean contains(int number) {
        if (number == this.number) {
            return true;
        } else if (number < this.number) {
            if (left != null) {
                return left.contains(number);
            } else {
                return false;
            }
        } else {
            if (right != null) {
                return right.contains(number);
            } else {
                return false;
            }
        }
    }
    void insert(int number) {
        if (number < this.number) {
            if (left == null) {
                left = new Node(number);
            } else {
                left.insert(number);
            }
        } else if (number > this.number) {
            if (right == null) {
                right = new Node(number);
            } else {
                right.insert(number);
            }
        }
    }

    //Aufgabe 1
    public Node getLeft() {
        return left;
    }

    public Node getRight() {
        return right;
    }

    public int getNumber() {
        return number;
    }
}