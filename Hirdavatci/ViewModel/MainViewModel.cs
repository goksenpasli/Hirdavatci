using Extensions;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Hirdavatci
{
    public class MainViewModel : InpcBase
    {
        private readonly CollectionViewSource CvsMalzemeler = (CollectionViewSource)Application.Current?.MainWindow?.TryFindResource("CvsMalzemeler");

        private readonly CollectionViewSource CvsSatış = (CollectionViewSource)Application.Current?.MainWindow?.TryFindResource("CvsSatış");

        public MainViewModel()
        {
            Malzeme = new Malzeme();
            Satis = new Satis();
            Taksit = new Taksit();
            Malzemeler = new Malzemeler
            {
                Malzeme = ExtensionMethods.MalzemeleriYükle()
            };

            DepoyaEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Malzeme dc)
                {
                    Malzeme malzeme = new()
                    {
                        Id = dc.Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        ToplamAdet = dc.ToplamAdet,
                        BirimFiyat = dc.BirimFiyat,
                        Aciklama = dc.Aciklama,
                        Barkod = dc.Barkod,
                        KalanAdet = dc.ToplamAdet,
                        ResimYolu = dc.ResimYolu,
                        MalzemeAlimTarihi = dc.MalzemeAlimTarihi
                    };
                    Malzemeler.Malzeme.Add(malzeme);
                    Malzemeler.Serialize();
                }
            }, parameter => !string.IsNullOrWhiteSpace(Malzeme.Aciklama) && !string.IsNullOrWhiteSpace(Malzeme.Barkod) && Malzeme.BirimFiyat > 0 && Malzeme.ToplamAdet > 0);

            MalzemeResimYükle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Malzeme dc)
                {
                    ExtensionMethods.ResimEkle(dc);
                }
            }, parameter => true);

            CsvDosyasınaYaz = new RelayCommand<object>(parameter =>
            {
                if (parameter is ObservableCollection<Malzeme> data)
                {
                    string dosyaismi = Path.GetTempPath() + Guid.NewGuid() + ".csv";
                    string seperator = new CultureInfo(CultureInfo.CurrentCulture.Name).TextInfo.ListSeparator;
                    File.AppendAllText(dosyaismi, $"MalzemeAdı{seperator}Karekod{seperator}Fiyat{seperator}Toplam Adet{seperator}Kalan Adet\n", Encoding.UTF8);
                    foreach (Malzeme item in data)
                    {
                        File.AppendAllText(dosyaismi, $"{item.Aciklama}{seperator}{item.Barkod}{seperator}{item.BirimFiyat}{seperator}{item.ToplamAdet}{seperator}{item.KalanAdet}\n", Encoding.UTF8);
                    }
                    _ = Process.Start(dosyaismi);
                }
            }, parameter => true);

            CsvDosyasındanVeriAl = new RelayCommand<object>(parameter =>
            {
                _ = MessageBox.Show("Malzeme Adı\nBarkod\nBirimFiyat\nToplamAdet\nKalanAdet\n\nSıralamasıyla Dosyada Başlık Satırlarını Silmeden Verileri İşleyin.\nMicrosoft Excel Virgülle Ayrılmış Değerler Dosyası (*.csv) Olarak Kaydedip Yükleyin.", "HIRDAVATÇI", MessageBoxButton.OK, MessageBoxImage.Information);
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "Csv Dosyası |*.csv"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    if (openFileDialog.FileName.ExceldenVeriAl() != null)
                    {
                        foreach (Malzeme item in openFileDialog.FileName.ExceldenVeriAl())
                        {
                            Malzemeler.Malzeme.Add(item);
                        }
                    }
                    Malzemeler.Serialize();
                }
            }, parameter => true);

            MalzemeResimGüncelle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Malzeme dc)
                {
                    ExtensionMethods.ResimEkle(dc);
                    Malzemeler.Serialize();
                }
            }, parameter => true);

            DepoyuSil = new RelayCommand<object>(parameter =>
            {
                if (parameter is Malzeme dc && MessageBox.Show("Seçili malzemeyi silmek istiyor musun? Dikkat bu malzemeye ait satışlar ve iadeler de silinecektir.", "HIRDAVATÇI", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    _ = Malzemeler.Malzeme.Remove(dc);
                    Malzemeler.Serialize();
                }
            }, parameter => true);

            MalzemeIadeEt = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] dc && MessageBox.Show("Seçili malzemeyi iade etmek istiyor musun?", "HIRDAVATÇI", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    Iadeler ıadeler = new()
                    {
                        IadeTarihi = DateTime.Today,
                        IadeMiktari = (dc[0] as Satis)?.IadeMiktari ?? 0,
                        IadeAciklama = (dc[0] as Satis)?.Aciklama
                    };
                    (dc[0] as Satis).IadeEdildiMi = true;
                    (dc[0] as Satis).SatisAdet -= (dc[0] as Satis).IadeMiktari;
                    (dc[0] as Satis)?.Iadeler.Add(ıadeler);
                    (dc[1] as Malzeme).KalanAdet += (dc[0] as Satis).IadeMiktari;
                    OnPropertyChanged(nameof(Malzeme));
                    Malzemeler.Serialize();
                }
            }, parameter => parameter is object[] dc && !string.IsNullOrWhiteSpace((dc[0] as Satis)?.Aciklama) && (dc[0] as Satis)?.IadeEdildiMi == false && (dc[0] as Satis)?.IadeMiktari > 0);

            DepoyaYeniMalzemeEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Malzeme dc)
                {
                    dc.KalanAdet += dc.EklenenMalzemeAdeti;
                    dc.ToplamAdet += dc.EklenenMalzemeAdeti;
                    OnPropertyChanged(nameof(Malzeme));
                    Malzemeler.Serialize();
                }
            }, parameter => true);

            KareKodYazdır = new RelayCommand<object>(parameter =>
            {
                if (parameter is Image dc)
                {
                    PrintDialog printDlg = new();
                    if (printDlg.ShowDialog() == true)
                    {
                        printDlg.PrintVisual(dc, "KareKod Yazdır.");
                    }
                }
            }, parameter => true);

            KareKodSakla = new RelayCommand<object>(parameter =>
            {
                if (parameter is BitmapImage bitmapimage)
                {
                    SaveFileDialog saveFileDialog = new()
                    {
                        Title = "SAKLA",
                        Filter = "Resim Dosyaları (*.png)|*.png",
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        byte[] bytes = bitmapimage.ToTiffJpegByteArray(Extensions.ExtensionMethods.Format.Png);
                        using FileStream imageFile = new(saveFileDialog.FileName, FileMode.Create);
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                        bytes = null;
                    }
                }
            }, parameter => true);

            SatışKaydıEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Malzeme dc)
                {
                    if (Satis.SatisAdet > dc.KalanAdet)
                    {
                        _ = MessageBox.Show("Satış Adeti Kalan Adetten Fazla Olamaz.", "HIRDAVATÇI", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    Satis satis = new()
                    {
                        Id = Satis.Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        SatinAlanKisi = Satis.SatinAlanKisi,
                        SatisAdet = Satis.SatisAdet,
                        SatisFiyat = Satis.SatisFiyat,
                        Tarih = Satis.Tarih,
                        Telefon = Satis.Telefon
                    };

                    if (Satis.SatisTipi == SatisTipi.TAKSİTLİ)
                    {
                        TaksitliSatışYap(satis, Taksit.TaksitSayisi, Taksit.ÖdemeAyı);
                    }

                    dc.KalanAdet -= Satis.SatisAdet;
                    dc.Satislar.Add(satis);
                    OnPropertyChanged(nameof(Malzeme));
                    Malzemeler.Serialize();
                }
            }, parameter => parameter is Malzeme malzeme && !string.IsNullOrWhiteSpace(Satis.SatinAlanKisi) && Satis.SatisAdet > 0 && Satis.SatisFiyat > 0);

            DosyaÇalıştır = new RelayCommand<object>(parameter =>
            {
                if (parameter is string filename)
                {
                    _ = Process.Start($"{Path.GetDirectoryName(ExtensionMethods.xmldatapath)}\\{filename}");
                }
            }, parameter => true);

            TaksitKapat = new RelayCommand<object>(parameter =>
            {
                if (parameter is Taksit taksit)
                {
                    if (Math.Round(taksit.OdenenTutar, 2) >= taksit.TaksitTutar)
                    {
                        taksit.TaksitBitti = true;
                        taksit.TaksitOdenmeTarihi = DateTime.Today;
                        Malzemeler.Serialize();
                    }
                    else
                    {
                        _ = MessageBox.Show("Ödenecek taksit tutarı, taksitin ana tutarından az olamaz.", "HIRDAVATÇI", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }, parameter => true);

            AyarKaydet = new RelayCommand<object>(parameter => Properties.Settings.Default.Save(), parameter => true);

            VerileriYedekle = new RelayCommand<object>(parameter =>
            {
                SaveFileDialog saveFileDialog = new()
                {
                    Title = "SAKLA",
                    Filter = "Zip Dosyası (*.zip)|*.zip",
                };
                if (saveFileDialog.ShowDialog() == true && File.Exists(ExtensionMethods.xmldatapath))
                {
                    CompressorViewModel compressorViewModel = new();
                    compressorViewModel.CompressorView.Dosyalar = new();
                    compressorViewModel.CompressorView.KayıtYolu = saveFileDialog.FileName;
                    if (Malzeme.ResimleriYedekle)
                    {
                        compressorViewModel.CompressorView.SıkıştırmaDerecesi = 0;
                        foreach (string item in Directory.GetFiles(Path.GetDirectoryName(ExtensionMethods.xmldatapath)))
                        {
                            compressorViewModel.CompressorView.Dosyalar.Add(item);
                        }
                    }
                    else
                    {
                        compressorViewModel.CompressorView.Dosyalar.Add(ExtensionMethods.xmldatapath);
                    }
                    compressorViewModel.Compress();
                }
            }, parameter => true);

            Malzeme.PropertyChanged += Malzeme_PropertyChanged;
            Satis.PropertyChanged += Satis_PropertyChanged;
            Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;
        }

        public ICommand AyarKaydet { get; }

        public ICommand CsvDosyasınaYaz { get; }

        public ICommand CsvDosyasındanVeriAl { get; }

        public ICommand DepoyaEkle { get; }

        public ICommand DepoyaYeniMalzemeEkle { get; }

        public ICommand DepoyuSil { get; }

        public ICommand DosyaÇalıştır { get; }

        public ICommand KareKodSakla { get; }

        public ICommand KareKodYazdır { get; }

        public Malzeme Malzeme { get; set; }

        public ICommand MalzemeIadeEt { get; }

        public Malzemeler Malzemeler { get; set; }

        public ICommand MalzemeResimGüncelle { get; }

        public ICommand MalzemeResimYükle { get; }

        public ICommand SatışKaydıEkle { get; }

        public Satis Satis { get; set; }

        public Taksit Taksit { get; set; }

        public ICommand TaksitKapat { get; }

        public ICommand VerileriYedekle { get; }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "QrHeight" or "QrWidth")
            {
                Malzeme.BarkodImage = Malzeme.GenerateBarCodeImage(Malzeme.BarcodeFormat);
            }
        }

        private void Malzeme_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "Barkod" or "BarcodeFormat")
            {
                Malzeme.BarkodImage = Malzeme.GenerateBarCodeImage(Malzeme.BarcodeFormat);
            }

            if (e.PropertyName == "BarKodAramaMetni")
            {
                CvsMalzemeler.Filter += (s, e) => e.Accepted = e.Item is Malzeme malzeme && malzeme.Barkod.Contains(Malzeme.BarKodAramaMetni);
            }

            if (e.PropertyName == "FiyatAramaMetni")
            {
                CvsMalzemeler.Filter += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(Malzeme.FiyatAramaMetni))
                    {
                        e.Accepted = e.Item is Malzeme malzeme && malzeme.BirimFiyat.ToString() == Malzeme.FiyatAramaMetni;
                    }
                    else
                    {
                        CvsMalzemeler.View.Filter = null;
                    }
                };
            }

            if (e.PropertyName == "SeçiliMalzeme")
            {
                Satis.SatisFiyat = Malzeme.SeçiliMalzeme?.BirimFiyat ?? 0;
            }
        }

        private void Satis_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "KişiAramaMetni")
            {
                CvsSatış.Filter += (s, e) => e.Accepted = e.Item is Satis satis && satis.SatinAlanKisi.Contains(Satis.KişiAramaMetni);
            }
        }

        private void TaksitliSatışYap(Satis satis, int taksitsayısı, int ödemeAyı)
        {
            for (int i = 0; i < taksitsayısı; i++)
            {
                Taksit taksit = new()
                {
                    Id = Taksit.Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                    TaksitSira = i + 1,
                    TaksitBitti = false,
                    TaksitTutar = Math.Round(Satis.SatisAdet * Satis.SatisFiyat / taksitsayısı, 2),
                };
                taksit.Vade = Taksit.BaşlangıçVade.AddMonths(ödemeAyı * i);
                satis.Taksitler.Taksit.Add(taksit);
            }
        }
    }
}