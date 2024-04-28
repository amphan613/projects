using System.Text.Encodings.Web;

namespace DataSanitize.services
{
    public interface IDataService
    {
         Task<string> GetHtmlData(); 
    }

    public class DataService : IDataService
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly UrlEncoder _urlEncoder;
        private readonly JavaScriptEncoder _javascriptEncoder;

        public DataService(HtmlEncoder htmlEncoder, UrlEncoder urlEncoder, JavaScriptEncoder javaScriptEncoder) {
        
                _htmlEncoder = htmlEncoder;
                
        }

        public async Task<string> GetHtmlData()
        {
            string encodedData = _htmlEncoder.Encode("<img src=x onerror=confirm(\"HACKED\")>");
            return await Task.FromResult(encodedData);
        }
    }
}
