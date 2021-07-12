using Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Hirdavatci
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Malzeme = new Malzeme();
            Satis = new Satis();
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
                        KalanAdet = dc.ToplamAdet
                    };
                    Malzemeler.Malzeme.Add(malzeme);
                    Malzemeler.Serialize();
                }
            }, parameter => !string.IsNullOrWhiteSpace(Malzeme.Aciklama) && !string.IsNullOrWhiteSpace(Malzeme.Barkod) && Malzeme.BirimFiyat > 0 && Malzeme.ToplamAdet > 0);

            DepoyuSil = new RelayCommand<object>(parameter =>
            {
                if (parameter is Malzeme dc && MessageBox.Show("Seçili malzemeyi silmek istiyor musun? Dikkat bu malzemeye ait satışlar da silinecektir.", "HIRDAVATÇI", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    Malzemeler.Malzeme.Remove(dc);
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
                        IadeMiktari = (dc[0] as Satis)?.SatisAdet ?? 0,
                        IadeAciklama = (dc[0] as Satis)?.IadeAciklama
                    };

                    (dc[0] as Satis)?.Iadeler.Add(ıadeler);
                    (dc[1] as Malzeme).KalanAdet += (dc[0] as Satis).SatisAdet;
                    Malzemeler.Serialize();
                }
            }, parameter => parameter is object[] dc && !string.IsNullOrWhiteSpace((dc[0] as Satis)?.IadeAciklama) && (dc[0] as Satis)?.Iadeler?.Any() == false);

            DepoyaYeniMalzemeEkle = new RelayCommand<object>(parameter =>
            {
                if (parameter is Malzeme dc)
                {
                    dc.KalanAdet += dc.EklenenMalzemeAdeti;
                    dc.ToplamAdet += dc.EklenenMalzemeAdeti;
                    Malzemeler.Serialize();
                }
            }, parameter => parameter is Malzeme dc && dc.EklenenMalzemeAdeti > 0);

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
                        Tarih = Satis.Tarih
                    };

                    dc.KalanAdet -= Satis.SatisAdet;
                    dc.Satislar.Add(satis);
                    Malzemeler.Serialize();
                }
            }, parameter => parameter is Malzeme malzeme && !string.IsNullOrWhiteSpace(Satis.SatinAlanKisi) && Satis.SatisAdet > 0 && Satis.SatisFiyat > 0);

            Malzeme.PropertyChanged += Malzeme_PropertyChanged;
        }

        public ICommand DepoyaEkle { get; }

        public ICommand DepoyuSil { get; }

        public ICommand KareKodYazdır { get; }

        public Malzeme Malzeme { get; set; }

        public Malzemeler Malzemeler { get; set; }

        public ICommand SatışKaydıEkle { get; }

        public ICommand DepoyaYeniMalzemeEkle { get; }

        public ICommand MalzemeIadeEt { get; }

        public Satis Satis { get; set; }

        private void Malzeme_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "Barkod" or "BarcodeFormat")
            {
                try
                {
                    Malzeme.BarkodImage = $"{Malzeme.Barkod}".GenerateBarCodeImage(Malzeme.BarcodeFormat);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(ex.Message, "HIRDAVATÇI", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

            if (e.PropertyName == "BarKodAramaMetni")
            {
                CollectionViewSource.GetDefaultView(Malzemeler.Malzeme).Filter = item => item is Malzeme malzeme && malzeme.Barkod.Contains(Malzeme.BarKodAramaMetni);
            }
        }
    }
}