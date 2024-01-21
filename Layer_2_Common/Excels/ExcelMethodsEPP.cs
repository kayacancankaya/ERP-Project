using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Operators;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Windows;

namespace Layer_2_Common.Excels
{
    public class ExcelMethodsEPP
    {

        ExcelPackage excelPackage = new ExcelPackage();

        public ExcelWorksheet Worksheet { get; set; }

        

        Color color = new ();
      

        public string CreateExcelFile(string filePath, string sheetName)
        {
            string fileExtension = ".xlsx";
            string filePathFinal = string.Empty;
            int fileCounter = 1;

            do
            {
                // Generate the full file path with a unique name
                filePathFinal = $"{filePath}_{fileCounter}{fileExtension}";
                fileCounter++;
            }
            while (File.Exists(filePathFinal)); // Continue generating until a unique file name is found

			using (var newExcelPackage = new ExcelPackage())
			{
				ExcelWorksheet worksheet = newExcelPackage.Workbook.Worksheets.Add(sheetName);
                // Save the Excel package to the final file path
                newExcelPackage.SaveAs(new FileInfo(filePathFinal));
			}

			return filePathFinal;
        }
        
        public bool CreateExcelSheetIfDoesntExists(ExcelPackage excelPackage, string filePath, string sheetName)
        {
            try
            {
                var result = excelPackage.Workbook.Worksheets.Where(ws => ws.Name == sheetName).Any();

                if (!result)
                {
                    excelPackage.Workbook.Worksheets.Add(sheetName);
                    SetRowHeight(excelPackage, sheetName, 1, 3);
                }
                else
                {
                    //clear content
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == sheetName);
                    worksheet.Cells.Clear();
                }

                excelPackage.SaveAs(new FileInfo(filePath));
                
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public DataTable ReadExcelFile(string filePath, int worksheetNumber,int startingColumn,int endingColumn
                                        ,int startingRow)
        {
            try
            {
                DataTable? dataTable = new();

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {

                    Worksheet = package.Workbook.Worksheets[worksheetNumber];

                    for (int col = startingColumn; col <= endingColumn; col++)
                    {
                        dataTable.Columns.Add(Worksheet.Cells[startingRow, col].Text);
                    }

                    for (int row = startingRow + 1; row <= Worksheet.Dimension.End.Row; row++)
                    {
                        if (Worksheet.Cells[row, startingColumn].Text == "")
                            break;

                        var dataRow = dataTable.NewRow();
                        for (int col = startingColumn; col <= endingColumn; col++)
                        {
                            dataRow[col - startingColumn] = Worksheet.Cells[row, col].Text;
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
                return dataTable;

            }
            catch (Exception ex) 
            {
                var message = ex.Message.ToString();  return null;
            }
            
        }

		public void CreateExcelSheet(string sheetName, string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            // Open the existing Excel file if it exists, or create a new one if it doesn't
            if (fileInfo.Exists)
            {
                using (var existingPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = existingPackage.Workbook.Worksheets.Add(sheetName);

                    existingPackage.Save();
                }
            }
            else
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);
                excelPackage.SaveAs(fileInfo); // Use SaveAs to create a new file
            }
        }

        public void OpenExcelFile(string filePath)
        {

            Process.Start(filePath);
        }
        public void CloseExcelFile(string filePath)
        {
            try
            {
                // Get a list of all running Excel processes
                Process[] excelProcesses = Process.GetProcessesByName("EXCEL");

                // Loop through the list of processes and try to close the one associated with the file
                foreach (Process process in excelProcesses)
                {
                    if (process.MainWindowTitle.Contains(filePath))
                    {
                        // Close the Excel process
                        process.CloseMainWindow();
                        process.WaitForExit();
                        process.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur while trying to close Excel
                MessageBox.Show("Exceli Kapatırken Hata Oluştu: " + ex.Message);
            }
        }
        public void SaveAsExcel(string filePath)
        {
            try
            {
                excelPackage.SaveAs(new FileInfo(filePath));
            }
            catch (Exception ex)
            {


                // Handle any exceptions that may occur while trying to close Excel
                MessageBox.Show("Excel'i kaydederken hata oluştu: " + ex.Message);

            }

        }
        public void SetColumnWidth(ExcelPackage existingPackage, string sheetName, int columnNumber, double columnWidth)
        { 
               var worksheet = existingPackage.Workbook.Worksheets[sheetName];
               worksheet.Column(columnNumber).Width = columnWidth;
               existingPackage.Save();   
        }

        public void SetRowHeight(ExcelPackage existingPackage, string sheetName, int rowNumber, double rowHeight)
        {
            var worksheet = existingPackage.Workbook.Worksheets[sheetName];

            worksheet.Row(rowNumber).Height = rowHeight;

            existingPackage.Save();
        }
        public void MergeAndCenterCells(ExcelPackage existingPackage, string worksheetName, string range)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            // Merge and center cells (e.g., merge cells A1 to D1 and center the content)
            worksheet.Cells[range].Merge = true;
            worksheet.Cells[range].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[range].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            existingPackage.Save();
        }
        public void MergeAndLeftAlignCells(ExcelPackage existingPackage, string worksheetName, string range)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            // Merge and center cells (e.g., merge cells A1 to D1 and center the content)
            worksheet.Cells[range].Merge = true;
            worksheet.Cells[range].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            worksheet.Cells[range].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            existingPackage.Save();
        }
        public void MergeAndRightAlignCells(ExcelPackage existingPackage, string worksheetName, string range)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            // Merge and center cells (e.g., merge cells A1 to D1 and center the content)
            worksheet.Cells[range].Merge = true;
            worksheet.Cells[range].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            worksheet.Cells[range].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            existingPackage.Save();
        }
        public void RightAlignCells(ExcelPackage existingPackage, string worksheetName, string range)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            worksheet.Cells[range].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            worksheet.Cells[range].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            existingPackage.Save();
        }
        public void ChangeColor(ExcelPackage existingPackage, string worksheetName, string range, string colorCode)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];
            // Create a new style with the specified background color
            var cellStyle = worksheet.Cells[range].Style;
            cellStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cellStyle.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(colorCode));
            existingPackage.Save();
        }
        public void InsertImage(ExcelPackage existingPackage, string worksheetName, string imagePath, string imageName, int columnNo, int rowNo, int width, int height)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            // Add the image to the worksheet
            var picture = worksheet.Drawings.AddPicture(imageName, new FileInfo(imagePath));
            picture.From.Column = columnNo - 1;
            picture.From.Row = rowNo - 1;
            picture.SetSize(width, height);

            existingPackage.Save();
        }
        public void MoveImageToCell(ExcelPackage existingPackage, string worksheetName, string imageName, int cellsToMoveRight)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];
            var picture = worksheet.Drawings[imageName] as ExcelPicture;
            picture.EditAs = eEditAs.TwoCell;
            picture.From.ColumnOff = cellsToMoveRight;
            picture.To.ColumnOff = cellsToMoveRight;

            existingPackage.Save();
        }
        public void SetLeftAlignIndent(ExcelPackage existingPackage, string worksheetName, string rangeAddress, int indentLevel)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            // Set the horizontal alignment to left-align with the specified left indent level
            worksheet.Cells[rangeAddress].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            // Set the left indent level
            worksheet.Cells[rangeAddress].Style.Indent = indentLevel;

            existingPackage.Save();
        }
        public void SetRightAlignIndent(ExcelPackage existingPackage, string worksheetName, string rangeAddress, int indentLevel)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            // Set the horizontal alignment to right-align
            worksheet.Cells[rangeAddress].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            // Set the left indent level
            worksheet.Cells[rangeAddress].Style.Indent = indentLevel;
            existingPackage.Save();
        }
        public void SetVerticalAlignment(ExcelPackage existingPackage, string worksheetName, string cellAddress)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            // Set the vertical alignment of the cell to bottom
            worksheet.Cells[cellAddress].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

            existingPackage.Save();
        }
        public void TextWrap(ExcelPackage existingPackage, string worksheetName, string rangeAddress, bool status)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            worksheet.Cells[rangeAddress].Style.WrapText = status;

            existingPackage.Save();
        }
		public void FontBold(ExcelPackage existingPackage, string sheetName, string rangeAddress, bool bold)
		{
			var worksheet = existingPackage.Workbook.Worksheets[sheetName];

			worksheet.Cells[rangeAddress].Style.Font.Bold = bold; 
		}
		public void FontSize(ExcelPackage existingPackage, string sheetName, string rangeAddress, int size)
		{
			var worksheet = existingPackage.Workbook.Worksheets[sheetName];

			worksheet.Cells[rangeAddress].Style.Font.Size = size; 
		}
		public void ShrinkToFit(ExcelPackage existingPackage, string worksheetName, string rangeAddress, bool beShrinked)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            // Set the "Shrink to Fit" property for the specified range of cells
            worksheet.Cells[rangeAddress].Style.ShrinkToFit = beShrinked;

            existingPackage.Save();
        }
        public void SetCellBackgroundColor(ExcelPackage existingPackage, string worksheetName, string cellAddress, string hexColorCode)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];
            // Set the background color of the cell
            color = ColorTranslator.FromHtml(hexColorCode);
            worksheet.Cells[cellAddress].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			worksheet.Cells[cellAddress].Style.Fill.BackgroundColor.SetColor(color);
			existingPackage.Save();
		}
        public void AddBottomBorder(ExcelPackage existingPackage, string worksheetName, string rangeAddress)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            // Get the cell range to which you want to add the bottom border
            var cellRange = worksheet.Cells[rangeAddress];

            // Set the border style for the bottom border
            cellRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            // Optionally, you can set the color of the bottom border
            cellRange.Style.Border.Bottom.Color.SetColor(Color.Black);

            existingPackage.Save();
        }

		public void VerticalAlignCenter(ExcelPackage existingPackage, string worksheetName, string rangeAddress)
		{
			var worksheet = existingPackage.Workbook.Worksheets[worksheetName];
            worksheet.Cells[rangeAddress].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
			existingPackage.Save();
		}
		public void VerticalAlignTop(ExcelPackage existingPackage, string worksheetName, string rangeAddress)
		{
			var worksheet = existingPackage.Workbook.Worksheets[worksheetName];
            worksheet.Cells[rangeAddress].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
			existingPackage.Save();
		}
		public void VerticalAlignBottom(ExcelPackage existingPackage, string worksheetName, string rangeAddress)
		{
			var worksheet = existingPackage.Workbook.Worksheets[worksheetName];
            worksheet.Cells[rangeAddress].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
			existingPackage.Save();
		}

		public void WriteTextToCell(ExcelPackage existingPackage, string worksheetName, string rangeAddress, string content, string fontName, int fontSize, string fontColorCode, bool bold)
        {
            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            color = ColorTranslator.FromHtml(fontColorCode);
            // Set the horizontal alignment to right-align
            worksheet.Cells[rangeAddress].Value = content;
            worksheet.Cells[rangeAddress].Style.Font.Size = fontSize;
            worksheet.Cells[rangeAddress].Style.Font.Name = fontName;
            worksheet.Cells[rangeAddress].Style.Font.Color.SetColor(color);
            worksheet.Cells[rangeAddress].Style.Font.Bold = bold;
            existingPackage.Save();
        }
        public void ExportDataToExcel(DataTable dataTable, ExcelPackage existingPackage, string worksheetName, int rowNumber, int columnNumber)
        {

            var worksheet = existingPackage.Workbook.Worksheets[worksheetName];

            int rowCount = dataTable.Rows.Count;
            int columnCount = dataTable.Columns.Count;
            // Export column headers

            for (int j = 0; j < columnCount; j++)
            {
                worksheet.Cells[rowNumber, columnNumber + j].Value = dataTable.Columns[j].ColumnName;
            }
            rowNumber++;

            // Export data rows
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    worksheet.Cells[rowNumber + i, columnNumber + j].Value = dataTable.Rows[i][j];
                }
            }


			// Save the Excel package to a file
			existingPackage.Save();
        }
		
		public void CreateStyledTable(ExcelPackage existingPackage, string sheetName, 
                                      string headerRange, string headerHexColor, 
                                      int rowCount, int startRow, 
                                      int columnCount, int startColumn,
                                      string firstStripeColor, string secondStripeColor, string tableName)
		{

			    var worksheet = existingPackage.Workbook.Worksheets[sheetName];

			    worksheet.Tables.Add(new ExcelAddressBase(startRow, startColumn,startRow + rowCount, startColumn + columnCount - 2), tableName);

				SetCellBackgroundColor(existingPackage, sheetName, headerRange, headerHexColor);
                FontBold(existingPackage, sheetName, headerRange, true);
                FontSize(existingPackage, sheetName, headerRange, 11);

				for (int i = 1; i < rowCount; i++)
				{
					string rowRange = $"{GetExcelColumnLetter(startColumn)}{startRow + i}:{GetExcelColumnLetter(startColumn + columnCount - 2)}{startRow + i}";

					string stripeColor = (i % 2 == 0) ? firstStripeColor : secondStripeColor;
					SetCellBackgroundColor(existingPackage, sheetName, rowRange, stripeColor);
					worksheet.Row(startRow + i).Height = -1; // -1 indicates autofit
				
			}

			    existingPackage.Save();
			
		}

		private void SetTableBorders(ExcelPackage existingPackage, string sheetName, int startRow, int startColumn, int rowCount, int columnCount)
		{
			var worksheet = existingPackage.Workbook.Worksheets[sheetName];

			// Set table side borders
			worksheet.Cells[startRow, startColumn, startRow + rowCount - 1, startColumn + columnCount - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
			worksheet.Cells[startRow, startColumn, startRow + rowCount - 1, startColumn + columnCount - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
			worksheet.Cells[startRow, startColumn, startRow + rowCount - 1, startColumn + columnCount - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
			worksheet.Cells[startRow, startColumn, startRow + rowCount - 1, startColumn + columnCount - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
		}

		public string GetExcelColumnLetter(int columnIndex)
		{
			
			string columnName = string.Empty;

			while (columnIndex > 0)
			{
				int modulo = (columnIndex - 1) % 26;
				columnName = Convert.ToChar(65 + modulo) + columnName;
				columnIndex = (columnIndex - modulo) / 26;
			}

			return columnName;
		}

        public void ChangeNumberFormat(ExcelPackage existingPackage, string sheetName, string numberFormat)
        {
            var worksheet = existingPackage.Workbook.Worksheets[sheetName];

            var cells = worksheet.Cells[range];

            foreach (var cell in cells)
            {
                cell.Style.Numberformat.Format = numberFormat;
            }

            existingPackage.Save();
        }

        public int GetFirstBlankRow(ExcelPackage existingPackage,string sheetName) 
        {
            ExcelWorksheet worksheet = existingPackage.Workbook.Worksheets[sheetName]; 

            int lastUsedRow = worksheet.Dimension.End.Row;
            return lastUsedRow + 1;
        }

    }
}
