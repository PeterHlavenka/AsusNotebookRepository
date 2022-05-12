#region

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.Pdf;
using Syncfusion.Windows.Forms.PdfViewer;
using Syncfusion.XlsIO;

#endregion

namespace ExcelToPdfExporter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var excelEngine = new ExcelEngine())
            {
                var application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                var workbook = application.Workbooks.Open(@"c:\Users\phlavenka\Documents\Průběhová analýza.xlsx", ExcelOpenType.Automatic);


                //Tuning chart image quality


                //ExportSettings exportSettings = m_analysis.Settings.ExportSettings;
                // var pdfSettings = exportSettings.PdfExportSettings;

                //Open the Excel document to Convert
                var converter = new ExcelToPdfConverter(workbook);

                //Initialize PDF document
                var pdfDocument = new PdfDocument();
                //pdfDocument.PageSettings.Size = new SizeF(5000, 1000);//PdfPageUtils.ToSize(pdfSettings.PageSize);   // toto mi nijak nepomuze 

                //Convert Excel document into PDF document
                var excelToPdfConverterSettings = new ExcelToPdfConverterSettings();
                excelToPdfConverterSettings.LayoutOptions = LayoutOptions.FitSheetOnOnePage;
                excelToPdfConverterSettings.CustomPaperSize = new SizeF(50, 50); // pro pdf 30,30// se poseru toto mi dela jen scaling !!!


                pdfDocument = converter.Convert(excelToPdfConverterSettings);
                // pdfDocument.PageSettings.Orientation = PdfPageOrientation.Landscape;
                var graf = pdfDocument.Pages[0];
                // graf.Section.PageSettings.Orientation = PdfPageOrientation.Landscape;
                // graf.Section.PageSettings.Size =  new SizeF(500, 900);


                //Save the PDF file
                // nastaveni velikosti infolistu : 
                var neco = pdfDocument.Pages[pdfDocument.Pages.Count - 1];
                var defaultSize = neco.Section.PageSettings.Size;
                // neco.Section.PageSettings.Size =  new SizeF(1000, defaultSize.Width);
                pdfDocument.Save("ExcelToPDF.pdf");


                // return;
                // konec pdf
            }


            var viewer = new PdfViewerControl();

            //viewer.Load("../../Data/HTTP Succinctly.pdf");

            Metafile[] images;


            viewer.Load("ExcelToPDF.pdf");

            images = viewer.ExportAsMetafile(0, viewer.PageCount - 1);


            foreach (var image in images)

            {
                //Create new MemoryStream

                var metafileStream = new MemoryStream();

                var OffScreenDC = Graphics.FromHwndInternal(IntPtr.Zero);

                var wrappedMetaFile = new Metafile(metafileStream, OffScreenDC.GetHdc(), EmfType.EmfOnly);


                //Draw image

                var g = Graphics.FromImage(wrappedMetaFile);

                g.DrawImage(image, 0, 0);

                g.Dispose();


                //Write to file

                var wfile = new FileStream("Output" + Guid.NewGuid() + ".emf", FileMode.Create);

                metafileStream.WriteTo(wfile);

                wfile.Close();

                OffScreenDC.ReleaseHdc();
            }
        }
    }
}