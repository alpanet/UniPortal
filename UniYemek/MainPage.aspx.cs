using System;
using System.Collections.Generic;
using System.Linq;
using UniPortal.Helpers;
using UniYemek.Object;
using UniYemek.Okul;

namespace UniYemek
{
    public partial class MainPage : System.Web.UI.Page
    {
        HtmlReader _htmlReader = new HtmlReader();
        DokuzEylul _dokuzEylül = new DokuzEylul();
        SuleymanDemirel _suleymanDemirel = new SuleymanDemirel();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)

            {
                OkulList.DataSource = Enum.GetNames(typeof(Object.Okul));
                OkulList.DataBind();
            }

        }

        protected void OkulList_TextChanged(object sender, EventArgs e)
        {

            Object.Okul selectedOkul = (Object.Okul)Enum.Parse(typeof(Object.Okul), OkulList.SelectedItem.Value);
            switch (selectedOkul)
            {
                case Object.Okul.DokuzEylul:
                    Tableolustur(_htmlReader.Starter(_dokuzEylül.okulUrl, Object.Okul.DokuzEylul));
                    break;
                case Object.Okul.SuleymanDemirel:
                    Tableolustur(_htmlReader.Starter(_suleymanDemirel.okulUrl, Object.Okul.SuleymanDemirel));
                    break;
            }

        }

        private void Tableolustur(List<GenelYemekListeObject> generalList)
        {
            GridView1.DataSource = generalList;
            GridView1.DataBind();
        }
    }
}