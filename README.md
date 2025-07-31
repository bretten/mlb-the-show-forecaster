# Further Documentation
Please see the [wiki](https://github.com/bretten/mlb-the-show-forecaster/wiki).

---

# What is it?
MLB The Show Marketplace Forecaster is an application that uses an MLB Player's real-world performance to predict the price of their card in MLB The Show.

## What is MLB The Show?
[MLB The Show](https://en.wikipedia.org/wiki/MLB:_The_Show) is a baseball video game that allows users to create custom teams with real-life players to play against other teams. Think along the lines of fantasy leagues, but you actually control your players.

In order to form your team, you need to collect in-game baseball cards that represent real-life players. To earn cards, you play baseball games to earn an in-game currency called "stubs". You then use stubs to either purchase a pack of randomized cards or use the in-game marketplace to buy cards from other users or sell your own. Of course, you can also use real money to purchase stubs (it's the [business model](https://store.playstation.com/en-us/product/UP9000-PPSA17085_00-STB0050000000000) after all).

A card's performance in-game is based on the actual player's real-life performance in the MLB. So a more performant player will likely get better "stats" and draw a [higher price](https://mlb24.theshow.com/items/7d6c7d95a1e5e861c54d20002585a809) in the market. The purpose of this forecaster is to then look for these trends and display them to the user.

---

# How does it work?
The application monitors a player, their performance, and their corresponding card in **MLB The Show**. All relevant data for a player is combined to create a comparison between a player's real-world performance and the marketplace performance of their card in **MLB The Show**.

## Architecture?
Primarily following DDD principles, each domain is split into its own microservice. They communicate using a message broker for events or APIs when on-demand data is needed. The microservices are deployed as containers into their own virtual network and are not accessible publicly. A separate, auth-protected [gateway app](src/Apps/MlbTheShowForecaster.Apps.Gateway/) then exposes only what needs public access to a SPA. The end user authenticates via the SPA and is able to start jobs and view player reports.

---

# Which technologies?
It is built with:
 - .NET
   - ASP.NET Core microservices
   - SignalR/Websockets (real-time updates for UI)
 - React + Vite ([UI repo](https://github.com/bretten/mlb-the-show-forecaster-ui))
   - [MUI component lib](https://mui.com/)
 - PostgreSQL operational db
 - MongoDB reporting db (for front-end consumption)
 - RabbitMQ message broker
 - Redis event store

## How is it deployed?
All the .NET apps are containerized and deployed to AWS Elastic Container Service via a Github Actions CD [pipeline](.github/workflows/cd-release.yml). The microservices live in their own VPC, with no public access. The gateway app provides access to these microservices via a load balancer that handles all incoming traffic. Authentication is handled with AWS Cognito.
 - Docker
 - GitHub Actions (CI/CD)
 - AWS
   - VPC
   - ECS
   - Cognito

## UI?
The UI is built in React + Vite and its [repository is separate](https://github.com/bretten/mlb-the-show-forecaster-ui). The release pipeline for the UI publishes the React build files and the ASP.NET Core app then loads these as static files.

---

# How can I run it?
A demo has been prepared both online and via `docker compose`. There are two demo users:
 - `user1` / `User1user1!` (`Admin` user group - **_can start jobs_**)
 - `user2` / `User2user2!` (`Viewer` user group - **_cannot start jobs_**)

## Docker compose
To start it, from the root dir:
```shell
docker compose -f .\docker-compose.yml -f .\docker-compose.demo.yml build --no-cache
docker compose -f .\docker-compose.yml -f .\docker-compose.demo.yml up -d
```

To stop it:
```shell
docker compose -f .\docker-compose.yml -f .\docker-compose.demo.yml down
```

## Online
Visit the demo [here](https://mlb-the-show-forecaster.brettnamba.com/).\
_~~Note that ECS Fargate Spot instances are used to reduce demo costs, so the containers may be [stopped and started](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/fargate-capacity-providers.html#fargate-capacity-providers-termination)~~._

From March 2025 (Opening Day) to June 2025, there was nearly a **quarter of a billion listing order rows** in the DB. To reduce the costs of order ingestion, the demo was turned into a static site with data up until **2025-06-06**.