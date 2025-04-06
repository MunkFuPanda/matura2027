package at.ac.htlwrn.dao;

import at.ac.htlwrn.model.Order;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

/**
 * DAO for the Order entity.
 * Extends CrudRepository for basic CRUD operations.
 */
@Repository
public interface OrderDao extends CrudRepository<Order, Long> {

    /**
     * Custom query to find an Order by clientName.
     * @param id Name of the client.
     * @return Optional containing the found Order, if any.
     */
    Optional<Order> findById(Long id);

    List<Order> findAll();

    Optional<Order> findByLastName(String lastName);
}
