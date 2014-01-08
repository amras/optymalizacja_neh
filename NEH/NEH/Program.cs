using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEH
{
    class Program
    {
        static int[] wczytaj_rozmiar_macierzy(string[] wiersze)
        {
            string[] dane = wiersze[0].Split();
            int[] rozmiar = new int[2];
            rozmiar[0] = int.Parse(dane[0]);
            rozmiar[1] = int.Parse(dane[1]);

            return rozmiar;
        }
        static int[,] wczytaj_plik(int[,] macierz, int liczba_procesów, int liczba_maszyn, string[] wiersze)
        {
            //int[,] macierz = new int[liczba_procesów, liczba_maszyn];
            int i, j;

            for (i = 0; i < liczba_procesów; i++)
            {
                string[] wartosci = wiersze[i + 1].Split();
                for (j = 0; j < liczba_maszyn; j++)
                {
                    macierz[i, j] = int.Parse(wartosci[j]);
                }
            }
            return macierz;
        }
        static void wyswietl_plik(int[,] macierz, int liczba_procesów, int liczba_maszyn)
        {
            int i, j;

            for (i = 0; i < liczba_procesów; i++)
            {
                for (j = 0; j < liczba_maszyn; j++)
                {
                    System.Console.Write(macierz[i, j] + " ");
                }
                System.Console.Write("\n");
            }
            System.Console.Write("\n");
        }
        static int Cmax(int[,] macierz, int liczba_procesów, int liczba_maszyn)
        {
            int i, j;
            int[,] macierz3 = new int[liczba_procesów, liczba_maszyn];

            macierz3[0, 0] = macierz[0, 0];
            for (i = 1; i < liczba_procesów; i++)
            {
                macierz3[i, 0] = macierz3[i - 1, 0] + macierz[i, 0];
            }

            for (j = 1; j < liczba_maszyn; j++)
            {
                macierz3[0, j] = macierz3[0, j - 1] + macierz[0, j];
                for (i = 1; i < liczba_procesów; i++)
                {
                    macierz3[i, j] = Math.Max(macierz3[i, j - 1], macierz3[i - 1, j]) + macierz[i, j];
                }
            }
            return macierz3[liczba_procesów - 1, liczba_maszyn - 1];
        }
        static void Cmax2(int[,] macierz, int liczba_procesów, int liczba_maszyn)
        {
            int i, j;
            int[,] macierz3 = new int[liczba_procesów, liczba_maszyn];

            macierz3[0, 0] = macierz[0, 0];
            for (i = 1; i < liczba_procesów; i++)
            {
                macierz3[i, 0] = macierz3[i - 1, 0] + macierz[i, 0];
            }

            for (j = 1; j < liczba_maszyn; j++)
            {
                macierz3[0, j] = macierz3[0, j - 1] + macierz[0, j];
                for (i = 1; i < liczba_procesów; i++)
                {
                    macierz3[i, j] = Math.Max(macierz3[i, j - 1], macierz3[i - 1, j]) + macierz[i, j];
                }
            }
            wyswietl_plik(macierz3, liczba_procesów, liczba_maszyn);
        }
        static int algorytm_SA(int[,] macierz, int liczba_procesów, int liczba_maszyn, Random rnd, string ścieżka)
        {
            int a, b, j;
            double p, q, delta, lambda = 0.99995, T = 40000000;
            int stary_koniec_ostatniej_operacji, nowy_koniec_ostatniej_operacji, min_koniec_ostatniej_operacji = 99999;
            stary_koniec_ostatniej_operacji = Cmax(macierz, liczba_procesów, liczba_maszyn);
            int[] wiersz = new int[liczba_maszyn];
            int[,] macierz2 = new int[liczba_procesów, liczba_maszyn];

            while (T > 0.5)
            {
                a = rnd.Next(0, liczba_procesów);
                b = rnd.Next(0, liczba_procesów);
                q = rnd.NextDouble();

                for (j = 0; j < liczba_maszyn; j++)
                {
                    wiersz[j] = macierz[a, j];
                }

                for (j = 0; j < liczba_maszyn; j++)
                {
                    macierz[a, j] = macierz[b, j];
                }

                for (j = 0; j < liczba_maszyn; j++)
                {
                    macierz[b, j] = wiersz[j];
                }

                nowy_koniec_ostatniej_operacji = Cmax(macierz, liczba_procesów, liczba_maszyn);

                if (stary_koniec_ostatniej_operacji >= nowy_koniec_ostatniej_operacji)
                {
                    if (min_koniec_ostatniej_operacji > nowy_koniec_ostatniej_operacji)
                    {
                        min_koniec_ostatniej_operacji = nowy_koniec_ostatniej_operacji;
                        macierz2 = przepisz_tablicę(macierz, macierz2, liczba_procesów, liczba_maszyn);
                    }
                    stary_koniec_ostatniej_operacji = nowy_koniec_ostatniej_operacji;
                }
                else
                {
                    delta = nowy_koniec_ostatniej_operacji - stary_koniec_ostatniej_operacji;
                    p = Math.Exp(-delta / T);
                    if (p < q)
                    {
                        for (j = 0; j < liczba_maszyn; j++)
                        {
                            wiersz[j] = macierz[b, j];
                        }

                        for (j = 0; j < liczba_maszyn; j++)
                        {
                            macierz[b, j] = macierz[a, j];
                        }

                        for (j = 0; j < liczba_maszyn; j++)
                        {
                            macierz[a, j] = wiersz[j];
                        }
                    }
                    else stary_koniec_ostatniej_operacji = nowy_koniec_ostatniej_operacji;
                }
                T *= lambda;
            }
            wyswietl_plik(macierz2, liczba_procesów, liczba_maszyn);
            //wpisz_do_pliku(macierz2, liczba_maszyn, liczba_procesów, ścieżka);
            Cmax2(macierz2, liczba_procesów, liczba_maszyn);
            return min_koniec_ostatniej_operacji;
        }
        static int[,] przepisz_tablicę(int[,] macierz, int[,] macierz2, int liczba_procesów, int liczba_maszyn)
        {
            for (int k = 0; k < liczba_procesów; k++)
            {
                for (int l = 0; l < liczba_maszyn; l++)
                {
                    macierz2[k, l] = macierz[k, l];
                }
            }
            return macierz2;
        }
        static void utwórz_plik(string ścieżka, string nazwa_pliku)
        {
            if (!System.IO.File.Exists(ścieżka))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(ścieżka))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.\n", nazwa_pliku);
            }
        }
        static void wpisz_do_pliku(int[,] macierz, int liczba_maszyn, int liczba_procesów, string ścieżka)
        {
            int j;
            string[] wiersz = new string[liczba_maszyn];
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(ścieżka))
            {
                file.Write(liczba_procesów.ToString() + " " + liczba_maszyn.ToString() + Environment.NewLine);
                for (int i = 0; i < liczba_procesów; i++)
                {
                    for (j = 0; j < liczba_maszyn; j++)
                    {
                        wiersz[j] = macierz[i, j].ToString();
                    }

                    foreach (string wier in wiersz)
                    {
                        file.Write(wier + " ");
                    }
                    file.Write(Environment.NewLine);
                }
            }
        }
        static void Main(string[] args)
        {
            Random rnd = new Random();
            int[] rozmiar = new int[2];
            int min_koniec_ostatniej_operacji;

            string nazwa_pliku_wczytywanego = "NEH1.txt";
            string ścieżka_pliku_wczytywanego = System.IO.Path.Combine(@"C:\Users\Michal\Documents\GitHub\optymalizacja_neh\NEH\NEH", nazwa_pliku_wczytywanego);
            string nazwa_pliku_zapisywanego = "NEH1 - najlepsza konfiguracja.txt";
            string ścieżka_pliku_zapisywanego = System.IO.Path.Combine(@"C:\Users\Michal\Documents\GitHub\optymalizacja_neh\NEH\NEH", nazwa_pliku_zapisywanego);

            string[] wiersze = System.IO.File.ReadAllLines(ścieżka_pliku_wczytywanego);
            utwórz_plik(ścieżka_pliku_zapisywanego, nazwa_pliku_zapisywanego);

            rozmiar = wczytaj_rozmiar_macierzy(wiersze);
            int liczba_procesów = rozmiar[0];
            int liczba_maszyn = rozmiar[1];
            System.Console.Write("Liczba maszyn:  " + liczba_maszyn + "\nLiczba procesów:   " + liczba_procesów + "\n\n");
            int[,] macierz = new int[liczba_procesów, liczba_maszyn];
            int[,] macierz2 = new int[liczba_procesów, liczba_maszyn];

            macierz = wczytaj_plik(macierz, liczba_procesów, liczba_maszyn, wiersze);
            wyswietl_plik(macierz, liczba_procesów, liczba_maszyn);
            Cmax2(macierz, liczba_procesów, liczba_maszyn);
            min_koniec_ostatniej_operacji = algorytm_SA(macierz, liczba_procesów, liczba_maszyn, rnd, ścieżka_pliku_zapisywanego);

            System.Console.Write("Koniec ostatniej operacji:  " + min_koniec_ostatniej_operacji + "\n");
            System.Console.Read();
        }
    }
}