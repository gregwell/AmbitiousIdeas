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

            string[] personal = new string[4];
            personal[0] = line[0];
            personal[1] = line[1];
            personal[2] = line[2];
            personal[3] = line[3];
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
                details_data[i] = new Cell().Add(new Paragraph(personal[i])).SetFontSize(10).SetBorder(Border.NO_BORDER);
                person.AddCell(details[i]);
                person.AddCell(details_data[i]);
            }

            // EXPERIENCE TABLE BELOW ----------------------------------

            Paragraph[] pworkname = new Paragraph[3];
            Paragraph[] pworkplace = new Paragraph[3];
            Cell[] work = new Cell[3];
            Cell[] work_dates = new Cell[3];

            Paragraph[] pschoolname = new Paragraph[3];
            Paragraph[] pschoolplace = new Paragraph[3];
            Cell[] school = new Cell[3];
            Cell[] school_dates = new Cell[3];

            //table structures
            var experience = new Table(new float[] { 2, 5, 2 });
            var education = new Table(new float[] { 2, 5, 2 });

            // left side / title or empty /

            Cell[] work_empty = new Cell[2];
            Cell[] school_empty = new Cell[2];
            Cell[] title = new Cell[2];

            for (int i = 0; i < 2; i++)
            {
                work_empty[i] = new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetWidth(100);
                school_empty[i] = new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetWidth(100);
            }

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
                //work
                pworkname[i] = new Paragraph(workname[i]);
                work_dates[i] = new Cell().Add(new Paragraph(workdate[i]));
                pworkplace[i] = new Paragraph(workplace[i]);

                pworkname[i].SetFontSize(13).SetFont(timesroman);
                pworkplace[i].SetFontSize(10).SetFont(timesitalic).SetFontColor(DeviceRgb.BLUE);

                work[i] = new Cell().Add(pworkname[i]).Add(pworkplace[i]).SetWidth(230).SetBorder(Border.NO_BORDER);
                work_dates[i].SetFont(timesroman).SetFontSize(11).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetWidth(120).SetBorder(Border.NO_BORDER);

                //school
                pschoolname[i] = new Paragraph(schoolname[i]);
                school_dates[i] = new Cell().Add(new Paragraph(schooldate[i]));
                pschoolplace[i] = new Paragraph(schoolplace[i]);

                pschoolname[i].SetFontSize(13).SetFont(timesroman);
                pschoolplace[i].SetFontSize(10).SetFont(timesitalic).SetFontColor(DeviceRgb.BLUE);

                school[i] = new Cell().Add(pschoolname[i]).Add(pschoolplace[i]).SetWidth(230).SetBorder(Border.NO_BORDER);
                school_dates[i].SetFont(timesroman).SetFontSize(11).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetWidth(120).SetBorder(Border.NO_BORDER);

                //adding data to tables.
                if (workplace[i] != "no data")
                {
                    if (i == 0)
                    {
                        experience.AddCell(title[0]);
                    }
                    else
                    {
                        experience.AddCell(work_empty[i - 1]);
                    }
                    experience.AddCell(work[i]);
                    experience.AddCell(work_dates[i]);
                }
                if (schoolplace[i] != "no data")
                {
                    if (i == 0)
                    {
                        education.AddCell(title[1]);
                    }
                    else
                    {
                        education.AddCell(school_empty[i - 1]);
                    }
                    education.AddCell(school[i]);
                    education.AddCell(school_dates[i]);
                }
            }

            document.Add(person);
            document.Add(new Paragraph("\n"));
            document.Add(separator);
            document.Add(experience);
            if (!experience.IsEmpty()) document.Add(separator);
            document.Add(education);
            if (!education.IsEmpty()) document.Add(separator);

            document.Add(photo);
            document.Close();
        }
    }
}