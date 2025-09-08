package at.ac.htlwrn.dao;


import at.ac.htlwrn.model.Order;
import at.ac.htlwrn.model.OrderedProducts;
import at.ac.htlwrn.model.Product;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;

import java.util.Date;
import java.util.Optional;

@DataJpaTest
public class OrderDaoTest {

    @Autowired
    private OrderDao orderDao;
    @Autowired
    private ProductDao productDao;
    @Autowired
    private OrderedProductsDao orderedProductsDao;

    private Order testOrder(String lastName, String address, String date, Double price, String salutation) {
        Order order = new Order();
        order.setLastName(lastName);
        order.setOrderPrice(price);
        order.setDate(date);
        order.setAddress(address);
        order.setSalutation(salutation);
        return orderDao.save(order);
    }

    @Test
    public void saveWithProduct() {
        Product product = new Product();
        product.setPrice(1.2);
        product.setValidFrom(String.valueOf(new Date()));
        product.setValidUntil(String.valueOf(new Date()));
        product.setProductName("Product");
        product.setImageName("Product.img");
        productDao.save(product);

        Order order = testOrder("lastName", "address", "date", 15.15, "Herr");
        OrderedProducts orderedProduct = new OrderedProducts();
        orderedProduct.setOrder(order);
        orderedProduct.setProduct(product);
        orderedProduct.setQuantity(1);
        orderedProductsDao.save(orderedProduct);
    }

    @Test
    public void testFindByIdOrder(){
        String clientName = "clientTestFinder";

        Order order = testOrder(clientName, "address", "date", 15.15, "Herr");
        Long id = order.getOrderId();

        Optional<Order> foundOrderOpt = orderDao.findById(id);
        Assertions.assertTrue(foundOrderOpt.isPresent());
        Order foundOrder = foundOrderOpt.get();
        Assertions.assertEquals(clientName, foundOrder.getLastName());
    }
}