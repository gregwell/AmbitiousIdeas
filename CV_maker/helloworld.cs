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

namespace CvMaker
{
    public class PdfMaker
    {
        public const String DEST = "D:/cv.pdf";
        public const String PHOTO = "D:/photo.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfMaker().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest)
        {
            //document formalities
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

            //writing to file
            string[] writerline = new string[2];
            using (var filewriter = new StreamWriter("D:/saved.txt"))
            {
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0: Console.Write("Identities:\n    name + surname: "); break;
                        case 1: Console.Write("    birthdate: "); break;
                        case 2: Console.Write("    phone:"); break;
                        case 3: Console.Write("    e-mail:"); break;
                    }
                    filewriter.WriteLine(Console.ReadLine());
                }

                ConsoleKeyInfo cki = new ConsoleKeyInfo((char)ConsoleKey.RightArrow, ConsoleKey.RightArrow, false, false, false);

                for (int j = 0; j < 4; j++)
                {
                    string wtf = "workplace";
                    string first = "proffesion";
                    string second = "work dates";
                    string third = "company name";

                    int counter = 1;

                    int noinfo = 3;

                    if (j == 1)
                    {
                        wtf = "school";
                        first = "school name";
                        second = "school dates";
                        third = "degree";
                    }
                    else if (j == 2)
                    {
                        wtf = "language";
                        first = "language";
                        second = "advancement level";
                        noinfo = 2;
                    }
                    else if (j == 3)
                    {
                        wtf = "interests";
                        first = "interest";
                        noinfo = 1;
                    }

                    Question(wtf, counter);
                    cki = ValidateCki(cki);

                    if (cki.Key.ToString() == "Y")
                    {
                        WriteData(wtf, first, second, third, j, counter, filewriter);
                        counter++;

                        Question(wtf, counter);
                        cki = ValidateCki(cki);

                        if (cki.Key.ToString() == "Y")
                        {
                            WriteData(wtf, first, second, third, j, counter, filewriter);
                            counter++;

                            Question(wtf, counter);
                            cki = ValidateCki(cki);

                            if (cki.Key.ToString() == "Y")
                            {
                                WriteData(wtf, first, second, third, j, counter, filewriter);
                            }
                            else for (int i = 0; i < noinfo; i++) filewriter.WriteLine("no data");
                        }
                        else for (int i = 0; i < 2 * noinfo; i++) filewriter.WriteLine("no data");
                    }
                    else for (int i = 0; i < 3 * noinfo; i++) filewriter.WriteLine("no data");
                }
            }

            //reading from the file
            string[] line = new string[31];
            using (var reader = new StreamReader("D:/saved.txt"))
            {
                for (int i = 0; i < line.Length; i++)
                {
                    line[i] = reader.ReadLine();
                }
            }

            //personal data
            string[] personal = new string[4];
            personal[0] = line[0];
            personal[1] = line[1];
            personal[2] = line[2];
            personal[3] = line[3];

            //workplace
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

            //education
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

            //language
            string[] languagename = new string[3];
            languagename[0] = line[22];
            languagename[1] = line[24];
            languagename[2] = line[26];
            string[] languagelevel = new string[3];
            languagelevel[0] = line[23];
            languagelevel[1] = line[25];
            languagelevel[2] = line[27];

            //interests
            string[] interestname = new string[3];
            interestname[0] = line[28];
            interestname[1] = line[29];
            interestname[2] = line[30];

            SolidLine solidline = new SolidLine(2f);
            LineSeparator separator = new LineSeparator(solidline);
            separator.SetMarginTop(10);
            separator.SetMarginBottom(10);

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
                details[i].SetFont(bold).SetFontSize(10).SetBorder(Border.NO_BORDER).SetWidth(100);
                details_data[i] = new Cell().Add(new Paragraph(personal[i + 1])).SetFontSize(10).SetBorder(Border.NO_BORDER);
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

            Paragraph[] planguagename = new Paragraph[3];
            Paragraph[] planguagelevel = new Paragraph[3];
            Cell[] language = new Cell[3];

            var pinterest = new Paragraph();

            //table structures
            var experience = new Table(new float[] { 2, 5, 2 });
            var education = new Table(new float[] { 2, 5, 2 });
            var languages = new Table(new float[] { 2, 5, 2 });
            var interests = new Table(new float[] { 2, 5, 2 });

            // left side / title or empty /

            Cell[] work_empty = new Cell[2];
            Cell[] school_empty = new Cell[2];
            Cell[] language_empty_left = new Cell[2];
            Cell[] language_empty_right = new Cell[2];
            Cell[] interest_empty_left = new Cell[2];
            Cell[] interest_empty_right = new Cell[2];
            Cell[] title = new Cell[4];

            for (int i = 0; i < 2; i++)
            {
                work_empty[i] = new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetWidth(100);
                school_empty[i] = new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetWidth(100);

                language_empty_left[i] = new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetWidth(100);
                language_empty_right[i] = new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetWidth(100);

                interest_empty_left[i] = new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetWidth(100);
                interest_empty_right[i] = new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetWidth(100);
            }

            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0: title[i] = new Cell().Add(new Paragraph("Experience:")); break;
                    case 1: title[i] = new Cell().Add(new Paragraph("Education")); break;
                    case 2: title[i] = new Cell().Add(new Paragraph("Languages:")); break;
                    case 3: title[i] = new Cell().Add(new Paragraph("Interests:")); break;
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

                //language cells
                planguagename[i] = new Paragraph(languagename[i] + " - " + languagelevel[i]);
                language[i] = new Cell().Add(planguagename[i]).SetWidth(230).SetFontSize(10).SetHeight(20).SetBorder(Border.NO_BORDER);

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
                if (languagename[i] != "no data")
                {
                    if (i == 0)
                    {
                        languages.AddCell(title[2]);
                        languages.AddCell(language[0]);
                        languages.AddCell(language_empty_right[0]);
                    }
                    else
                    {
                        languages.AddCell(language_empty_left[i - 1]);
                        languages.AddCell(language[i]);
                        languages.AddCell(language_empty_right[i - 1]);
                    }
                }
                if (interestname[i] != "no data")
                {
                    if (i == 0)
                    {
                        pinterest.Add(interestname[i]);
                    }
                    else
                    {
                        pinterest.Add(", " + interestname[i]);
                    }
                }
            }

            var cinterest = new Cell().SetWidth(230).SetFontSize(10).SetHeight(20).SetBorder(Border.NO_BORDER);
            cinterest.Add(pinterest);

            interests.AddCell(title[3]);
            interests.AddCell(cinterest);
            interests.AddCell(interest_empty_right[0]);

            document.Add(person);
            document.Add(new Paragraph("\n"));
            document.Add(separator);
            document.Add(experience);
            if (!experience.IsEmpty()) document.Add(separator);
            document.Add(education);
            if (!education.IsEmpty()) document.Add(separator);
            document.Add(languages);
            if (!languages.IsEmpty()) document.Add(separator);
            document.Add(interests);
            if (!interests.IsEmpty()) document.Add(separator);

            document.Add(photo);
            document.Close();
        }

        public ConsoleKeyInfo ValidateCki(ConsoleKeyInfo cki)
        {
            cki = Console.ReadKey();
            while (cki.Key.ToString() != "Y" && cki.Key.ToString() != "N")
            {
                Console.WriteLine(" (Invalid input. Type y-yes or n-no)");
                cki = Console.ReadKey();
            }
            return cki;
        }

        public void Question(string name, int number)
        {
            Console.Write("\nWould you like to add {0} {1}? y - yes  n - no : ", name, number);
        }

        public void WriteData(string name, string first, string second, string third, int j, int counter, StreamWriter filewriter)
        {
            Console.WriteLine("\n{0} {1}: ", name, counter);
            Console.Write("    {0}:", first);
            filewriter.WriteLine(Console.ReadLine());
            if (j < 3)
            {
                Console.Write("    {0}:", second);
                filewriter.WriteLine(Console.ReadLine());
                if (j < 2)
                {
                    Console.Write("    {0}:", third);
                    filewriter.WriteLine(Console.ReadLine());
                }
            }
        }
    }
}