using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using Context = Android.Content.Context;

namespace Stock_Quote
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Spinner sp;
        ArrayAdapter adapter;
        ArrayList favorites;
        Button addFavorite;
        ImageButton search;
        EditText et;
        private ISharedPreferences prefs = Application.Context.GetSharedPreferences("APP_DATA", FileCreationMode.Private);
        int count;
        string currentStock;
        Intent stockInfoActivity;
        


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            
            addFavorite = FindViewById<Button>(Resource.Id.btnAddFavorite);
            sp = FindViewById<Spinner>(Resource.Id.sp);
            et = FindViewById<EditText>(Resource.Id.editTxtSymbol);            
            count = prefs.GetInt("count", 0);
            favorites = new ArrayList();
            favorites.Add("Pick One");            
            LoadFavorites();
            adapter = new ArrayAdapter(this,Android.Resource.Layout.SimpleListItem1,favorites);
            sp.Adapter = adapter;            
            sp.ItemSelected += Sp_ItemSelected;
            addFavorite.Click += AddFavorite_Click;
            search = FindViewById<ImageButton>(Resource.Id.imgBtnSearch);
            search.Click += Search_Click;
            stockInfoActivity = new Intent(this, typeof(StockInfoActivity1));
        }

        private void Search_Click(object sender, EventArgs e)
        {
            ISharedPreferencesEditor editor = prefs.Edit();
            currentStock = et.Text.ToUpper();
            editor.PutString("current", currentStock);
            editor.Apply();
            StartActivity(stockInfoActivity);
        }

        private void AddFavorite_Click(object sender, EventArgs e)
        {            
            LoadFavorites();
            string temp;
            temp = et.Text.ToUpper();
            ISharedPreferencesEditor editor = prefs.Edit();
            

            if (CheckFavorites(temp))
            {
                Toast.MakeText(this, "Already in favorites", ToastLength.Short).Show();
            }
            else
            {
                favorites.Add(temp);
                editor.PutString("symbol" + count, temp);
                count++;
                editor.PutInt("count", count);
                editor.Apply();
                LoadFavorites();
            }
            this.Recreate();
            adapter.NotifyDataSetChanged();
        }

        private bool CheckFavorites(string s)
        {
            if (favorites.Contains(s))
            {                
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Sp_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ISharedPreferencesEditor editor = prefs.Edit();
            currentStock = sp.GetItemAtPosition(e.Position).ToString().ToUpper();
            editor.PutString("current", currentStock);
            editor.Apply();
            StartActivity(stockInfoActivity);
        }

        private void LoadFavorites()
        {
            ISharedPreferencesEditor editor = prefs.Edit();
            string t;
            for (int i = 0; i < 21; i++)
            {
                t = Convert.ToString(i);
                favorites.Add(prefs.GetString("symbol" + t, " ").ToString());
            }                
        }
    }    
}