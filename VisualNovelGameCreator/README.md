# Visual Novel Creator
This is a Visual Novel Creator written in C#;

## Usage
You can locate the [resources here](AVG_GameBuild/Resources)
By editing the TestStory.txt file you can edit the stories of the file.
You can also just change the txt file inside the program under [StoryControl.cs](AVG_GameBuild/StoryControl.cs)
The images inside the folder can be used at given time by writting commands in the TestStory.txt.
The resources in side the resources folder are some random images and audio files from the internet. I don't think I have the rights to use them freely. If there's any form of concerns I will change them immediately.

## Commands
All the commands works as following and might be changed in the future.

The line following the command contains the parameters of the command. By calling a command an action is performed, and the next line is skipped.

Commands need to be bracked with [].

Commands currently available are:

1. Name
2. Jump
3. Choice
4. Background Imgae
5. Character Image
6. BGM

#### [Name]
The parameter will be treated as a string and displayed at the Name field.
#### [Jump]
Need an Integer, jump to the corresponding line number in the txt file.
#### [Choice]
Parameters are split by comma, with corresbonding line to jump to and the descriptiong to display for this choice.
One choice must come in form: Line, Text
At most three choice available with comma in between to seperate them.
Line, Text, Line, Text, Line, Text
#### [Background Imgae]
Change the background image of the game to another.
#### [Character Image]
Change the character image of a certain position into the given image.
Has format similar to choice
Place, Image, Place, Image ...
Maximum 3 at the same time
#### [BGM]
Change the background music of the game, play, or stops the music.

More details to be added in the future.