using System;
using System.Windows;

namespace Delegates
{
    /// <summary>
    ///  //Více na: https://www.zive.cz/clanky/poznavame-c-a-microsoftnet-15-dil--delegaty/sc-3-a-123479/default.aspx
    /// </summary>
    public partial class MainWindow : Window
    {
        //pokud vytvarime delegata vytvarime predpis pro signaturu  metody
        //Specifikátor_přístupu delegate návratový_typ identifikátor(seznam_formálních_parametrů) 
        public delegate void MatematickaOperaceDelegate (int x, int y);


       

        // delegat je reference na metodu
        public MainWindow()
        {
            InitializeComponent();

            
        }

        public class  MathOperations
        {
            // metody jsou v podstate stejne a vyhovuji predpisu pro delegata MatematickeOperaceDelegate
            public static double Soucet(int a, int b)
            {
                return a + b;
            }

            public static double Rozdil(int a, int b)
            {
                return a - b;
            }


           
        }

        public class Calculator
        {
            MatematickaOperaceDelegate objektDelegata = new MatematickaOperaceDelegate(MathOperations.Soucet);

            public void ProvedOperaci(int a, int b, MatematickaOperaceDelegate operace)
            {
                Console.WriteLine(operace(a,b);
            }
        }


    }
}