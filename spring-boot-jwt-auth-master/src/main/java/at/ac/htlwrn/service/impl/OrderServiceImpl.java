package at.ac.htlwrn.service.impl;

import at.ac.htlwrn.dao.OrderDao;
import at.ac.htlwrn.dao.OrderedProductsDao;
import at.ac.htlwrn.dao.ProductDao;
import at.ac.htlwrn.dto.OrderDto;
import at.ac.htlwrn.dto.ProductDto;
import at.ac.htlwrn.model.Order;
import at.ac.htlwrn.model.OrderedProducts;
import at.ac.htlwrn.model.Product;
import at.ac.htlwrn.service.OrderService;
import org.apache.commons.lang3.Validate;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.springframework.beans.BeanUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.transaction.Transactional;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Optional;

@Service(value = "orderService")
public class OrderServiceImpl implements OrderService {

    private static Logger logger = LogManager.getLogger(OrderServiceImpl.class);

    @Autowired
    private OrderDao orderDao;

    @Autowired
    private ProductDao productDao;

    @Autowired
    private OrderedProductsDao orderedProductsDao;


    @Transactional
    public Order createOrder(OrderDto orderDto) {
        logger.info("createOrder");
        Validate.notNull(orderDto);
        Validate.notNull(orderDto.getLastName(), "orderDto.clientName must not be null");


        Optional<Order> testOrder = orderDao.findByLastName(orderDto.getLastName());
        if (testOrder.isPresent()) {
            logger.warn("Order with id " + orderDto.getOrderId() + " already exists");
            throw new IllegalArgumentException("Order with id " + orderDto.getOrderId() + " already exists");
        }

        Order order = new Order();
        BeanUtils.copyProperties(orderDto, order);

        double orderPrice = 0;

        for (int i = 0; i < orderDto.getProducts().size(); i++) {
            Validate.notNull(orderDto.getProducts());

            ProductDto productDto = orderDto.getProducts().get(i);
            Product product = new Product();
            BeanUtils.copyProperties(productDto, product);

            OrderedProducts orderedProducts = new OrderedProducts();
            orderedProducts.setProduct(product);
            orderedProducts.setOrder(order);
            orderedProducts.setQuantity(productDto.getQuantity());

            orderPrice += orderedProducts.getProduct().getPrice() *  orderedProducts.getQuantity();

            productDao.save(product);
            orderedProductsDao.save(orderedProducts);
        }

        order.setOrderPrice(orderPrice);
        return orderDao.save(order);
    }

    public List<Order> findAll() {
        logger.debug("Retrieving order list");
        return (List<Order>) orderDao.findAll();
    }

    public List<Order> getActiveOrders() {
        logger.debug("Retrieving active orders for client");
        List<Order> allOrders = orderDao.findAll();
        List<Order> activeOrders = new ArrayList<>();
        allOrders.forEach(order -> {
            if (order.getDoneDate().isEmpty() && order.getCancelledDate().isEmpty()) {
                activeOrders.add(order);
            }
        });
        return activeOrders;
    }

    public Order cancelOrder(Long orderId) {
        Order order = orderDao.findById(orderId).orElse(null);
        if (order != null) {
            if (order.getDoneDate() == null) {
                order.setCancelledDate(String.valueOf(new Date()));
                logger.info("Order with id " + orderId + " has been cancelled");
                return orderDao.save(order);
            }
        } else {
            logger.warn("Order with id " + orderId + " does not exist");
        }

        return order;
    }

    public Order finishOrder(Long orderId) {
        Order order = orderDao.findById(orderId).orElse(null);
        if (order != null) {
            if (order.getCancelledDate() == null) {
                order.setDoneDate(String.valueOf(new Date()));
                logger.info("Order with id " + orderId + " has been finished");
                return orderDao.save(order);
            }
        } else {
            logger.warn("Order with id " + orderId + " does not exist");
        }

        return order;
    }
}
