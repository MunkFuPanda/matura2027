package at.ac.htlwrn.dao;

import at.ac.htlwrn.model.User;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

/**
 * Interface for Data Access Object Layer (DAO-Layer).
 * Implementation is done by Spring Data!
 * CrudRepository<User,Long> beacuse User is the entity and Long is the primary key of User
 */
@Repository
public interface UserDao extends CrudRepository<User, Long> {

    /**
     * This works because there is a field "username" in the class User exists!
     * findByUsername is then converted to e.g. "Select * from user where username='alex123'"
     * @param username Username to search for
     * @return the found user in an Optional
     */
    Optional<User> findByUsername(String username);

    //If you are looking for method "findById(Long id)". It is in the class CrudRepository.
    //Press CTRL (or STRG in german) an click on the class CrudRepository to view all methods.

}
