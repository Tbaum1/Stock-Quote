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
        TextView txtSymbol, txtOpen, txtClose, txtHigh, txtLow, txtVolume, txtLast, txtDate, txtChange, txtChangePercent;
        //string webservice_url = "https://api.iextrading.com/1.0/stock/msft/chart/1y";

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
            txtLast = FindViewById<TextView>(Resource.Id.txtLast);
            txtDate = FindViewById<TextView>(Resource.Id.txtDate);
            txtChange = FindViewById<TextView>(Resource.Id.txtChange);
            txtChangePercent = FindViewById<TextView>(Resource.Id.txtChangePercent);
            string current = prefs.GetString("current", "no stok symbol found");
            string global = "Global Quote";

            try
            {
                var webRequestDaily = WebRequest.Create(new Uri("https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=" + current + "&apikey=56SDAVUQPBKNYAL5"));
                //var webRequestDaily = WebRequest.Create(new Uri("https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=" + current + "&apikey=56SDAVUQPBKNYAL5"));

                if (webRequestDaily != null)
                {

                    webRequestDaily.Method = "GET";
                    webRequestDaily.ContentType = "application/json";

                    //Get the response 
                    WebResponse wr = webRequestDaily.GetResponseAsync().Result;
                    Stream receiveStream = wr.GetResponseStream();
                    StreamReader reader = new StreamReader(receiveStream);
                    string resp = reader.ReadToEnd();
                    Newtonsoft.Json.Linq.JObject jobj = Newtonsoft.Json.Linq.JObject.Parse(resp);
                    txtDate.Text = jobj[global]["07. latest trading day"].ToString();
                    txtSymbol.Text = jobj[global]["01. symbol"].ToString();
                    txtLast.Text = jobj[global]["05. price"].ToString();
                    txtOpen.Text = jobj[global]["02. open"].ToString();
                    txtClose.Text = jobj[global]["08. previous close"].ToString();
                    txtHigh.Text = jobj[global]["03. high"].ToString();
                    txtLow.Text = jobj[global]["04. low"].ToString();
                    txtVolume.Text = jobj[global]["06. volume"].ToString();
                    txtChange.Text = jobj[global]["09. change"].ToString();
                    txtChangePercent.Text = jobj[global]["10. change percent"].ToString();
                    //txtSymbol.Text = current;
                    //string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    //txtOpen.Text = jobj["Time Series (Daily)"][currentDate]["1. open"].ToString();
                    //txtHigh.Text = jobj["Time Series (Daily)"][currentDate]["2. high"].ToString();
                    //txtLow.Text = jobj["Time Series (Daily)"][currentDate]["3. low"].ToString();
                    //txtClose.Text = jobj["Time Series (Daily)"][currentDate]["4. close"].ToString();
                    //txtVolume.Text = jobj["Time Series (Daily)"][currentDate]["5. volume"].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}