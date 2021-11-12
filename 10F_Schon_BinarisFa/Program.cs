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
                        this.szulo.jobb = this;
                        // szulo.bal = this; // ez szerintem kell.
                    }
                    else if (rel(szulo.ertek, ertek) == -1) // jobbra vagy balra
                        this.szulo.jobb = this;
                    else
                        this.szulo.bal = this; // itt most kihasználtuk, hogy ezt csak olyna helyen hívjuk meg, ahol az egyenlőséggel már foglalkoztunk.
                    this.ertek = ertek;
                    this.szulo = szulo;
                }

                public override string ToString()
                {
                    string sum = "";
                    if (BalraVanValaki())
                    {
                        sum += $"{this.ertek} -> {this.bal.ertek};\n";
                        sum += this.bal.ToString()+ "\n";
                    }
                    if (JobbraVanValaki())
                    {
                        sum += $"{this.ertek} -> {this.jobb.ertek};\n";
                        sum += this.jobb.ToString() + "\n";
                    }
                    return sum;
                }

                bool JobbraVanValaki() => this.jobb != null;
                bool BalraVanValaki() => this.bal != null;
                bool Level() => !(JobbraVanValaki() || BalraVanValaki());

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

            private Elem<T> Keres(T ertek)
            {
                return null;
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
                if (Ures())
                    return "";
                return fejelem.jobb.ToString();
            }
        }
        static void Main(string[] args)
        {
            KeresoFa<int> halmaz = new KeresoFa<int>((x,y)=>x.CompareTo(y));
            Console.WriteLine(halmaz);
            halmaz.Add(10);
            Console.WriteLine(halmaz);
            halmaz.Add(5);
            Console.WriteLine(halmaz);
            halmaz.Add(20);
            Console.WriteLine(halmaz);
            halmaz.Add(7);
            Console.WriteLine(halmaz);
            halmaz.Add(6);
            Console.WriteLine(halmaz);
            halmaz.Add(17);
            Console.WriteLine(halmaz);
            halmaz.Add(30);
            Console.WriteLine(halmaz);
            halmaz.Add(15);
            Console.WriteLine(halmaz);
            halmaz.Add(14);
            Console.WriteLine(halmaz);
            halmaz.Add(16);
            Console.WriteLine(halmaz);
        }
    }
}
