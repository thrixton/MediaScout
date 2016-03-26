using MediaScoutGUI.Properties;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MediaScoutGUI.Wizard
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public class Wizard : NavigationWindow, IComponentConnector
	{
		private bool _contentLoaded;

		public Wizard()
		{
			this.InitializeComponent();
		}

		private void NavigationWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (Settings.Default.EnableGlassFrame)
			{
				this.GetBoundsForGlassFrame();
				HwndSource hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
				hwndSource.AddHook(new HwndSourceHook(this.WndProc));
				this.SetGlassFrame(true);
			}
		}

		public Rect GetBoundsForGlassFrame()
		{
			return VisualTreeHelper.GetContentBounds(this);
		}

		public bool SetGlassFrame(bool ExtendGlass)
		{
			bool result;
			if (ExtendGlass)
			{
				Rect boundsForGlassFrame = this.GetBoundsForGlassFrame();
				result = GlassHelper.ExtendGlassFrame(this, new Thickness(boundsForGlassFrame.Left, boundsForGlassFrame.Top, boundsForGlassFrame.Right, boundsForGlassFrame.Bottom));
			}
			else
			{
				GlassHelper.DisableGlassFrame(this);
				result = true;
			}
			return result;
		}

		public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			int num = 798;
			if (msg == num)
			{
				this.SetGlassFrame(GlassHelper.IsGlassEnabled);
				handled = true;
			}
			return IntPtr.Zero;
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/MediaScoutGUI;component/wizard/wizard.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}

		[EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}
	}
}