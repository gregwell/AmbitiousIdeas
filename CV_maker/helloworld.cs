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
            string[] line = new string[20];
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

            Cell[] details = new Cell[3];
            Cell[] details_data = new Cell[3];

            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0: details[i] = new Cell().Add(new Paragraph("Birth date:")); break;
                    case 1: details[i] = new Cell().Add(new Paragraph("Phone")); break;
                    case 2: details[i] = new Cell().Add(new Paragraph("E-mail:")); break;
                }
                details[i].SetFont(bold).SetFontSize(10).SetBorder(Border.NO_BORDER);
                details_data[i] = new Cell().Add(new Paragraph(line[i + 1])).SetFontSize(10).SetBorder(Border.NO_BORDER);
                person.AddCell(details[i]);
                person.AddCell(details_data[i]);
            }

            // EXPERIENCE TABLE BELOW ----------------------------------

            Paragraph[] worktype = new Paragraph[3];
            Paragraph[] workplace = new Paragraph[3];
            Cell[] work = new Cell[3];
            Cell[] work_dates = new Cell[3];

            //table structures
            var experience = new Table(new float[] { 2, 5, 2 });

            // left side / title or empty /
            Cell empty = new Cell().Add(new Paragraph("")).SetFont(timesroman).SetFontSize(15).SetBorder(Border.NO_BORDER).SetWidth(100);
            Cell title_experience = new Cell().Add(new Paragraph("Experience:")).SetFont(timesroman).SetFontSize(15).SetBorder(Border.NO_BORDER).SetWidth(100);

            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        experience.AddHeaderCell(title_experience);
                        worktype[i] = new Paragraph(line[4]);
                        work_dates[i] = new Cell().Add(new Paragraph(line[5]));
                        workplace[i] = new Paragraph(line[6]);
                        break;

                    case 1:
                        experience.AddCell(empty);
                        worktype[i] = new Paragraph(line[7]);
                        work_dates[i] = new Cell().Add(new Paragraph(line[8]));
                        workplace[i] = new Paragraph(line[9]);
                        break;

                    case 2:
                        experience.AddCell(empty);
                        worktype[i] = new Paragraph(line[10]);
                        work_dates[i] = new Cell().Add(new Paragraph(line[11]));
                        workplace[i] = new Paragraph(line[12]);
                        break;
                }

                worktype[i].SetFontSize(13).SetFont(timesroman);
                workplace[i].SetFontSize(9).SetFont(timesitalic).SetFontColor(DeviceRgb.BLUE);

                work[i] = new Cell().Add(worktype[i]).Add(workplace[i]).SetWidth(230).SetBorder(Border.NO_BORDER);
                work_dates[i].SetFont(timesroman).SetFontSize(11).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetWidth(120).SetBorder(Border.NO_BORDER);

                if (i == 0)
                {
                    experience.AddHeaderCell(work[i]);
                    experience.AddHeaderCell(work_dates[i]);
                }
                else
                {
                    experience.AddCell(work[i]);
                    experience.AddCell(work_dates[i]);
                }
            }

            // EXPERIENCE ABOVE ----------------------------------------
            // school TABLE BELOW ----------------------------------

            Paragraph[] schoolname = new Paragraph[2];
            Paragraph[] schooltype = new Paragraph[2];
            Cell[] school = new Cell[2];
            Cell[] school_dates = new Cell[2];

            //table structures
            var education = new Table(new float[] { 2, 5, 2 });

            // left side / title or empty /
            Cell title_education = new Cell().Add(new Paragraph("Education:")).SetFont(timesroman).SetFontSize(15).SetBorder(Border.NO_BORDER).SetWidth(100);

            for (int i = 0; i < 2; i++)
            {
                switch (i)
                {
                    case 0:
                        education.AddHeaderCell(title_education);
                        schoolname[i] = new Paragraph(line[13]);
                        school_dates[i] = new Cell().Add(new Paragraph(line[14]));
                        schooltype[i] = new Paragraph(line[15]);
                        break;

                    case 1:
                        education.AddCell(empty);
                        schoolname[i] = new Paragraph(line[16]);
                        school_dates[i] = new Cell().Add(new Paragraph(line[17]));
                        schooltype[i] = new Paragraph(line[18]);
                        break;
                }

                schoolname[i].SetFontSize(12).SetFont(timesroman);
                schooltype[i].SetFontSize(9).SetFont(timesitalic).SetFontColor(DeviceRgb.BLUE);

                school[i] = new Cell().Add(schoolname[i]).Add(schooltype[i]).SetWidth(230).SetBorder(Border.NO_BORDER);
                school_dates[i].SetFont(timesroman).SetFontSize(11).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetWidth(120).SetBorder(Border.NO_BORDER);

                if (i == 0)
                {
                    education.AddHeaderCell(school[i]);
                    education.AddHeaderCell(school_dates[i]);
                }
                else
                {
                    education.AddCell(school[i]);
                    education.AddCell(school_dates[i]);
                }
            }

            // school ABOVE ----------------------------------------

            //var education = new Table(new float[] { 2, 5, 2 });
            //education.AddHeaderCell(new Cell().Add(new Paragraph("Education:")).SetFont(timesroman).SetFontSize(15).SetBorder(Border.NO_BORDER).SetWidth(100));
            //education.AddHeaderCell(new Cell().Add(new Paragraph(line[13] + "\n" + line[15])).SetFont(timesroman).SetFontSize(14).SetWidth(250).SetBorder(Border.NO_BORDER));
            //education.AddHeaderCell(new Cell().Add(new Paragraph(line[14])).SetFont(timesroman).SetFontSize(12).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetWidth(100).SetBorder(Border.NO_BORDER));

            document.Add(person);
            document.Add(new Paragraph("\n"));
            document.Add(separator);
            document.Add(experience);
            document.Add(separator);
            document.Add(education);
            document.Add(separator);

            document.Add(photo);
            document.Close();
        }
    }
}