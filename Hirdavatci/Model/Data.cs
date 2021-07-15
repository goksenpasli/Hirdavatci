﻿using Extensions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using ZXing;

namespace Hirdavatci
{
    [XmlRoot(ElementName = "Iadeler")]
    public class Iadeler : InpcBase
    {
        private string ıadeAciklama;

        private int ıadeMiktari;

        private DateTime ıadeTarihi;

        [XmlAttribute(AttributeName = "IadeAciklama")]
        public string IadeAciklama
        {
            get => ıadeAciklama;

            set
            {
                if (ıadeAciklama != value)
                {
                    ıadeAciklama = value;
                    OnPropertyChanged(nameof(IadeAciklama));
                }
            }
        }

        [XmlAttribute(AttributeName = "IadeMiktari")]
        public int IadeMiktari
        {
            get => ıadeMiktari;

            set
            {
                if (ıadeMiktari != value)
                {
                    ıadeMiktari = value;
                    OnPropertyChanged(nameof(IadeMiktari));
                }
            }
        }

        [XmlAttribute(AttributeName = "IadeTarihi")]
        public DateTime IadeTarihi
        {
            get => ıadeTarihi;

            set
            {
                if (ıadeTarihi != value)
                {
                    ıadeTarihi = value;
                    OnPropertyChanged(nameof(IadeTarihi));
                }
            }
        }
    }

    [XmlRoot(ElementName = "Malzeme")]
    public class Malzeme : InpcBase
    {
        private string aciklama;

        private BarcodeFormat barcodeFormat = BarcodeFormat.QR_CODE;

        private string barkod;

        private string barKodAra;

        private BitmapImage barkodImage;

        private double birimFiyat;

        private int eklenenMalzemeAdeti;

        private string fiyatAramaMetni;

        private int ıd;

        private int kalanAdet;

        private bool resimleriYedekle;

        private string resimYolu;

        private ObservableCollection<Satis> satislar = new();

        private Malzeme seçiliMalzeme;

        private int toplamAdet;

        [XmlAttribute(AttributeName = "Aciklama")]
        public string Aciklama
        {
            get => aciklama;

            set
            {
                if (aciklama != value)
                {
                    aciklama = value;
                    OnPropertyChanged(nameof(Aciklama));
                }
            }
        }

        [XmlIgnore]
        public BarcodeFormat BarcodeFormat
        {
            get => barcodeFormat;

            set
            {
                if (barcodeFormat != value)
                {
                    barcodeFormat = value;
                    OnPropertyChanged(nameof(BarcodeFormat));
                }
            }
        }

        [XmlAttribute(AttributeName = "Barkod")]
        public string Barkod
        {
            get => barkod;

            set
            {
                if (barkod != value)
                {
                    barkod = value;
                    OnPropertyChanged(nameof(Barkod));
                }
            }
        }

        [XmlIgnore]
        public string BarKodAramaMetni
        {
            get => barKodAra;

            set
            {
                if (barKodAra != value)
                {
                    barKodAra = value;
                    OnPropertyChanged(nameof(BarKodAramaMetni));
                }
            }
        }

        [XmlIgnore]
        public BitmapImage BarkodImage
        {
            get => barkodImage;

            set
            {
                if (barkodImage != value)
                {
                    barkodImage = value;
                    OnPropertyChanged(nameof(BarkodImage));
                }
            }
        }

        [XmlAttribute(AttributeName = "BirimFiyat")]
        public double BirimFiyat
        {
            get => birimFiyat;

            set
            {
                if (birimFiyat != value)
                {
                    birimFiyat = value;
                    OnPropertyChanged(nameof(BirimFiyat));
                }
            }
        }

        [XmlIgnore]
        public int EklenenMalzemeAdeti
        {
            get => eklenenMalzemeAdeti;

            set
            {
                if (eklenenMalzemeAdeti != value)
                {
                    eklenenMalzemeAdeti = value;
                    OnPropertyChanged(nameof(EklenenMalzemeAdeti));
                }
            }
        }

        [XmlIgnore]
        public string FiyatAramaMetni
        {
            get => fiyatAramaMetni;

            set
            {
                if (fiyatAramaMetni != value)
                {
                    fiyatAramaMetni = value;
                    OnPropertyChanged(nameof(FiyatAramaMetni));
                }
            }
        }

        [XmlAttribute(AttributeName = "Id")]
        public int Id
        {
            get => ıd;

            set
            {
                if (ıd != value)
                {
                    ıd = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        [XmlAttribute(AttributeName = "KalanAdet")]
        public int KalanAdet
        {
            get => kalanAdet;

            set
            {
                if (kalanAdet != value)
                {
                    kalanAdet = value;
                    OnPropertyChanged(nameof(KalanAdet));
                }
            }
        }

        [XmlIgnore]
        public bool ResimleriYedekle
        {
            get => resimleriYedekle;

            set
            {
                if (resimleriYedekle != value)
                {
                    resimleriYedekle = value;
                    OnPropertyChanged(nameof(ResimleriYedekle));
                }
            }
        }

        [XmlAttribute(AttributeName = "ResimYolu")]
        public string ResimYolu
        {
            get => resimYolu;

            set
            {
                if (resimYolu != value)
                {
                    resimYolu = value;
                    OnPropertyChanged(nameof(ResimYolu));
                }
            }
        }

        [XmlElement(ElementName = "Satislar")]
        public ObservableCollection<Satis> Satislar
        {
            get => satislar;

            set
            {
                if (satislar != value)
                {
                    satislar = value;
                    OnPropertyChanged(nameof(Satislar));
                }
            }
        }

        [XmlIgnore]
        public Malzeme SeçiliMalzeme
        {
            get => seçiliMalzeme;

            set
            {
                if (seçiliMalzeme != value)
                {
                    seçiliMalzeme = value;
                    OnPropertyChanged(nameof(SeçiliMalzeme));
                }
            }
        }

        [XmlAttribute(AttributeName = "ToplamAdet")]
        public int ToplamAdet
        {
            get => toplamAdet;

            set
            {
                if (toplamAdet != value)
                {
                    toplamAdet = value;
                    OnPropertyChanged(nameof(ToplamAdet));
                }
            }
        }
    }

    [XmlRoot(ElementName = "Malzemeler")]
    public class Malzemeler : InpcBase
    {
        private ObservableCollection<Malzeme> malzeme;

        [XmlElement(ElementName = "Malzeme")]
        public ObservableCollection<Malzeme> Malzeme
        {
            get => malzeme;

            set
            {
                if (malzeme != value)
                {
                    malzeme = value;
                    OnPropertyChanged(nameof(Malzeme));
                }
            }
        }
    }

    [XmlRoot(ElementName = "Satis")]
    public class Satis : InpcBase
    {
        private string aciklama;

        private bool ıadeEdildiMi;

        private ObservableCollection<Iadeler> ıadeler = new();

        private int ıadeMiktari;

        private int ıd;

        private string satinAlanKisi;

        private int satisAdet;

        private double satisFiyat;

        private DateTime tarih = DateTime.Today;

        private string telefon;

        private double toplamGelir;

        [XmlIgnore]
        public string Aciklama
        {
            get => aciklama;

            set
            {
                if (aciklama != value)
                {
                    aciklama = value;
                    OnPropertyChanged(nameof(Aciklama));
                }
            }
        }

        [XmlAttribute(AttributeName = "IadeEdildiMi")]
        public bool IadeEdildiMi
        {
            get => ıadeEdildiMi;

            set
            {
                if (ıadeEdildiMi != value)
                {
                    ıadeEdildiMi = value;
                    OnPropertyChanged(nameof(IadeEdildiMi));
                }
            }
        }

        [XmlElement(ElementName = "Iadeler")]
        public ObservableCollection<Iadeler> Iadeler
        {
            get => ıadeler;

            set
            {
                if (ıadeler != value)
                {
                    ıadeler = value;
                    OnPropertyChanged(nameof(Iadeler));
                }
            }
        }

        [XmlIgnore]
        public int IadeMiktari
        {
            get => ıadeMiktari;

            set
            {
                if (ıadeMiktari != value)
                {
                    ıadeMiktari = value;
                    OnPropertyChanged(nameof(IadeMiktari));
                }
            }
        }

        [XmlAttribute(AttributeName = "Id")]
        public int Id
        {
            get => ıd;

            set
            {
                if (ıd != value)
                {
                    ıd = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        [XmlAttribute(AttributeName = "SatinAlanKisi")]
        public string SatinAlanKisi
        {
            get => satinAlanKisi;

            set
            {
                if (satinAlanKisi != value)
                {
                    satinAlanKisi = value;
                    OnPropertyChanged(nameof(SatinAlanKisi));
                }
            }
        }

        [XmlAttribute(AttributeName = "SatisAdet")]
        public int SatisAdet
        {
            get => satisAdet;

            set
            {
                if (satisAdet != value)
                {
                    satisAdet = value;
                    OnPropertyChanged(nameof(SatisAdet));
                }
            }
        }

        [XmlAttribute(AttributeName = "SatisFiyat")]
        public double SatisFiyat
        {
            get => satisFiyat;

            set
            {
                if (satisFiyat != value)
                {
                    satisFiyat = value;
                    OnPropertyChanged(nameof(SatisFiyat));
                }
            }
        }

        [XmlAttribute(AttributeName = "Tarih")]
        public DateTime Tarih
        {
            get => tarih;

            set
            {
                if (tarih != value)
                {
                    tarih = value;
                    OnPropertyChanged(nameof(Tarih));
                }
            }
        }

        [XmlAttribute(AttributeName = "Telefon")]
        public string Telefon
        {
            get => telefon;

            set
            {
                if (telefon != value)
                {
                    telefon = value;
                    OnPropertyChanged(nameof(Telefon));
                }
            }
        }

        [XmlAttribute(AttributeName = "ToplamGelir")]
        public double ToplamGelir
        {
            get
            {
                toplamGelir = SatisFiyat * SatisAdet;
                return toplamGelir;
            }

            set
            {
                if (toplamGelir != value)
                {
                    toplamGelir = value;
                    OnPropertyChanged(nameof(ToplamGelir));
                }
            }
        }
    }

    [XmlRoot(ElementName = "Satislar")]
    public class Satislar : InpcBase
    {
        private Satis satis;

        [XmlElement(ElementName = "Satis")]
        public Satis Satis
        {
            get => satis;

            set
            {
                if (satis != value)
                {
                    satis = value;
                    OnPropertyChanged(nameof(Satis));
                }
            }
        }
    }
}