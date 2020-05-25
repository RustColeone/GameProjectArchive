using ACG_GameBuild;
using System;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AVG_GameBuild
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        StoryControl storyControls = new StoryControl();
        string output;
        string SpeakerName;
        int LineLength = 0;
        int CharacterTypeIndex = 0;
        int PlaySpeed = 30;
        string[] ChoicesCode = new string[3];
        string BackgroundImageSource = "";
        bool BGMStatus = false;

        MediaPlayer BackgroundMusic = new MediaPlayer();

        #region UIControl

        public GamePage()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, PlaySpeed);
            //dispatcherTimer.Start();
            InitializeComponent();

            LogGrid.Visibility = Visibility.Hidden;

            ChoiceVisibilityControl(false, 3);
            //{Binding ElementName=VolumeSlider,Path=Value}
        }

        private void MainWindow1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            SizeChangedEventArgs CanvasChangedArguments = e;
            if (CanvasChangedArguments.PreviousSize.Width == 0) { return; }
            double HeightChangedRatio = CanvasChangedArguments.NewSize.Height / CanvasChangedArguments.PreviousSize.Height;
            double WidthChangedRatio = CanvasChangedArguments.NewSize.Width / CanvasChangedArguments.PreviousSize.Width;
            foreach (FrameworkElement element in Grid1.Children)
            {
                //Canvas.SetLeft(element,Canvas.GetLeft(element) * WidthChangedRatio + 10);
                //Canvas.SetTop(element, Canvas.GetTop(element) * HeightChangedRatio + 10);
                element.Width = element.Width * WidthChangedRatio;
                element.Height = element.Height * HeightChangedRatio;

                LogTextBox.Height = LogGrid.Height - 10;
                LogTextBox.Width = LogGrid.Width - 30;

                MiscellaneousResizes();
            }
        }

        private void MiscellaneousResizes()
        {
            //Canvas.SetLeft(element,Canvas.GetLeft(element) * WidthChangedRatio + 10);
            //Canvas.SetTop(element, Canvas.GetTop(element) * HeightChangedRatio + 10);
            Portrait1.Height = CharacterImageGrid.Height - 20;
            Portrait2.Height = CharacterImageGrid.Height - 20;
            Portrait3.Height = CharacterImageGrid.Height - 20;

            Portrait1.Width = (CharacterImageGrid.Width - 30) / 3;
            Portrait2.Width = (CharacterImageGrid.Width - 30) / 3;
            Portrait3.Width = (CharacterImageGrid.Width - 30) / 3;

            Choice1.Height = ChoiceGrid.Height * 2 / 9;
            Choice1.Width = ChoiceGrid.Width * 0.8;

            Choice2.Height = ChoiceGrid.Height * 2 / 9;
            Choice2.Width = ChoiceGrid.Width * 0.8;

            Choice3.Height = ChoiceGrid.Height * 2 / 9;
            Choice3.Width = ChoiceGrid.Width * 0.8;

            Choice1.FontSize = Choice1.Height / 2;
            Choice2.FontSize = Choice2.Height / 2;
            Choice3.FontSize = Choice3.Height / 2;

            CloseLog.FontSize = CloseLog.Height * 14 / 20;
            CloseLog.Height = LogTextBox.Height / 20;
            CloseLog.Width = CloseLog.Height;

            NextButton.FontSize = NextButton.Height / 2;
            ConversationBox.FontSize = ConversationBox.Height * 2 / 9;
            LogTextBox.FontSize = LogTextBox.Height / 20;
            ToolBarTray1.Height = 26;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //NameLabel.Content = SpeedSlider.Value;
            PlaySpeed = Convert.ToInt32(30 - 1.5 * SpeedSlider.Value);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, PlaySpeed);
        }

        private void Button_Click(object sender, RoutedEventArgs e)//Open Log
        {
            LogGrid.Visibility = Visibility.Visible;
        }

        private void CloseLog_Click(object sender, RoutedEventArgs e)
        {
            LogGrid.Visibility = Visibility.Hidden;
        }

        private void ChoiceVisibilityControl(bool OnOff, int NumberControlled)
        {
            if (NumberControlled == 0)
            {
                OnOff = false;
            }
            if (OnOff == true)
            {
                ChoiceGrid.Visibility = Visibility.Visible;
                switch (NumberControlled)
                {
                    default:
                        break;
                    case 1:
                        Choice1.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        Choice1.Visibility = Visibility.Visible;
                        Choice2.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        Choice1.Visibility = Visibility.Visible;
                        Choice2.Visibility = Visibility.Visible;
                        Choice3.Visibility = Visibility.Visible;
                        break;
                }
            }
            else
            {
                ChoiceGrid.Visibility = Visibility.Hidden;
                switch (NumberControlled)
                {
                    default:
                        break;
                    case 1:
                        Choice1.Visibility = Visibility.Hidden;
                        break;
                    case 2:
                        Choice1.Visibility = Visibility.Hidden;
                        Choice2.Visibility = Visibility.Hidden;
                        break;
                    case 3:
                        Choice1.Visibility = Visibility.Hidden;
                        Choice2.Visibility = Visibility.Hidden;
                        Choice3.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }

        #endregion

        private void TextAnimations()
        {

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // code goes here
            if (CharacterTypeIndex < LineLength)
            {
                ConversationBox.Text += output[CharacterTypeIndex];
                CharacterTypeIndex++;
            }
            else
            {
                CharacterTypeIndex = 0;
                storyControls.row++;
                dispatcherTimer.Stop();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ConversationBox.Clear();
            CharacterTypeIndex = 0;
            if (dispatcherTimer.IsEnabled == false)//when the animation of typewrite is not going, start animation
            {
                output = storyControls.ReadLine(storyControls.row);

                if (storyControls.IsCommandCheck(output))
                {
                    ExecuteCommand(output);//This will execute the command
                    storyControls.row += 2;//One line after command is the command's execution, so skip
                    output = storyControls.ReadLine(storyControls.row);
                }

                ConversationBox.Text += SpeakerName + "\r\n";

                LineLength = output.Length;
                dispatcherTimer.Start();
                if (LogTextBox.Text == "") { LogTextBox.Text += output; }
                else { LogTextBox.Text += "\r\n" + output; }
            }
            else//if the animation is not done and it was clicked the second time, stop typewrite and instantly display everything
            {
                CharacterTypeIndex = 0;
                dispatcherTimer.Stop();
                ConversationBox.Text += SpeakerName + "\r\n" + output;
                storyControls.row++;
            }
        }

        private void ExecuteCommand(string Command)
        {
            switch (Command)
            {
                //Next line of the commend is what the commend is going to carried out
                default: break;
                case "":
                    for (int i = 1; storyControls.ReadLine(storyControls.row + 1 + i) == "";)
                    {
                        storyControls.row++;
                    }
                    output = storyControls.ReadLine(storyControls.row);
                    //ConversationBox.Text = output;
                    break;
                case "[Name]":
                    SpeakerName = storyControls.ReadLine(storyControls.row + 1);
                    LogTextBox.Text += "\r\n >" + SpeakerName;
                    break;
                case "[Choice]":
                    ChoiceCommand(storyControls.ReadLine(storyControls.row + 1));
                    storyControls.row -= 3;
                    break;
                case "[Jump]":
                    storyControls.row = Convert.ToInt32(storyControls.ReadLine(storyControls.row + 1)) - 3;
                    break;
                case "[Background Image]":
                    string FileURL = "/Resources/" + storyControls.ReadLine(storyControls.row + 1);
                    BackgroundImageSource = FileURL;
                    this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), FileURL)));
                    break;
                case "[Character Image]":
                    CharacterPortrait(storyControls.ReadLine(storyControls.row + 1));
                    break;
                case "[BGM]":
                    string GivenURL = storyControls.ReadLine(storyControls.row + 1);

                    switch (GivenURL)
                    {
                        default:
                            try
                            {
                                BGM.Source = new Uri(@"Resources/" + GivenURL, UriKind.Relative);
                                BGMStatus = BGMControl("Play");
                            }
                            catch {
                                break;
                            }
                            break;
                        case "Stop":
                            BGMStatus = BGMControl("Stop");
                            break;
                        case "Play":
                            BGMStatus = BGMControl("Play");
                            break;
                        case "Clear":
                            BGMStatus = BGMControl("Stop");
                            BGM.Source = null;
                            break;
                    }
                    break;
            }
        }

        #region ChoiceRelated

        private void ChoiceCommand(string Choices)
        {
            string[] ChooseBranch = Choices.Split(new string[] { "," }, StringSplitOptions.None);
            ChoiceGrid.Visibility = Visibility.Visible;
            switch (ChooseBranch.Length)
            {
                default:
                    break;
                case 2://Every case follows a pattern of changing the text of button first
                    ChoiceVisibilityControl(true, 1);
                    Choice1.Content = " #1 " + ChooseBranch[1];
                    //Then uploade the data to the ChoiceCodes so you can access
                    ChoicesCode[0] = ChooseBranch[0];
                    break;
                case 4:
                    ChoiceVisibilityControl(true, 2);
                    Choice1.Content = " #1 " + ChooseBranch[1];
                    Choice2.Content = " #2 " + ChooseBranch[3];

                    ChoicesCode[0] = ChooseBranch[0];
                    ChoicesCode[1] = ChooseBranch[2];
                    break;
                case 6:
                    ChoiceVisibilityControl(true, 3);
                    Choice1.Content = " #1 " + ChooseBranch[1];
                    Choice2.Content = " #2 " + ChooseBranch[3];
                    Choice3.Content = " #3 " + ChooseBranch[5];

                    ChoicesCode[0] = ChooseBranch[0];
                    ChoicesCode[1] = ChooseBranch[2];
                    ChoicesCode[2] = ChooseBranch[4];
                    break;
            }
            //SizeControl();
        }

        private void Choice1_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            ConversationBox.Text += SpeakerName + "\r\n" + output;
            ChoiceVisibilityControl(false, 3);
            ClickedChoice(1);
        }

        private void Choice2_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            ConversationBox.Text += SpeakerName + "\r\n" + output;
            ChoiceVisibilityControl(false, 3);
            ClickedChoice(2);
        }

        private void Choice3_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            ConversationBox.Text += SpeakerName + "\r\n" + output;
            ChoiceVisibilityControl(false, 3);
            ClickedChoice(3);
        }

        public void ClickedChoice(int ButtonClicked)
        {
            CharacterTypeIndex = 0;
            ConversationBox.Clear();
            storyControls.row = Convert.ToInt32(ChoicesCode[ButtonClicked - 1]) - 1;
            output = storyControls.ReadLine(storyControls.row);

            if (storyControls.IsCommandCheck(output))
            {
                ExecuteCommand(output);//This will execute the command
                storyControls.row += 2;//One line after command is the command's execution, so skip
                output = storyControls.ReadLine(storyControls.row);
            }

            ConversationBox.Text = SpeakerName + "\r\n" + output;

            storyControls.row++;
        }

        #endregion

        private void CharacterPortrait(string rawdata)
        {
            string[] ImageData = rawdata.Split(new string[] { "," }, StringSplitOptions.None);
            if (ImageData[0] == "Clear")
            {
                Portrait1.Source = null;
                Portrait2.Source = null;
                Portrait3.Source = null;
            }
            switch (ImageData.Length)
            {
                default:
                    break;
                case 2:
                    SetPortrait(ImageData[0], ImageData[1]);
                    break;
                case 4:
                    SetPortrait(ImageData[0], ImageData[1]);
                    SetPortrait(ImageData[2], ImageData[3]);
                    break;
                case 6:
                    SetPortrait(ImageData[0], ImageData[1]);
                    SetPortrait(ImageData[2], ImageData[3]);
                    SetPortrait(ImageData[4], ImageData[5]);
                    break;
            }
        }

        private void SetPortrait(string Place, string GivenURL)
        {
            ImageSource DestinatedSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/" + GivenURL, UriKind.Absolute));
            switch (Place)
            {
                default:
                    Portrait2.Source = DestinatedSource;
                    break;
                case "1":
                    Portrait1.Source = DestinatedSource;
                    break;
                case "2":
                    Portrait2.Source = DestinatedSource;
                    break;
                case "3":
                    Portrait3.Source = DestinatedSource;
                    break;
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem MI = sender as MenuItem;
            int ItemIndex = 0;
            if (MI != null)
                ItemIndex = Convert.ToInt32(MI.Tag.ToString());
            string ConversationData = ConversationBox.Text;
            int LineData = storyControls.row;
            string[] PortraitData = { Portrait1.Source.ToString(), Portrait2.Source.ToString(), Portrait3.Source.ToString() };
            string[] MediaData = { BGM.Source.ToString(), BGM.Position.Hours.ToString(), BGM.Position.Minutes.ToString(), BGM.Position.Seconds.ToString(), BGMStatus.ToString() };
            string[] ChoiseStates = { Choice1.IsVisible.ToString(), Choice2.IsVisible.ToString(), Choice3.IsVisible.ToString()};
            string[] ChoisesJump = ChoicesCode;
            string BackgroundImage = BackgroundImageSource;
            string WriteToFile = ConversationData + "_Split_"
                + LineData + "_Split_" 
                + PortraitData[0] + "_Split_" + PortraitData[1] + "_Split_" + PortraitData[2] 
                + "_Split_" + MediaData[0]
                + "_Split_" + MediaData[1]
                + "_Split_" + MediaData[2]
                + "_Split_" + MediaData[3]
                + "_Split_" + MediaData[4]
                + "_Split_" + ChoiseStates[0]
                + "_Split_" + ChoiseStates[1]
                + "_Split_" + ChoiseStates[2]
                + "_Split_" + ChoisesJump[0]
                + "_Split_" + ChoisesJump[1]
                + "_Split_" + ChoisesJump[2]
                + "_Split_" + BackgroundImage;
            System.IO.File.WriteAllText(@"Save" + ItemIndex + ".txt", WriteToFile);
        }

        private void LoadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem MI = sender as MenuItem;
            int ItemIndex = 0;
            if (MI != null)
                ItemIndex = Convert.ToInt32(MI.Tag.ToString());
            string ConversationData;
            int LineData;
            string[] PortraitData = new string[3];
            string[] MediaData = new string[5];
            string[] ChoiseStates = new string[3];
            string[] ChoisesJump = new string[3];
            string BackgroundImage;
            string text = System.IO.File.ReadAllText(@"Save" + ItemIndex + ".txt");
            string[] RawData = text.Split(new string[] { "_Split_" }, StringSplitOptions.None);
            ConversationData = RawData[0];
            LineData = Convert.ToInt32(RawData[1]);
            PortraitData[0] = RawData[2];
            PortraitData[1] = RawData[3];
            PortraitData[2] = RawData[4];
            MediaData[0] = RawData[5];
            MediaData[1] = RawData[6];
            MediaData[2] = RawData[7];
            MediaData[3] = RawData[8];
            MediaData[4] = RawData[9];
            ChoiseStates[0] = RawData[10];
            ChoiseStates[1] = RawData[11];
            ChoiseStates[2] = RawData[12];
            ChoisesJump[0] = RawData[13];
            ChoisesJump[1] = RawData[14];
            ChoisesJump[2] = RawData[15];
            BackgroundImage = RawData[16];

            ConversationBox.Text = ConversationData;
            storyControls.row = LineData;
            Portrait1.Source = new BitmapImage(new Uri(PortraitData[0], UriKind.Absolute));
            Portrait2.Source = new BitmapImage(new Uri(PortraitData[1], UriKind.Absolute));
            Portrait3.Source = new BitmapImage(new Uri(PortraitData[2], UriKind.Absolute));
            BGM.Source = BGM.Source = new Uri(MediaData[0], UriKind.Relative);

            if(MediaData[4] == "true")
            {
                BGMStatus = BGMControl("Play");
            }
            else {
                BGMStatus = BGMControl("Stop");
            }

            int Count = 0;
            for (int i = 0; i < 3; i++) {
                Count += Convert.ToInt32(Convert.ToBoolean(ChoiseStates[i]));
            }
            ChoiceVisibilityControl(true, Count);
            ChoicesCode = ChoisesJump;
            BackgroundImageSource = BackgroundImage;
            string FileURL = BackgroundImageSource;
            try {
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), FileURL)));
            }
            catch
            {
                FileURL = "/Resources/BathRoomDark.jpg";
            }
        }

        private bool BGMControl(string PlayOrStop)
        {
            if (PlayOrStop == "Play" || PlayOrStop == "play")
            {
                BGM.Play();
                return true;
            }
            else
            {
                BGM.Stop();
                return false;
            }
        }
    }
}
