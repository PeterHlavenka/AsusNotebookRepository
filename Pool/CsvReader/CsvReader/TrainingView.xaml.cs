using System;
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

    private ObservableCollection<TranslatedObject> AllowedWords { get; set; }
    private List<TranslatedObject> AllWords { get; set; } = new();
    private int CurrentPositon { get; set; }
    public List<LanguageInfo> AllowedLanguages { get; set; } = new();

    public void DoNext()
    {
        CurrentPositon += 1;
        var newWord = AllowedWords.FirstOrDefault(d => int.Parse(d.Position) == CurrentPositon);

        if (newWord == null) CurrentPositon = 1;

        newWord = AllowedWords.First(d => int.Parse(d.Position) == CurrentPositon);
        var language = AllowedLanguages.ElementAt(new Random().Next(AllowedLanguages.Count));
        
        if (language.Name == LanguageInfo.CzName)
            TextBlock.Text = newWord.Cz;
        if (language.Name == LanguageInfo.EnName)
            TextBlock.Text = newWord.En;
        if (language.Name == LanguageInfo.DeName)
            TextBlock.Text = newWord.De;
        
        CounterTextBlock.Text = newWord.Position;
    }

    public void Initialize(FileInfo inputFileName)
    {
        using (var excelEngine = new ExcelEngine())
        {
            //Initialize application
            var app = excelEngine.Excel;

            //Set default application version as Xlsx
            app.DefaultVersion = ExcelVersion.Xlsx;

            //Open existing Excel workbook from the specified location
            var workbook = app.Workbooks.Open(inputFileName.FullName, ExcelOpenType.Automatic);

            //Access the first worksheet
            var worksheet = workbook.Worksheets[0];

            //Access the used range of the Excel file
            var usedRange = worksheet.UsedRange;
            var lastRow = usedRange.LastRow;
            var lastColumn = usedRange.LastColumn;
            //Iterate the cells in the used range and print the cell values
            var translatedObjects = new List<TranslatedObject>();
            var obj = new TranslatedObject();
            for (var row = 2; row <= lastRow; row++) // indexy od 1 + preskakuju header row
            {
                for (var col = 1; col <= lastColumn; col++)
                {
                    var propertyInfo = obj.GetType().GetProperties().ElementAt(col - 1);
                    propertyInfo.SetValue(obj, worksheet[row, col].Value);
                }

                translatedObjects.Add(new TranslatedObject(obj.Position, obj.Cz, obj.En, obj.De));
            }

            AllWords = translatedObjects.Where(d => !(string.IsNullOrWhiteSpace(d.Cz) &&
                                                       string.IsNullOrWhiteSpace(d.En) &&
                                                       string.IsNullOrWhiteSpace(d.De))).ToList();
            AllowedWords = AllWords.ToObservableCollection();
            CurrentPositon = 0;
            TextBlock.Text = AllowedWords.ElementAt(CurrentPositon).Cz;
            CounterTextBlock.Text = AllowedWords.ElementAt(CurrentPositon).Position;
        }
    }
}