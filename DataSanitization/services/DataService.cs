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

        public DataService(
            HtmlEncoder htmlEncoder, 
            UrlEncoder urlEncoder, 
            JavaScriptEncoder javaScriptEncoder)
        {
            _htmlEncoder = htmlEncoder;
            _urlEncoder = urlEncoder;
            _javascriptEncoder = javaScriptEncoder;
        }

        public async Task<string> GetHtmlData()
        {
            string htmlData = "<img src=x onerror=confirm(\"HACKED\")><script><%>";
           // string encodedData = _htmlEncoder.Encode(htmlData);
            return await Task.FromResult(htmlData);
        }
    }
}
