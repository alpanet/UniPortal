using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using UniYemek.Object;

namespace UniPortal.Helpers
{
    public class HtmlReader
    {
        string _baseUrl = "";
        private List<GenelYemekListeObject> _generalList;
        public List<GenelYemekListeObject> Starter(string uniUrl, Okul okul)
        {
            _baseUrl = uniUrl;
            HtmlDocument htmlDocument = new HtmlDocument();
            _generalList=new List<GenelYemekListeObject>();
            if (_baseUrl != null)
            {
                StreamReader reader = new StreamReader(WebRequest.Create(_baseUrl).GetResponse().GetResponseStream(), Encoding.UTF8); //put your encoding            
                htmlDocument.Load(reader);

                HtmlSplit(htmlDocument, okul);
                return _generalList;
            }
            return null;
        }

        public void HtmlSplit(HtmlDocument loadedDocument, Okul okul)
        {

            switch (okul)
            {
                #region Pdf Read

                //case Okul.DokuzEylül:
                //    string sayfaSelector = "//p[@class='gde-text']";
                //    sayfaNodeList = loadedDocument.DocumentNode.SelectNodes(sayfaSelector);
                //    var links = sayfaNodeList.Descendants("a").ToList();
                //    foreach (var hrefNode in links)
                //    {
                //        string hrefurl = hrefNode.GetAttributeValue("href", string.Empty);
                //        if (!string.IsNullOrEmpty(hrefurl))
                //        {
                //            PdfRead.Starter(okul, hrefurl);
                //        }
                //    }
                //    break;

                #endregion

                case Okul.DokuzEylul:
                    DokuzEylulHtmlReader(loadedDocument, okul);
                    break;
                case Okul.SuleymanDemirel:
                    SuleymanDemirelHtmlReader(loadedDocument, okul);
                    break;
            }
        }

        #region Okullar

        private void DokuzEylulHtmlReader(HtmlDocument loadedDocument, Okul okul)
        {
            _generalList = new List<GenelYemekListeObject>();
            HtmlNodeCollection sayfaDivList = new HtmlNodeCollection(null);
            string sayfaDivSelector = "//div[@id='tm_lunch_menu_widget-3']";
            sayfaDivList = loadedDocument.DocumentNode.SelectNodes(sayfaDivSelector);
            var sayfaStrongList = sayfaDivList.Descendants("strong").ToList();
            var sayfaBrList = sayfaDivList.Descendants("br").ToList();

            foreach (var strongNode in sayfaStrongList)
            {
                GenelYemekListeObject generalYemek = new GenelYemekListeObject();
                string strongDateTime = strongNode.InnerHtml;
                string brInnerHtml = strongNode.NextSibling.NextSibling.InnerHtml;
                if (!string.IsNullOrEmpty(strongDateTime))
                {
                    generalYemek.TarihString = strongNode.InnerHtml;
                    generalYemek.TarihDateTime = DatetimeSet(strongDateTime, okul);
                }
                if (!string.IsNullOrEmpty(brInnerHtml))
                {
                    generalYemek.YemekIcerik = StringClean(brInnerHtml, okul);
                }
                _generalList.Add(generalYemek);
            }
        }

        private void SuleymanDemirelHtmlReader(HtmlDocument loadedDocument, Okul okul)
        {
            _generalList = new List<GenelYemekListeObject>();
            HtmlNodeCollection sayfaSpanList = new HtmlNodeCollection(null);
            string sayfatableSelector = "//table[@id='cph_body_dgYemekListesi']";
            sayfaSpanList = loadedDocument.DocumentNode.SelectNodes(sayfatableSelector);
            for (int i = 1; i < sayfaSpanList[0].ChildNodes.Count; i++)
            {
                GenelYemekListeObject generalYemek = new GenelYemekListeObject();
                HtmlNodeCollection sayfaTdList = sayfaSpanList[0].ChildNodes[i].ChildNodes;
                generalYemek.TarihString = sayfaTdList[0].InnerText;
                generalYemek.TarihDateTime = DatetimeSet(sayfaTdList[0].InnerText, okul);
                generalYemek.YemekIcerik = StringClean(sayfaTdList[1].ChildNodes[1].InnerHtml,okul);
                _generalList.Add(generalYemek);
            }
        }

        #endregion



        #region General Event

        private string StringClean(string temizlencekString, Okul okul)
        {
            string returnString = "";
            switch (okul)
            {
                case Okul.DokuzEylul:
                    int indexsonParantez = temizlencekString.IndexOf(")") + 1;
                    //int sonIndex = temizlencekString.Length;
                    returnString = temizlencekString.Substring(0, indexsonParantez);
                    break;
                case Okul.SuleymanDemirel:
                    returnString = temizlencekString.Replace("<br>", " ");
                    break;
            }
            return returnString;
        }

        private DateTime DatetimeSet(string dateTimeString, Okul okul)
        {
            DateTime returnDatetimeValue = new DateTime();
            try
            {
                string date = "";
                string mount = "";
                switch (okul)
                {
                    case Okul.DokuzEylul:
                        int index = dateTimeString.IndexOf(",") + 2;
                        int lastIndex = dateTimeString.Length;
                        mount = dateTimeString.Substring(index, lastIndex - index - 2);
                        date = dateTimeString.Substring(index + 3, lastIndex - (index + 3));
                        returnDatetimeValue = TurkceDateTime(mount, date);
                        break;
                    case Okul.SuleymanDemirel:
                        returnDatetimeValue = Convert.ToDateTime(dateTimeString);
                        break;
                }

                return returnDatetimeValue;
            }
            catch (Exception e)
            {
                return DateTime.MaxValue;
            }

        }

        private DateTime TurkceDateTime(string turkceAy, string turkceGun)
        {
            DateTime gecerliTarih = new DateTime();
            switch (turkceAy)
            {
                case "Oca":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/01/" + System.DateTime.Now.Year);
                    break;
                case "Şub":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/02/" + System.DateTime.Now.Year);
                    break;
                case "Mar":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/03/" + System.DateTime.Now.Year);
                    break;
                case "Nis":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/04/" + System.DateTime.Now.Year);
                    break;
                case "May":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/05/" + System.DateTime.Now.Year);
                    break;
                case "Haz":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/06/" + System.DateTime.Now.Year);
                    break;
                case "Tem":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/07/" + System.DateTime.Now.Year);
                    break;
                case "Ağu":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/08/" + System.DateTime.Now.Year);
                    break;
                case "Eyl":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/09/" + System.DateTime.Now.Year);
                    break;
                case "Eki":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/10/" + System.DateTime.Now.Year);
                    break;
                case "Kas":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/11/" + System.DateTime.Now.Year);
                    break;
                case "Ara":
                    gecerliTarih = Convert.ToDateTime(turkceGun + "/12/" + System.DateTime.Now.Year);
                    break;
            }
            return gecerliTarih;
        }

        #endregion
    }
}
