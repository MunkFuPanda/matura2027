package at.ac.htlwrn.dto;

import java.util.ArrayList;

public class OrderDto {

    private Long orderId;
    private String salutation;
    private String firstName;
    private String lastName;
    private String address;
    private String date;
    private double orderPrice;
    private String doneDate;
    private String cancelledDate;
    private ArrayList<ProductDto> products;


    public Long getOrderId() {
        return orderId;
    }

    public void setOrderId(Long orderId) {
        this.orderId = orderId;
    }

    public String getSalutation() {
        return salutation;
    }

    public void setSalutation(String salutation) {
        this.salutation = salutation;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }

    public double getOrderPrice() {
        return orderPrice;
    }

    public void setOrderPrice(double orderPrice) {
        this.orderPrice = orderPrice;
    }

    public String getDoneDate() {
        return doneDate;
    }

    public void setDoneDate(String doneDate) {
        this.doneDate = doneDate;
    }

    public String getCancelledDate() {
        return cancelledDate;
    }

    public void setCancelledDate(String dateCancelled) {
        this.cancelledDate = dateCancelled;
    }

    public ArrayList<ProductDto> getProducts() {
        return products;
    }

    public void setProducts(ArrayList<ProductDto> products) {
        this.products = products;
    }
}
