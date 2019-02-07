﻿#pragma checksum "..\..\..\Schedules\SchedulerView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7E64740ADA7687F389CFB7E3D71AB9AB58D1CC20"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AFH_Scheduler.Schedules;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
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


namespace AFH_Scheduler.Schedules {
    
    
    /// <summary>
    /// SchedulerView
    /// </summary>
    public partial class SchedulerView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 51 "..\..\..\Schedules\SchedulerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FilterTextBox;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Schedules\SchedulerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DatePickerStart;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\Schedules\SchedulerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ToTextBlock;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\Schedules\SchedulerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DatePickerEnd;
        
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
            System.Uri resourceLocater = new System.Uri("/AFH_Scheduler;component/schedules/schedulerview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Schedules\SchedulerView.xaml"
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
            this.FilterTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 52 "..\..\..\Schedules\SchedulerView.xaml"
            this.FilterTextBox.IsEnabledChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.FilterTextBox_IsEnabledChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DatePickerStart = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 3:
            this.ToTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.DatePickerEnd = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 5:
            
            #line 284 "..\..\..\Schedules\SchedulerView.xaml"
            ((MaterialDesignThemes.Wpf.PopupBox)(target)).Closed += new System.Windows.RoutedEventHandler(this.PopupBox_OnClosed);
            
            #line default
            #line hidden
            
            #line 285 "..\..\..\Schedules\SchedulerView.xaml"
            ((MaterialDesignThemes.Wpf.PopupBox)(target)).Opened += new System.Windows.RoutedEventHandler(this.PopupBox_OnOpened);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

