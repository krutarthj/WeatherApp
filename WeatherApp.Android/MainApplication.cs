﻿using System;
using Acr.UserDialogs;
using Android.App;
using Android.Runtime;
using MvvmCross.Droid.Support.V7.AppCompat;
using WeatherApp.Core;

namespace WeatherApp.Android
{
#if DEBUG
    [Application(Debuggable = true)]
#else
    [Application(Debuggable = false)]
#endif
    public class MainApplication : MvxAppCompatApplication<Setup, App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Xamarin.Essentials.Platform.Init(this);
            UserDialogs.Init(this);
        }
    }
}
