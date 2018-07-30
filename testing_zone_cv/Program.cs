using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.IO.Image;

using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Font;

using iText.Layout;
using iText.Layout.Element;
using iText.IO.Font.Constants;
using iText.Layout.Borders;

namespace Tutorial.Chapter02
{
    public class C02E01_Axes
    {
        public const String DEST = "D:/axe.pdf";
        public const String PHOTO = "D:/photo.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E01_Axes().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest)
        {
            var pdf = new PdfDocument(new PdfWriter(dest));
            var ps = PageSize.A4.Rotate();
            var page = pdf.AddNewPage(ps);
            var canvas = new PdfCanvas(page);

            //photo import
            var photo = new Image(ImageDataFactory.Create(PHOTO));
            photo.SetHeight(90);

            //Replace the origin of the coordinate system to the center of the page
            canvas.ConcatMatrix(1, 0, 0, 1, ps.GetWidth() / 2, ps.GetHeight() / 2);
            C02E01_Axes.DrawAxes(canvas, ps);

            pdf.Close();
        }

        public static void DrawAxes(PdfCanvas canvas, PageSize ps)
        {
            //Draw X axis
            canvas.MoveTo(-(ps.GetWidth() / 2 - 15), 0).AddImage(PHOTO).Stroke();
            //Draw X axis arrow
            //canvas.SetLineJoinStyle(PdfCanvasConstants.LineJoinStyle.ROUND).MoveTo(ps.GetWidth() / 2 - 25, -10).LineTo
            //    (ps.GetWidth() / 2 - 15, 0).LineTo(ps.GetWidth() / 2 - 25, 10).Stroke().SetLineJoinStyle(PdfCanvasConstants.LineJoinStyle
            //    .MITER);
            ////Draw Y axis
            //canvas.MoveTo(0, -(ps.GetHeight() / 2 - 15)).LineTo(0, ps.GetHeight() / 2 - 15).Stroke();
            ////Draw Y axis arrow
            //canvas.SaveState().SetLineJoinStyle(PdfCanvasConstants.LineJoinStyle.ROUND).MoveTo(-10, ps.GetHeight() / 2
            //     - 25).LineTo(0, ps.GetHeight() / 2 - 15).LineTo(10, ps.GetHeight() / 2 - 25).Stroke().RestoreState();
            ////Draw X serif
            //for (int i = -((int)ps.GetWidth() / 2 - 61); i < ((int)ps.GetWidth() / 2 - 60); i += 40)
            //{
            //    canvas.MoveTo(i, 5).LineTo(i, -5);
            //}
            ////Draw Y serif
            //for (int j = -((int)ps.GetHeight() / 2 - 57); j < ((int)ps.GetHeight() / 2 - 56); j += 40)
            //{
            //    canvas.MoveTo(5, j).LineTo(-5, j);
            //}
            canvas.Stroke();
        }
    }
}