import time
import sys
import os

DataArray = []
LineCount = 2
PrintSpeed = 0.1
inGame = True
ExtraInfoThisLine = ""
Location = {
		"Name":""
}
PlayerStatus = {
    "Name":""
}
ConvStatus = {
    "Name":""
}

def ReadStory():# Read the entire story file
    global DataArray
    GameFile = open("story.dat","r")
    DataArray = GameFile.readlines()
    DataArray = [value for value in DataArray if value != "\n"]
    DataArray = [Lines.rstrip('\n') for Lines in DataArray]
def GameSaveManager(mode):#Decides what to do based on "mode"
    if(mode.lower() == "starting"):# if mode is Starting
        StartGame = input("Enter a number between 1 to 2 to: \n#1 New Game\n#2 Load Game\n")
        while True:# Ask for an input and unless 1 or 2, keep asking
            if StartGame == "1":
                #Perform "New Game" here
                TypeAnimation("Starting New Game\n")
                break
            elif StartGame == "2":
                #Perform "Load Game" here
                LoadGame()
                break
            else:
                StartGame = input("Invalid input")# if no match, keep looping
    if(mode == "InGame"):# if mode is InGame
        SaveGame = input("Enter a number between 1 to 2 to: \n#1 Save Game\n#2 Load Game\n")
        while True:
            if SaveGame == "1" :
                #save the game here
                break
            if SaveGame == "2" :
                #load a game here
                LoadGame()
                break
            else: SaveGame = input("Invalid input")
    return
def LoadGame():#Loading a game
    address = (DataArray[0] + ".save").replace("\n","")# Remember the Address
    SaveN = 0
    while True:# Ask for the player to choose a save, retry if invalid
        try:
            SaveN = int(input("Which one?"))# Break if valid
            break
        except:
            print("invalid input")
            continue
    TypeAnimation("Loading Game")
    if(os.path.isfile(address)):# If File exists in the address
        Save = open(address,"r")
        SavesAvailable = Save.readlines()
        if len(SavesAvailable) > SaveN:
            RawData = SavesAvailable[SaveN - 1]
            RawSave = RawData.split(",")
        else:
            print("Save Does Not Exists")
    else:
        TypeAnimation("Failed to Load Game, please check your files")
    return
def Combat():
    return
def isCommand(PotentialCmd, isPlayerCommand):
    global LineCount
    if(isPlayerCommand):
        pass
        if  PotentialCmd[:2] == "//" :# Check if Line is Command
            return True
    if  PotentialCmd[:3] == "//[" and PotentialCmd[-1:] == "]":# Check if Line is Command
        PotentialCmd = PotentialCmd[3:-1].lower()
        ProcessCommand(PotentialCmd)
        LineCount += 1
        return True
    return False
def ProcessCommand(command):
    global PlayerStatus
    global ConvStatus
    global Location
    global LineCount
    RawCmd = command.split(";")
    if(RawCmd[0] == "initialPlayerStatus"):# InitialPlayerStatus sets the player's status in this game //[initialPlayerStatus; Something = some number; SomethingElse = some number]
        for i in range(1,len(RawCmd)):
            try:
                Parameter = RawCmd[i].split("=")
                PlayerStatus[Parameter[0].strip(" ")] = Parameter[1].strip(" ")
            except:
                print("-Invalid Command, initialPlayerStatus failed-")
        pass
    if(RawCmd[0] == "playerStatusChange"):# playerStatusChange edits the player's status in this game //[playerStatusChange; Something, +, somenumber; SomethingElse, -, some number]
        for i in range(1,len(RawCmd)):
            Parameter = RawCmd[i].split(",")
            try:
            	if(Parameter[1] == "+"):
                	PlayerStatus[Parameter[0]] = Parameter[0] + int(Parameter[2])
            	if(Parameter[1] == "-"):
                	PlayerStatus[Parameter[0]] = Parameter[0] - int(Parameter[2])
            except:
            	print("-Invalid Command, playerStatusChange failed-")
        pass
    if(RawCmd[0] == "conversation"):# conversation command sets the elements of the current conversation //[conversation; false; Name = ConversationName ]
        if(RawCmd[1].lower() == "false"):
            #not a forced conversation, check if can skip
            pass
        for i in range(2,len(RawCmd)):
            Parameter = RawCmd[i].split("=")
            ConvStatus[Parameter[0]] = Parameter[1]
    if(RawCmd[0] == "location"):# location command sets the elements of the current location //[location; Name = LocationName]
        for i in range(1,len(RawCmd)):
            Parameter = RawCmd[i].split("=")
            ConvStatus[Parameter[0]] = Parameter[1]
        pass
    if(RawCmd[0] == "enemy"):# enemy command sets the elements of meeting an enemy, drops, chance of meeting
        pass
    if(RawCmd[0] == "boss"):# boss command sets the elements of a boss
        pass
    if(RawCmd[0] == "choice"):# choice command gives the options to choose
        Choices = []
        for i in range(1,len(RawCmd)):
            Parameter = RawCmd[i].split(",")
            print(" ",i,Parameter[0])
            Choices.append([Parameter[0], Parameter[1]])
        while True:
            try:
                PlayerInput = int(input("\nPlease Make a choice>")) - 1
                if(PlayerInput<= len(Choices)):
                    LineCount = int(Choices[PlayerInput][1])-3
                    break
            except:
                print("-Invalid Input, please try again-")
        pass
    if(RawCmd[0] == "skip"):# skip command can skip to a certain line
        LineCount = int(RawCmd[1])-3
        pass
def PlayerInput():
    global LineCount
    InputData = input(">")
    if not isCommand(InputData, True):
        LineCount += 1
def TypeAnimation(ToPrint):#Print the characters one by one to create a typing animation Ctrl+C to interupt
    CharLeft = ToPrint
    for char in ToPrint:
        print(char, end = "")
        CharLeft = CharLeft[1:]
        try:
            time.sleep(PrintSpeed)
        except KeyboardInterrupt:
            print(CharLeft, end="")
            break
def Update():# Update is called constantly
    if(len(DataArray) > LineCount - 1):# If Line number still in range of array
        Line = DataArray[LineCount - 1]
        if not isCommand(Line, False):
            TypeAnimation(Line)
            PlayerInput()
    else:
        TypeAnimation("The End\n")
        if input("enter Exit to quit the game>").lower() == "exit":
            global inGame
            inGame = False
    return
def Start():# Start only initiates
    ReadStory()
    GameSaveManager("starting")
    return


Start()

while inGame:
    Update()


