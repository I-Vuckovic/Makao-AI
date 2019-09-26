using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIG.AV.Karte;

namespace Makao_v2._0
{
    class Move : IMove
    {
        /* 
         Ideja za jacinu poteza: Kada alfa beta dodje do kraja (terminalnog) jacina mu se dodaje za nivo na kom se nalazi puta/plus nesto, u sustini da se uzima u obzir dubina na kojoj se desilo
         manja dubina znaci brzi zavrsetak znaci bolji rezultat
             
             */

        protected TipPoteza tip;
        protected List<Karta> karte;
        protected Boja novaBoja;

        public TipPoteza Tip { get => tip; set => tip = value; }
        public List<Karta> Karte { get => karte; set => karte = value; }
        public Boja NovaBoja { get => novaBoja; set => novaBoja = value; }


        //stvari za stablo
        private List<Karta> ruka = new List<Karta>();
        private Move parent;
        private List<Move> children;
        private int jacina;

        public Move Parent { get => parent; set => parent = value; }
        public List<Move> Children { get => children; set => children = value; }
        public List<Karta> Ruka { get => ruka; set => ruka = value; }

        public int Jacina
        {
            get
            {
                return jacina;
            }
            set
            {
                jacina = value;
            }
        }

        public void PrintPretty(string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }
            foreach (Karta k in karte)
            {
                Console.Write(k.Broj+" "+k.Boja.ToString()+" - ");
            }
            Console.WriteLine();

            for (int i = 0; i < Children.Count; i++)
                Children[i].PrintPretty(indent, i == Children.Count - 1);
        }

        public Move()
        {
            karte = new List<Karta>();
            children = new List<Move>();
        }

        public Move(Karta k)
        {
            karte = new List<Karta>();
            children = new List<Move>();
            karte.Add(k);
        }

        //Mozda ne treba
        public Move(TipPoteza tip)
        {
            children = new List<Move>();
            karte = new List<Karta>();
            this.tip = tip;
        }

        public void OdrediJacinu(bool isPlayer, int depth)
        {
            if (parent.parent != null)
                jacina = parent.Jacina;

            int trenutna = 0;
            foreach (Karta k in karte)
            {
                trenutna += JacinaKarte(k);
            }

            //jacina = jacina * depth; // Ova matematika mozda treba da se doradi ali sustina je da vecu vrednost imaju cvorovi pri vrhu
            // Tako da ako se "makao" pronadje na dubini primera radi 5 vise ce da se vrednuje neko makao pronadjen na dubini 10

            if (karte.Count > 0 && karte.Last().Broj == "A")
                trenutna -= 10;

            if (tip == TipPoteza.KrajPotezaMakao)
                trenutna += 2000;

            /* switch (tip)
            {
                case (TipPoteza.KupiKazneneKarte):
                    trenutna -= 40;
                    break;
                case (TipPoteza.KupiKartu):
                    trenutna -= 20;
                    break;
                case (TipPoteza.KrajPotezaMakao):
                    trenutna += 2000;
                    break;
                default:
                    break;
            } */

            trenutna *= depth;

            if (isPlayer)
                jacina = jacina + trenutna;
            else
                jacina = jacina - trenutna;


            //Dok je stajalo ovo previse su veliku individualnu jacinu imali cvorovi u dubini sto je zapravo pokusano da se izbegne u pocetku
            //jacina *= depth; 

            //Jacine igracevih poteza se sabiraju a oduzimaju od protivnikovih kako bi se dole video end result "bitke" u toj grani

        }

        int JacinaKarte(Karta k)
        {
            //if (k.Broj == "2" && k.Boja == Boja.Tref)//Mozda ne treba ovaj if jer postoji u alfa beti uslov if tip=kupikaznene
            //{
            //    return 30;
            //}   
                switch (k.Broj)
                {
                    case ("A"):
                        return 12;
                    case "Q":
                        return 10;
                    case "K":
                        return 10;
                    case "8":
                        return 12;
                    case "J":
                        return -10;
                    default:
                        return int.Parse(k.Broj);
                }
            
        }
    }
}