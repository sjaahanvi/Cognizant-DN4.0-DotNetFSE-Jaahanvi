public class PdfDocumentFactory : DocumentFactory
{
    public override Document CreateDocument() => new PdfDocument();
}
