# Project Setup and Delivery:

## Docker-compose
I have created a docker-compose for the Redis, MongoDB and MySQL. Since I chose MySQL the script will automatically be run, and the table are ready to go.
-	I have created some dummy data to be used, but the reference to the object id in MongoDB is null for they do not exist

![compose](/docs/img/dockercompose.png)

## C# Api application
I’ve created docker-compose with the API, using the Swagger, to easy test the different calls, so all you need to do is run the docker-compose up command, and it should work.

![swagger](/docs/img/swaggerOnDocker.png)


[return](/README.md)