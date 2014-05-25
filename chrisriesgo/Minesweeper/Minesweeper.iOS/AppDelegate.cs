﻿using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Minesweeper.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		// class-level declarations
		public override UIWindow Window {
			get;
			set;
		}

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
//		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
//		{
//			// create a new window instance based on the screen size
//			window = new UIWindow(UIScreen.MainScreen.Bounds);
//			
//			// If you have defined a root view controller, set it here:
//			window.RootViewController = new MinesweeperViewController();
//			
//			// make the window visible
//			window.MakeKeyAndVisible();
//			
//			return true;
//		}
	}

	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}
