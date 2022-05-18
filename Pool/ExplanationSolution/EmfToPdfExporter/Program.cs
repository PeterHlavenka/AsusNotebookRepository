using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace EmfToPdfExporter
{
    internal class Program
    {
        // public static void Main(string[] args)
        // {
//             //Create a PDF Document
//             PdfDocument doc = new PdfDocument();
// //Add pages to the document
//             PdfPage page = doc.Pages.Add();
// //Create PDF graphics for the page
//             PdfGraphics graphics = page.Graphics;
// //Create the layout format
//             PdfMetafileLayoutFormat format = new PdfMetafileLayoutFormat();
// //Split text and image between pages
//             format.SplitImages = true;
//             format.SplitTextLines = true;
// //Create a metafile instance
//             PdfMetafile metaChart = new PdfMetafile(@"d:\AsusNotebookRepository\Pool\ExplanationSolution\EmfToPdfExporter\bin\Debug\Průběhová analýza.emf");
// //Draw the metafile in the page
//             metaChart.Draw(page, PointF.Empty, format);
// //Save the document
//             doc.Save("EMFToPDF.pdf");
// //Close the document
//             doc.Close(true);
// //This will open the PDF file so, the result will be seen in default PDF viewer
//             Process.Start("EMFToPDF.pdf");
        // }
        
        // SYNCFUSION RESENI OD Gowthamraj Kumar 
        public static void Main(string[] args)
        {
            //Create a PDF Document
            var doc = new PdfDocument();
       
            //Create a metafile instance
            var metafile = new Metafile(@"..\..\Průběhová analýza.emf");
            metafile = ReCreateMetafile(metafile);
            doc.PageSettings.Size = new SizeF(metafile.Size);
            //Add pages to the document
            var page = doc.Pages.Add();
            //Create PDF graphics for the page
            var graphics = page.Graphics;

            PdfMetafile metaChart = new PdfMetafile(metafile);
            //Draw the metafile in the page
            graphics.DrawImage(metaChart, PointF.Empty);
            //Save the document
            doc.Save("../../EMFToPDF_Recreate.pdf");
            //Close the document
            doc.Close(true);
            
        }
        private static Metafile ReCreateMetafile(Metafile metafile)
        {
            Rectangle frameRect = new Rectangle(0, 0, metafile.Width, metafile.Height);
            Bitmap bitmap = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(bitmap);
            IntPtr ipHdc = graphics.GetHdc();
            MemoryStream stream = new MemoryStream();
            Metafile result = new Metafile(stream, ipHdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfOnly);
            graphics.Dispose();
            graphics = Graphics.FromImage(result);
            graphics.DrawImage(metafile, frameRect, frameRect, GraphicsUnit.Pixel);
            graphics.Dispose();
            stream.Dispose();
            return result;
        }

    }
}