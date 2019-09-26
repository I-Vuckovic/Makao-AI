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
    class Program
    {
        static void Main(string[] args)
        {
            List<Karta> proba = new List<Karta>();
            //proba.Add(null);
            Console.WriteLine(proba.Count);

            #region Prvi test

            //Random rnd = new Random();
            //Spil spil = new Spil();
            //Igra igra1 = new Igra();
            //List<Karta> talon = new List<Karta>();
            //for (int i = 0; i < 20; i++)
            //{
            //    talon.Add(spil.Karte[rnd.Next(spil.Karte.Count)]);
            //    spil.Karte.RemoveAll(x => talon.Exists(y => y.Broj == x.Broj && y.Boja == x.Boja));
            //}
            //igra1.Bacenekarte(talon, talon.Last().Boja, 5);
            //List<Karta> ruka = new List<Karta>();

            //ruka.Add(new Karta { Boja = Boja.Tref, Broj = "5" });
            //ruka.Add(new Karta { Boja = Boja.Tref, Broj = "6" });
            //ruka.Add(new Karta { Boja = Boja.Tref, Broj = "7" });
            //ruka.Add(new Karta { Boja = Boja.Tref, Broj = "8" });
            //ruka.Add(new Karta { Boja = Boja.Tref, Broj = "9" });

            //igra1.SetRuka(ruka);

            //igra1.BeginBestMove();

            #endregion

            #region Drugi test: Specijalni slucajevi 

            //Random rnd = new Random();
            //Igra igrac1 = new Igra();
            //Igra igrac2 = new Igra();

            //Spil spil = new Spil();

            //List<Karta> lista = new List<Karta>();

            ////for (int i = 0; i < 6; i++)
            ////{
            ////    lista.Add(spil.Karte[rnd.Next(spil.Karte.Count)]);
            ////    spil.Karte.RemoveAll(x => lista.Exists(y => y.Broj == x.Broj && y.Boja == x.Boja));
            ////}

            //lista.Add(new Karta() { Boja = Boja.Herz, Broj = "5" });
            //lista.Add(new Karta() { Boja = Boja.Karo, Broj = "J" });
            //lista.Add(new Karta() { Boja = Boja.Karo, Broj = "4" });
            ////lista.Add(new Karta() { Boja = Boja.Karo, Broj = "Q" });
            ////lista.Add(new Karta() { Boja = Boja.Tref, Broj = "2" });
            ////lista.Add(new Karta() { Boja = Boja.Tref, Broj = "8" });

            //igrac1.SetRuka(lista);


            //Console.WriteLine("Igrac u ruci drzi: ");
            //foreach (Karta k in igrac1.ruka)
            //{
            //    Console.WriteLine(k.Boja.ToString() + " " + k.Broj);
            //}

            //lista.Clear();
            //lista.Add(new Karta() { Boja = Boja.Herz, Broj = "3" });
            ////lista.Add(spil.Karte[rnd.Next(spil.Karte.Count)]);

            //Console.WriteLine();
            //Console.WriteLine("Na talonu je " + lista.Last().Boja.ToString() + " " + lista.Last().Broj);

            //igrac1.Bacenekarte(lista, lista.Last().Boja, 6);

            ////lista.Clear();
            ////lista.Add(new Karta() { Boja = Boja.Herz, Broj = "Q" });
            ////lista.Add(new Karta() { Boja = Boja.Herz, Broj = "6" });
            ////lista.Add(new Karta() { Boja = Boja.Herz, Broj = "2" });
            ////lista.Add(new Karta() { Boja = Boja.Pik, Broj = "K" });
            ////lista.Add(new Karta() { Boja = Boja.Pik, Broj = "10" });
            ////lista.Add(new Karta() { Boja = Boja.Pik, Broj = "7" });

            ////igrac1.spil.Karte.Clear();
            ////igrac1.spil.Karte.AddRange(lista);



            //lista.Clear();
            //igrac1.BeginBestMove();

            //Thread.Sleep(1000);

            //igrac1.EndBestMove();

            //Console.Write("Igrac je odlucio da odigra: ");
            //foreach (Karta k in igrac1.BestMove.Karte)
            //{
            //    Console.Write(k.Boja.ToString() + " " + k.Broj + " ");
            //}

            //Console.WriteLine(igrac1.BestMove.Tip.ToString());

            //Console.WriteLine("Igrac u ruci drzi: ");
            //foreach (Karta k in igrac1.ruka)
            //{
            //    Console.WriteLine(k.Boja.ToString() + " " + k.Broj);
            //}

            //igrac1.BeginBestMove();

            //Thread.Sleep(1000);

            //igrac1.EndBestMove();

            //Console.Write("Igrac je odlucio da odigra: ");
            //foreach (Karta k in igrac1.BestMove.Karte)
            //{
            //    Console.Write(k.Boja.ToString() + " " + k.Broj + " ");
            //}

            //Console.WriteLine(igrac1.BestMove.Tip.ToString());

            //igrac1.BeginBestMove();

            //Thread.Sleep(1000);

            //igrac1.EndBestMove();

            //Console.Write("Igrac je odlucio da odigra: ");
            //foreach (Karta k in igrac1.BestMove.Karte)
            //{
            //    Console.Write(k.Boja.ToString() + " " + k.Broj + " ");
            //}

            //Console.WriteLine(igrac1.BestMove.Tip.ToString());



            #endregion

            #region Treci test

            //Spil spil = new Spil();
            //spil.Promesaj();
            //Igra igrac1 = new Igra();
            //Igra igrac2 = new Igra();

            //List<Karta> ruka = new List<Karta>();
            //for (int i = 0; i < 6; i++)
            //{
            //    ruka.Add(spil.Karte.Last());
            //    spil.Karte.Remove(spil.Karte.Last());
            //}

            //igrac1.SetRuka(ruka);

            //Console.WriteLine("Igrac 1 u ruci drzi: ");
            //foreach (Karta k in igrac1.ruka)
            //{
            //    Console.WriteLine(k.Boja.ToString() + " " + k.Broj);
            //}

            //ruka.Clear();

            //for (int i = 0; i < 6; i++)
            //{
            //    ruka.Add(spil.Karte.Last());
            //    spil.Karte.Remove(spil.Karte.Last());
            //}

            //igrac2.SetRuka(ruka);

            //Console.WriteLine("Igrac 2 u ruci drzi: ");
            //foreach (Karta k in igrac2.ruka)
            //{
            //    Console.WriteLine(k.Boja.ToString() + " " + k.Broj);
            //}

            //List<Karta> bacene = new List<Karta>();
            //bacene.Add(spil.Karte.Last());
            //spil.Karte.Remove(spil.Karte.Last());

            //Console.WriteLine();
            //Console.WriteLine("Na talonu je " + bacene.Last().Boja.ToString() + " " + bacene.Last().Broj);

            //igrac1.Bacenekarte(bacene, bacene.Last().Boja, igrac2.ruka.Count);

            //int j = 0;
            //while (j < 10)
            //{


            //    igrac1.BeginBestMove();
            //    Thread.Sleep(1000);
            //    igrac1.EndBestMove();

            //    Console.Write("Igrac 1 je odlucio da odigra: ");
            //    foreach (Karta k in igrac1.BestMove.Karte)
            //    {
            //        Console.Write(k.Boja.ToString() + " " + k.Broj + " ");
            //    }

            //    Console.WriteLine();

            //    bacene.AddRange(igrac1.BestMove.Karte);

            //    if (igrac1.BestMove.Karte.Last().Broj != "J")
            //        igrac2.Bacenekarte(bacene, bacene.Last().Boja, igrac1.ruka.Count);
            //    else
            //        igrac2.Bacenekarte(bacene, igrac1.BestMove.NovaBoja, igrac1.ruka.Count);

            //    igrac2.BeginBestMove();
            //    Thread.Sleep(1000);
            //    igrac2.EndBestMove();

            //    Console.Write("Igrac 2 je odlucio da odigra: ");
            //    foreach (Karta k in igrac2.BestMove.Karte)
            //    {
            //        Console.Write(k.Boja.ToString() + " " + k.Broj + " ");
            //    }

            //    Console.WriteLine();


            //    bacene.AddRange(igrac2.BestMove.Karte);

            //    if (igrac2.BestMove.Karte.Last().Broj != "J")
            //        igrac1.Bacenekarte(bacene, bacene.Last().Boja, igrac2.ruka.Count);
            //    else
            //        igrac1.Bacenekarte(bacene, igrac1.BestMove.NovaBoja, igrac2.ruka.Count);

            //    j++;
            //}

            #endregion

            #region Cetvrti test (Sa tekstualnim file-om)

            //for (int j = 0; j < 10; j++)
            //{
            //    Igra igrac1 = new Igra();
            //    Igra igrac2 = new Igra();
            //    Engine engine = new Engine();
            //    engine.SetCards(igrac1, igrac2);

            //    using (StreamWriter sw = new StreamWriter("tokIgre" + (j + 1).ToString() + ".txt"))
            //    {
            //        for (int i = 0; i < 12; i++)
            //        {
            //            if (engine.Spil.Karte.Count > 0)
            //            {
            //                engine.Potez(sw, 1, igrac1, igrac2);
            //                engine.Potez(sw, 2, igrac2, igrac1);
            //            }
            //        }
            //    }

            //    igrac1.Reset();
            //    igrac2.Reset();
            //}

            #endregion

            #region Peti test (dva igraca specificni slucajevi)

            //Spil spil = new Spil(true);
            //Igra igrac1 = new Igra();
            //Igra igrac2 = new Igra();

            //List<Karta> talon = new List<Karta>();

            //talon.Add(new Karta { Broj = "9", Boja = Boja.Tref });

            //List<Karta> ruka = new List<Karta>();

            ////ruka.Add(new Karta { Broj = "7", Boja = Boja.Tref });
            //ruka.Add(new Karta { Broj = "2", Boja = Boja.Tref });
            //ruka.Add(new Karta { Broj = "A", Boja = Boja.Karo });
            //ruka.Add(new Karta { Broj = "8", Boja = Boja.Karo });
            //ruka.Add(new Karta { Broj = "8", Boja = Boja.Pik });
            //ruka.Add(new Karta { Broj = "A", Boja = Boja.Pik });


            //igrac1.SetRuka(ruka);
            //igrac1.Bacenekarte(talon, talon.Last().Boja, 6);

            //ruka.Clear();

            //ruka.Add(new Karta { Broj = "Q", Boja = Boja.Karo });
            //ruka.Add(new Karta { Broj = "Q", Boja = Boja.Herz });
            //ruka.Add(new Karta { Broj = "6", Boja = Boja.Herz });
            //ruka.Add(new Karta { Broj = "Q", Boja = Boja.Pik });
            //ruka.Add(new Karta { Broj = "6", Boja = Boja.Pik });
            //ruka.Add(new Karta { Broj = "A", Boja = Boja.Pik });

            //igrac2.SetRuka(ruka);

            //igrac1.BeginBestMove();

            //Thread.Sleep(1000);

            //igrac1.EndBestMove();

            //talon.AddRange(igrac1.BestMove.Karte);

            //igrac2.Bacenekarte(talon, talon.Last().Boja, igrac1.ruka.Count);

            //igrac2.BeginBestMove();

            //Thread.Sleep(1000);

            //igrac2.EndBestMove();

            //igrac2.BeginBestMove();

            //Thread.Sleep(1000);

            //igrac2.EndBestMove();

            //igrac2.BeginBestMove();

            //Thread.Sleep(1000);

            //igrac2.EndBestMove();

            //// igrac1.Bacenekarte(new List<Karta>(), Boja.Unknown, 6);

            //igrac1.BeginBestMove();

            //Thread.Sleep(1000);

            //igrac1.EndBestMove();




            #endregion

            #region Sesti test (Od teslasa)

            //Igra g = new Igra();
            //g.Reset();
            //g.Bacenekarte(new List<Karta>() { new Karta() { Boja = Boja.Herz, Broj = "5" } }, Boja.Unknown, 6);

            //List<Karta> Hand = new List<Karta>();
            //Hand.Add(new Karta() { Boja = Boja.Herz, Broj = "A" });
            //Hand.Add(new Karta() { Boja = Boja.Herz, Broj = "8" });
            //Hand.Add(new Karta() { Boja = Boja.Tref, Broj = "2" });
            ////Hand.Add(new Karta() { Boja = Boja.Karo, Broj = "4" });
            ////Hand.Add(new Karta() { Boja = Boja.Herz, Broj = "5" });


            //g.SetRuka(Hand);

            //g.BeginBestMove();
            //Thread.Sleep(1000);
            //g.EndBestMove();

            #endregion

        }
    }
}
