using Extensions;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

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
                        Barkod = dc.Barkod
                    };
                    Malzemeler.Malzeme.Add(malzeme);
                    XmlSerializer serializer = new(typeof(Malzemeler));
                    using TextWriter writer = new StreamWriter(ExtensionMethods.xmldatapath);
                    serializer.Serialize(writer, Malzemeler);
                }
            }, parameter => !string.IsNullOrWhiteSpace(Malzeme.Aciklama) && !string.IsNullOrWhiteSpace(Malzeme.Barkod) && Malzeme.BirimFiyat > 0 && Malzeme.ToplamAdet > 0);

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
                    if (Satis.SatisAdet > dc.ToplamAdet)
                    {
                        MessageBox.Show("Satış Adeti Toplam Adetten Fazla Olmaz.", "HIRDAVATÇI", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

                    dc.Satislar.Add(satis);
                    XmlSerializer serializer = new(typeof(Malzemeler));
                    using TextWriter writer = new StreamWriter(ExtensionMethods.xmldatapath);
                    serializer.Serialize(writer, Malzemeler);
                }
            }, parameter => parameter is Malzeme malzeme && !string.IsNullOrWhiteSpace(Satis.SatinAlanKisi) && Satis.SatisAdet > 0 && Satis.SatisFiyat > 0);

            Malzeme.PropertyChanged += Malzeme_PropertyChanged;
        }

        public ICommand DepoyaEkle { get; }

        public ICommand KareKodYazdır { get; }

        public Malzeme Malzeme { get; set; }

        public Malzemeler Malzemeler { get; set; }

        public ICommand SatışKaydıEkle { get; }

        public Satis Satis { get; set; }

        private void Malzeme_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "Barkod" or "BirimFiyat" or "Aciklama")
            {
                Malzeme.BarkodImage = $"{Malzeme.Aciklama}\n{Malzeme.Barkod}\nFiyat:{Malzeme.BirimFiyat} TL".GenerateBarCodeImage();
            }

            if (e.PropertyName == "BarKodAramaMetni")
            {
                CollectionViewSource.GetDefaultView(Malzemeler.Malzeme).Filter = item => item is Malzeme malzeme && malzeme.Barkod.Contains(Malzeme.BarKodAramaMetni);
            }
        }
    }
}