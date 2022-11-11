using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Syncfusion.Data.Extensions;
using Syncfusion.XlsIO;

namespace CsvReader;

public partial class TrainingView : UserControl
{
    public TrainingView()
    {
        InitializeComponent();
    }

    public void DoNext()
    {
	    CurrentPositon += 1;
	    var newWord = Words.FirstOrDefault(d => int.Parse(d.Position) == CurrentPositon);

	    if (newWord == null)
	    {
		    CurrentPositon = 1;
	    }
            
	    newWord = Words.First(d => int.Parse(d.Position) == CurrentPositon);
	    TextBlock.Text = newWord.Cz;
	    CounterTextBlock.Text = newWord.Position;
    }
    
    private ObservableCollection<TranslatedObject> Words { get; set; }
    private int CurrentPositon { get; set; }
    public List<LanguageInfo> AllowedLanguages { get; set; } = new();

    public void Initialize(FileInfo inputFileName)
    {
	    using (ExcelEngine excelEngine = new ExcelEngine())
	    {
		    //Initialize application
		    IApplication app = excelEngine.Excel;

		    //Set default application version as Xlsx
		    app.DefaultVersion = ExcelVersion.Xlsx;

		    //Open existing Excel workbook from the specified location
		    IWorkbook workbook = app.Workbooks.Open(inputFileName.FullName, ExcelOpenType.Automatic);

		    //Access the first worksheet
		    IWorksheet worksheet = workbook.Worksheets[0];
		    
			//Access the used range of the Excel file
		    IRange usedRange = worksheet.UsedRange;                
		    int lastRow = usedRange.LastRow;
		    int lastColumn = usedRange.LastColumn;
		    //Iterate the cells in the used range and print the cell values
		    var translatedObjects = new List<TranslatedObject>();
		    var obj = new TranslatedObject();
		    for (int row = 2; row <= lastRow; row++)  // indexy od 1 + preskakuju header row
		    {
			    for (int col = 1; col <= lastColumn; col++)
			    {
				    var propertyInfo = obj.GetType().GetProperties().ElementAt(col - 1);
				    propertyInfo.SetValue(obj, worksheet[row, col].Value);
			    }
			    translatedObjects.Add(new TranslatedObject(obj.Position, obj.Cz, obj.En, obj.De));
		    }

		    Words = translatedObjects.Where(d => !d.IsEmpty).ToObservableCollection();
		    CurrentPositon = 0;
		    TextBlock.Text = Words.ElementAt(CurrentPositon).Cz;
		    CounterTextBlock.Text = Words.ElementAt(CurrentPositon).Position;
	    }
    }
}