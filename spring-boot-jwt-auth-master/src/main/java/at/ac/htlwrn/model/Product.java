package at.ac.htlwrn.model;

import javax.persistence.*;
import java.util.Set;

@Entity
@Table(name = "Product")
public class Product {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long productId;

    @Column(name = "productName", nullable = false)
    private String productName;

    @Column(name = "price", nullable = false)
    private double price;

    @Column(name = "imageName", nullable = false)
    private String imageName;

    @Column(name = "validFrom", nullable = false)
    private String validFrom;

    @Column(name = "validUntil")
    private String validUntil;

    @OneToMany(mappedBy = "product", cascade = CascadeType.ALL, orphanRemoval = true)
    private Set<OrderedProducts> orderedProducts;

    public long getProductId() {
        return productId;
    }

    public void setProductId(long id) {
        this.productId = id;
    }

    public String getProductName() {
        return productName;
    }

    public void setProductName(String productName) {
        this.productName = productName;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }

    public String getImgName() {
        return imageName;
    }

    public void setImgName(String imgName) {
        this.imageName = imgName;
    }

    public String getValidFrom() {
        return validFrom;
    }

    public void setValidFrom(String validFrom) {
        this.validFrom = validFrom;
    }

    public String getValidUntil() {
        return validUntil;
    }

    public void setValidUntil(String validUntil) {
        this.validUntil = validUntil;
    }
}
