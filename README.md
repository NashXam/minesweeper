minesweeper
===========

A Nashville Xamarin User Group coding dojo excercise

Write a Xamarin app named "Minesweeper" that, um, implements a simple version of [Minesweeper](http://en.wikipedia.org/wiki/Minesweeper_(video_game))!

How to contribute
-----------------
Contribute your solution by adding a folder named {your twitter handle} such as:

````
/bryan_hunter
````

Create your solution below your folder. Push as you go. Enjoy!


Specs
-----

````
GIVEN I have just installed the application
WHEN I open the application
THEN I will be presented with a game board consisting of an 8x8 grid of tiles, a high score, and a current score.

HS:12       CS:00

| | | | | | | | |
| | | | | | | | |
| | | | | | | | |
| | | | | | | | |
| | | | | | | | |
| | | | | | | | |
| | | | | | | | |
| | | | | | | | |

GIVEN a new game
WHEN I tap anywhere on the game board
THEN a random tile that contains no mine is selected and
     each neighboring tile is labeled with a digit indicating the number of adajent mines and
     the game is being played

GIVEN a game being played
WHEN I long-tap an empty tile that contains no mine
THEN the tapped tile changes color to shows it's clear and
     each neighboring tile is labeled with a digit indicating the number of adajent mines and
     current score is incremented by 1 and
     high score is set to current score if current score is greater

GIVEN a game being played
WHEN I long-tap a tile that contains a mine
THEN you lose! and
     the game is over

GIVEN a game being played
WHEN I long-tap the last empty tile that contains no mine
THEN you win! and
     the game is over

GIVEN a game being played
WHEN I tap an empty tile
THEN the tapped tile is flagged

GIVEN a game being played
WHEN I tap a flagged tile
THEN the tapped tile is emptied

GIVEN a game being played or over
WHEN I shake the device
THEN all tiles are emptied and
     a new game is started and
     current score is set to 0

GIVEN a game that is "over"
WHEN I tap anywhere on the game board
THEN all tiles are emptied and
     a new game is started and
     current score is set to 0
````
