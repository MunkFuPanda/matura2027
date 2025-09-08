# Spring Boot Security Jwt Authentication

This is a sample project to provide example on how to add JWT token authentication in a spring boot application.
The example uses maven as a build tool and also sample script to run on application startup so that anybody can get started by simply running Application.java
 
The complete explanation can be found on my blog here - [Spring Boot Security JWT Authentication](http://www.devglan.com/spring-security/spring-boot-jwt-auth)
## Technology Used

 1. Spring Boot (2.5.7.RELEASE)
 2. JWT (0.6.0)
 3. H2 Database (for unit tests)
 4. Mysql (production mode)
 5. Java 17

## Important info:
Swagger-UI: http://localhost:8081/swagger-ui/

H2-Console:http://localhost:8081/h2-console/

## Configure SSH-Key on Gitlab

1. Install Git Bash and start it
2. ssh-keygen -t rsa -b 2048 -C "SSH key of <yourname>" #replaye yourname with your name, Press 3 times Enter
3. cat ~/.ssh/id_rsa.pub | clip  # | clip copies content to clipboard. Use without "| clip" for testing
4. Login to gitlab.htlwrn.ac.at and go to Avatar -> Edit Profile -> SSH Keys
5. Click "Add Key" and insert printed key from Git Bash (from step 3)
6. Open your git project. Click button "Code" and "Clone with SSH"
7. Insert "git clone " and right click paste on the git bash console
8. Projekt is cloned and ready to use. No username+password needed for pull/push. 

##Further Reading
Templated based on Devglan. The complete explanation can be found on my blog here - [Spring Boot Security JWT Authentication](http://www.devglan.com/spring-security/spring-boot-jwt-auth)

You may be interested in other spring security articles:

[Spring Boot Security OAUTH2 Example](http://www.devglan.com/spring-security/spring-boot-security-oauth2-example).

[Spring Boot Security Hibernate Login](http://www.devglan.com/spring-security/spring-boot-security-hibernate-login-example)

[Simple E-Commerce Implementation with Spring and Angular](https://www.baeldung.com/spring-angular-ecommerce)

[Securing Actuator Endpoints with Spring Security](http://www.devglan.com/spring-security/securing-spring-boot-actuator-endpoints-with-spring-security)
