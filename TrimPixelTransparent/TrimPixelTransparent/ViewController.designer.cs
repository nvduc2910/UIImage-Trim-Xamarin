// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TrimPixelTransparent
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton btnTrim { get; set; }

		[Outlet]
		UIKit.UIImageView ivAfterTrim { get; set; }

		[Outlet]
		UIKit.UIImageView ivOrginal { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ivOrginal != null) {
				ivOrginal.Dispose ();
				ivOrginal = null;
			}

			if (ivAfterTrim != null) {
				ivAfterTrim.Dispose ();
				ivAfterTrim = null;
			}

			if (btnTrim != null) {
				btnTrim.Dispose ();
				btnTrim = null;
			}
		}
	}
}
