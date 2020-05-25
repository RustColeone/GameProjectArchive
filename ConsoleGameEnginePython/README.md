# ConsoleGameEngine
Console game engine built on multiple languages
## Purpose
This is a very simple game engine built by interpreting contents in a file and translating them into commands. The game engine features no gui and is currently under construction. There are multiple versions of this, and now I am currently working on a python version. Updates and examples are comming soon.

The logics behind this is very simple, you take a file, put the data in it in a certain format, then the program translates it and do things line by line. There's command in player's side and the writer's side.

## Currently available versions
1. C#(Not uploaded)
2. Python(Still working on it)

## For Python Version
List of commands:
1. conversation command sets the elements of the current conversation //[conversation; false; Name = ConversationName]
2. playerStatusChange edits the player's status in this game //[playerStatusChange; Something, +, somenumber; SomethingElse, -, some number]
3. conversation command sets the elements of the current conversation //[conversation; false; Name = ConversationName ]
4. location command sets the elements of the current location //[location; Name = LocationName]
5. choice command gives the options to choose //[choice; InfoToShow, Line number to go to; ...]
6. skip command can skip to a certain line //[skip, Line number to go to]

In python, things in //[...strings here...] are translated as an command. And each parameter is separated inside with the split function by a semicolum (;), and for each piece of text, assign a different function. Please see code


