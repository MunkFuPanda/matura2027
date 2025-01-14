package at.ac.htlwrn.service;

import at.ac.htlwrn.dto.UserDto;
import at.ac.htlwrn.model.User;

import java.util.List;

public interface UserService {

    /**
     * Save new User
     * @param user {@link UserDto} with user details
     * @return instance of {@link User} that contains the database id
     */
    User save(UserDto user);

    /**
     * Find all users
     * @return all Users in the table User
     */
    List<User> findAll();

    void delete(Long id);

    /**
     * Returns Searches for User by it's username
     * @param username username of User
     * @return Instance of {@link User} if exists, else null
     */
    User findByName(String username);

    /**
     * Returns Gets User by it's id
     * @param id id of User
     * @return Instance of {@link User} if exists, else null
     */
    User findById(Long id);

    /**
     * Update user with give userDto.id
     * @param userDto user data for updating
     * @return UserDto with updated data
     */
    UserDto update(UserDto userDto);
}
