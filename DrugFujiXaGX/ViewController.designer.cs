// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DrugFujiXaGX
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextView outTextView { get; set; }

		[Action ("convertButton:")]
		partial void convertButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (outTextView != null) {
				outTextView.Dispose ();
				outTextView = null;
			}
		}
	}
}
