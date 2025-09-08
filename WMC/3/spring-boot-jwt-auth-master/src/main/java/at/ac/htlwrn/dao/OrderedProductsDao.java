package at.ac.htlwrn.dao;

import at.ac.htlwrn.model.OrderedProducts;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

/**
 * DAO for the OrderedProducts entity.
 * Extends CrudRepository for basic CRUD operations.
 */
@Repository
public interface OrderedProductsDao extends CrudRepository<OrderedProducts, Long> {

}
