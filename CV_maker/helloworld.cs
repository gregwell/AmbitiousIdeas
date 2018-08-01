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
            string[] line = new string[26];
            using (var reader = new StreamReader("D:/data.txt"))
            {
                for (int i = 0; i < line.Length; i++)
                {
                    line[i] = reader.ReadLine();
                }
            }

            string name = line[0];
            string birthdate = line[1];
            string phone = line[2];
            string email = line[3];
            string[] workname = new string[3];
            workname[0] = line[4];
            workname[1] = line[7];
            workname[2] = line[10];
            string[] workdate = new string[3];
            workdate[0] = line[5];
            workdate[1] = line[8];
            workdate[2] = line[11];
            string[] workplace = new string[3];
            workplace[0] = line[6];
            workplace[1] = line[9];
            workplace[2] = line[12];

            string[] schoolname = new string[3];
            schoolname[0] = line[13];
            schoolname[1] = line[16];
            schoolname[2] = line[19];
            string[] schooldate = new string[3];
            schooldate[0] = line[14];
            schooldate[1] = line[17];
            schooldate[2] = line[20];
            string[] schoolplace = new string[3];
            schoolplace[0] = line[15];
            schoolplace[1] = line[18];
            schoolplace[2] = line[21];

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

            Paragraph[] pworkname = new Paragraph[3];
            Paragraph[] pworkplace = new Paragraph[3];
            Cell[] work = new Cell[3];
            Cell[] work_dates = new Cell[3];

            Paragraph[] pschoolname = new Paragraph[2];
            Paragraph[] pschooltype = new Paragraph[2];
            Cell[] school = new Cell[2];
            Cell[] school_dates = new Cell[2];

            //table structures
            var experience = new Table(new float[] { 2, 5, 2 });
            var education = new Table(new float[] { 2, 5, 2 });

            // left side / title or empty /
            Cell empty = new Cell().Add(new Paragraph("")).SetFont(timesroman).SetFontSize(15).SetBorder(Border.NO_BORDER).SetWidth(100);
            Cell[] title = new Cell[2];

            for (int i = 0; i < 2; i++)
            {
                switch (i)
                {
                    case 0: title[i] = new Cell().Add(new Paragraph("Experience:")); break;
                    case 1: title[i] = new Cell().Add(new Paragraph("Education")); break;
                }
                title[i].SetFont(timesroman).SetFontSize(15).SetBorder(Border.NO_BORDER).SetWidth(100);
            }

            //experience table constructor
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        experience.AddHeaderCell(title[0]);
                        pworkname[i] = new Paragraph(line[4]);
                        work_dates[i] = new Cell().Add(new Paragraph(line[5]));
                        pworkplace[i] = new Paragraph(line[6]);
                        break;

                    case 1:
                        experience.AddCell(empty);
                        pworkname[i] = new Paragraph(line[7]);
                        work_dates[i] = new Cell().Add(new Paragraph(line[8]));
                        pworkplace[i] = new Paragraph(line[9]);
                        break;

                    case 2:
                        experience.AddCell(empty);
                        pworkname[i] = new Paragraph(line[10]);
                        work_dates[i] = new Cell().Add(new Paragraph(line[11]));
                        pworkplace[i] = new Paragraph(line[12]);
                        break;
                }

                pworkname[i].SetFontSize(13).SetFont(timesroman);
                pworkplace[i].SetFontSize(10).SetFont(timesitalic).SetFontColor(DeviceRgb.BLUE);

                work[i] = new Cell().Add(pworkname[i]).Add(pworkplace[i]).SetWidth(230).SetBorder(Border.NO_BORDER);
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

            // left side / title or empty /

            for (int i = 0; i < 2; i++)
            {
                switch (i)
                {
                    case 0:
                        education.AddHeaderCell(title[1]);
                        pschoolname[i] = new Paragraph(line[13]);
                        school_dates[i] = new Cell().Add(new Paragraph(line[14]));
                        pschooltype[i] = new Paragraph(line[15]);
                        break;

                    case 1:
                        education.AddCell(empty);
                        pschoolname[i] = new Paragraph(line[16]);
                        school_dates[i] = new Cell().Add(new Paragraph(line[17]));
                        pschooltype[i] = new Paragraph(line[18]);
                        break;
                }

                pschoolname[i].SetFontSize(12).SetFont(timesroman);
                pschooltype[i].SetFontSize(10).SetFont(timesitalic).SetFontColor(DeviceRgb.BLUE);

                school[i] = new Cell().Add(pschoolname[i]).Add(pschooltype[i]).SetWidth(230).SetBorder(Border.NO_BORDER);
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