using System;

namespace ZindanKefen
{
    class Program
    {
        static Random rastgele = new Random();

        // Durum değişkenleri (seviye atlasan da atak, zırh, coin ve ilaç korunur)
        static int guc = 120, maxGuc = 120, atak = 10, zirh = 0, coin = 0, ilac = 1;
        static int tur = 0, seviye = 1;
        const int MAX_TUR = 60;
        const int ODA_SAYISI = 7;
        const int ENVANTER_BOYUTU = 5;
        static bool anahtarVar = false;

        static bool[] temizOda = new bool[ODA_SAYISI];
        static string[] envanter = new string[ENVANTER_BOYUTU];
        static string[] dusmanAdi = { "", "İskelet", "Hayalet", "Zindan Bekçisi", "Goblin", "Mumya" };
        static string[] bossAdi = { "", "Kara Şövalye", "Zindan Ejderhası" };
        static int[] komsuA = { 0, 2, 1, 1, 2, 3 };
        static int[] komsuB = { 0, 3, 4, 5, 5, 4 };

        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("          Z İ N D A N - K E F E N");
            Console.WriteLine("    Oyun Yapımcısı: Sadık Kerim Aksan");
            Console.WriteLine("========================================\n");

            string ad;
            do
            {
                Console.Write("Adın nedir? ");
                ad = Console.ReadLine();
            } while (ad == "");

            int yas = 0;
            bool yasGecerli = false;
            do
            {
                Console.Write("Yaşın kaç? ");
                try
                {
                    yas = Convert.ToInt32(Console.ReadLine());
                    yasGecerli = true;
                }
                catch
                {
                    yasGecerli = false;
                    Console.WriteLine("Yaşı sadece rakamla yaz!");
                }
                if (yasGecerli && (yas < 1 || yas > 120))
                {
                    yasGecerli = false;
                    Console.WriteLine("Geçerli bir yaş yaz!");
                }
            } while (!yasGecerli);

            if (yas < 18)
            {
                Console.WriteLine("\nÜzgünüm " + ad + ", bu oyun 18 yaş altına kapalı!");
                return;
            }


            bool oyunAktif = true;
            while (oyunAktif)
            {
                if (!seviyeOyna())
                {
                    Console.WriteLine("\n*** OYUN BİTTİ EZİK KAYBETTİN ***");
                    oyunAktif = false;
                    continue;
                }
                int secim;
                if (seviye == 2)
                {
                    Console.WriteLine("\n*** OYUNU KAZANDIN TARİHE GEÇTİN " + ad + "! ***");
                    secim = menuSec(new string[] { "Tekrar oyna", "Oyunu sonlandır" }, 2);
                    if (secim == 2) secim = 3;
                }
                else
                {
                    Console.WriteLine("\n*** SEVİYE " + seviye + " TAMAMLANDI! ***");
                    secim = menuSec(new string[] { "Bu seviyeyi tekrar oyna", "Sıradaki seviyeye geç", "Oyunu sonlandır" }, 3);
                }
                if (secim == 2) seviye++;
                else if (secim == 3)
                {
                    Console.WriteLine("\nGüle güle " + ad + "! Zindanın kapıları seni bekliyor olacak...");
                    oyunAktif = false;
                }
            }
        }


        static bool seviyeOyna()
        {
            tur = 0;
            guc = maxGuc;
            anahtarVar = false;
            for (int i = 0; i < ODA_SAYISI; i++) temizOda[i] = false;
            for (int i = 0; i < ENVANTER_BOYUTU; i++) envanter[i] = "-";
            int oda = 1;
            bool aktif = true, kazandi = false;
            Console.WriteLine("\n######## SEVİYE " + seviye + " ########");
            while (aktif)
            {
                tur++;
                if (tur > MAX_TUR)
                {
                    Console.WriteLine("\nSüren doldu, zindan seni yuttu!");
                    break;
                }
                durumGoster(oda);
                if (temizOda[oda])
                {
                    Console.WriteLine("Bu oda temizlendi, burada kimse yok.");
                }
                else
                {
                    bool galip;
                    if (oda == 6) galip = dovus(bossAdi[seviye], 50 + 40 * seviye, 5 + 4 * seviye);
                    else galip = dovus(dusmanAdi[oda], 15 + 8 * seviye, 2 + 2 * seviye);
                    if (!galip) break;
                    temizOda[oda] = true;
                    if (oda == 6)
                    {
                        kazandi = true;
                        aktif = false;
                        continue;
                    }
                    ganimet(oda);
                }
                if (oda == 1 || oda == 5) tuccar();
                oda = odaGec(oda);
            }
            return kazandi;
        }


        static void durumGoster(int oda)
        {
            Console.WriteLine("\n----------------------------------------------");
            Console.WriteLine("Seviye " + seviye + " | Tur: " + tur + "/" + MAX_TUR + " | Oda: " + odaAdi(oda));
            Console.WriteLine("Güç: " + guc + "/" + maxGuc + "  Atak: " + atak + "  Zırh: " + zirh + "  Coin: " + coin + "  İlaç: " + ilac);
            Console.Write("Envanter: ");
            for (int i = 0; i < ENVANTER_BOYUTU; i++) Console.Write("[" + envanter[i] + "] ");
            Console.WriteLine("\n----------------------------------------------");
        }


        static string odaAdi(int oda)
        {
            switch (oda)
            {
                case 1:
                    return "Giriş Salonu";
                case 2:
                    return "Kütüphane";
                case 3:
                    return "Zindan Koridoru";
                case 4:
                    return "Hazine Odası";
                case 5:
                    return "Gizli Geçit";
                default:
                    return "Kaçış Odası";
            }
        }


        static int menuSec(string[] secenekler, int adet)
        {
            string giris;
            int secim;
            do
            {
                for (int i = 0; i < adet; i++)
                    Console.WriteLine((i + 1) + ") " + secenekler[i]);
                Console.Write("Seçimin: ");
                giris = Console.ReadLine();
                secim = 0;
                switch (giris)
                {
                    case "1":
                        secim = 1;
                        break;
                    case "2":
                        secim = 2;
                        break;
                    case "3":
                        secim = 3;
                        break;
                    case "4":
                        secim = 4;
                        break;
                }
                if (secim < 1 || secim > adet)
                {
                    secim = 0;
                    Console.WriteLine("Geçersiz seçim! Listedeki numaralardan birini yaz.\n");
                }
            } while (secim == 0);
            return secim;
        }


        static int hasarHesapla(int atakGucu)
        {
            return atakGucu + rastgele.Next(1, 7);
        }


        static void esyaEkle(string esya)
        {
            for (int i = 0; i < ENVANTER_BOYUTU; i++)
            {
                if (envanter[i] == "-")
                {
                    envanter[i] = esya;
                    break;
                }
            }
        }


        static void ganimet(int oda)
        {
            switch (oda)
            {
                case 1:
                    atak += 4;
                    esyaEkle("Kılıç");
                    Console.WriteLine("Ganimet: Paslı Kılıç! Atak +4");
                    break;
                case 2:
                    zirh += 3;
                    esyaEkle("Zırh");
                    Console.WriteLine("Ganimet: Deri Zırh! Zırh +3");
                    break;
                case 3:
                    coin += 40;
                    esyaEkle("Coin Kesesi");
                    Console.WriteLine("Ganimet: Coin Kesesi! +40 coin");
                    break;
                case 4:
                    anahtarVar = true;
                    coin += 30;
                    esyaEkle("Anahtar");
                    Console.WriteLine("Ganimet: ZİNDAN ANAHTARI ve +30 coin!");
                    break;
                case 5:
                    coin += 50;
                    ilac++;
                    esyaEkle("İlaç Şişesi");
                    Console.WriteLine("Ganimet: İlaç şişesi ve +50 coin!");
                    break;
            }
        }


        static bool dovus(string ad, int dGuc, int dAtak)
        {
            Console.WriteLine("\n>>> " + ad + " karşına çıktı!");
            while (true)
            {
                if (dGuc <= 0)
                {
                    Console.WriteLine(ad + " yenildi!");
                    break;
                }
                if (guc <= 0) break;
                tur++;
                if (tur > MAX_TUR)
                {
                    Console.WriteLine("Süren doldu!");
                    guc = 0;
                    break;
                }

                Console.WriteLine("\n" + ad + " gücü: " + dGuc + " | Senin gücün: " + guc);
                int secim = menuSec(new string[] { "Saldır", "Savun", "İlaç İç" }, 3);
                if (secim == 2)
                {
                    Console.WriteLine("Kalkanını kaldırdın, " + ad + " bu tur vuramadı!");
                    continue;
                }
                if (secim == 1)
                {
                    int h = hasarHesapla(atak);
                    dGuc -= h;
                    Console.WriteLine(ad + " yaralandı! -" + h);
                }
                else if (ilac > 0)
                {
                    ilac--;
                    guc += 30;
                    if (guc > maxGuc) guc = maxGuc;
                    Console.WriteLine("İlaç içtin, güç: " + guc);
                }
                else Console.WriteLine("İlacın kalmadı!");

                if (dGuc > 0)
                {
                    int h = rastgele.Next(1, 7) + dAtak;
                    h = h - zirh;
                    if (h < 1) h = 1;
                    guc -= h;
                    Console.WriteLine(ad + " sana " + h + " hasar verdi!");
                }
            }
            return guc > 0;
        }


        static bool odeme(int fiyat)
        {
            if (coin >= fiyat)
            {
                coin -= fiyat;
                return true;
            }
            Console.WriteLine("Yetersiz coin!");
            return false;
        }


        static void tuccar()
        {
            Console.WriteLine("\nTüccar: Coin'in varsa güçlenebilirsin!");
            int secim;
            do
            {
                Console.WriteLine("Coin: " + coin);
                secim = menuSec(new string[] { "Kılıcı bile (+3 atak) - 30 coin", "Zırhı güçlendir (+2 zırh) - 30 coin", "İlaç al - 20 coin", "Çık" }, 4);
                if (secim == 1 && odeme(30)) atak += 3;
                else if (secim == 2 && odeme(30)) zirh += 2;
                else if (secim == 3 && odeme(20)) ilac++;
            } while (secim != 4);
        }


        static int odaGec(int oda)
        {
            int a = komsuA[oda], b = komsuB[oda];
            Console.WriteLine("\nNereye gideceksin?");
            int secim;
            if (oda == 5) secim = menuSec(new string[] { odaAdi(a), odaAdi(b), "Kaçış Odası" }, 3);
            else secim = menuSec(new string[] { odaAdi(a), odaAdi(b) }, 2);
            if (secim == 1) return a;
            if (secim == 2) return b;
            if (anahtarVar)
            {
                Console.WriteLine("Anahtarı kilide soktun, kapı açıldı!");
                return 6;
            }
            Console.WriteLine("Kapı kilitli! Önce anahtarı bulmalısın.");
            return 5;
        }
    }
}