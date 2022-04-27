using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;

namespace QR_Creator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap bitmap;
        public MainWindow()
        {
            InitializeComponent();

            TB_Organization.Text = "Дятловский лесхоз";
            TB_InventNumber.Text = "011545404";
            TB_SustemBlok.Text = "MB: AsRock H61NM, CPU: Intel I5-2300, DDR3 4Gb, SSD 480 Gb, 500wt";
            TB_Monitor.Text = "DVK 24S300CRT";
            TB_Mb_Kb.Text = "Logitech K120, Logitech M100";
            TB_Person.Text = "Веснянский О.П.";
            DP_Data_Exp.SelectedDate = DateTime.Now;
        }

        private void BTN_Create_QR_Code_Click(object sender, RoutedEventArgs e)
        {
            Device device = new Device();

            device.Organization = TB_Organization.Text;
            device.InventNumber = TB_InventNumber.Text;
            device.SustemBlok = TB_SustemBlok.Text;
            device.Monitor = TB_Monitor.Text;
            device.Mb_Kb = TB_Mb_Kb.Text;
            device.Person = TB_Person.Text;
            device.Date_Exp = DP_Data_Exp.SelectedDate.Value;

            string resultString = Organization.Text + device.Organization + "\n" +
                                   InventNumber.Text + device.InventNumber + "\n" +
                                   "Спецификация:\n" +
                                   device.SustemBlok + "\n" +
                                   device.Monitor + "\n" +
                                   device.Mb_Kb + "\n" +
                                   Person.Text + device.Person + "\n" +
                                   Date_exp.Text + device.Date_Exp;

            Im_QR_Code.Source = CreateCode(resultString, 1000, 1000);
        }

        public BitmapImage CreateCode(string text, int width, int heith)
        {
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = width,
                    Height = heith,
                    CharacterSet = "UTF-8"
                },
                Renderer = new BitmapRenderer()
            };

            bitmap = writer.Write(text);
            var photoFile = ToBitmapImage(bitmap);
            return photoFile;
        }

        public static BitmapImage ToBitmapImage(Bitmap bitmap) //конвертер
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                var photo = new BitmapImage();
                photo.BeginInit();
                photo.CacheOption = BitmapCacheOption.OnLoad;
                photo.StreamSource = stream;
                photo.CacheOption = BitmapCacheOption.OnLoad;
                photo.EndInit();
                photo.Freeze();
                return photo;
            }
        }

        private void BTN_Print_All_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(Im_QR_Code, "1"); // параметры - поле для печати и название файла
            }
        }

        private void BTN_Add_object_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "PNG|*.png";
            saveFile.FileName = "QRcode_" + TB_InventNumber.Text;
            if (saveFile.ShowDialog() == true)
            {
                if (bitmap != null)
                {
                    bitmap.Save(string.Concat(saveFile.FileName, ".png"), ImageFormat.Png);
                }
            }
        }
    }
}
