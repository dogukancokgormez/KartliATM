using System;
//  KARTLI ATM PROGRAMI  (C# - Yeni baslayanlar icin)
//  - Sadece IF ve GOTO kullanilmistir (for, while, switch YOK)
//  - KARTSIZ GIRIS YOKTUR: ATM'ye girmek icin kart numarasi
//    ve dogru PIN sart. Baska bir giris yolu yoktur.
namespace KartliATM
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1) MUSTERI BILGILERI (3 kayitli kart)
            string[] kartNo = { "1111222233334444", "5555666677778888", "9999000011112222" };
            string[] pin = { "1234", "4321", "1111" };
            string[] isim = { "Ahmet Yilmaz", "Ayse Demir", "Mehmet Kaya" };
            int[] bakiye = { 5000, 12000, 800 };
            bool[] bloke = { false, false, false };
            // Gunluk cekilen para (her kart icin)
            int[] gunlukCekilen = { 0, 0, 0 };
            int gunlukLimit = 10000;
            int tekSeferLimit = 5000;
            // Son 5 islemi saklamak icin (her kart icin 5 satir)
            string[,] gecmis = new string[3, 5];
            int[] gecmisSayisi = { 0, 0, 0 };
            // 2) YARDIMCI DEGISKENLER (hepsi basta tanimli)
            string kartGirdi = "";
            string pinGirdi = "";
            string secim = "";
            string girdi = "";
            string evetHayir = "";
            string eskiPin = "";
            string yeniPin = "";
            string yeniPin2 = "";
            string hedefKart = "";
            string gecmisYaz = "";
            int kartIndex = -1;
            int hedefIndex = -1;
            int hak = 3;
            int tutar = 0;
            int i = 0;
            int d = 0;
            int j = 0;
            int donus = 0;
            int gecmisKisi = 0;
            int yazdirSayac = 0;
            bool sayiMi = false;
            // 3) KARTA GIRIS EKRANI
            //    Kart numarasi girilmeden hicbir yere gecilemez.
        KARTA_GIRIS:
            Console.Clear();
            Console.WriteLine("==========================================");
            Console.WriteLine("        HOSGELDINIZ - ATM'YE");
            Console.WriteLine("==========================================");
            Console.WriteLine("\nLutfen kartinizi takin.");
            Console.WriteLine("(16 haneli kart numaranizi yazin)");
            Console.WriteLine("Programdan cikmak icin 0 yazin.");
            Console.WriteLine("");
            Console.Write("Kart No: ");
            kartGirdi = Console.ReadLine();
            if (kartGirdi == null) goto KARTA_GIRIS;
            if (kartGirdi == "0") goto PROGRAM_BITIS;
            if (kartGirdi.Length != 16)
            {
                Console.WriteLine("\nHATA: Kart numarasi 16 haneli olmali!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto KARTA_GIRIS;
            }
            // 4) KARTI LISTEDE ARA (goto ile dongu)
            kartIndex = -1;
            i = 0;
        KART_ARA:
            if (i >= 3) goto KART_ARA_BITTI;
            if (kartNo[i] == kartGirdi)
            {
                kartIndex = i;
                goto KART_ARA_BITTI;
            }
            i = i + 1;
            goto KART_ARA;
        KART_ARA_BITTI:
            if (kartIndex == -1)
            {
                Console.WriteLine("\nHATA: Bu kart sistemde kayitli degil!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto KARTA_GIRIS;
            }
            if (bloke[kartIndex] == true)
            {
                Console.WriteLine("\nBu kart BLOKELIDIR!");
                Console.WriteLine("Lutfen bankanizla iletisime gecin.");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto KARTA_GIRIS;
            }
            // 5) PIN KONTROLU (3 hak)
            hak = 3;
        PIN_SOR:
            Console.WriteLine("\nKart okundu: " + isim[kartIndex]);
            Console.Write("PIN (4 hane): ");
            pinGirdi = Console.ReadLine();
            if (pinGirdi == pin[kartIndex]) goto GIRIS_BASARILI;
            hak = hak - 1;
            if (hak == 0) goto KART_BLOKE_OLDU;
            Console.WriteLine("HATALI PIN! Kalan hakkiniz: " + hak);
            goto PIN_SOR;
        KART_BLOKE_OLDU:
            bloke[kartIndex] = true;
            Console.WriteLine("\n3 kez hatali PIN girdiniz!");
            Console.WriteLine("KARTINIZ BLOKE EDILDI.");
            Console.WriteLine("Devam etmek icin Enter'a basin...");
            Console.ReadLine();
            goto KARTA_GIRIS;
            // 6) GIRIS BASARILI -> ANA MENU
        GIRIS_BASARILI:
            Console.WriteLine("\nGiris basarili. Hosgeldiniz " + isim[kartIndex] + "!");
            Console.WriteLine("Devam etmek icin Enter'a basin...");
            Console.ReadLine();
        MENU:
            Console.Clear();
            Console.WriteLine("==========================================");
            Console.WriteLine("  ANA MENU  -  " + isim[kartIndex]);
            Console.WriteLine("==========================================");
            Console.WriteLine("  1 - Bakiye Sorgula");
            Console.WriteLine("  2 - Para Cek");
            Console.WriteLine("  3 - Para Yatir");
            Console.WriteLine("  4 - Havale / Transfer");
            Console.WriteLine("  5 - Son Islemler");
            Console.WriteLine("  6 - PIN Degistir");
            Console.WriteLine("  7 - Kartimi Iade Et (Cikis)");
            Console.WriteLine("==========================================");
            Console.Write("Seciminiz: ");
            secim = Console.ReadLine();
            if (secim == "1") goto BAKIYE_SORGULA;
            if (secim == "2") goto PARA_CEK;
            if (secim == "3") goto PARA_YATIR;
            if (secim == "4") goto TRANSFER;
            if (secim == "5") goto SON_ISLEMLER;
            if (secim == "6") goto PIN_DEGISTIR;
            if (secim == "7") goto KARTI_IADE;
            Console.WriteLine("\nGecersiz secim! 1 ile 7 arasinda bir sayi girin.");
            Console.WriteLine("Devam etmek icin Enter'a basin...");
            Console.ReadLine();
            goto MENU;
            // 1 - BAKIYE SORGULA
        BAKIYE_SORGULA:
            Console.Clear();
            Console.WriteLine("------------- BAKIYE SORGULAMA -------------");
            Console.WriteLine("Hesap Sahibi : " + isim[kartIndex]);
            Console.WriteLine("Guncel Bakiye: " + bakiye[kartIndex] + " TL");
            Console.WriteLine("Bugun Cekilen: " + gunlukCekilen[kartIndex] + " TL");
            Console.WriteLine("Kalan Limit  : " + (gunlukLimit - gunlukCekilen[kartIndex]) + " TL");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Ana menuye donmek icin Enter'a basin...");
            Console.ReadLine();
            goto MENU;
            // 2 - PARA CEK
        PARA_CEK:
            Console.Clear();
            Console.WriteLine("---------------- PARA CEKME ----------------");
            Console.WriteLine("Bakiyeniz: " + bakiye[kartIndex] + " TL");
            Console.WriteLine("Tek seferde en fazla " + tekSeferLimit + " TL cekebilirsiniz.");
            Console.WriteLine("Tutar 10'un kati olmalidir.");
            Console.WriteLine("Iptal icin 0 yazin.");
            Console.Write("Cekilecek tutar: ");
            girdi = Console.ReadLine();
            sayiMi = int.TryParse(girdi, out tutar);
            if (sayiMi == false)
            {
                Console.WriteLine("\nHATA: Lutfen sadece sayi girin!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto PARA_CEK;
            }
            if (tutar == 0) goto MENU;
            if (tutar < 0)
            {
                Console.WriteLine("\nHATA: Negatif tutar girilemez!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto PARA_CEK;
            }
            if (tutar % 10 != 0)
            {
                Console.WriteLine("\nHATA: Tutar 10'un kati olmali!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto PARA_CEK;
            }
            if (tutar > tekSeferLimit)
            {
                Console.WriteLine("\nHATA: Tek seferlik limiti astiniz!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto PARA_CEK;
            }
            if (tutar > bakiye[kartIndex])
            {
                Console.WriteLine("\nHATA: Yetersiz bakiye!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto PARA_CEK;
            }
            if (gunlukCekilen[kartIndex] + tutar > gunlukLimit)
            {
                Console.WriteLine("\nHATA: Gunluk cekim limitini asiyorsunuz!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto PARA_CEK;
            }
            bakiye[kartIndex] = bakiye[kartIndex] - tutar;
            gunlukCekilen[kartIndex] = gunlukCekilen[kartIndex] + tutar;
            gecmisKisi = kartIndex;
            gecmisYaz = "Para cekme   : -" + tutar + " TL";
            donus = 1;
            goto GECMIS_EKLE;
        CEKIM_SONRA:
            Console.WriteLine("\nParaniz hazirlaniyor... Paranizi alabilirsiniz: " + tutar + " TL");
            Console.WriteLine("Yeni bakiye: " + bakiye[kartIndex] + " TL");
            Console.WriteLine("Ana menuye donmek icin Enter'a basin...");
            Console.ReadLine();
            goto MENU;
            // 3 - PARA YATIR
        PARA_YATIR:
            Console.Clear();
            Console.WriteLine("---------------- PARA YATIRMA ----------------");
            Console.WriteLine("Bakiyeniz: " + bakiye[kartIndex] + " TL");
            Console.WriteLine("Tutar 10'un kati olmali. Iptal icin 0 yazin.");
            Console.Write("Yatirilacak tutar: ");
            girdi = Console.ReadLine();
            sayiMi = int.TryParse(girdi, out tutar);
            if (sayiMi == false)
            {
                Console.WriteLine("\nHATA: Lutfen sadece sayi girin!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto PARA_YATIR;
            }
            if (tutar == 0) goto MENU;
            if (tutar < 0)
            {
                Console.WriteLine("\nHATA: Negatif tutar girilemez!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto PARA_YATIR;
            }
            if (tutar % 10 != 0)
            {
                Console.WriteLine("\nHATA: Tutar 10'un kati olmali!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto PARA_YATIR;
            }
            if (tutar > 50000)
            {
                Console.WriteLine("\nHATA: Tek seferde en fazla 50000 TL yatirilabilir!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto PARA_YATIR;
            }
            Console.WriteLine("");
            Console.Write(tutar + " TL yatirilacak. Onayliyor musunuz? (E/H): ");
            evetHayir = Console.ReadLine();
            if (evetHayir == "E") goto YATIR_ONAY;
            if (evetHayir == "e") goto YATIR_ONAY;
            Console.WriteLine("Islem iptal edildi. Enter'a basin...");
            Console.ReadLine();
            goto MENU;
        YATIR_ONAY:
            bakiye[kartIndex] = bakiye[kartIndex] + tutar;
            gecmisKisi = kartIndex;
            gecmisYaz = "Para yatirma : +" + tutar + " TL";
            donus = 2;
            goto GECMIS_EKLE;
        YATIR_SONRA:
            Console.WriteLine("\nParaniz hesabiniza yatirildi.");
            Console.WriteLine("Yeni bakiye: " + bakiye[kartIndex] + " TL");
            Console.WriteLine("Ana menuye donmek icin Enter'a basin...");
            Console.ReadLine();
            goto MENU;
            // 4 - HAVALE / TRANSFER
        TRANSFER:
            Console.Clear();
            Console.WriteLine("-------------- HAVALE / TRANSFER --------------");
            Console.WriteLine("Bakiyeniz: " + bakiye[kartIndex] + " TL");
            Console.WriteLine("Gonderilecek kisinin kart numarasini girin.");
            Console.WriteLine("Iptal icin 0 yazin.");
            Console.Write("Hedef Kart No: ");
            hedefKart = Console.ReadLine();
            if (hedefKart == null) goto TRANSFER;
            if (hedefKart == "0") goto MENU;
            if (hedefKart == kartNo[kartIndex])
            {
                Console.WriteLine("\nHATA: Kendinize para gonderemezsiniz!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto TRANSFER;
            }
            hedefIndex = -1;
            i = 0;
        HEDEF_ARA:
            if (i >= 3) goto HEDEF_ARA_BITTI;
            if (kartNo[i] == hedefKart)
            {
                hedefIndex = i;
                goto HEDEF_ARA_BITTI;
            }
            i = i + 1;
            goto HEDEF_ARA;
        HEDEF_ARA_BITTI:
            if (hedefIndex == -1)
            {
                Console.WriteLine("\nHATA: Hedef hesap bulunamadi!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto TRANSFER;
            }
            Console.WriteLine("\nAlici: " + isim[hedefIndex]);
            Console.Write("Gonderilecek tutar: ");
            girdi = Console.ReadLine();
            sayiMi = int.TryParse(girdi, out tutar);
            if (sayiMi == false)
            {
                Console.WriteLine("\nHATA: Lutfen sadece sayi girin!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto TRANSFER;
            }
            if (tutar <= 0)
            {
                Console.WriteLine("\nHATA: Tutar sifirdan buyuk olmali!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto TRANSFER;
            }
            if (tutar > bakiye[kartIndex])
            {
                Console.WriteLine("\nHATA: Yetersiz bakiye!");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto TRANSFER;
            }
            Console.WriteLine("");
            Console.Write(isim[hedefIndex] + " kisisine " + tutar + " TL gonderilsin mi? (E/H): ");
            evetHayir = Console.ReadLine();
            if (evetHayir == "E") goto TRANSFER_ONAY;
            if (evetHayir == "e") goto TRANSFER_ONAY;
            Console.WriteLine("Islem iptal edildi. Enter'a basin...");
            Console.ReadLine();
            goto MENU;
        TRANSFER_ONAY:
            bakiye[kartIndex] = bakiye[kartIndex] - tutar;
            bakiye[hedefIndex] = bakiye[hedefIndex] + tutar;
            gecmisKisi = kartIndex;
            gecmisYaz = "Giden havale : -" + tutar + " TL";
            donus = 3;
            goto GECMIS_EKLE;
        TRANSFER_SONRA_1:
            gecmisKisi = hedefIndex;
            gecmisYaz = "Gelen havale : +" + tutar + " TL";
            donus = 5;
            goto GECMIS_EKLE;
        TRANSFER_SONRA_2:
            Console.WriteLine("\nTransfer basarili!");
            Console.WriteLine("Yeni bakiyeniz: " + bakiye[kartIndex] + " TL");
            Console.WriteLine("Ana menuye donmek icin Enter'a basin...");
            Console.ReadLine();
            goto MENU;
            // 5 - SON ISLEMLER
        SON_ISLEMLER:
            Console.Clear();
            Console.WriteLine("---------------- SON ISLEMLER ----------------");
            if (gecmisSayisi[kartIndex] == 0)
            {
                Console.WriteLine("Henuz hic islem yapilmadi.");
                goto SON_ISLEMLER_BITTI;
            }
            yazdirSayac = 0;
        SON_ISLEMLER_DONGU:
            if (yazdirSayac >= gecmisSayisi[kartIndex]) goto SON_ISLEMLER_BITTI;
            Console.WriteLine((yazdirSayac + 1) + ") " + gecmis[kartIndex, yazdirSayac]);
            yazdirSayac = yazdirSayac + 1;
            goto SON_ISLEMLER_DONGU;
        SON_ISLEMLER_BITTI:
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Guncel bakiye: " + bakiye[kartIndex] + " TL");
            Console.WriteLine("Ana menuye donmek icin Enter'a basin...");
            Console.ReadLine();
            goto MENU;
            // 6 - PIN DEGISTIR
        PIN_DEGISTIR:
            Console.Clear();
            Console.WriteLine("---------------- PIN DEGISTIRME ----------------");
            hak = 3;
        ESKI_PIN_SOR:
            Console.Write("Mevcut PIN'inizi girin (iptal icin 0): ");
            eskiPin = Console.ReadLine();
            if (eskiPin == "0") goto MENU;
            if (eskiPin == pin[kartIndex]) goto YENI_PIN_SOR;
            hak = hak - 1;
            if (hak == 0)
            {
                bloke[kartIndex] = true;
                Console.WriteLine("\n3 kez hatali PIN! Kartiniz BLOKE EDILDI.");
                Console.WriteLine("Devam etmek icin Enter'a basin...");
                Console.ReadLine();
                goto KARTA_GIRIS;
            }
            Console.WriteLine("HATALI PIN! Kalan hak: " + hak);
            goto ESKI_PIN_SOR;
        YENI_PIN_SOR:
            Console.Write("Yeni PIN (4 haneli rakam): ");
            yeniPin = Console.ReadLine();
            if (yeniPin == null) goto YENI_PIN_SOR;
            if (yeniPin.Length != 4)
            {
                Console.WriteLine("HATA: PIN tam 4 haneli olmali!");
                goto YENI_PIN_SOR;
            }
            d = 0;
        RAKAM_KONTROL:
            if (d >= 4) goto RAKAM_KONTROL_BITTI;
            if (char.IsDigit(yeniPin[d]) == false)
            {
                Console.WriteLine("HATA: PIN sadece rakamlardan olusmali!");
                goto YENI_PIN_SOR;
            }
            d = d + 1;
            goto RAKAM_KONTROL;
        RAKAM_KONTROL_BITTI:
            if (yeniPin == pin[kartIndex])
            {
                Console.WriteLine("HATA: Yeni PIN eskisiyle ayni olamaz!");
                goto YENI_PIN_SOR;
            }
            Console.Write("Yeni PIN'i tekrar girin: ");
            yeniPin2 = Console.ReadLine();
            if (yeniPin != yeniPin2)
            {
                Console.WriteLine("HATA: PIN'ler birbiriyle uyusmuyor!");
                goto YENI_PIN_SOR;
            }
            pin[kartIndex] = yeniPin;
            gecmisKisi = kartIndex;
            gecmisYaz = "PIN degistirildi";
            donus = 4;
            goto GECMIS_EKLE;
        PIN_SONRA:
            Console.WriteLine("\nPIN'iniz basariyla degistirildi.");
            Console.WriteLine("Ana menuye donmek icin Enter'a basin...");
            Console.ReadLine();
            goto MENU;
            // 7 - KARTI IADE ET
        KARTI_IADE:
            Console.Clear();
            Console.WriteLine("==========================================");
            Console.WriteLine("  Iyi gunler dileriz, " + isim[kartIndex] + "!");
            Console.WriteLine("  Lutfen kartinizi almayi unutmayin.");
            Console.WriteLine("==========================================");
            Console.WriteLine("Enter'a basin...");
            Console.ReadLine();
            kartIndex = -1;
            goto KARTA_GIRIS;
            // ORTAK ISLEM: GECMISE KAYIT EKLE
            //  gecmisKisi -> kimin gecmisine yazilacak
            //  gecmisYaz  -> yazilacak metin
            //  donus      -> islem bitince nereye donulecek
        GECMIS_EKLE:
            if (gecmisSayisi[gecmisKisi] < 5)
            {
                gecmis[gecmisKisi, gecmisSayisi[gecmisKisi]] = gecmisYaz;
                gecmisSayisi[gecmisKisi] = gecmisSayisi[gecmisKisi] + 1;
                goto GECMIS_BITTI;
            }
            j = 0;
        KAYDIR:
            if (j >= 4) goto KAYDIR_BITTI;
            gecmis[gecmisKisi, j] = gecmis[gecmisKisi, j + 1];
            j = j + 1;
            goto KAYDIR;
        KAYDIR_BITTI:
            gecmis[gecmisKisi, 4] = gecmisYaz;
        GECMIS_BITTI:
            if (donus == 1) goto CEKIM_SONRA;
            if (donus == 2) goto YATIR_SONRA;
            if (donus == 3) goto TRANSFER_SONRA_1;
            if (donus == 4) goto PIN_SONRA;
            if (donus == 5) goto TRANSFER_SONRA_2;
            goto MENU;
            // PROGRAMIN SONU
        PROGRAM_BITIS:
            Console.WriteLine("\nATM kapatiliyor. Gule gule!");
            Console.WriteLine("Cikmak icin bir tusa basin...");
            Console.ReadKey();
        }
    }
}
// TEST KARTLARI: 1111222233334444 / 1234 , 5555666677778888 / 4321 , 9999000011112222 / 1111
// NOT: Kartsiz giris yoktur. 3 yanlis PIN girilirse kart bloke olur.
