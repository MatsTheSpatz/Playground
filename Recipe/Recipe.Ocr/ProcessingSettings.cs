using System;
using System.Text;

namespace RecipeOcr
{
    public class ProcessingSettings 
    {
        private string _language = "english";

        public ProcessingSettings()
        {
            Language = RecognitionLanguage.English;
        }

        public RecognitionLanguage Language
        {
            set { _language = value.ToString(); }
        }

        public void SetLanguage(string lang)
        {
            _language = lang;
        }

        public OutputFormat OutputFormat
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public TextType TextTypes
        {
            get; set;
        }

        /// <summary>
        /// Any Url parameters that are passed as-is to the server
        /// </summary>
        public String CustomOptions
        {
            get; set;
        }

        /// <summary>
        /// Gets default extension for given output format
        /// </summary>
        public string OutputFileExt
        {
            get
            {
                switch (this.OutputFormat)
                {
                    case OutputFormat.docx:
                        return ".docx";
                    case OutputFormat.pdfSearchable:
                    case OutputFormat.pdfTextAndImages:
                        return ".pdf";
                    case OutputFormat.pptx:
                        return ".pptx";
                    case OutputFormat.rtf:
                        return ".rtf";
                    case OutputFormat.txt:
                        return ".txt";
                    case OutputFormat.xlsx:
                        return ".xlsx";
                    case OutputFormat.xml:
                        return ".xml";
                }

                return ".bin";
            }
        }

        public string AsUrlParams
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append(String.Format("language={0}&exportFormat={1}",
                    Uri.EscapeDataString(_language),
                    Uri.EscapeDataString(OutputFormat.ToString())));

                string textType = TextTypes.AsUrlParams();
                if (!String.IsNullOrEmpty(textType))
                    result.AppendFormat("&textType={0}", textType);

                if (!String.IsNullOrEmpty(CustomOptions))
                    result.Append("&"+CustomOptions);

                return result.ToString();
            }
        }
    }
}
