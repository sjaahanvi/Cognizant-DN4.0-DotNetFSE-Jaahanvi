class Program
{
    static void Main(string[] args)
    {
        DocumentFactory factory;

        factory = new WordDocumentFactory();
        var wordDoc = factory.CreateDocument();
        wordDoc.Print();

        factory = new PdfDocumentFactory();
        var pdfDoc = factory.CreateDocument();
        pdfDoc.Print();

        factory = new ExcelDocumentFactory();
        var excelDoc = factory.CreateDocument();
        excelDoc.Print();
    }
}
