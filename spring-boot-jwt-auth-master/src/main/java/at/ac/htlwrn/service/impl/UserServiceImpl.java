package at.ac.htlwrn.service.impl;

import at.ac.htlwrn.dao.UserDao;
import at.ac.htlwrn.dto.UserDto;
import at.ac.htlwrn.exception.UserAlredyExistsException;
import at.ac.htlwrn.model.User;
import at.ac.htlwrn.service.UserService;
import org.apache.commons.lang3.Validate;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.springframework.beans.BeanUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Optional;


@Service(value = "userService")
public class UserServiceImpl implements UserDetailsService, UserService {

	private static Logger logger = LogManager.getLogger(UserServiceImpl.class);

	@Autowired
	private UserDao userDao;

	@Autowired
	private BCryptPasswordEncoder bcryptEncoder;

	public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
		logger.debug("loading user {}",username);
		Optional<User> user = userDao.findByUsername(username);

		if(user.isEmpty()){
			logger.warn("User {} not found", username);
			throw new UsernameNotFoundException("Invalid username or password.");
		}
		return new org.springframework.security.core.userdetails.User(user.get().getUsername(), user.get().getPassword(), getAuthority());
	}

	private List<SimpleGrantedAuthority> getAuthority() {
		return Arrays.asList(new SimpleGrantedAuthority("ROLE_ADMIN"));
	}

	public List<User> findAll() {
		logger.debug("Retrieving user list");
		List<User> list = new ArrayList<>();
		userDao.findAll().iterator().forEachRemaining(list::add);
		return list;
	}

	@Override
	public void delete(Long id) {
		Validate.notNull(id, "id must not be null");
		logger.info("Deleting user with id {}",id);
		userDao.deleteById(id);
	}

	@Override
	public User findByName(String username) {
		return userDao.findByUsername(username).orElse(null);
	}

	@Override
	public User findById(Long id) {
		Validate.notNull(id,"id must not be null");
		logger.debug("Find user by id {}",id);
		Optional<User> optionalUser = userDao.findById(id);
		return optionalUser.orElse(null);
	}

    @Override
    public UserDto update(UserDto userDto) {
		Validate.notNull(userDto);
		Validate.notNull(userDto.getId(), "userDto.id must not be null");
		logger.info("Updating user {}",userDto.getId());

		Optional<User> optionalUser = userDao.findById(userDto.getId());
        Validate.isTrue(optionalUser.isPresent());
		User user = optionalUser.get();
		BeanUtils.copyProperties(userDto, user, "password");
		userDao.save(user);
        return userDto;
    }

    @Override
    public User save(UserDto userDto) {
		Validate.notNull(userDto);
		Validate.notNull(userDto.getUsername(), "username must not be null");

		logger.info("Saving user new user {}",userDto.getUsername());

		//Username must be unique. Therefore, we search if there is already a user with the same username.
		//If a user with this username already exists we cancel saving with an exception.
		Optional<User> user = userDao.findByUsername(userDto.getUsername());
		if(user.isPresent()){
			logger.warn("User {} already exists", userDto.getUsername());
			throw new UserAlredyExistsException("Username already exists!");
		}

	    User newUser = new User();
	    newUser.setUsername(userDto.getUsername());
	    newUser.setFirstName(userDto.getFirstName());
	    newUser.setLastName(userDto.getLastName());
	    newUser.setPassword(bcryptEncoder.encode(userDto.getPassword()));
		newUser.setAge(userDto.getAge());
		newUser.setSalary(userDto.getSalary());
        return userDao.save(newUser);
    }
}
