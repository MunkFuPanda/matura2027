package at.ac.htlwrn.dao;

import at.ac.htlwrn.model.Product;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;

import java.util.Optional;

@DataJpaTest
public class ProductDaoTest {

    @Autowired
    private ProductDao productDao;

    private Product testProduct(String productName, double price, String imageName, String validFrom) {
        Product product = new Product();
        product.setProductName(productName);
        product.setPrice(price);
        product.setImageName(imageName);
        product.setValidFrom(validFrom);
        return productDao.save(product);
    }

    @Test
    public void findById() {
        String productName = "product1";

        Product product = testProduct(productName, 100, "img1", "validFrom");
        Long id = product.getProductId();

        Optional<Product> optional = productDao.findById(id);
        Assertions.assertTrue(optional.isPresent());
        Product foundProduct = optional.get();
        Assertions.assertEquals(productName, foundProduct.getProductName());
    }
}
