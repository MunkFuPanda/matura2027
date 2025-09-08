package at.ac.htlwrn.dao;

import at.ac.htlwrn.model.User;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import java.util.Optional;

@DataJpaTest
public class UserDaoTest {

    @Autowired
    private UserDao userDao;

    private User saveUser(String username, String firstname){
        User user = new User();
        user.setFirstName(firstname);
        user.setLastName("lastname1");
        user.setUsername(username);
        user.setPassword("bla");
        //Framework generates user id on save
        return userDao.save(user);
    }

    @Test
    public void testFindById(){
        String username="testuser";
        User user=saveUser(username,"firstname");
        Long id = user.getId();
        Optional<User> foundUserOpt=userDao.findById(id);
        Assertions.assertTrue(foundUserOpt.isPresent());
        User foundUser=foundUserOpt.get();
        Assertions.assertEquals(username, foundUser.getUsername());
    }

    @Test
    public void testFindByUserName(){
        String username = "user1";
        String firstname = "firstname1";

        saveUser(username, firstname);

        Optional<User> user = userDao.findByUsername(username);
        Assertions.assertTrue(user.isPresent());
        Assertions.assertEquals(username, user.get().getUsername());
        Assertions.assertEquals(firstname, user.get().getFirstName());
    }
    @Test
    public void testFindByUserNameFromDataSql() {
        //this user is inserted by data.sql
        Optional<User> user = userDao.findByUsername("alex123");
        Assertions.assertTrue(user.isPresent());
    }
    @Test
    public void testFindByUserName_UnknownUsernameNotFound(){
        Optional<User> user = userDao.findByUsername("unknown");
        Assertions.assertFalse(user.isPresent());
    }

    @Test
    public void testDeleteUser(){
        String username = "user2";
        String firstname = "firstname2";

        saveUser(username, firstname);

        Optional<User> user = userDao.findByUsername(username);
        Assertions.assertTrue(user.isPresent());

        userDao.delete(user.get());

        user = userDao.findByUsername(username);
        Assertions.assertFalse(user.isPresent());
    }
}
