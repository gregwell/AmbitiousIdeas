using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace CV_maker
{
    internal class helloworld
    {
        private static void Main(string[] args)
        {
            var exportFile = System.IO.Path.Combine("D:", "Test.pdf");

            using (var writer = new PdfWriter(exportFile))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var doc = new Document(pdf);
                    doc.Add(new Paragraph("hello world"));
                }
            }
        }
    }
}