using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;


namespace MyMaps
{

    public class mLatLng
    {
        public double latitude { get; set; }
        public double longitude { get; set; }

        public mLatLng(double p0, double p1)
        {
            latitude = p0;
            longitude = p1;
        }
    }
    
    public class myMap : View
    {

        public myMap()
        { }

        public List<mLatLng> polyPoints= new List<mLatLng>();
    }
    

	public class App
	{

        static myMap map;

		public static Page GetMainPage()
		{

            ContentPage myContentPage = new ContentPage();
            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
            myContentPage.Content = stack;

            Button toggleScroll = new Button() { Text = "Toggle" };

            map = new myMap();
            stack.Children.Add(toggleScroll);
            stack.Children.Add(map);
          
            return myContentPage;
		}
	}
}
