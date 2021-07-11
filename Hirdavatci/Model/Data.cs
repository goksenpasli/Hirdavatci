using Extensions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Hirdavatci
{
    [XmlRoot(ElementName = "Malzeme")]
    public class Malzeme : InpcBase
    {
        private string aciklama;

        private string barkod;

        private string barKodAra;

        private BitmapImage barkodImage;

        private double birimFiyat;

        private int ıd;

        private ObservableCollection<Satis> satislar = new();

        private int toplamAdet;
        private int kalanAdet;

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
        private int ıd;

        private string satinAlanKisi;

        private int satisAdet;

        private double satisFiyat;

        private DateTime tarih = DateTime.Today;
        private double toplamGelir;

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
    }

    [XmlRoot(ElementName = "Satislar")]
    public class Satislar : InpcBase
    {
        [XmlElement(ElementName = "Satis")]
        public Satis Satis { get; set; }
    }
}