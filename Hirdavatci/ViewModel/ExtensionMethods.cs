﻿using Extensions;
using SharpCompress.Common;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
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

        public static BitmapImage GenerateBarCodeImage(this string Barkod, BarcodeFormat format = BarcodeFormat.QR_CODE)
        {
            BarcodeWriter writer = new()
            {
                Format = format,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 175,
                    Width = 175,
                    Margin = 0
                }
            };
            if (!string.IsNullOrWhiteSpace(Barkod))
            {
                using System.Drawing.Bitmap image = writer.Write(Barkod);
                return image.ToBitmapImage(ImageFormat.Png);
            }

            return null;
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

        public static void Serialize<T>(this T dataToSerialize) where T : class
        {
            XmlSerializer serializer = new(typeof(T));
            using TextWriter stream = new StreamWriter(xmldatapath);
            serializer.Serialize(stream, dataToSerialize);
        }
    }
}