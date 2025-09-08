package at.ac.htlwrn.controller;

import at.ac.htlwrn.dto.LoginUser;
import org.junit.jupiter.api.Test;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.web.server.LocalServerPort;

import static io.restassured.RestAssured.*;
import static org.hamcrest.Matchers.*;

@AutoConfigureTestDatabase
@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
public class AuthenticationControllerIT {

    @LocalServerPort
    private int port;

    @Test
    public void whenPostOnGenerateTokenWithValidCredentialsReturnOK() {
        LoginUser loginUser = new LoginUser();
        loginUser.setUsername("alex123");
        loginUser.setPassword("alex123");

        given().port(port).when()
                .contentType("application/json")
                .body(loginUser)
                .post("/token/generate-token")
                .then().statusCode(200)
                .assertThat()
                .body("message", equalTo("success"));
    }
}
