package at.ac.htlwrn.exception;

public class UserAlredyExistsException extends RuntimeException{

    public UserAlredyExistsException(String s) {
        super(s);
    }
}
