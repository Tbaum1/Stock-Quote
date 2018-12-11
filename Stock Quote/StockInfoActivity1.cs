using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using static Android.Provider.ContactsContract;

namespace Stock_Quote
{
    [Activity(Label = "StockInfoActivity1")]
    public class StockInfoActivity1 : Activity
    {
        private ISharedPreferences prefs = Application.Context.GetSharedPreferences("APP_DATA", FileCreationMode.Private);
        TextView txtSymbol, txtOpen, txtClose, txtHigh, txtLow, txtVolume;
        //string webservice_url = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=";
        string webservice_url = "https://api.iextrading.com/1.0/stock/msft/chart/1y";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_stock_info);
            txtSymbol = FindViewById<TextView>(Resource.Id.txtSymbol);
            txtOpen = FindViewById<TextView>(Resource.Id.txtOpen);
            txtClose = FindViewById<TextView>(Resource.Id.txtClose);
            txtHigh = FindViewById<TextView>(Resource.Id.txtHigh);
            txtLow = FindViewById<TextView>(Resource.Id.txtLow);
            txtVolume = FindViewById<TextView>(Resource.Id.txtVolume);
            string current = prefs.GetString("current", "no stok symbol found");
            
            //txtSymbol.Text = current;

            try
            {
                //webservice_url = webservice_url + current + "&apikey=56SDAVUQPBKNYAL5";

                //Uri url = new Uri(webservice_url);
                var webRequest = WebRequest.Create(new Uri("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=" + current + "&apikey=56SDAVUQPBKNYAL5"));
                //var webRequest = WebRequest.Create(webservice_url);

                if (webRequest != null)
                {
                    
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";

                    //Get the response 
                    WebResponse wr = webRequest.GetResponseAsync().Result;
                    Stream receiveStream = wr.GetResponseStream();
                    StreamReader reader = new StreamReader(receiveStream);
                    string resp = reader.ReadToEnd();
                    Newtonsoft.Json.Linq.JObject jobj = Newtonsoft.Json.Linq.JObject.Parse(resp);
                    
                    //RootObject currentStockInfo = JsonConvert.DeserializeObject<RootObject>(reader.ReadToEnd());

                    
                    txtSymbol.Text = current;
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    txtOpen.Text = jobj["Time Series (Daily)"][currentDate]["1. open"].ToString();
                    txtHigh.Text = jobj["Time Series (Daily)"][currentDate]["2. high"].ToString();
                    txtLow.Text = jobj["Time Series (Daily)"][currentDate]["3. low"].ToString();
                    txtClose.Text = jobj["Time Series (Daily)"][currentDate]["4. close"].ToString();
                    txtVolume.Text = jobj["Time Series (Daily)"][currentDate]["5. volume"].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
    //public class MetaData
    //{
    //    public string Information { get; set; }
    //    public string Symbol { get; set; }
    //    public string Last { get; set; }
    //    public string Size { get; set; }
    //    public string TimeZone { get; set; }
    //}

    //public class StockInfo
    //{
    //    public string Open { get; set; }
    //    public string High { get; set; }
    //    public string Low { get; set; }
    //    public string Close { get; set; }
    //    public string Volume { get; set; }
    //}

    //public class RestResponse
    //{       
    //    public MetaData metaData { get; set; }
    //    public StockInfo stockInfo { get; set; }
    //}

    //public class Stock
    //{
    //    public RestResponse RestResponse { get; set; }
    //}

    //public class RootObject
    //{
    //    public string date { get; set; }
    //    public double open { get; set; }
    //    public double high { get; set; }
    //    public double low { get; set; }
    //    public double close { get; set; }
    //    public int volume { get; set; }
    //    public int unadjustedVolume { get; set; }
    //    public double change { get; set; }
    //    public double changePercent { get; set; }
    //    public double vwap { get; set; }
    //    public string label { get; set; }
    //    public double changeOverTime { get; set; }
    //}

}