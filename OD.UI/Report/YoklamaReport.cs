using iTextSharp.text;
using iTextSharp.text.pdf;
using OD.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using OD.UI.Utils;

namespace OD.UI.Report
{
    public class YoklamaReport
    {
        #region Declaration
        int _totalcolumn = 4;
        Document _document;
        //Font _fontStyle = fontNormal;
        PdfPTable _pdfTable = new PdfPTable(4);
        PdfPCell _pdfCell;
        MemoryStream _memorystream = new MemoryStream();
        List<YoklamaViewModel> _yoklamaListe = new List<YoklamaViewModel>();
        #endregion

        public byte[] PrepareReport(List<YoklamaViewModel> YoklamaListe, int Sinif, string Sube,DateTime Tarih)
        {
            _yoklamaListe = YoklamaListe;

            #region
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            PdfWriter.GetInstance(_document, _memorystream);
            _document.Open();
            _pdfTable.SetWidths(new float[] { 30f, 70f,100f,100f });
            #endregion

            this.ReportHeader(Sinif, Sube,Tarih);
            this.ReportBody();
            _pdfTable.HeaderRows = 3;
            _document.Add(_pdfTable); 
            _document.Close();
            return _memorystream.ToArray();
        }

        private void ReportHeader(int Sinif, string Sube,DateTime Tarih)
        {
            BaseFont Vn_Helvetica = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", "Identity-H", BaseFont.EMBEDDED);
            Font _fontStyle = new Font(Vn_Helvetica, 12, Font.BOLD);
            _pdfCell = new PdfPCell(new Phrase(Sinif + "/" + Sube + " Günlük Yoklama Listesi ", _fontStyle));
            _pdfCell.Colspan = _totalcolumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();

            _pdfCell = new PdfPCell(new Phrase("Tarih : " + Tarih.ToLongDateString(), _fontStyle));
            _pdfCell.Colspan = _totalcolumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();

            _pdfCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfCell.Colspan = _totalcolumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();

        }

        private void ReportBody()
        {
            Fonksiyon f = new Fonksiyon();

            #region Table header

            BaseFont Vn_Helvetica = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", "Identity-H", BaseFont.EMBEDDED);
            Font _fontStyle = new Font(Vn_Helvetica, 11, Font.NORMAL);

            _pdfCell = new PdfPCell(new Phrase("Sıra No", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfCell.Padding = 7;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Öğrenci No", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfCell.Padding = 7;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Adı", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfCell.Padding = 7;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Soyadı", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfCell.Padding = 7;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
            #endregion

            #region Table Body
            int sirano = 1;
            foreach (YoklamaViewModel item in _yoklamaListe)
            {
                string dersSaati = f.DersSaatiGetir(Convert.ToInt32(item.DersGrupID)).DersGrupAdi;
                string dersAdi = f.DersGetir(Convert.ToInt32(item.DersID)).DersAdi;
                string ogretmenAdi = f.OgretmenGetir(Convert.ToInt32(item.OgretmenID)).AdSoyad;

                _pdfCell = new PdfPCell(new Phrase(dersSaati + " - " + dersAdi + " [ " + ogretmenAdi + " ]", _fontStyle));
                _pdfCell.Colspan = _totalcolumn;
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
                _pdfCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _pdfCell.Padding = 7;
                _pdfTable.AddCell(_pdfCell);
                _pdfTable.CompleteRow();
                sirano = 1;
                if (item.OgrenciIDListe == null)
                {
                    _pdfCell = new PdfPCell(new Phrase("Sınıfta Gelmeyen Yoktur.", _fontStyle));
                    _pdfCell.Colspan = _totalcolumn;
                    _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfCell.BackgroundColor = BaseColor.WHITE;
                    _pdfCell.Padding = 5;
                    _pdfTable.AddCell(_pdfCell);
                }
                else
                {
                    foreach (var ogrenci in item.OgrenciIDListe)
                    {
                        _pdfCell = new PdfPCell(new Phrase(sirano++.ToString(), _fontStyle));
                        _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfCell.BackgroundColor = BaseColor.WHITE;
                        _pdfCell.Padding = 5;
                        _pdfTable.AddCell(_pdfCell);

                        _pdfCell = new PdfPCell(new Phrase(f.OgrenciGetir(Convert.ToInt32(ogrenci)).OgrenciNo.ToString(), _fontStyle));
                        _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfCell.BackgroundColor = BaseColor.WHITE;
                        _pdfCell.Padding = 5;
                        _pdfTable.AddCell(_pdfCell);

                        _pdfCell = new PdfPCell(new Phrase(f.OgrenciGetir(Convert.ToInt32(ogrenci)).Adi.ToString(), _fontStyle));
                        _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfCell.BackgroundColor = BaseColor.WHITE;
                        _pdfCell.Padding = 5;
                        _pdfTable.AddCell(_pdfCell);

                        _pdfCell = new PdfPCell(new Phrase(f.OgrenciGetir(Convert.ToInt32(ogrenci)).Soyadi.ToString(), _fontStyle));
                        _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfCell.BackgroundColor = BaseColor.WHITE;
                        _pdfCell.Padding = 5;
                        _pdfTable.AddCell(_pdfCell);
                        _pdfTable.CompleteRow();
                    }
                }
            }
            #endregion
        }
    }
}