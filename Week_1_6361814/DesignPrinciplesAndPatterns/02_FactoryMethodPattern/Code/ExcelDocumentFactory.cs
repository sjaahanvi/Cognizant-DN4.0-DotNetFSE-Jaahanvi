public class ExcelDocumentFactory : DocumentFactory
{
    public override Document CreateDocument() => new ExcelDocument();
}
