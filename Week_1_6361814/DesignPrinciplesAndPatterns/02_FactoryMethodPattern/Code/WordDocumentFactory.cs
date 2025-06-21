public class WordDocumentFactory : DocumentFactory
{
    public override Document CreateDocument() => new WordDocument();
}

