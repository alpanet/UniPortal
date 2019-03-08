using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using iTextSharp.text.pdf;
using UniYemek.Object;

namespace UniPortal.Helpers
{
    public class PdfRead

    {
        public void Starter(Okul okul, string pdfDownloandUrl)
        {
            switch (okul)
            {
                case Okul.DokuzEylul:
                    string pdfServerMap = Pdfdowloand(pdfDownloandUrl);
                    List<string> gunyemek = PdfReader(pdfServerMap);
                    break;
            }
        }

        private List<string> PdfReader(string toRead)
        {
            List<GenelYemekListeObject> yemekListesi = new List<GenelYemekListeObject>();
            var pdfReader = new PdfReader(toRead);
            int count = 1;
            TextAsParagraphsExtractionStrategy s = new TextAsParagraphsExtractionStrategy();
            for (int i = 0; i < pdfReader.NumberOfPages; i++)
            {
                iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, i + 1, s);
                count++;
            }
            var yemekDate1 = s.coordinates.Where(x => x.Item2 < 523 && x.Item2 > 521);
            var yemekDate2 = s.coordinates.Where(x => x.Item2 < 434 && x.Item2 > 433);
            var yemekDate3 = s.coordinates.Where(x => x.Item2 < 330 && x.Item2 > 329);
            var yemekDate4 = s.coordinates.Where(x => x.Item2 < 238 && x.Item2 > 237);
            var yemekDate5 = s.coordinates.Where(x => x.Item2 < 147 && x.Item2 > 146);
            //var enumerable = yemekGunler as Tuple<float, float, string>[] ?? yemekGunler.ToArray();
            //if (!yemekGunler.Any() || yemekGunler.Any(x => (x.Item3 ?? "").Length < 6))
            //{
            //    throw new PdfParseException() { HataMesaj = "Pdf formatı hatalı." };
            //}

            //foreach (var tuple in yemekGunler)
            //{
               
            //}
            //var item3 = yemekGunler.Select(x => x.Item3).ToList();

            return null;
        }
        public class PdfParseException : Exception
        {
            public string HataMesaj { get; set; }
        }

        private string Pdfdowloand(string downUrl)
        {
            string startupPath = Environment.CurrentDirectory;
            string downloadedPdfUrl = "\\" + RandomString();
            WebClient webClient = new WebClient();
            webClient.DownloadFile(downUrl, startupPath + downloadedPdfUrl);

            return startupPath + downloadedPdfUrl;
        }

        private string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 15)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string InnerTrim(string input)
        {
            return input.Trim().Replace(" ", string.Empty);
        }

    }
}
