# What is it?
MLB The Show Marketplace Forecaster is an application that uses an MLB Player's real-world performance to predict the price of their card in MLB The Show.

## What is MLB The Show?
[MLB The Show](https://en.wikipedia.org/wiki/MLB:_The_Show) is a baseball video game that allows users to create custom teams with real-life players to play against other teams. Think along the lines of fantasy leagues but you actually control your players.

In order to form your team, you need to collect in-game baseball cards that represent real-life players. To earn cards, you play baseball games to earn an in-game currency called "stubs". You then use stubs to either purchase a pack of randomized cards or you can use the in-game marketplace to buy cards from other users or sell your own.

A card's performance in-game is based on the actual player's real-life performance in the MLB. So a more performant player will likely get better "stats" and draw a higher price in the market.

And of course, they allow you to use real money to purchase stubs...it's part of the game's business model.

## What does this application forecast?
The application uses real-life MLB stats and attempts to predict which cards will go up in value and which will lose value. In order to do this, it will weigh a player's performance at different points in the season to their current performance.

# What is the domain's ubiquitous language?
Approaching this application as a real-world platform, involved parties would need to understand the following terms.

## Basic terms
- `Player` - A player in the MLB. *Not used in regards to users of the video game itself*
- `Card` - A player in MLB The Show is represented by a card that you can collect. You can have more than one and any sell extras.

## Value terms
The following terms are variables that will **determine the value of a player's card**
 - `Player status` - It is important to conceptualize a player as being active or being a free agent as it has a direct effect on the card's value.
 - `Player performance` - A player's in-game stats can represent a trend and is a variable in determining a card's price
 - `Marketplace history` - We need to look at the value of the player throughout the whole season as this may predict its value later in the season
