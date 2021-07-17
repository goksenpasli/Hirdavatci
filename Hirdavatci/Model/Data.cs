using Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using ZXing;

namespace Hirdavatci
{
    public enum SatisTipi
    {
        PEŞİN = 0,

        TAKSİTLİ = 1
    }

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

        private string barkodError;

        private BitmapImage barkodImage;

        private double birimFiyat;

        private int eklenenMalzemeAdeti;

        private string fiyatAramaMetni;

        private int ıd;

        private int kalanAdet;

        private DateTime malzemeAlimTarihi = DateTime.Today;

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
        public string BarkodError
        {
            get => barkodError;

            set
            {
                if (barkodError != value)
                {
                    barkodError = value;
                    OnPropertyChanged(nameof(BarkodError));
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

        [XmlAttribute(AttributeName = "MalzemeAlimTarihi")]
        public DateTime MalzemeAlimTarihi
        {
            get => malzemeAlimTarihi;

            set
            {
                if (malzemeAlimTarihi != value)
                {
                    malzemeAlimTarihi = value;
                    OnPropertyChanged(nameof(MalzemeAlimTarihi));
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

        private int satisAdet = 1;

        private double satisFiyat;

        private SatisTipi satisTipi;

        private Taksitler taksitler = new();

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
                    OnPropertyChanged(nameof(ToplamGelir));
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
                    OnPropertyChanged(nameof(ToplamGelir));
                }
            }
        }

        [XmlIgnore]
        public SatisTipi SatisTipi
        {
            get { return satisTipi; }

            set
            {
                if (satisTipi != value)
                {
                    satisTipi = value;
                    OnPropertyChanged(nameof(SatisTipi));
                }
            }
        }

        [XmlElement(ElementName = "Taksitler")]
        public Taksitler Taksitler
        {
            get => taksitler;

            set
            {
                if (taksitler != value)
                {
                    taksitler = value;
                    OnPropertyChanged(nameof(Taksitler));
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

    [XmlRoot(ElementName = "Taksit")]
    public class Taksit : InpcBase
    {
        private DateTime başlangıçVade = DateTime.Today;

        private int ıd;

        private double odenenTutar;

        private int ödemeAyı = 1;

        private bool taksitBitti;

        private DateTime taksitOdenmeTarihi;

        private int taksitSayisi = 2;

        private int taksitSira;

        private double taksitTutar;

        private DateTime vade;

        [XmlIgnore]
        public DateTime BaşlangıçVade
        {
            get => başlangıçVade;

            set
            {
                if (başlangıçVade != value)
                {
                    başlangıçVade = value;
                    OnPropertyChanged(nameof(BaşlangıçVade));
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

        [XmlAttribute(AttributeName = "OdenenTutar")]
        public double OdenenTutar
        {
            get => odenenTutar;

            set
            {
                if (odenenTutar != value)
                {
                    odenenTutar = value;
                    OnPropertyChanged(nameof(OdenenTutar));
                }
            }
        }

        [XmlIgnore]
        public int ÖdemeAyı
        {
            get => ödemeAyı;

            set
            {
                if (ödemeAyı != value)
                {
                    ödemeAyı = value;
                    OnPropertyChanged(nameof(ÖdemeAyı));
                }
            }
        }

        [XmlIgnore]
        public IEnumerable<int> ÖdemeAyListesi { get; set; } = Enumerable.Range(1, 12);

        [XmlAttribute(AttributeName = "TaksitBitti")]
        public bool TaksitBitti
        {
            get => taksitBitti;

            set
            {
                if (taksitBitti != value)
                {
                    taksitBitti = value;
                    OnPropertyChanged(nameof(TaksitBitti));
                }
            }
        }

        [XmlAttribute(AttributeName = "TaksitOdenmeTarihi")]
        public DateTime TaksitOdenmeTarihi
        {
            get => taksitOdenmeTarihi;

            set
            {
                if (taksitOdenmeTarihi != value)
                {
                    taksitOdenmeTarihi = value;
                    OnPropertyChanged(nameof(TaksitOdenmeTarihi));
                }
            }
        }

        [XmlIgnore]
        public int TaksitSayisi
        {
            get => taksitSayisi;

            set
            {
                if (taksitSayisi != value)
                {
                    taksitSayisi = value;
                    OnPropertyChanged(nameof(TaksitSayisi));
                }
            }
        }

        [XmlAttribute(AttributeName = "TaksitSira")]
        public int TaksitSira
        {
            get => taksitSira;

            set
            {
                if (taksitSira != value)
                {
                    taksitSira = value;
                    OnPropertyChanged(nameof(TaksitSira));
                }
            }
        }

        [XmlAttribute(AttributeName = "TaksitTutar")]
        public double TaksitTutar
        {
            get => taksitTutar;

            set
            {
                if (taksitTutar != value)
                {
                    taksitTutar = value;
                    OnPropertyChanged(nameof(TaksitTutar));
                }
            }
        }

        [XmlAttribute(AttributeName = "Vade")]
        public DateTime Vade
        {
            get => vade;

            set
            {
                if (vade != value)
                {
                    vade = value;
                    OnPropertyChanged(nameof(Vade));
                }
            }
        }
    }

    [XmlRoot(ElementName = "Taksitler")]
    public class Taksitler : InpcBase
    {
        private ObservableCollection<Taksit> taksit = new();

        [XmlElement(ElementName = "Taksit")]
        public ObservableCollection<Taksit> Taksit
        {
            get => taksit;

            set
            {
                if (taksit != value)
                {
                    taksit = value;
                    OnPropertyChanged(nameof(Taksit));
                }
            }
        }
    }
}