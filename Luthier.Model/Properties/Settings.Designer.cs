﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Luthier.Model.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.5.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?><GraphicPlaneGridAppearence xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><MajorAxisColour><X>0.6</X><Y>0.6</Y><Z>0.6</Z><W>1</W></MajorAxisColour><MinorAxisColour><X>0.8</X><Y>0.8</Y><Z>0.8</Z><W>1</W></MinorAxisColour></GraphicPlaneGridAppearence>")]
        public global::Luthier.Model.CustomSettings.GraphicPlaneGridAppearence GraphicPlaneGridAppearance {
            get {
                return ((global::Luthier.Model.CustomSettings.GraphicPlaneGridAppearence)(this["GraphicPlaneGridAppearance"]));
            }
            set {
                this["GraphicPlaneGridAppearance"] = value;
            }
        }
    }
}
