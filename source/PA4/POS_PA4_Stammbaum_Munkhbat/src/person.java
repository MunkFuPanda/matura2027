import java.time.LocalDate;
import java.util.ArrayList;

public class person {
    private String firstName;
    private String lastName;
    private int age;
    private LocalDate dateOfBirth;
    private String gender;
    private person mother;
    private person father;
    private ArrayList<person> children;

    public person(String firstName, String lastName, LocalDate dateOfBirth, String gender) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.dateOfBirth = dateOfBirth;
        this.gender = gender;
        this.age = LocalDate.now().getYear() - dateOfBirth.getYear();
        this.mother = null;
        this.father = null;
        this.children = new ArrayList<>();
    }

    public String getFirstName() {
        return firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public String getFullName() {
        return firstName + " " + lastName;
    }

    public int getAge() {
        return age;
    }

    public LocalDate getDateOfBirth() {
        return dateOfBirth;
    }

    public String getGender() {
        return gender;
    }

    public person getMother() {
        return mother;
    }

    public person getFather() {
        return father;
    }

    public ArrayList<person> getChildren() {
        return children;
    }

    public void setMother(person mother) {
        this.mother = mother;
    }

    public void setFather(person father) {
        this.father = father;
    }

    public void addChild(person child) {
        this.children.add(child);
    }

    public ArrayList<String> getAll() {
        ArrayList<String> list = new ArrayList<>();
        list.add(firstName);
        list.add(lastName);
        list.add(age+"");
        list.add(dateOfBirth.toString());
        list.add(gender);
        if (mother != null) {
            list.add(mother.toString());
        }
        if (father != null) {
            list.add(father.toString());
        }
        if (children != null) {
            for (person child : children) {
                list.add(child.toString());
            }
        }
        return list;
    }
}
