﻿#pragma checksum "..\..\Update.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5E023CF97153FA0628E290BF13828CDC"
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
    /// Update
    /// </summary>
    public partial class Update : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\Update.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal _Hell_WPF_Multipack_Launcher.Update PageUpdate;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Update.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lDownloadFromLink;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\Update.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbNotify;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\Update.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bUpdate;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\Update.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bCancel;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\Update.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox gbCaption;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\Update.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label newVersion;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\Update.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbContent;
        
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
            System.Uri resourceLocater = new System.Uri("/Multipack_Launcher;component/update.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Update.xaml"
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
            this.PageUpdate = ((_Hell_WPF_Multipack_Launcher.Update)(target));
            
            #line 8 "..\..\Update.xaml"
            this.PageUpdate.Loaded += new System.Windows.RoutedEventHandler(this.PageUpdate_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lDownloadFromLink = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.cbNotify = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 4:
            this.bUpdate = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\Update.xaml"
            this.bUpdate.Click += new System.Windows.RoutedEventHandler(this.bUpdate_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.bCancel = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\Update.xaml"
            this.bCancel.Click += new System.Windows.RoutedEventHandler(this.bCancel_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.gbCaption = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 7:
            this.newVersion = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.tbContent = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

