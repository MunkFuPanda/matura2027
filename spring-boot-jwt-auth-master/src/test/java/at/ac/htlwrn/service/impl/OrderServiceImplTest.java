package at.ac.htlwrn.service.impl;

import at.ac.htlwrn.dao.OrderDao;
import at.ac.htlwrn.dao.ProductDao;
import at.ac.htlwrn.dto.OrderDto;
import at.ac.htlwrn.dto.ProductDto;
import at.ac.htlwrn.model.Order;
import at.ac.htlwrn.model.OrderedProducts;
import at.ac.htlwrn.model.Product;
import at.ac.htlwrn.service.OrderService;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.context.SpringBootTest;

import java.util.*;

@AutoConfigureTestDatabase
@SpringBootTest
public class OrderServiceImplTest {

    @Autowired
    private OrderService orderService;

    @Autowired
    private OrderDao orderDao;

    @Autowired
    private ProductDao productDao;

    @Test
    public void testOrder() {
        Order order = new Order();
        order.setSalutation("Herr");
        order.setLastName("Name1");
        order.setAddress("Test Straße");
        order.setDate(String.valueOf(new Date()));
        order.setOrderPrice(15.15);
        order.setOrderedProducts(new HashSet<>());


        Optional<Product> productOpt = productDao.findById(-1L);
        Assertions.assertTrue(productOpt.isPresent());
        Product product = productOpt.get();

        OrderedProducts orderedProducts = new OrderedProducts();
        orderedProducts.setOrder(order);
        orderedProducts.setProduct(product);
        orderedProducts.setQuantity(2);
        order.getOrderedProducts().add(orderedProducts);

        orderDao.save(order);
    }

    @Test
    public void testCreateOrder() {
        OrderDto orderDto = new OrderDto();
        orderDto.setSalutation("Herr");
        orderDto.setLastName("TestMannshofer");
        orderDto.setAddress("Test Straße");
        orderDto.setDate(String.valueOf(new Date()));
        orderDto.setOrderPrice(15.15);


        ArrayList<ProductDto>  productDtos = new ArrayList<>();
        ProductDto productDto1 = new ProductDto();
        productDto1.setProductName("Product 1");
        productDto1.setPrice(15.15);
        productDto1.setImageName("Test Img");
        productDto1.setValidFrom(String.valueOf(new Date()));
        productDto1.setQuantity(2);

        ProductDto productDto2 = new ProductDto();
        productDto2.setProductName("Product 2");
        productDto2.setPrice(20);
        productDto2.setImageName("Test Img");
        productDto2.setValidFrom(String.valueOf(new Date()));
        productDto2.setQuantity(4);

        productDtos.add(productDto1);
        productDtos.add(productDto2);
        orderDto.setProducts(productDtos);

        orderService.createOrder(orderDto);
    }

    @Test
    public void testFindAll() {
        Assertions.assertNotNull(orderService.findAll());
    }

    @Test
    public void testGetActiveOrders() {
        Assertions.assertNotNull(orderService.getActiveOrders());
    }

    @Test
    public void testCancelOrder() {
        Assertions.assertNotNull(orderService.cancelOrder(1L).getDoneDate());
    }

    @Test
    public void testFinishOrder() {
        Assertions.assertNotNull(orderService.finishOrder(1L).getDoneDate());
    }

}
