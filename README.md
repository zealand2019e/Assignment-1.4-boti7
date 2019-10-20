# Assignment 4: TCP server

You must make a TCP server (Console Application). The application must be a concurrent server that can handle multiple clients running on port 4646.

The server should be able to handle books of the same type that you have already created in Task 1. The server should be able to understand the following request (protocol), there are always two lines:

* GetAll // all books from the server, line two is empty eg Retrieve All
    ```
    GetAll

    ```
* Get \<isbn13> // A book with that title will be downloaded
    ```
    Get
    9780133594140
    ```
* Save \<book in json> // Book is saved (added to static list)
    ```
    Save
    {"Title": "UML", "Author": "Larman", "Page Number": 654, "Isbn13": "9780133594140"}
    ```

The server has a static list to keep the book items. GetAll returns a list of books as a JsonString, Get returns a book as a JsonString, while Save does not return anything.

You need to test your solution with SocketTest.
