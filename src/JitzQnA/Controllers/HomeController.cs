using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using JitzQnA.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace JitzQnA.Controllers
{
    public class HomeController : Controller
    {
        private static string _OcpApimSubscriptionKey;
        private static string _KnowledgeBase;

        public HomeController(IOptions<CustomSettings> CustomSettings)
        {
            _OcpApimSubscriptionKey = CustomSettings.Value.OcpApimSubscriptionKey;
            _KnowledgeBase = CustomSettings.Value.KnowledgeBase;
        }

        public async Task<ActionResult> Index(string searchString)
        {
            QnAQuery objQnAResult = new QnAQuery();
            try
            {
                if (searchString != null)
                {
                    objQnAResult = await QueryQnABot(searchString);
                }
                return View(objQnAResult);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View(objQnAResult);
            }
        }

        private static async Task<QnAQuery> QueryQnABot(string Query)
        {
            QnAQuery QnAQueryResult = new QnAQuery();
            using (System.Net.Http.HttpClient client =
                new System.Net.Http.HttpClient())
            {
                string RequestURI = String.Format("{0}{1}{2}{3}{4}",
                    @"https://westus.api.cognitive.microsoft.com/",
                    @"qnamaker/v1.0/",
                    @"knowledgebases/",
                    _KnowledgeBase,
                    @"/generateAnswer");
                var httpContent =
                    new StringContent($"{{\"question\": \"{Query}\"}}",
                    Encoding.UTF8, "application/json");
                httpContent.Headers.Add(
                    "Ocp-Apim-Subscription-Key", _OcpApimSubscriptionKey);
                System.Net.Http.HttpResponseMessage msg =
                    await client.PostAsync(RequestURI, httpContent);
                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse =
                        await msg.Content.ReadAsStringAsync();
                    QnAQueryResult =
                        JsonConvert.DeserializeObject<QnAQuery>(JsonDataResponse);
                }
            }
            return QnAQueryResult;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
