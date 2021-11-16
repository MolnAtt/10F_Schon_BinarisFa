using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree
{
    class Program
    {
        class KeresoFa<T>
        {
            Func<T, T, int> relacio;
            private Elem<T> Gyoker { get => fejelem.jobb; } // nincs szülője, belőle mindenki elérhető!
            private Elem<T> fejelem; // magára mutat, vagy a gyökérre. Ezzel oldjuk meg, hogy lehessen üres (gyökér nélküli) fa is
            public KeresoFa(Func<T, T, int> rel)
            {
                fejelem = new Elem<T>();
                relacio = rel;
            }
            public bool Ures() => fejelem.jobb == fejelem;
            
            private class Elem<T>
            {
                public T ertek { get; set; }
                public Elem<T> bal { get; set; }
                public Elem<T> jobb { get; set; }
                public Elem<T> szulo { get; set; }
                public Elem() // csak a fejelemre vonatkozik
                {
                    szulo = this;
                    jobb = this;
                    bal = this;
                }
                public Elem(Elem<T> szulo, T ertek, Func<T, T, int> rel)
                {
                    if (szulo.szulo == szulo) // ez a gyökér-e vagy sem
                    {
                        szulo.jobb = this;
                        // szulo.bal = this; // ez szerintem kell.
                    }
                    else if (rel(szulo.ertek, ertek) == -1) // jobbra vagy balra
                        szulo.jobb = this;
                    else
                        szulo.bal = this; // itt most kihasználtuk, hogy ezt csak olyna helyen hívjuk meg, ahol az egyenlőséggel már foglalkoztunk.
                    this.ertek = ertek;
                    this.szulo = szulo;
                }

                public int Height()
                {
                    if (Level())
                        return 1;
                    if (JobbraVanValaki() && BalraVanValaki())
                        return Math.Max(jobb.Height(), bal.Height())+1;
                    if (JobbraVanValaki())
                        return jobb.Height() + 1;
                    if (BalraVanValaki())
                        return bal.Height() + 1;
                    return -1;
                }

                public List<T> Where(Func<T, bool> predicate)
                {
                    
                    if (Level())
                    {
                        List<T> result = new List<T>();
                        if (predicate(ertek))
                        {
                            result.Add(ertek);
                        }
                        return result;
                    }


                }
                public override string ToString()
                {
                    string sum = "";
                    if (BalraVanValaki())
                    {
                        sum += $"{this.ertek} -> {this.bal.ertek} [ label=\"bal\" ];\n";
                        sum += this.bal.ToString();
                    }
                    if (JobbraVanValaki())
                    {
                        sum += $"{this.ertek} -> {this.jobb.ertek} [ label=\"jobb\" ];\n";
                        sum += this.jobb.ToString();
                    }
                    return sum;
                }
                /*
                */

                public bool JobbraVanValaki() => this.jobb != null;
                public bool BalraVanValaki() => this.bal != null;
                public bool Level() => !(JobbraVanValaki() || BalraVanValaki());

                public Elem<T> Helye_rek(T ertek, Func<T, T, int> rel)
                {
                    switch (rel(this.ertek, ertek))
                    {
                        case -1:
                            if (JobbraVanValaki())
                                return this.jobb.Helye_rek(ertek, rel);
                            break;
                        case 1:
                            if (BalraVanValaki())
                                return this.bal.Helye_rek(ertek, rel);
                            break;
                    }
                    return this;
                }

            }

            private Elem<T> Helye_rek(T ertek)
            {
                if (Ures())
                {
                    return null;
                }
                return Gyoker.Helye_rek(ertek, relacio);
            }


            private Elem<T> Helye_while(T ertek)
            {
                return null;
            }

            public bool Contains(T ertek)
            {
                return 0 == relacio(Gyoker.Helye_rek(ertek, relacio).ertek, ertek);
            }

            public int Height()
            {
                if (Ures())
                    return 0;
                return fejelem.jobb.Height();
            }

            public List<T> Where(Func<T, bool> predicate)
            {
                if (Ures())
                {
                    return new List<T>();
                }
                return fejelem.jobb.Where(predicate);
            }

            public void Remove(T ertek)
            {
                if (Ures())
                {
                    Console.Error.WriteLine("Ez nem fog menni, mert üres");
                    throw new Exception("ejnye");
                }
                Elem<T> kiveendő = Helye_rek(ertek);
                if(relacio(kiveendő.ertek,ertek)!=0)
                {
                    Console.Error.WriteLine("Ez nem fog menni, mert nincs benne ilyen");
                }
                else
                {
                    if (kiveendő.Level())
                    {
                        if (kiveendő.szulo.jobb == kiveendő)
                            kiveendő.szulo.jobb = null;
                        else
                            kiveendő.szulo.bal = null;
                    }
                    else if (kiveendő.BalraVanValaki() && kiveendő.JobbraVanValaki())
                    { }
                    else if (kiveendő.BalraVanValaki())
                    { }
                    else if (kiveendő.JobbraVanValaki())
                    { }
                }

            }

            public void Add(T ertek)
            {
                if (Ures())
                {
                    new Elem<T>(fejelem, ertek, relacio);
                }
                else
                {
                    Elem<T> elem = Helye_rek(ertek);

                    if (relacio(elem.ertek, ertek) != 0)
                    {
                        new Elem<T>(elem, ertek, relacio);
                    }
                }
            }

            public override string ToString()
            {
                Console.Error.WriteLine("-------------------------");
                if (Ures())
                    return "";
                // return fejelem.jobb.ToString();
                return "";
            }
        }
        static void Main(string[] args)
        {
            KeresoFa<int> halmaz = new KeresoFa<int>((x,y)=>x.CompareTo(y));
            halmaz.Add(10);
            halmaz.Add(5);
            halmaz.Add(20);
            halmaz.Add(7);
            halmaz.Add(6);
            halmaz.Add(17);
            halmaz.Add(30);
            halmaz.Add(15);
            halmaz.Add(14);
            halmaz.Add(16);
            Console.WriteLine(halmaz);

            Console.WriteLine(halmaz.Height());
        }
    }
}
