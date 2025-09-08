package at.ac.htlwrn.dao;

import at.ac.htlwrn.model.Product;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

/**
 * DAO for the Product entity.
 * Extends CrudRepository for basic CRUD operations.
 */
@Repository
public interface ProductDao extends CrudRepository<Product, Long> {

    /**
     * Custom query to find a Product by its name.
     * @param id Name of the product.
     * @return Optional containing the found Product, if any.
     */
    Optional<Product> findById(Long id);
}
