# To run program
1. In appsettings.json:
	* Set DbIsSqlServer to false for in-memoryh mode
	* Set DbIsSqlServer to true for Sql Server, and set connection string in GuidApiServiceDatabase
2. Run the program
3. In browser go to https://localhost:7055/swagger/index.html to test via swagger


# Questions/Open items

1. Date and time
Datetime can get complicated here. In this program we store as default timezone.
A better option is to store as UTC and return as such. 
The client code can convert to user's detected timezone.

2. Guid generation
If guid that I generate exists in DB already, I throw an exception and ask to try again.
Better options: Handle guid handling in DB - insert key if given, and generate in DB if not.
Another less better option is to retry a set number of times. Since it is a guid,
chances of second collision with generate guid is very small.

3. Guid reuse
Assuming once a guid was generated, it cannot be reused even if expired.
Depending on requirement, this may or may not be the right option.

4. Operation atomicity
In current code, between an EF find and save/remove we dont do an atomic operation
If we have enough callers, we would want to make the operations atomic
Concurrent operations in EF: https://docs.microsoft.com/en-us/ef/ef6/saving/concurrency?redirectedfrom=MSDN 

5. Sanitize input
Not doing this in current code. Depends on application usage and scope: 
https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-6.0 

6. Overposting
Not doing this in current code:
https://go.microsoft.com/fwlink/?linkid=2123754

7. Caching not implemented

8. As database gets more complex, can introduce a repository pattern by abstracing what is in GuidService

9. Only some unit tests are written as example

10. Not all items are documented, most of the important public ones are covered
