using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound; 

        Random random = new Random();
        public MainWindow()
        {
            
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetupGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if(matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Jogar novamente? ";
            }
        }

        private void SetupGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🐎","🐎",
                "🐘","🐘",
                "🐻","🐻",
                "🐇","🐇",
                "🦁","🦁",
                "🐮","🐮",
                "🐷","🐷",
                "🐶","🐶",
            };

            foreach(TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0; 
        }

        TextBlock lastTextBlockCliked;
        bool findingMatch = false; 

        /*metodo executado quando o user clica em um textblock "carta"*/
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock; 

            if(findingMatch == false) /*Primeiro clique: guarda a carta p comparação */ 
            {
                textBlock.Visibility = Visibility.Hidden; /*esconde a carta clicada*/
                lastTextBlockCliked = textBlock; /*guarda a carta clicada p comparar depois*/
                findingMatch = true; /* indica que agr estamos esperando a 2ª carta */
            }
            else if(textBlock.Text == lastTextBlockCliked.Text) /*cartas são iguais:par encontrado*/
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden; /*esconde a 2ª carta */
                findingMatch = false; /*reinicia a busca por um par*/
            }
            else /* cartas diferentes: mostra a 1ª carta novamete */
            {
                lastTextBlockCliked.Visibility = Visibility.Visible; /*mostra novamente a 1ª carta clicada*/
                findingMatch = false; /*reinicia a busca por um par*/
            }

        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(matchesFound == 8) /*redefine o jogo caso os 8 pares sejam encontrados*/
            {
                SetupGame();
            }

        }
    }
}