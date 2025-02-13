The application consists of 2 microservices:
•	AuctionInventory: Responsible for the Vehicle catalog
•	AuctionService: Responsible for Auctions

This separation allows us to develop, deploy and scale the services individually. 
Each service could run on a dedicated docker container or cloud serverless service like Azure App Service.
The AuctionInventory is a web api based on a simplified CQRS architecture, with commands for create and update operations, and query for data retrieval. 
It saves the data in an in-memory db for simplicity.

The AuctionService is also a web api based on a simplified CQRS architecture, with commands for create and update operations, and query for data retrieval. 
The CreateBid endpoint explores the scenario where we can expect a very big inbound data flow to place bids. 
It simply adds the bid into a message queue and returns the response. 

AuctionService has an interface with AuctionInventory to retrieve the vehicle data. 
For simplicity this has been done as a http call, but ideally it should be a message queue or a cache.
After data, we have a BidConsumer, which asynchronously consumes the bis from the queue, executes the business logic and saves the data. 
To make the bid processing as fast as possible, the bids are also save in a memory cache, so when we need to retrieve the bids, we can get them from the cache instead of the db.
With this approach we can horizontally scale up the AuctionService if we need more throughput.  

For the sake of simplicity for this exercise, the BidConsumer is a bit simplistic. 
Ideally it should be on a separate service, which could be a worker service instead of a web api. 
The memory cache should be a real distributed cache like Redis.
Also, we could add more parallel processing if we created one queue per Auction. 
