using System;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using MyMaps;
using System.Collections.Generic;

[assembly: ExportRenderer(typeof(myMap), typeof(MyMaps.Droid.myMapRenderer))]

namespace MyMaps.Droid
{
    [Activity(Label = "MyMaps", MainLauncher = true, ScreenOrientation=Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this, bundle);
            SetPage(App.GetMainPage());
        }
    }

    public class myMapRenderer : NativeRenderer
    {
        public myMapRenderer() { }

        PolylineOptions polylineoptions;
        Polyline polyline;
        myMap _myMapView;
        private static GoogleMap _map;
        private MapFragment _mapFragment;
        FrameLayout fram_map;
        Android.Widget.RelativeLayout containerView;

        protected override void OnModelChanged(VisualElement oldModel, VisualElement newModel)
        {
            _myMapView = (myMap)newModel;
            Activity activity = (Activity) this.Context;
            base.OnModelChanged(oldModel, newModel);

            LayoutInflater inflatorservice = (LayoutInflater)Context.GetSystemService(Android.Content.Context.LayoutInflaterService);
            containerView = (Android.Widget.RelativeLayout)inflatorservice.Inflate(Resource.Layout.map_layout, null, false);
            SetNativeControl(containerView); 

            _mapFragment = activity.FragmentManager.FindFragmentById(Resource.Id.map) as MapFragment;

            _map = _mapFragment.Map;
            _map.MyLocationEnabled = true;
            LatLng Phoenix = new LatLng(33.5, -112);
            _map.MoveCamera(CameraUpdateFactory.NewLatLng(Phoenix));

            fram_map =FindViewById<FrameLayout>(Resource.Id.fram_map);
            fram_map.Touch += fram_map_Touch;    
        }

        private void fram_map_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            string message;
            fram_map.OnTouchEvent(e.Event);
            Projection proj = _map.Projection;
            LatLng ll;

            switch (e.Event.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:
                    message = "Down";
                    ll = proj.FromScreenLocation(new Android.Graphics.Point(Convert.ToInt32(e.Event.GetX()),Convert.ToInt32(e.Event.GetY())));
                    _myMapView.polyPoints.Add(new mLatLng(ll.Latitude, ll.Longitude));
                    polylineoptions = new PolylineOptions();
                    polylineoptions.Add(ll);
                    polyline = _map.AddPolyline(polylineoptions);
                    break;

                case MotionEventActions.Move:
                    message = "Move";
                    ll = proj.FromScreenLocation(new Android.Graphics.Point(Convert.ToInt32(e.Event.GetX()),Convert.ToInt32(e.Event.GetY())));
                    _myMapView.polyPoints.Add(new mLatLng(ll.Latitude, ll.Longitude));
                    List<LatLng>l=new  List<LatLng>();

                    foreach (mLatLng p in _myMapView.polyPoints) {
                        l.Add(new LatLng(p.latitude,p.longitude));
                    }
                    polyline.Points = l;

                    Console.WriteLine(ll.Latitude + ", " + ll.Longitude);
                    break;

                case MotionEventActions.Up:
                    message = "Up";       
                    ll = proj.FromScreenLocation(new Android.Graphics.Point(Convert.ToInt32(e.Event.GetX()),Convert.ToInt32(e.Event.GetY())));
                    break;

                default:
                    message = string.Empty;
                    message = e.Event.Action.ToString();
                    break;
            }   
        }
    }
}