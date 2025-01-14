package at.ac.htlwrn.service.impl;

import at.ac.htlwrn.dto.UserDto;
import at.ac.htlwrn.exception.UserAlredyExistsException;
import at.ac.htlwrn.model.User;
import at.ac.htlwrn.service.UserService;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.context.SpringBootTest;

import java.util.Optional;

@AutoConfigureTestDatabase
@SpringBootTest
public class UserServiceImplTest {

    @Autowired
    private UserService userService;

    private void createUser(String username, String firstname){
        UserDto user = new UserDto();
        user.setUsername(username);
        user.setFirstName(firstname);
        user.setLastName("lastname2");
        user.setPassword("bla2");
        userService.save(user);
    }

    @Test
    public void testFindOne() {
        //this user is inserted by data.sql
        User user = userService.findByName("alex123");
        Assertions.assertNotNull(user);
    }

    @Test
    public void testSave() {
        String username="username2";
        String firstname="firstname2";
        createUser(username, firstname);

        Optional<User> foundUser = userService.findAll().stream().filter(u -> u.getUsername().equals(username)).findFirst();

        Assertions.assertTrue(foundUser.isPresent());
        Assertions.assertEquals(username, foundUser.get().getUsername());
        Assertions.assertEquals(firstname, foundUser.get().getFirstName());
    }


    @Test
    public void testSaveTwice() {
        String username = "username3";
        String firstname = "firstname3";
        createUser(username, firstname);
        try {
            createUser(username, firstname);
            Assertions.fail();
        } catch (UserAlredyExistsException ex){
            Assertions.assertEquals("Username already exists!",ex.getMessage());
        }

    }

    @Test
    public void testUpdateUser(){
        UserDto userDto = new UserDto();
        userDto.setId(-2L);
        userDto.setFirstName("Thomas");
        userDto.setLastName("Maier");
        userDto.setAge(23);
        userService.update(userDto);

        User user=userService.findById(-2L);
        Assertions.assertEquals("Thomas",user.getFirstName());
        Assertions.assertEquals("Maier", user.getLastName());
        Assertions.assertEquals(23, user.getAge());
    }

}
