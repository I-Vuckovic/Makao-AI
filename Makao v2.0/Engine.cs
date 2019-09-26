using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIG.AV.Karte;
using System.Threading;
using System.IO;

namespace Makao_v2._0
{
    class Engine
    {
        Spil spil;
        List<Karta> talon;
        Boja trenutnaBoja;

        public Spil Spil { get => spil; set => spil = value; }

        public Engine()
        {
            talon = new List<Karta>();
            Spil = new Spil(true);
            talon.Add(Spil.Karte.Last());
            trenutnaBoja = spil.Karte.Last().Boja;
            Spil.Karte.Remove(Spil.Karte.Last());
        }

        public void SetCards(Igra igrac1, Igra igrac2)
        {
            List<Karta> lista1 = new List<Karta>();
            List<Karta> lista2 = new List<Karta>();

            for (int i = 0; i < 6; i++)
            {
                lista1.Add(Spil.Karte.Last());
                Spil.Karte.Remove(Spil.Karte.Last());
                lista2.Add(Spil.Karte.Last());
                Spil.Karte.Remove(Spil.Karte.Last());
            }

            igrac1.SetRuka(lista1);
            igrac2.SetRuka(lista2);
            igrac1.Bacenekarte(talon, talon.Last().Boja, 6);
            igrac2.Bacenekarte(talon, talon.Last().Boja, 6);
        }

        public void Potez(StreamWriter sw, int k, Igra igrac, Igra drugi)
        {
            sw.WriteLine("Na talonu je: " + talon.Last().Broj + " " + trenutnaBoja.ToString());
            sw.Write("Igrac" + k.ToString() + " u ruci drzi: ");
            for (int i = 0; i < igrac.ruka.Count; i++)
            {
                sw.Write(igrac.ruka[i].Broj + " ");
                sw.Write(igrac.ruka[i].Boja.ToString() + "  ");

            }
            sw.Write(sw.NewLine);

            igrac.BeginBestMove();
            Thread.Sleep(1000);
            igrac.EndBestMove();

            if (igrac.BestMove.Tip == TipPoteza.KupiKartu)
            {
                List<Karta> zaKupovinu = new List<Karta>();
                zaKupovinu.Add(Spil.Karte.Last());
                Spil.Karte.Remove(Spil.Karte.Last());
                sw.WriteLine("Igrac" + k.ToString() + "je odlucio da kupi kartu");
                igrac.KupioKarte(zaKupovinu);

                Potez(sw, k, igrac, drugi);
            }
            else if (igrac.BestMove.Tip == TipPoteza.KrajPoteza)
            {
                sw.WriteLine("Igrac" + k.ToString() + " je odlucio da zavrsi potez");
                drugi.Bacenekarte(new List<Karta>(), trenutnaBoja, igrac.ruka.Count);
            }
            else if (igrac.BestMove.Tip == TipPoteza.KupiKazneneKarte)
            {
                if (talon.Last().Broj == "2")
                {
                    List<Karta> zaKupovinu = new List<Karta>();
                    for (int i = 0; i < 4; i++)
                    {
                        if (spil.Karte.Count > 0)
                        {
                            zaKupovinu.Add(Spil.Karte.Last());
                            Spil.Karte.Remove(Spil.Karte.Last());
                        }
                    }
                    sw.WriteLine("Igrac" + k.ToString() + "je odlucio da kupi kaznene karte");
                    igrac.KupioKarte(zaKupovinu);
                }
                if (talon.Last().Broj == "7")
                {
                    List<Karta> zaKupovinu = new List<Karta>();
                    for (int i = 0; i < 2; i++)
                    {
                        if (spil.Karte.Count > 0)
                        {
                            zaKupovinu.Add(Spil.Karte.Last());
                            Spil.Karte.Remove(Spil.Karte.Last());
                        }
                    }
                    sw.WriteLine("Igrac" + k.ToString() + " je odlucio da kupi kaznene karte");
                    igrac.KupioKarte(zaKupovinu);
                }

                Potez(sw, k, igrac, drugi);

            }
            else
            {
                sw.Write("Igrac" + k.ToString() + " je odlucio da baci: ");
                talon.AddRange(igrac.BestMove.Karte);
                for (int i = 0; i < igrac.BestMove.Karte.Count; i++)
                {
                    sw.Write(igrac.BestMove.Karte[i].Broj + " ");
                    sw.Write(igrac.BestMove.Karte[i].Boja.ToString() + "  ");
                    
                    sw.Write("\n");
                }
                if (igrac.BestMove.Karte.Last().Broj == "J")
                {
                    sw.Write("Igrac menja boju u: " + igrac.BestMove.NovaBoja.ToString());
                    drugi.Bacenekarte(igrac.BestMove.Karte, igrac.BestMove.NovaBoja, igrac.ruka.Count);
                    trenutnaBoja = igrac.BestMove.NovaBoja;
                }
                else
                {
                    drugi.Bacenekarte(igrac.BestMove.Karte, igrac.BestMove.Karte.Last().Boja, igrac.ruka.Count);
                    trenutnaBoja = igrac.BestMove.Karte.Last().Boja;
                }
            }

            sw.Write(sw.NewLine);

        }

    }
}
