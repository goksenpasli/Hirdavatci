using Extensions;
using Microsoft.Win32;
using SharpCompress.Common;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using ZXing;

namespace Hirdavatci
{
    public static class ExtensionMethods
    {
        public static readonly string xmldatapath = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath) + @"\Data.xml";

        public static void Compress(this CompressorViewModel compressorViewModel)
        {
            using FileStream stream = File.OpenWrite(compressorViewModel.CompressorView.KayıtYolu);
            using ZipWriter writer = new(stream, new ZipWriterOptions(CompressionType.Deflate) { UseZip64 = true, DeflateCompressionLevel = (SharpCompress.Compressors.Deflate.CompressionLevel)compressorViewModel.CompressorView.SıkıştırmaDerecesi });
            foreach (string dosya in compressorViewModel.CompressorView.Dosyalar)
            {
                writer.Write(Path.GetFileName(dosya), dosya);
            }
        }

        public static IEnumerable<Malzeme> ExceldenVeriAl(this string dosyayolu)
        {
            try
            {
                string[] satırlar = File.ReadAllLines(dosyayolu, Encoding.Default);
                CultureInfo culture = new(CultureInfo.CurrentCulture.Name);
                return satırlar.Skip(1).Select(satır =>
                {
                    string[] dc = satır.Split(culture.TextInfo.ListSeparator.ToCharArray());
                    return new Malzeme
                    {
                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        ToplamAdet = Convert.ToInt32(dc[3]),
                        BirimFiyat = Convert.ToDouble(dc[2]),
                        Aciklama = dc[0],
                        Barkod = dc[1],
                        KalanAdet = Convert.ToInt32(dc[4]),
                    };
                });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static BitmapImage GenerateBarCodeImage(this Malzeme malzeme, BarcodeFormat format = BarcodeFormat.QR_CODE)
        {
            try
            {
                BarcodeWriter writer = new()
                {
                    Format = format,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Height = (int)Properties.Settings.Default.QrHeight,
                        Width = (int)Properties.Settings.Default.QrWidth,
                        Margin = 0
                    }
                };
                if (!string.IsNullOrWhiteSpace(malzeme.Barkod))
                {
                    using System.Drawing.Bitmap image = writer.Write(malzeme.Barkod);
                    return image.ToBitmapImage(ImageFormat.Png);
                }
                malzeme.BarkodError = "";
                return null;
            }
            catch (Exception ex)
            {
                malzeme.BarkodError = ex.Message;
                return null;
            }
        }

        public static ObservableCollection<Malzeme> MalzemeleriYükle()
        {
            if (File.Exists(xmldatapath))
            {
                XmlSerializer serializer = new(typeof(Malzemeler));
                using StreamReader reader = new(xmldatapath);
                Malzemeler malzemeler = (Malzemeler)serializer.Deserialize(reader);
                return malzemeler.Malzeme;
            }
            _ = Directory.CreateDirectory(Path.GetDirectoryName(xmldatapath));
            return new ObservableCollection<Malzeme>();
        }

        public static void ResimEkle(Malzeme dc)
        {
            OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(xmldatapath)}\\{filename}");
                dc.ResimYolu = filename;
            }
        }

        public static void Serialize<T>(this T dataToSerialize) where T : class
        {
            XmlSerializer serializer = new(typeof(T));
            using TextWriter stream = new StreamWriter(xmldatapath);
            serializer.Serialize(stream, dataToSerialize);
        }
    }
}