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
using iText.IO.Image;

namespace Tutorial.Chapter01
{
    public class C01E04_UnitedStates
    {
        public const String DEST = "D:/cv.pdf";
        public const String PHOTO = "D:/photo.jpg";

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

            //photo import
            var photo = new Image(ImageDataFactory.Create(PHOTO));
            photo.SetHeight(90);
            photo.SetFixedPosition(470, 650);

            //reading from the file
            string[] line = new string[5];

            using (var reader = new StreamReader("D:/data.txt"))
            {
                for (int i = 0; i < line.Length; i++)
                {
                    line[i] = reader.ReadLine();
                }
            }

            document.Add(new Paragraph(line[0]).SetFont(timesroman).SetFontSize(20));
            document.Add(new Paragraph("Cirraculum Vitae").SetFontSize(9).SetFont(italic));

            var person = new Table(new float[] { 1, 2 });

            person.AddHeaderCell(new Cell().Add(new Paragraph("Birth date:")).SetFont(bold).SetFontSize(10).SetBorder(Border.NO_BORDER));
            person.AddHeaderCell(new Cell().Add(new Paragraph(line[1])).SetFontSize(10).SetBorder(Border.NO_BORDER));
            person.AddCell(new Cell().Add(new Paragraph("Phone:")).SetFontSize(10).SetFont(bold).SetBorder(Border.NO_BORDER));
            person.AddCell(new Cell().Add(new Paragraph(line[2])).SetFontSize(10).SetBorder(Border.NO_BORDER));
            person.AddCell(new Cell().Add(new Paragraph("E-mail:")).SetFontSize(10).SetFont(bold).SetBorder(Border.NO_BORDER));
            person.AddCell(new Cell().Add(new Paragraph(line[3])).SetFontSize(10).SetBorder(Border.NO_BORDER));

            var experience = new Table(new float[] { 2, 5, 2 });

            document.Add(person);
            document.Add(experience);
            document.Add(photo);
            document.Close();
        }
    }
}