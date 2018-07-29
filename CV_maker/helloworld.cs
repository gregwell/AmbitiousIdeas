using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Font.Constants;
using iText.Layout.Borders;

namespace Tutorial.Chapter01
{
    public class C01E04_UnitedStates
    {
        public const String DEST = "D:/cv.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C01E04_UnitedStates().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest)
        {
            //formal shit
            var writer = new PdfWriter(dest);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4);
            document.SetMargins(80, 80, 80, 80);

            //fonts
            var timesroman = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            var helvatica = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var italic = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

            //reading from the file
            string line;
            using (var reader = new StreamReader("D:/data.txt"))
            {
                line = reader.ReadLine();
            }

            document.Add(new Paragraph(line).SetFont(timesroman).SetFontSize(20));
            document.Add(new Paragraph("Cirraculum Vitae").SetFontSize(9).SetFont(italic));

            var table = new Table(new float[] { 1, 2 });

            table.AddHeaderCell(new Cell().Add(new Paragraph("Birth date:")).SetFont(bold).SetFontSize(10).SetBorder(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph("birthdate")).SetFontSize(10).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Phone:")).SetFontSize(10).SetFont(bold).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("phone")).SetFontSize(10).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("E-mail:")).SetFontSize(10).SetFont(bold).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("email")).SetFontSize(10).SetBorder(Border.NO_BORDER));

            document.Add(table);
            document.Close();
        }
    }
}