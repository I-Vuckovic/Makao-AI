using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIG.AV.Karte;
using System.Threading;

namespace Makao_v2._0
{
    class Igra : IIgra
    {
        #region Stuff that needs to be done

        /*
        Version 2.0
        Starting idea: Raditi uz pomoc kombinatorike, problem u tome previse izracunavanja kada su nevidjene 45 karte
        Ideja sa optimizaciju: Proveriti prvo kombinacije koje vracaju vrednost vecu od nule nakon sto prodju kroz metodu "CheckValidne"
         
            Idea: Imate Early, Mid i Late game alfa bete
            Prvi bi mogo da radi samo sa mogucim potezima, mid malo vecu dubinu
            Late bi radio full on kombinatoriku kakvu protivnik ruku zapravo moze da ima 

            Kombinatorika zauzima mnooooogo vremena

            Fixed: zavisnost update-a kazennih od tipa poteza koji se odluci da se odigra

            Za sav potencijalno neiskoriscen kod traziti "***" time su oznacene stvari koje na kraj su zavrsile neiskoriscene

            "void BrojBacenih(Move move, bool isPlayer)" ima potencijalnu gresku, proveritis

            VAZNO: Uradjen "easy fix" u funkciji "SetTip" jer je u nekim, jos neutvrdjenim specijalnim prilikama ulazio u tu funkciju bez karata, za sad spazeno 
            samo kada je protivnik na potezu, ne zna se da li se desava i kada je igrac na potezu, ne zna se jos da li ce ovakav fix imati neke posledice, obratiti paznju

        */

        #endregion

        #region Attributes

        protected bool promenaTalona;
        protected bool daLiJeStvarnoKupio; //Nakon par nervnog sloma dodavanjem ovakvog bula je sredjen problem sa kaznenim
        protected bool kupioKaznene;
        protected bool pruning;
        protected bool kupio;
        protected List<Karta> validne;
        public Spil spil;
        protected List<Karta> talon;
        public List<Karta> ruka;
        protected Boja trenutnaBoja;
        protected int protivnik;
        protected Move bestMove;
        protected List<Karta> vidjeneKaznene;
        Thread logika;
        public int maxDepth;

        public Igra()
        {
            Reset();
        }

        int BrojCvorova(Move root)
        {
            int i = 0;

            i += root.Children.Count;
            if (root.Children.Count != 0)
            {
                foreach (Move child in root.Children)
                {
                    i += BrojCvorova(child);
                }
            }

            return i;
        }

        #endregion


        #region Interface Methods

        public IMove BestMove { get => bestMove; set => throw new NotImplementedException(); }

        public void Bacenekarte(List<Karta> karte, Boja boja, int BrojKarataProtivnika)
        {
            if (karte != null)
                talon.AddRange(karte);

            if (boja != Boja.Unknown)
                trenutnaBoja = boja;
            else if (karte.Count != 0)
                trenutnaBoja = karte.Last().Boja;

            if (karte.Count == 0)
                promenaTalona = false;
            else
                promenaTalona = true;

                
            protivnik = BrojKarataProtivnika;
            spil.Karte.RemoveAll(x => karte.Exists(y => y.Broj == x.Broj && y.Boja == x.Boja));
        }

        public void BeginBestMove()
        {

            //if (spil.Karte.Count > 35 && protivnik > 3)
            //{
            //    EarlyAlphaBeta();
            //}
            //else if (spil.Karte.Count > 23)
            //{
            //    MidAlphaBeta();
            //}
            //else
            //{
            //    LateAlphaBeta();
            //}

            if (bestMove.Tip == TipPoteza.KupiKartu && bestMove.Karte.Count == 0)
                kupio = true;
            else
                kupio = false;

            if (bestMove.Tip == (TipPoteza.KupiKartu | TipPoteza.BacaKartu))
                kupio = true;


            if (bestMove.Tip != TipPoteza.KupiKartu)
            {
                if (bestMove.Tip == TipPoteza.KupiKazneneKarte || !promenaTalona)
                    daLiJeStvarnoKupio = true;
                else
                    daLiJeStvarnoKupio = false;
            }


            //Prilikom debagiranja odredjenih slucajeva iskljuciti thread, pokrenuti direktno "Start()" i u startu staviti fiksu dubinu po zelji u while uslovu

            //Start();

            bestMove = new Move();

            logika = new Thread(Start);
            logika.Start();

            //var watch = System.Diagnostics.Stopwatch.StartNew();

            //List<List<Karta>> test = MakeCombinations(spil.Karte);

            //watch.Stop();
            //var elapsed = watch.ElapsedMilliseconds;

            //Console.WriteLine(elapsed);
        }

        public void EndBestMove()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            logika.Abort();

            ruka.RemoveAll(x => bestMove.Karte.Exists(y => y == x));
            talon.AddRange(bestMove.Karte);
            if (bestMove.Karte.Count != 0)
            {
                if (bestMove.Karte.Last().Broj == "J")
                    trenutnaBoja = bestMove.NovaBoja;
                else
                    trenutnaBoja = bestMove.Karte.Last().Boja;
            }

            watch.Stop();
            var elapsed = watch.ElapsedMilliseconds;
            //Console.WriteLine(elapsed);

        }

        public void KupioKarte(List<Karta> karte)
        {
            ruka.AddRange(karte);
            spil.Karte.RemoveAll(x => karte.Exists(y => y.Broj == x.Broj && y.Boja == x.Boja));

            UptadeKaznene(karte);
        }

        public void Reset()
        {
            ruka = new List<Karta>();
            talon = new List<Karta>();
            spil = new Spil();
            kupio = false;
            promenaTalona = true;
            validne = new List<Karta>();
            bestMove = new Move();
            vidjeneKaznene = new List<Karta>();

        }

        public void SetRuka(List<Karta> karte)
        {
            ruka.AddRange(karte);

            spil.Karte.RemoveAll(x => ruka.Exists(y => y.Broj == x.Broj && y.Boja == x.Boja));

            UptadeKaznene(karte);
        }

        #endregion

        #region My Methods

        #region Kombinatorika u pokusaju

        List<List<Karta>> MakeCombinations(List<Karta> lista)
        {
            List<List<Karta>> zavracanje = new List<List<Karta>>();
            List<Karta> zaRad = new List<Karta>();

            foreach (Karta k in lista.ToList())
            {
                List<Karta> nova = new List<Karta>();
                nova.Add(k);
                lista.Remove(k);
                zaRad.AddRange(lista);
                MakeList(nova, zaRad, zavracanje);

            }


            return zavracanje;
        }

        void MakeList(List<Karta> lista, List<Karta> source, List<List<Karta>> zavracanje)
        {
            foreach (Karta k in source.ToList())
            {
                lista.Add(k);
                if (lista.Count != 6)
                {
                    List<Karta> preostale = new List<Karta>();
                    preostale.AddRange(source);
                    preostale.Remove(k);
                    MakeList(lista, preostale, zavracanje);
                }

                if (lista.Count == 6)
                {
                    List<Karta> zaUpis = new List<Karta>();
                    zaUpis.AddRange(lista);
                    zavracanje.Add(zaUpis);
                }
                lista.Remove(k);

                source.Remove(k);
            }
        }

        #endregion

        void Start()
        {
            Move root = new Move(talon.Last());

            maxDepth = 1;
            while (true)
            {
                Move najbolji = AlphaBeta(root, maxDepth, -2147483647, +2147483647, true, CheckValidne(trenutnaBoja, talon.Last().Broj, ruka));
                while (najbolji.Parent != null && najbolji.Parent.Parent != null)
                    najbolji = najbolji.Parent;
                bestMove = najbolji;
                maxDepth++;
                //Console.WriteLine(BrojCvorova(root));
                //if (maxDepth == 5)
                //{
                //    root.PrintPretty("", true);
                //}
                root = new Move(talon.Last());
            }
        }

        void UptadeKaznene(List<Karta> karte)
        {
            //if (karte.Exists(x => x.Broj == "2" && x.Boja == Boja.Tref))
            //{
            //    vidjeneKaznene.Add(bestMove.Karte.Find(y => y.Broj == "2" && y.Boja == Boja.Tref));
            //}
            //else if (karte.Exists(x => x.Broj == "7"))
            //{
            //    vidjeneKaznene.AddRange(bestMove.Karte.FindAll(y => y.Broj == "7"));
            //}

            foreach(Karta k in karte)
            {
                if (k.Broj == "7" || (k.Broj == "2" && k.Boja == Boja.Tref))
                {
                    vidjeneKaznene.Add(new Karta() { Broj = k.Broj, Boja = k.Boja });
                }
            }
        }


        Move AlphaBeta(Move root, int depth, int alpha, int beta, bool isPlayer, List<Karta> trenutneValidne)
        {

            if (depth == 0)
                return root;

            if (depth == maxDepth)
                kupioKaznene = daLiJeStvarnoKupio;

            if (isPlayer)
            {
                Move najbolji = new Move();
                najbolji.Parent = root;

                if (root.Karte.Last().Broj == "2" && root.Karte.Last().Boja == Boja.Tref && (talon.Count != 1 || depth < maxDepth) && (!kupioKaznene || vidjeneKaznene.Count == 0 || !vidjeneKaznene.Exists(x => x.Broj == root.Karte.Last().Broj && x.Boja == root.Karte.Last().Boja)))
                {
                    if (maxDepth == 1)
                    {
                        vidjeneKaznene.Add(new Karta() { Broj = root.Karte.Last().Broj, Boja = root.Karte.Last().Boja });
                    }
                    najbolji.Tip = TipPoteza.KupiKazneneKarte;
                    root.Children.Add(najbolji);
                    return najbolji;
                }

                if (root.Karte.Last().Broj == "7" && (talon.Count != 1 || depth < maxDepth) && (!kupioKaznene || vidjeneKaznene.Count == 0 || !vidjeneKaznene.Exists(x => x.Broj == root.Karte.Last().Broj && x.Boja == root.Karte.Last().Boja)))
                {
                    if (maxDepth == 1)
                    {
                        vidjeneKaznene.Add(new Karta() { Broj = root.Karte.Last().Broj, Boja = root.Karte.Last().Boja });
                    }
                    trenutneValidne.RemoveAll(x => x.Broj != "7");
                    if (trenutneValidne.Count == 0)
                    {
                        najbolji.Tip = TipPoteza.KupiKazneneKarte;
                        root.Children.Add(najbolji);
                        return najbolji;
                    }
                }

                if (trenutneValidne.Count == 0 && !kupio)
                {
                    najbolji.Tip = TipPoteza.KupiKartu;
                    root.Children.Add(najbolji);
                    return najbolji;
                }

                if (trenutneValidne.Count == 0 && kupio)
                {
                    najbolji.Tip = TipPoteza.KrajPoteza;
                    root.Children.Add(najbolji);
                    return najbolji;
                }

                kupioKaznene = false;
                kupio = false;

                najbolji = new Move
                {
                    Jacina = -2147483640
                };

                foreach (Karta k in trenutneValidne.ToList())
                {
                    Move move = MakeMove(root, k, alpha, beta, isPlayer, najbolji, ruka, depth);
                    pruning = false;

                    SetTip(move, isPlayer);
                    move.OdrediJacinu(isPlayer, depth);

                    List<Karta> nova = new List<Karta>();
                    nova.AddRange(spil.Karte);

                    nova = IspitajZacu(nova, move); //Proverava ako je bacena zaca na osnovu koje boje se odredjuju validne karte
                                                    // I takodje izbacuje sve karte vidjene u tekucoj grani

                    Move donji = AlphaBeta(move, depth - 1, alpha, beta, !isPlayer, nova);

                    if (donji.Tip == TipPoteza.KupiKartu)
                    {
                        donji = move;
                        donji.Jacina += 300*depth;
                    }//Vrednuje se ako se naislo na scenario gde protivnik ne moze da igra vise
                    if (donji.Tip == TipPoteza.KupiKazneneKarte)
                    {
                        donji = move;
                        donji.Jacina += 600*depth;
                    }//Ako protivnik je prinudjen da kupi kaznene (nema sedmice vise kojom moze da pokrije) daje se prioritet

                    if (najbolji.Jacina < donji.Jacina)
                    {
                        najbolji = donji;
                    }

                    alpha = Math.Max(alpha, najbolji.Jacina);

                    if (beta <= alpha)
                        break;
                }

                return najbolji;
            }
            else
            {
                Move najbolji = new Move();
                najbolji.Parent = root;

                if (root.Karte.Last().Broj == "2" && root.Karte.Last().Boja == Boja.Tref)
                {
                    najbolji.Tip = TipPoteza.KupiKazneneKarte;
                    root.Children.Add(najbolji);
                    return najbolji;
                }

                if (root.Karte.Last().Broj == "7")
                {
                    trenutneValidne.RemoveAll(x => x.Broj != "7");
                    if (trenutneValidne.Count == 0)
                    {
                        najbolji.Tip = TipPoteza.KupiKazneneKarte;
                        root.Children.Add(najbolji);
                        return najbolji;
                    }
                }

                if (trenutneValidne.Count == 0)
                {
                    najbolji.Tip = TipPoteza.KupiKartu;
                    root.Children.Add(najbolji);
                    return najbolji;
                }


                najbolji = new Move();
                najbolji.Jacina = 2147483640;

                foreach (Karta k in trenutneValidne.ToList())
                {
                    Move move = MakeMove(root, k, alpha, beta, isPlayer, najbolji, spil.Karte, depth);
                    pruning = false;

                    SetTip(move, isPlayer);
                    move.OdrediJacinu(isPlayer, depth);

                    List<Karta> nova = new List<Karta>();
                    nova.AddRange(ruka);

                    nova = IspitajZacu(nova, move); //Proverava ako je bacena zaca na osnovu koje boje se odredjuju validne karte
                                                    // I takodje izbacuje sve karte vidjene u tekucoj grani;

                    Move donji = AlphaBeta(move, depth - 1, alpha, beta, !isPlayer, nova);

                    if (donji.Tip == TipPoteza.KupiKartu)
                    {
                        donji = move;
                        donji.Jacina += 300*depth;
                    }//Vrednuje se ako se naislo na scenario gde protivnik ne moze da igra vise
                    if (donji.Tip == TipPoteza.KupiKazneneKarte)
                    {
                        donji = move;
                        donji.Jacina += 600*depth;
                    }//Ako protivnik je prinudjen da kupi kaznene (nema sedmice vise kojom moze da pokrije) daje se prioritet

                    if (najbolji.Jacina > donji.Jacina)
                    {
                        najbolji = donji;
                    }

                    beta = Math.Min(beta, najbolji.Jacina);

                    if (beta <= alpha)
                        break;
                }

                return najbolji;
            }

        }

        List<Karta> IspitajZacu(List<Karta> nova, Move move)
        {
            if (move.Karte.Last().Broj == "J")
            {
                nova = CheckValidne(move.NovaBoja, move.Karte.Last().Broj, nova);
            }
            else
            {
                nova = CheckValidne(move.Karte.Last().Boja, move.Karte.Last().Broj, nova);
            }
            nova = ClearUsed(nova, move);
            return nova;
        }

        void SetTip(Move move, bool isPlayer)
        {
            if (move.Karte.Count > 0)
            {
                if (move.Karte.Last().Broj == "A")
                {
                    move.Tip = move.Tip | TipPoteza.KupiKartu;
                }
                else
                {
                    move.Tip = move.Tip | TipPoteza.KrajPoteza;
                }

                int brojBacenih = BrojBacenih(move);

                if (isPlayer)
                {
                    if ((ruka.Count - brojBacenih) < 2)
                        move.Tip = move.Tip | TipPoteza.Poslednja;
                   // if ((ruka.Count - brojBacenih) < 1)
                      //  move.Tip = move.Tip | TipPoteza.Makao;
                }
                else
                {
                    if ((protivnik - brojBacenih) < 2)
                        move.Tip = move.Tip | TipPoteza.Poslednja;
                    //if ((protivnik - brojBacenih) < 1)
                      //  move.Tip = move.Tip | TipPoteza.Makao;
                }

                if (move.Karte.Last().Broj == "J")
                {
                    List<Karta> preostale = new List<Karta>();
                    if (isPlayer)
                        preostale.AddRange(ruka);
                    else
                        preostale.AddRange(spil.Karte);
                    preostale = ClearUsed(preostale, move);
                    move.NovaBoja = ColorsLeft(preostale);

                    move.Tip = move.Tip | TipPoteza.PromeniBoju;
                }
            }
        }

        int BrojBacenih(Move move)
        {
            Move pom = move;
            int p = 0;
            while (pom.Parent != null)
            {
                p += pom.Karte.Count;
                if (pom.Parent.Parent == null)
                    break;
                pom = pom.Parent.Parent;
            }

            return p;
        }


        Move MakeMove(Move root, Karta k, int alpha, int beta, bool isPlayer, Move najbolji, List<Karta> trenutneValidne, int depth)
        {

            Move move = new Move(k);
            root.Children.Add(move);
            move.Parent = root;
            move.Tip = TipPoteza.BacaKartu;
            if (k.Broj == "A" || k.Broj == "8")
            {

                List<Karta> preostale = new List<Karta>();
                preostale.AddRange(CheckValidne(k.Boja, k.Broj, trenutneValidne));
                preostale.Remove(k);
                preostale = ClearUsed(preostale, move);

                if (preostale.Count > 0)
                {
                    Move zaVracanje = new Move();

                    foreach (Karta p in preostale.ToList())
                    {
                        pruning = false;
                        zaVracanje = MakeChain(move, preostale, p, isPlayer, najbolji, alpha, beta, depth);

                        najbolji = zaVracanje;
                        if (pruning)
                            break;

                        ShiftLeft(preostale);
                    }

                    root.Children.Remove(move);
                    root.Children.Add(najbolji);
                    return najbolji;
                }
            }
            /*else if (k.Broj == "J")
            {
                List<Karta> preostale = new List<Karta>();
                if (isPlayer)
                    preostale.AddRange(ruka);
                else
                    preostale.AddRange(spil.Karte);
                preostale = ClearUsed(preostale, move);
                move.NovaBoja = ColorsLeft(preostale);

                move.Tip = move.Tip | TipPoteza.PromeniBoju;

            }*/

            return move;
        }

        Move MakeChain(Move root, List<Karta> preostale, Karta k, bool isPlayer, Move najbolji, int alpha, int beta, int depth)
        {
            int i = 0;
            List<Karta> pomocna = new List<Karta>();
            pomocna.AddRange(preostale);
            Move child = new Move();
            child.Karte.AddRange(root.Karte);
            child.Parent = root.Parent;
            child.Tip = TipPoteza.BacaKartu;

            if (k.Broj == "8" || k.Broj == "A")
            {
                if ((k.Boja == child.Karte.Last().Boja || k.Broj == child.Karte.Last().Broj) &&
                    (child.Karte.Last().Broj == "8" || child.Karte.Last().Broj == "A"))
                {
                    child.Karte.Add(k);

                    pomocna.Clear();
                    if (isPlayer)
                    {
                        pomocna.AddRange(CheckValidne(k.Boja, k.Broj, ruka)); //ponovo proverava sve validne iz ruku na osnovu nove karte na vrhu lanca
                    }
                    else
                    {
                        pomocna.AddRange(CheckValidne(k.Boja, k.Broj, spil.Karte));

                    }
                    pomocna = ClearUsed(pomocna, child);

                    if (pomocna.Count > 0)
                    {
                        Move zaTestiranje = new Move();
                        Move zaVracanje = new Move();
                        if (isPlayer)
                            zaVracanje.Jacina = -2147483640;
                        else
                            zaVracanje.Jacina = 2147483640;

                        for (i = 0; i < pomocna.Count; i++)
                        {
                            if (!pruning)
                            {
                                if (isPlayer)
                                {
                                    zaTestiranje = MakeChain(child, pomocna, pomocna[i], isPlayer, najbolji, alpha, beta, depth);
                                    if (zaTestiranje.Jacina > zaVracanje.Jacina)
                                        zaVracanje = zaTestiranje;

                                }
                                else if (child.Karte.Count < protivnik)
                                {
                                    zaTestiranje = MakeChain(child, pomocna, pomocna[i], isPlayer, najbolji, alpha, beta, depth);
                                    if (zaTestiranje.Jacina < zaVracanje.Jacina)
                                        zaVracanje = zaTestiranje;
                                }
                                else
                                {
                                    return List(child, alpha, beta, najbolji, isPlayer, depth);
                                }

                            }

                        }

                        return zaVracanje;

                    }

                }
            }
            else
            {
                if ((k.Boja == child.Karte[child.Karte.Count - 1].Boja &&
                    (child.Karte[child.Karte.Count - 1].Broj == "A" || child.Karte[child.Karte.Count - 1].Broj == "8")) ||
                    k.Broj == "J")
                {
                    child.Karte.Add(k);

                }
            }

            if (i == 0)
            {
                return List(child, alpha, beta, najbolji, isPlayer, depth);
            }

            return child;
        }

        Move List(Move child, int alpha, int beta, Move najbolji, bool isPlayer, int depth)
        {
            SetTip(child, isPlayer);
            child.OdrediJacinu(isPlayer, depth);

            if (isPlayer)
            {
                if (najbolji.Jacina < child.Jacina)
                {
                    najbolji = child;
                }

                alpha = Math.Max(alpha, najbolji.Jacina);
            }
            else
            {
                if (najbolji.Jacina > child.Jacina)
                {
                    najbolji = child;
                }

                beta = Math.Min(beta, najbolji.Jacina);
            }

            if (beta <= alpha)
                pruning = true;

            return najbolji;
        }

        List<Karta> CheckValidne(Boja boja, string broj, List<Karta> zaTestiranje)
        {
            List<Karta> karte = new List<Karta>();
            foreach (Karta k in zaTestiranje)
            {
                if (k.Boja == boja || k.Broj == broj || k.Broj == "J")
                {
                    karte.Add(k);
                }
            }
            return karte;
        }

        public void ShiftLeft(List<Karta> lista)
        {
            Karta k = lista[0];

            for (int i = 0; i < lista.Count - 1; i++)
            {
                lista[i] = lista[i + 1];
            }

            lista[lista.Count - 1] = k;
        }

        //Iz liste "zaCiscenje" izbacuje sve karte koje su vec vidjene u grani u kojoj se Move "start" nalazi
        List<Karta> ClearUsed(List<Karta> zaCiscenje, Move start)
        {
            List<Karta> zaVracanje = new List<Karta>();
            zaVracanje.AddRange(zaCiscenje);
            zaVracanje.RemoveAll(x => start.Karte.Exists(y => y.Boja == x.Boja && y.Broj == x.Broj));
            if (start.Parent != null)
                zaVracanje = ClearUsed(zaVracanje, start.Parent);


            return zaVracanje;
        }

        // ***
        void Sedmice(Move root)
        {
            Move move = new Move();
            if (ruka.Exists(x => x.Broj == "7") && spil.Karte.FindAll(x => x.Broj == "7").Count < ruka.FindAll(x => x.Broj == "7").Count)
            {
                move.Karte.Add(ruka.Find(x => x.Broj == "7"));
                move.Tip = TipPoteza.KrajPotezaBacikartu;
            }
            else
            {
                move.Tip = TipPoteza.KupiKazneneKarte;
            }
            root.Children.Add(move);
            move.Parent = root;
        }

        //root ovde predstavlja glavni root kome trebaju da se vrate ovi terminalni cvorovi, ne root manjeg stabla
        //move predstavlja move manjeg stabla iz kog se izvlace terminalni
        //Po novom sistemu alpha-bete ovo mozda uopste nece trebati ***
        void ReturnTerminal(Move move, Move root)
        {
            if (move.Children.Count == 0)
            {
                if (move.Karte.Last().Broj == "A")
                {
                    move.Tip = TipPoteza.BacaKartu | TipPoteza.KupiKartu;
                }
                else if (move.Karte.Last().Broj == "J")
                {
                    move.Tip = TipPoteza.BacaKartu | TipPoteza.KrajPotezaPromeniBoju;
                    move.NovaBoja = ColorsLeft(ClearUsed(ruka, move));
                }
                else
                {
                    move.Tip = TipPoteza.KrajPotezaBacikartu;
                }
                root.Children.Add(move);
                move.Parent = root;
            }
            else
            {
                foreach (Move m in move.Children)
                {
                    ReturnTerminal(m, root);
                }
            }
        }

        Boja ColorsLeft(List<Karta> preostale)
        {
            int karo = 0;
            int herz = 0;
            int pik = 0;
            int tref = 0;

            foreach (Karta k in preostale)
            {
                switch (k.Boja)
                {
                    case Boja.Karo:
                        karo++;
                        break;
                    case Boja.Herz:
                        herz++;
                        break;
                    case Boja.Tref:
                        tref++;
                        break;
                    case Boja.Pik:
                        pik++;
                        break;
                }
            }

            if (karo > herz && karo > pik && karo > tref)
                return Boja.Karo;
            else
            {
                if (herz > pik && herz > tref)
                    return Boja.Herz;
                else
                {
                    if (pik > tref)
                        return Boja.Pik;
                    else
                        return Boja.Tref;
                }
            }


        }

        #endregion
    }
}
