using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;

namespace Alma
{
    public static class PDFMerger
    {
        public static void MergeMultiplePDFIntoSinglePDF(string _inputPath, string _outputFilePath )
        {
            string[] pdfFiles = Directory.GetFiles(_inputPath);
            Console.WriteLine("Merging started.....");
            PdfDocument outputPDFDocument = new PdfDocument();
            foreach (string pdfFile in pdfFiles)
            {
                if(FileReadyToRead(pdfFile, 40))
                {
                    PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);
                    outputPDFDocument.Version = inputPDFDocument.Version;
                    foreach (PdfPage page in inputPDFDocument.Pages)
                    {
                        outputPDFDocument.AddPage(page);
                    }
                    Console.WriteLine(pdfFile + " is merged.");
                }
                else
                {
                    Console.WriteLine(pdfFile + " is fucked.");
                }
            }
            outputPDFDocument.Save(_outputFilePath);
            Console.WriteLine("Merging Completed");
        }
        private static bool FileReadyToRead(string filePath, int maxDuration)
        {
            int readAttempt = 0;
            while (readAttempt < maxDuration)
            {
                readAttempt++;
                try
                {
                    using (StreamReader stream = new StreamReader(filePath))
                    {
                        return true;
                    }
                }
                catch
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            return false;
        }
    }

}
