package at.ac.htlwrn.service;

import at.ac.htlwrn.dto.OrderDto;
import at.ac.htlwrn.model.Order;

import java.util.List;

public interface OrderService {

    /**
     * Saves a new order.
     * @param orderDto the order to be saved, given as an instance of {@link OrderDto}
     * @return the saved order with its id set
     */
    Order createOrder(OrderDto orderDto);

    /**
     * Retrieves all orders.
     * @return a list of all {@link Order} entities in the database
     */
    List<Order> findAll();

    /**
     * Sets Order canceld.
     * @return the order thats been canceld
     */
    Order cancelOrder(Long id);

    /**
     * Sets Order finished.
     * @return the order thats been finished
     */
    Order finishOrder(Long id);

    /**
     * Retrieves all orders that have not been finished.
     *
     * @return a list of all unfinished orders as {@link OrderDto} instances
     */
    List<Order> getActiveOrders();
}
