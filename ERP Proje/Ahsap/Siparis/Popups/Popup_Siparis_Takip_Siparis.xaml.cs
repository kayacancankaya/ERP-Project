using Layer_2_Common.Excels;
using Layer_2_Common.Type;
using Layer_Business;
using Layer_UI.Methods;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Layer_UI.Ahsap.Siparis.Popups
{
    /// <summary>
    /// Interaction logic for Popup_Sevk_Takip_Siparis.xaml
    /// </summary>
    public partial class Popup_Siparis_Takip_Siparis : Window
	{
		public Popup_Siparis_Takip_Siparis(ObservableCollection<Cls_Sevk> siparisReportCollection,Dictionary<string,string> restrictionDictionary, string restrictionQuery)
		{
			InitializeComponent();

			dg_Rapor_Siparis_Detay.ItemsSource = siparisReportCollection;
			restrictionDict = restrictionDictionary;
			restrictionQueries = restrictionQuery;
			Mouse.OverrideCursor = null;
		}
		Cls_Sevk sevk = new();
		ObservableCollection<Cls_Sevk> wholeReportCollection = new();
		Dictionary<string,string> restrictionDict = new Dictionary<string,string>();
		string restrictionQueries = string.Empty;
		ExcelMethodsEPP excelMethodsEPP = new();
		ObservableCollection<Cls_Sevk> InvoiceCollection = new();
		private void detail_button_clicked(object sender, RoutedEventArgs e)
		{
			try
			{
				Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

				Cls_Sevk item = UIinteractions.GetDataItemFromButton<Cls_Sevk>(sender);

				wholeReportCollection = sevk.PopulateWholeReportCollection(restrictionDict, restrictionQueries,item.SiparisKodu,"Ahşap");

				if(!wholeReportCollection.Any()) 
				{ CRUDmessages.GeneralFailureMessage("Rapor Bilgileri Alınırken");Mouse.OverrideCursor = null; return; }

				Popup_Siparis_Takip_Detay _popup = new(wholeReportCollection);
				_popup.ShowDialog();

			}
			catch 
			{
				CRUDmessages.GeneralFailureMessage("Detay Bilgileri Alınırken"); Mouse.OverrideCursor = null;
			}
		}
		
		private void excel_button_clicked(object sender, RoutedEventArgs e)
		{
			Mouse.OverrideCursor = new Cursor(Variables.LoadingSymbolofCursor);

			Cls_Sevk item = UIinteractions.GetDataItemFromButton<Cls_Sevk>(sender);

			string fileName = string.Format("{0}_{1}", item.SiparisKodu, DateTime.Now.ToString("yyyyMMddHHmmss"));
			string filePath = "C:\\excel-c\\siparis\\" + fileName;
			string sheetName = item.SiparisKodu + DateTime.Now.ToString("yyyyMMddHHmmss");
			string imagePath = "\\\\192.168.1.11\\Netsis\\Images\\vb.png";

			filePath = excelMethodsEPP.CreateExcelFile(filePath, sheetName);

			FileInfo fileInfo = new FileInfo(filePath);

			ExcelPackage existingPackage = new ExcelPackage(fileInfo);

			excelMethodsEPP.SetColumnWidth(existingPackage, sheetName, 1, 23);
			excelMethodsEPP.SetColumnWidth(existingPackage, sheetName, 2, 49);
			excelMethodsEPP.SetColumnWidth(existingPackage, sheetName, 3, 13);
			excelMethodsEPP.SetColumnWidth(existingPackage, sheetName, 4, 7);
			excelMethodsEPP.SetColumnWidth(existingPackage, sheetName, 5, 8);
			excelMethodsEPP.SetColumnWidth(existingPackage, sheetName, 6, 8);
			excelMethodsEPP.SetColumnWidth(existingPackage, sheetName, 7, 6);
			excelMethodsEPP.SetColumnWidth(existingPackage, sheetName, 8, 6);
			excelMethodsEPP.SetColumnWidth(existingPackage, sheetName, 9, 10);

			excelMethodsEPP.InsertImage(existingPackage, sheetName, imagePath, "logo", 9, 1, 70, 68);

			excelMethodsEPP.MergeAndCenterCells(existingPackage, sheetName, "A1:I1");
			excelMethodsEPP.MergeAndCenterCells(existingPackage, sheetName, "A2:I2");
			excelMethodsEPP.MergeAndCenterCells(existingPackage, sheetName, "A3:I3");
			excelMethodsEPP.MergeAndLeftAlignCells(existingPackage, sheetName, "B8:C8");
			excelMethodsEPP.MergeAndLeftAlignCells(existingPackage, sheetName, "B9:C9");


			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "A1", "Vita Bianca Ahşap Aksesuar Mobilya San. Ve Tic. Ltd Şti.", "Calibri", 11, "#0000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "A2", "ITOB OSB Mahallesi 10036. Sokak No:22", "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "A3", "Menderes/İzmir/Türkiye", "Calibri", 11, "#000000", false);

			excelMethodsEPP.SetRightAlignIndent(existingPackage, sheetName, "A7:A12", 0);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "A7", "Company Name:", "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "A8", "Address:", "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "A9", "", "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "A10", "Email:", "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "A11", "Phone:", "Calibri", 11, "#000000", false);

			InvoiceCollection = sevk.PopulateInvoiceCollection(item.SiparisKodu,"Ahşap");
			if (!InvoiceCollection.Any())
				CRUDmessages.QueryIsEmpty("Siparişe Ait Bilgiler");

			string? cariAdi = InvoiceCollection.Select(x => x.CariAdi).FirstOrDefault();
			string? adres = InvoiceCollection.Select(x => x.CariAdres).FirstOrDefault();
			string adres1 = string.Empty;
			string adres2 = string.Empty;
			if(!string.IsNullOrEmpty(adres))
			{ 
				if (adres.Length > 45)
				{
					adres1 = adres.Substring(0, 45) ;
					adres2 = adres.Substring(46, Math.Min(45, adres.Length - 46));
				}
				else
				 adres1 = adres.Substring(0, Math.Min(45, adres.Length));
			}
			string? email = InvoiceCollection.Select(x => x.CariEmail).FirstOrDefault();
			string? tel = InvoiceCollection.Select(x => x.CariTel).FirstOrDefault();
			excelMethodsEPP.ShrinkToFit(existingPackage, sheetName, "B8", true);
			excelMethodsEPP.ShrinkToFit(existingPackage, sheetName, "B9", true);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "B7", cariAdi, "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "B8", adres1, "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "B9", adres2, "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "B10", email, "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "B11", tel, "Calibri", 11, "#000000", false);
			
			string? siparisTarih = string.Empty;
			if (!string.IsNullOrEmpty(InvoiceCollection.Select(x => x.SiparisTarih).FirstOrDefault()))
				  siparisTarih = InvoiceCollection.Select(x => x.SiparisTarih).FirstOrDefault().Substring(0, 10); 
			

			excelMethodsEPP.SetRightAlignIndent(existingPackage, sheetName, "F10:F11", 0);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "F10", "Order Number:", "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "F11", "Order Date:", "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "G10", item.SiparisKodu, "Calibri", 11, "#000000", false);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "G11", siparisTarih, "Calibri", 11, "#000000", false);

			excelMethodsEPP.SetRowHeight(existingPackage, sheetName, 12, 1);
			excelMethodsEPP.AddBottomBorder(existingPackage, sheetName, "A12:I12");
			excelMethodsEPP.SetRowHeight(existingPackage, sheetName, 13, 1);

			excelMethodsEPP.SetRowHeight(existingPackage, sheetName, 14, 40);
			excelMethodsEPP.TextWrap(existingPackage, sheetName,"A14:I14",true);

			DataTable dataTable = GetDataFromCollection(InvoiceCollection);
			int rowCount = dataTable.Rows.Count;
			decimal totalAmount = InvoiceCollection.Sum(item => Convert.ToDecimal(item.DovizTutarToplamSiparis));
			string? currency = InvoiceCollection.Select(item => item.DovizTipi.ToString()).FirstOrDefault();

			excelMethodsEPP.TextWrap(existingPackage, sheetName, "A15:C"+rowCount + 15, true);


			excelMethodsEPP.ExportDataToExcel(dataTable, existingPackage, sheetName, 14, 1);

			excelMethodsEPP.AddBottomBorder(existingPackage, sheetName, "A" + (rowCount + 14) + ":H" + (rowCount + 14));

			excelMethodsEPP.MergeAndRightAlignCells(existingPackage, sheetName, "A" + (rowCount + 15) + ":G" + (rowCount + 15));
			excelMethodsEPP.MergeAndRightAlignCells(existingPackage, sheetName, "H" + (rowCount + 15) + ":I" + (rowCount + 15));
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "A" + (rowCount + 15), "Total Amount (" + currency + "):","Calibri",13,"#000000",true);
			excelMethodsEPP.WriteTextToCell(existingPackage, sheetName, "H" + (rowCount + 15), totalAmount.ToString(),"Calibri",13,"#000000",true);
			
			Mouse.OverrideCursor = null;

			MessageBox.Show("Invoice Excele Aktarıldı.");
		}

		private static DataTable GetDataFromCollection(ObservableCollection<Cls_Sevk> invoiceCollection)
		{
			var dataTable = new DataTable();

			dataTable.Columns.Add("Product Code");
			dataTable.Columns.Add("Product Name");
			dataTable.Columns.Add("GTIP Code");
			dataTable.Columns.Add("Volume");
			dataTable.Columns.Add("Weight");
			dataTable.Columns.Add("Amount");
			dataTable.Columns.Add("Unit Price");
			dataTable.Columns.Add("Total Price");
			dataTable.Columns.Add("Currency");


			foreach (Cls_Sevk item in invoiceCollection)
			{
				
				if (item != null)
				{
					Cls_Sevk sevkValues = new();
					ObservableCollection<Cls_Sevk> agirlikCollection = sevkValues.GetProductVolumeAndWeight(item.UrunKodu,"Ahşap");
					item.UrunHacim = agirlikCollection.Select(item=> item.UrunHacim).FirstOrDefault();
					item.UrunAgirlik = agirlikCollection.Select(item => item.UrunAgirlik).FirstOrDefault();

					var dataRow = dataTable.NewRow();

					// Map the properties of cls_Irsaliye to the DataTable columns
					dataRow["Product Code"] = item.UrunKodu;
					dataRow["Product Name"] = item.UrunAdi;
					dataRow["GTIP Code"] = item.GTIPNo;
					dataRow["Volume"] = item.UrunHacim;
					dataRow["Weight"] = item.UrunAgirlik;
					dataRow["Unit Price"] = item.SiparisFiyatDovizUrun;
					dataRow["Total Price"] = item.DovizTutarToplamSiparis;
					dataRow["Currency"] = item.DovizTipi;

					dataTable.Rows.Add(dataRow);
				}
			}

			return dataTable;
		}

	}
}
