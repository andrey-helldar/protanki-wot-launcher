﻿#pragma checksum "..\..\WarApiOpenID.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5E267D362C26536DB9E9D52748F08996"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace _Hell_WPF_Multipack_Launcher {
    
    
    /// <summary>
    /// WarApiOpenID
    /// </summary>
    public partial class WarApiOpenID : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\WarApiOpenID.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal _Hell_WPF_Multipack_Launcher.WarApiOpenID WarApiOpenID1;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\WarApiOpenID.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WebBrowser WB;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Multipack Launcher 2;component/warapiopenid.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\WarApiOpenID.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.WarApiOpenID1 = ((_Hell_WPF_Multipack_Launcher.WarApiOpenID)(target));
            
            #line 4 "..\..\WarApiOpenID.xaml"
            this.WarApiOpenID1.Loaded += new System.Windows.RoutedEventHandler(this.WarApiOpenID1_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.WB = ((System.Windows.Controls.WebBrowser)(target));
            
            #line 9 "..\..\WarApiOpenID.xaml"
            this.WB.LoadCompleted += new System.Windows.Navigation.LoadCompletedEventHandler(this.WebBrowser_LoadCompleted);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

