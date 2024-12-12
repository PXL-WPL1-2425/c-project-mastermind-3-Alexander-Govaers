﻿using Microsoft.VisualBasic;
using System;
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
using System.Windows.Threading;

namespace WpfApp1_mastermind3_final_edition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //random nummer genereren
        Random rnd = new Random();

        //kleuren doorheen de code gebruiken
        string[] kleuren = new string[4];

        int attempts;
        //start van het spel 100 punten
        int points = 100;

        int row = 0;

        // timer instellen
        DispatcherTimer timer = new DispatcherTimer();
        DateTime clicked;
        TimeSpan elapsedTime;
        bool timerStarted = false;
        bool hasWonGame = false;
        bool endGame = false;

        int maxAttempts = 0;

        string[] highscores = new string[15];
        int spelerIndex;
        string inputNaam;
        string highscoreTekst;

        List<string> spelerNamen = new List<string>();
        bool extraSpeler = true;

        public MainWindow()
        {
            InitializeComponent();



        }

        /// <summary>
        /// Zorgt voor een tick-event van de timer, bijgehouden tijd en pogingen worden geüpdatet.
        /// </summary>
        /// <param name="sender">De timer die het Tick-event heeft getriggerd.</param>
        /// <param name="e">De starten van het event</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - clicked;
            timerTextBlock.Text = elapsedTime.TotalSeconds.ToString("N3");




            if (attempts < maxAttempts)
            {

                if (hasWonGame)
                {
                    Stopcountdown();
                }
                else if (elapsedTime.TotalSeconds >= 10)
                {

                    Stopcountdown();
                    checkButton_Click(null, null);
                    clicked = DateTime.Now;
                    Startcountdown();
                }

            }
            else if (attempts == maxAttempts)
            {
                Stopcountdown();
            }



            this.Title = $"Mastermind: {attempts} / {maxAttempts} pogingen ondernomen";

        }
        /// <summary>
        /// Wanneer de window ingeladen word, worden de willekeurige nummers aangemaakt en de score aangemaakt.
        /// </summary>
        /// <param name="sender">Het venster dat wordt ingeladen</param>
        /// <param name="e">Het laden van u venster</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            spelerIndex = 0;
            Startgame();
            maxAttempts = Pogingen();
            GenerateRandomColors();

            pointslabel.Content = $"Jouw huidige score: {points}/ 100";
            solutionTextBox.Visibility = Visibility.Hidden;


        }
        /// <summary>
        /// Deze method maakt willerkeurige kleuren
        /// </summary>
        private void GenerateRandomColors()
        {

            kleuren[0] = ChooseColor(rnd.Next(0, 6));
            kleuren[1] = ChooseColor(rnd.Next(0, 6));
            kleuren[2] = ChooseColor(rnd.Next(0, 6));
            kleuren[3] = ChooseColor(rnd.Next(0, 6));

            solutionTextBox.Text = $"MasterMind: {kleuren[0]}, {kleuren[1]}, {kleuren[2]}, {kleuren[3]}";
        }
        /// <summary>
        /// Via deze method wordt er een stringnaam geven aan de indexnummer.
        /// </summary>
        /// <param name="willekeurigNummer">Het willekeurig nummer dat gegenereerd word tussen 0-5</param>
        /// <returns>Een string die een kleurnaam geeft aan het willekeurig nummer</returns>
        private string ChooseColor(int willekeurigNummer)
        {
            if (willekeurigNummer == 0)
            {
                return "Rood";
            }
            else if (willekeurigNummer == 1)
            {
                return "Geel";
            }
            else if (willekeurigNummer == 2)
            {
                return "Oranje";
            }
            else if (willekeurigNummer == 3)
            {
                return "Wit";
            }
            else if (willekeurigNummer == 4)
            {
                return "Groen";
            }
            else if (willekeurigNummer == 5)
            {
                return "Blauw";
            }
            else
            {
                return "ERROR";
            }
        }
        /// <summary>
        /// Verandert de achtergrondkleur van een label op basis van de selectie in een ComboBox. 
        /// </summary>
        /// <param name="sender"> De combobox waaraan een aanpassing gedaan wordt</param>
        /// <param name="e">De selectie van de keuze van de combobox</param>
        private void colorChange(object sender, SelectionChangedEventArgs e)
        {

            ComboBox changedcombobox = sender as ComboBox;

            if (changedcombobox == comboBox1)
            {
                label1.Background = GetColorFromIndex(changedcombobox.SelectedIndex);
            }
            else if (changedcombobox == comboBox2)
            {
                label2.Background = GetColorFromIndex(changedcombobox.SelectedIndex);
            }
            else if (changedcombobox == comboBox3)
            {
                label3.Background = GetColorFromIndex(changedcombobox.SelectedIndex);
            }
            else if (changedcombobox == comboBox4)
            {
                label4.Background = GetColorFromIndex(changedcombobox.SelectedIndex);
            }

        }
        /// <summary>
        /// De method linkt een indexnummer aan een kleur.
        /// </summary>
        /// <param name="selectedIndex">Het indexnummer van het geselecteerde item van de combobox</param>
        /// <returns>Een kleur</returns>
        private Brush GetColorFromIndex(int selectedIndex)
        {

            switch (selectedIndex)
            {
                case 0:

                    return Brushes.Red;

                case 1:

                    return Brushes.White;

                case 2:
                    return Brushes.Yellow;

                case 3:
                    return Brushes.Orange;

                case 4:
                    return Brushes.Green;

                case 5:
                    return Brushes.Blue;

                default:
                    return Brushes.Transparent;


            }
        }


        /// <summary>
        /// Keydown event voor de oplossing te tonen (f12)
        /// </summary>
        /// <param name="sender">de f12 toest</param>
        /// <param name="e">tonen van de oplossing</param>
        private void Toggledebug(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12 && solutionTextBox.Visibility == Visibility.Visible)
            {
                solutionTextBox.Visibility = Visibility.Hidden;
            }
            else if (e.Key == Key.F12)
            {
                solutionTextBox.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// De labels gecontroleerd, de timer opgestart, de pogingen verhoogd en een historiek bijgehouden.
        /// <para>In de if statement wordt er gekeken als de pogingen niet overschreden worden en bij elke klik verhoogt. Indien de speler verliest wordt er een messagebox getoont</para>
        /// </summary>
        /// <param name="sender">De button knop</param>
        /// <param name="e">het klik event van de knop</param>
        private void checkButton_Click(object sender, RoutedEventArgs e)
        {

            if (attempts != maxAttempts && !hasWonGame)
            {

                LabelChanged(label1, 0, comboBox1);
                LabelChanged(label2, 1, comboBox2);
                LabelChanged(label3, 2, comboBox3);
                LabelChanged(label4, 3, comboBox4);


                attempts++;
                Startcountdown();
                UpdateTitle();
                Historiek();
                pointslabel.Content = $"Jouw huidige score: {points}/100";
                HasWon();

                if (attempts >= maxAttempts && !hasWonGame)
                {
                    highscores[spelerIndex] = $"{inputNaam} - {attempts}/{maxAttempts} pogingen - {points}/100 punten";
                    Stopcountdown();
                    MessageBoxResult result = MessageBox.Show($"You Failed!" +
                     $" De juiste kleurencombinatie: {kleuren[0]}, {kleuren[1]}, {kleuren[2]}, {kleuren[3]}  \r\n ", "Failed",
                     MessageBoxButton.OK, MessageBoxImage.Information);

                    spelerIndex++;

                }

            }

        }
        /// <summary>
        /// In de titel worden de pogingen getoont
        /// </summary>
        private void UpdateTitle()
        {

            if (attempts >= maxAttempts)
            {
                attempts = maxAttempts;
                timerTextBlock.Text = "";
            }
            this.Title = $"Mastermind: {attempts} / {maxAttempts} pogingen ondernomen";

        }

        /// <summary>
        /// Controleert de spelers invoer en past de kleur van het label aan op en vergelijkt deze met de oplossing.
        /// <para>Er wordt gecontroleerd en daarbij wordt de kleurrand van het label aangepast</para>
        /// </summary>
        /// <param name="kleurLabel">Het label dat wordt aangepast.</param>
        /// <param name="positie">De positie van de oplossing in de kleurenarray.</param>
        /// <param name="input">De keuze van de speler.</param>
        private void LabelChanged(Label kleurLabel, int positie, ComboBox input)

        {
            string oplossing;


            switch (positie)
            {
                case 0:
                    oplossing = kleuren[0];
                    break;

                case 1:
                    oplossing = kleuren[1];
                    break;

                case 2:
                    oplossing = kleuren[2];
                    break;
                case 3:
                    oplossing = kleuren[3];
                    break;
                default:
                    oplossing = "error";
                    break;

            }

            if (input.Text == oplossing)
            {
                kleurLabel.BorderBrush = Brushes.DarkRed;
                kleurLabel.BorderThickness = new Thickness(4);

            }
            else if (solutionTextBox.Text.Contains(input.Text) && input.Text != "")
            {
                kleurLabel.BorderBrush = Brushes.Wheat;
                points -= 1;
                kleurLabel.BorderThickness = new Thickness(4);
            }
            else
            {
                kleurLabel.BorderThickness = new Thickness(0);
                points -= 2;
            }

        }
        /// <summary>
        /// Voegt een nieuwe rij toe aan het historiekgrid met labels en de randen worden aangepast volgens de keuze van de speler .
        /// </summary>
        private void Historiek()
        {
            RowDefinition newRow = new RowDefinition();
            newRow.Height = GridLength.Auto;
            historiekgrid.RowDefinitions.Add(newRow);

            Label historiekLabel1 = new Label();
            historiekLabel1.Height = 22;
            historiekLabel1.Width = 75;
            historiekLabel1.Margin = new Thickness(2);
            historiekLabel1.Background = label1.Background;
            historiekLabel1.BorderBrush = label1.BorderBrush;
            historiekLabel1.BorderThickness = label1.BorderThickness;

            Grid.SetRow(historiekLabel1, row);
            Grid.SetColumn(historiekLabel1, 0);

            Label historiekLabel2 = new Label();
            historiekLabel2.Height = 22;
            historiekLabel2.Width = 75;
            historiekLabel2.Margin = new Thickness(2);
            historiekLabel2.Background = label2.Background;
            historiekLabel2.BorderBrush = label2.BorderBrush;
            historiekLabel2.BorderThickness = label2.BorderThickness;
            Grid.SetRow(historiekLabel2, row);
            Grid.SetColumn(historiekLabel2, 1);

            Label historiekLabel3 = new Label();
            historiekLabel3.Height = 22;
            historiekLabel3.Width = 75;
            historiekLabel2.Margin = new Thickness(2);
            historiekLabel3.Background = label3.Background;
            historiekLabel3.BorderBrush = label3.BorderBrush;
            historiekLabel3.BorderThickness = label3.BorderThickness;
            Grid.SetRow(historiekLabel3, row);
            Grid.SetColumn(historiekLabel3, 2);

            Label historiekLabel4 = new Label();
            historiekLabel4.Height = 22;
            historiekLabel4.Width = 75;
            historiekLabel2.Margin = new Thickness(2);
            historiekLabel4.Background = label4.Background;
            historiekLabel4.BorderBrush = label4.BorderBrush;
            historiekLabel4.BorderThickness = label4.BorderThickness;
            Grid.SetRow(historiekLabel4, row);
            Grid.SetColumn(historiekLabel4, 3);

            historiekgrid.Children.Add(historiekLabel1);
            historiekgrid.Children.Add(historiekLabel2);
            historiekgrid.Children.Add(historiekLabel3);
            historiekgrid.Children.Add(historiekLabel4);

            row++;

        }

        /// <summary>
        /// Controleert of de speler heeft gewonnen. Indien gewonnen, biedt een keuze aan om opnieuw te spelen of te stoppen.
        /// </summary>
        private void HasWon()
        {


            if (label1.BorderBrush == Brushes.DarkRed && label2.BorderBrush == Brushes.DarkRed && label3.BorderBrush == Brushes.DarkRed && label4.BorderBrush == Brushes.DarkRed)
            {
                highscores[spelerIndex] = $"{inputNaam} - {attempts}/{maxAttempts} pogingen - {points}/100 punten";
                spelerIndex++;
                hasWonGame = true;
                Stopcountdown();
                MessageBoxResult result = MessageBox.Show($"Je hebt gewonnen in {attempts} beurten!",
                    "WINNER!", MessageBoxButton.OK, MessageBoxImage.Information);

            }

        }
        /// <summary>
        /// Voert het event uit bij het sluiten van het venster en stelt een vraag aan de gebruiker.
        /// </summary>
        /// <param name="sender">Het sluiten van het spel.</param>
        /// <param name="e">Het afsluit-event.</param>
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();

            if (endGame == true)
            {
                e.Cancel = false;
                return;
            }

            MessageBoxResult reply = MessageBox.Show("Ben je zeker dat je wilt afsluiten?", $"Mastermind: {attempts}/{maxAttempts} pogingen ondernomen", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (reply == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                Startcountdown();

            }
        }

        /// <summary>
        /// De timer opstarten, stoppen en herstarten.
        /// </summary>
        private void Startcountdown()
        {
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;

            if (timerStarted == false)
            {
                clicked = DateTime.Now;
                timer.Start();
                timerStarted = true;

            }
            else
            {
                Stopcountdown();
                clicked = DateTime.Now;
                timer.Start();

            }
        }

        /// <summary>
        /// Timer stopt 
        /// </summary>
        private void Stopcountdown()
        {
            timer.Stop();
        }

        /// <summary>
        /// Reset het spel naar het begin.
        /// </summary>
        private void ResetGame()
        {
            maxAttempts = Pogingen();
            hasWonGame = false;
            points = 100;
            attempts = 0;
            UpdateTitle();
            GenerateRandomColors();
            timerTextBlock.Text = "";

            comboBox1.Text = "";
            label1.BorderThickness = new Thickness(0);
            label1.BorderBrush = null;
            comboBox2.Text = "";
            label2.BorderThickness = new Thickness(0);
            label2.BorderBrush = null;
            comboBox3.Text = "";
            label3.BorderThickness = new Thickness(0);
            label3.BorderBrush = null;
            comboBox4.Text = "";
            label4.BorderThickness = new Thickness(0);
            label4.BorderBrush = null;

            historiekgrid.Children.Clear();
            pointslabel.Content = $"Jouw huidige score: {points}/100";
        }
        /// <summary>
        /// Inputbox wordt gegenereerd, deze blijft genereren totdat de input correct is ingevuld
        /// </summary>
        /// <returns>Naam speler</returns>
        private string Startgame()
        {
            while (extraSpeler)
            {
                inputNaam = Interaction.InputBox("Voer een correcte naam in:", "Mastermind");
                bool validInput = int.TryParse(inputNaam, out int isGetal);

                while (string.IsNullOrEmpty(inputNaam) || validInput)
                {
                    MessageBox.Show("Geef een correcte naam!", "Foutieve invoer");
                    inputNaam = Interaction.InputBox("Geef een naam", "Invoer");
                    validInput = int.TryParse(inputNaam, out isGetal);
                    MessageBox.Show("Nog een naam invullen?", "Extra speler", MessageBoxButton.YesNo, MessageBoxImage.Question);
                }

                spelerNamen.Add(inputNaam);
                MessageBoxResult result = MessageBox.Show("Nog een naam invullen?", "Extra speler", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {

                    extraSpeler = false;

                }
               
            }
            return inputNaam;
        }

                private void Menurestart_Click(object sender, RoutedEventArgs e)
                {
                ResetGame();
                Startgame();



                }

            private void Menu_close_Click(object sender, RoutedEventArgs e)
            {
                this.Close();
            }

            private void Menu_pogingen_Click(object sender, RoutedEventArgs e)
            {
                Stopcountdown();


            }
            private int Pogingen()
            {

                string inputAttempts = Interaction.InputBox("Hoeveel pogingen wens je te spelen?" + "Kies een getal tussen 3 en 20", "Mastermind");
                bool validNumber = int.TryParse(inputAttempts, out int inputGetal);


                while (string.IsNullOrEmpty(inputAttempts) || !validNumber || inputGetal < 3 || inputGetal > 20)
                {
                    MessageBox.Show("Geef een correct aantal tussen 3 - 20!", "Foutieve invoer");
                    inputAttempts = Interaction.InputBox("Geef een correct getal", "Invoer");

                    validNumber = int.TryParse(inputAttempts, out inputGetal);

                }

                return inputGetal;
            }

            private void menu_Highscores_click(object sender, RoutedEventArgs e)
            {
                highscoreTekst = "";
                for (int i = 0; i < spelerIndex; i++)
                {
                    highscoreTekst += $"{highscores[i]}";
                }

                MessageBox.Show($"Scoreboard: \r\n {highscoreTekst}");


            }
        }
    }
