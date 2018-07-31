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
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Colorspace;

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
            var timesitalic = PdfFontFactory.CreateFont(StandardFonts.TIMES_ITALIC);
            var helvatica = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var italic = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

            //photo import
            var photo = new Image(ImageDataFactory.Create(PHOTO));
            photo.SetHeight(90);
            photo.SetFixedPosition(450, 650);

            //reading from the file
            string[] line = new string[19];
            using (var reader = new StreamReader("D:/data.txt"))
            {
                for (int i = 0; i < line.Length; i++)
                {
                    line[i] = reader.ReadLine();
                }
            }

            SolidLine solidline = new SolidLine(2f);
            LineSeparator separator = new LineSeparator(solidline);

            document.Add(new Paragraph(line[0]).SetFont(timesroman).SetFontSize(20));
            document.Add(new Paragraph("Cirraculum Vitae").SetFontSize(9).SetFont(italic));

            var person = new Table(new float[] { 1, 2 });

            person.AddHeaderCell(new Cell().Add(new Paragraph("Birth date:")).SetFont(bold).SetFontSize(10).SetBorder(Border.NO_BORDER));
            person.AddHeaderCell(new Cell().Add(new Paragraph(line[1])).SetFontSize(10).SetBorder(Border.NO_BORDER));
            person.AddCell(new Cell().Add(new Paragraph("Phone:")).SetFontSize(10).SetFont(bold).SetBorder(Border.NO_BORDER));
            person.AddCell(new Cell().Add(new Paragraph(line[2])).SetFontSize(10).SetBorder(Border.NO_BORDER));
            person.AddCell(new Cell().Add(new Paragraph("E-mail:")).SetFontSize(10).SetFont(bold).SetBorder(Border.NO_BORDER));
            person.AddCell(new Cell().Add(new Paragraph(line[3])).SetFontSize(10).SetBorder(Border.NO_BORDER));

            Paragraph worktype1 = new Paragraph(line[4]).SetFontSize(13).SetFont(timesroman);
            Paragraph workplace1 = new Paragraph(line[6]).SetFontSize(9).SetFont(timesitalic).SetFontColor(DeviceRgb.BLUE);

            Paragraph worktype2 = new Paragraph(line[7]).SetFontSize(13).SetFont(timesroman);
            Paragraph workplace2 = new Paragraph(line[9]).SetFontSize(9).SetFont(timesitalic).SetFontColor(DeviceRgb.BLUE);

            Paragraph worktype3 = new Paragraph(line[10]).SetFontSize(13).SetFont(timesroman);
            Paragraph workplace3 = new Paragraph(line[12]).SetFontSize(9).SetFont(timesitalic).SetFontColor(DeviceRgb.BLUE);

            var experience = new Table(new float[] { 2, 5, 2 });
            experience.AddHeaderCell(new Cell().Add(new Paragraph("Experience:")).SetFont(timesroman).SetFontSize(15).SetBorder(Border.NO_BORDER).SetWidth(100));
            experience.AddHeaderCell(new Cell().Add(worktype1).Add(workplace1).SetWidth(250).SetBorder(Border.NO_BORDER));
            experience.AddHeaderCell(new Cell().Add(new Paragraph(line[5])).SetFont(timesroman).SetFontSize(11).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetWidth(100).SetBorder(Border.NO_BORDER));

            experience.AddCell(new Cell().Add(new Paragraph("")).SetFont(timesroman).SetFontSize(15).SetBorder(Border.NO_BORDER).SetWidth(100));
            experience.AddCell(new Cell().Add(worktype2).Add(workplace2).SetWidth(250).SetBorder(Border.NO_BORDER));
            experience.AddCell(new Cell().Add(new Paragraph(line[8])).SetFont(timesroman).SetFontSize(11).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetWidth(100).SetBorder(Border.NO_BORDER));

            experience.AddCell(new Cell().Add(new Paragraph("")).SetFont(timesroman).SetFontSize(15).SetBorder(Border.NO_BORDER).SetWidth(100));
            experience.AddCell(new Cell().Add(worktype3).Add(workplace3).SetWidth(250).SetBorder(Border.NO_BORDER));
            experience.AddCell(new Cell().Add(new Paragraph(line[11])).SetFont(timesroman).SetFontSize(11).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetWidth(100).SetBorder(Border.NO_BORDER));

            var education = new Table(new float[] { 2, 5, 2 });
            education.AddHeaderCell(new Cell().Add(new Paragraph("Education:")).SetFont(timesroman).SetFontSize(15).SetBorder(Border.NO_BORDER).SetWidth(100));
            education.AddHeaderCell(new Cell().Add(new Paragraph(line[13] + "\n" + line[15])).SetFont(timesroman).SetFontSize(14).SetWidth(250).SetBorder(Border.NO_BORDER));
            education.AddHeaderCell(new Cell().Add(new Paragraph(line[14])).SetFont(timesroman).SetFontSize(12).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetWidth(100).SetBorder(Border.NO_BORDER));

            document.Add(person);
            document.Add(new Paragraph("\n"));
            document.Add(separator);
            document.Add(experience);
            document.Add(separator);
            document.Add(education);

            document.Add(photo);
            document.Close();
        }
    }
}