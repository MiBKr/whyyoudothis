namespace Alma
{
    class Program
    {
        static void Main(string[] args)
        {
            AlmaPrinter alma = new AlmaPrinter(@"D:\Library\Aalto\Books\");
            alma.GetBook("IACBXXBTAFJB");
            PDFMerger.MergeMultiplePDFIntoSinglePDF(alma.bookPath, alma.outputPath);
            //PDFMerger.MergeMultiplePDFIntoSinglePDF(@"D:\Library\Aalto\Books\Sopimusoikeus", @"D:\Library\Aalto\Books\Sopimusoikeus.pdf");
        }
    }
}
