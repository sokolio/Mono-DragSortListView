using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using DragSortList;
using System.Collections.Generic;
using Android.Util;
using Android.Graphics;

namespace DragSortList
{
    [Activity(Label = "DemoDragDropListView", MainLauncher = true, Icon = "@drawable/icon")]
    public class DragSortListActivity : Activity
    {
        private DragSortListView listView;
        //private ListView listView;
        private CountriesAdapter simpleAdapter;
        //private DragSortListAdapter simpleAdapter;
        //DslvAdapterWrapper simpleAdapter;
        private List<string> list;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_drag_sort_list_view);
            
            listView = listView?? FindViewById<DragSortListView>(Resource.Id.simpleDragListview);

            initList();

            simpleAdapter = new CountriesAdapter(listView, list);

            listView.SetAdapter(simpleAdapter);

            listView.ItemClick += listView_ItemClick;

        }

        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            listView.SetItemChecked(e.Position, true);
            simpleAdapter.NotifyDataSetChanged();
        }

        private void initList() {

		    if (list == null)
			    list = new List<String>();

		    list.Add("Russia");
		    list.Add("Canada");
		    list.Add("United States of America");
		    list.Add("China");
		    list.Add("Brazil");
		    list.Add("Australia");
		    list.Add("India");
		    list.Add("Argentina");
		    list.Add("Kazakhstan");
		    list.Add("Sudan");
		    list.Add("Algeria");
		    list.Add("Congo (Dem. Rep. of )");
		    list.Add("Mexico");
		    list.Add("Saudi Arabia");
		    list.Add("Indonesia");
		    list.Add("Libya");
		    list.Add("Iran");
		    list.Add("Mongolia");
		    list.Add("Peru");
		    list.Add("Chad");
		    list.Add("Niger");
		    list.Add("Angola");
		    list.Add("Mali");
		    list.Add("South Africa  ");
		    list.Add("Colombia");
		    list.Add("Ethiopia");
		    list.Add("Bolivia  ");
		    list.Add("Mauritania");
		    list.Add("Egypt");
		    list.Add("Tanzania  ");
		    list.Add("Nigeria");
		    list.Add("Venezuela");
		    list.Add("Namibia");
		    list.Add("Pakistan");
		    list.Add("Mozambique");
		    list.Add("Turkey");
		    list.Add("Chile  ");
		    list.Add("Zambia");
		    list.Add("Myanmar");
		    list.Add("Afghanistan");
		    list.Add("Somalia");
		    list.Add("Central African Republic");
		    list.Add("Ukraine");
		    list.Add("Botswana");
		    list.Add("Madagascar");
		    list.Add("Kenya");
		    list.Add("France");
		    list.Add("Yemen");
		    list.Add("Thailand");
		    list.Add("Spain");
		    list.Add("Turkmenistan");
		    list.Add("Cameroon");
		    list.Add("Papua New Guinea");
		    list.Add("Sweden");
		    list.Add("Uzbekistan");
		    list.Add("Morocco");
		    list.Add("Iraq");
		    list.Add("Paraguay");
		    list.Add("Zimbabwe");
		    list.Add("Japan");
		    list.Add("Germany");
		    list.Add("Congo (Rep.)");
		    list.Add("Finland");
		    list.Add("Malaysia");
		    list.Add("Vietnam");
		    list.Add("Norway");
		    list.Add("Cote d'Ivoire  ");
		    list.Add("Poland");
		    list.Add("Italy");
		    list.Add("Philippines");
		    list.Add("Ecuador");
		    list.Add("Burkina Faso");
		    list.Add("New Zealand");
		    list.Add("Gabon");
		    list.Add("Guinea");
		    list.Add("United Kingdom (UK)");
		    list.Add("Ghana");
		    list.Add("Romania");
		    list.Add("Laos");
		    list.Add("Uganda");
		    list.Add("Guyana");
		    list.Add("Oman");
		    list.Add("Belarus");
		    list.Add("Kyrgyzstan");
		    list.Add("Senegal");
		    list.Add("Syria");
		    list.Add("Cambodia");
		    list.Add("Uruguay");
		    list.Add("Tunisia");
		    list.Add("Suriname");
		    list.Add("Bangladesh");
		    list.Add("Tajikstan");
		    list.Add("Nepal");
		    list.Add("Greece");
		    list.Add("Nicaragua");
		    list.Add("Eritrea");
		    list.Add("Korea (North)");
		    list.Add("Malawi");
		    list.Add("Benin  ");
		    list.Add("Honduras");
		    list.Add("Liberia");
		    list.Add("Bulgaria ");
		    list.Add("Cuba");
		    list.Add("Guatemala");
		    list.Add("Iceland");
		    list.Add("Serbia & Montenegro");
		    list.Add("Korea (South)");
		    list.Add("Hungary");
		    list.Add("Portugal");
		    list.Add("Jordan");
		    list.Add("Azerbaijan");
		    list.Add("Austria");
		    list.Add("United Arab Emirates");
		    list.Add("Czech Republic");
		    list.Add("Panama");
		    list.Add("Sierra Leone");
		    list.Add("Ireland");
		    list.Add("Georgia");
		    list.Add("Sri Lanka  ");
		    list.Add("Lithuania");
		    list.Add("Latvia");
		    list.Add("Togo");
		    list.Add("Croatia");
		    list.Add("Bosnia & Herzegovina");
		    list.Add("Costa Rica");
		    list.Add("Slovakia");
		    list.Add("Dominican Republic");
		    list.Add("Bhutan");
		    list.Add("Estonia");
		    list.Add("Denmark");
		    list.Add("Netherlands  ");
		    list.Add("Switzerland");
		    list.Add("Guinea-Bissau");
		    list.Add("Moldova");
		    list.Add("Belgium");
		    list.Add("Lesotho");
		    list.Add("Armenia");
		    list.Add("Albania");
		    list.Add("Solomon Islands");
		    list.Add("Equatorial Guinea");
		    list.Add("Burundi");
		    list.Add("Haiti");
		    list.Add("Rwanda");
		    list.Add("Macedonia");
		    list.Add("Djibouti");
		    list.Add("Belize");
		    list.Add("El Salvador");
		    list.Add("Israel");
		    list.Add("Slovenia");
		    list.Add("Fiji");
		    list.Add("Kuwait");
		    list.Add("Swaziland");
		    list.Add("East Timor");
		    list.Add("Bahamas");
		    list.Add("Vanuatu");
		    list.Add("Qatar");
		    list.Add("Gambia");
		    list.Add("Jamaica");
		    list.Add("Lebanon");
		    list.Add("Cyprus");
		    list.Add("Brunei");
		    list.Add("Trinidad & Tobago");
		    list.Add("Cape Verde");
		    list.Add("Samoa");
		    list.Add("Luxembourg");
		    list.Add("Comoros");
		    list.Add("Mauritius");
		    list.Add("Sao Tome & Principe");
		    list.Add("Kiribati");
		    list.Add("Dominica");
		    list.Add("Tonga");
		    list.Add("Micronesia");
		    list.Add("Singapore");
		    list.Add("Bahrain");
		    list.Add("Saint Lucia");
		    list.Add("Andorra");
		    list.Add("Palau");
		    list.Add("Seychelles");
		    list.Add("Antigua & Barbuda");
		    list.Add("Barbados");
		    list.Add("St. Vincent & the Grenadines");
		    list.Add("Grenada");
		    list.Add("Malta");
		    list.Add("Maldives");
		    list.Add("Saint Kitts & Nevis");
		    list.Add("Marshall Islands");
		    list.Add("Liechtenstein");
		    list.Add("San Marino");
		    list.Add("Tuvalu");
		    list.Add("Nauru");
		    list.Add("Monaco");
		    list.Add("Vatican City");

	    }
    }


    public class CountriesAdapter : BaseAdapter , DragSortListView.IDragSortListener
    {
        List<string> mItems;
        Activity mContext;
        DragSortListView mDslv;

        public CountriesAdapter(DragSortListView dslv, List<string> items)
            : base()
        {
            mItems = items;
            mDslv = dslv;
            mContext = (Activity)mDslv.Context;
        }

        public override int Count
        {
            get { return mItems.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return mItems[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View v = convertView ?? mContext.LayoutInflater.Inflate(Resource.Layout.simple_drag_list_item, null);
            TextView tv = v.FindViewById<TextView>(Resource.Id.name);
            tv.Text = mItems[position];

            bool isChecked = position == ((ListView)parent).CheckedItemPosition;

            tv.SetTextColor(isChecked ? Color.Yellow : Color.White);

            return v;
        }


        public void OnItemRemove(int position)
        {
            if (mItems != null)
            {
                mItems.RemoveAt(position);
                mDslv.RemoveCheckState(position);
                NotifyDataSetChanged();
            }
        }

        public void OnItemDrop(int from, int to)
        {
            if (from != to)
            {
                mItems.MoveItem(from, to);
                mDslv.MoveCheckState(from, to);
                NotifyDataSetChanged();
            }
            string msg = String.Format("item moved from {0} to {1}", from, to);
            Toast.MakeText(mContext, msg, ToastLength.Short).Show();
        }

        public void OnItemDrag(int from, int to)
        {
            //do nothing
        }
    }

    public static class ListHelper
    {
        public static void MoveItem<T>(this IList<T> list, int from, int to)
        {
            if (list.Count > from && list.Count > to)
            {
                T item = list[from];
                list.RemoveAt(from);
                list.Insert(to, item);
            }
        }
    }
}

