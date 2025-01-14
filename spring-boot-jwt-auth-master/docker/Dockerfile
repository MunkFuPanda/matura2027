FROM eclipse-temurin:17-jre

RUN mkdir -p /opt/application
COPY spring-boot-jwt-auth-1.0-SNAPSHOT.jar /opt/application/backend.jar
WORKDIR /opt/application
EXPOSE 8081
CMD [ "java", "-jar", "backend.jar" ]
