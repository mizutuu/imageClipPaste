namespace imageClipPaste.Properties {
    
    
    // このクラスでは設定クラスでの特定のイベントを処理することができます:
    //  SettingChanging イベントは、設定値が変更される前に発生します。
    //  PropertyChanged イベントは、設定値が変更された後に発生します。
    //  SettingsLoaded イベントは、設定値が読み込まれた後に発生します。
    //  SettingsSaving イベントは、設定値が保存される前に発生します。
    internal sealed partial class Settings {
        
        public Settings() {
            this.SettingsLoaded += Settings_SettingsLoaded;
        }

        /// <summary>
        /// 設定値が読み込まれた後に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Settings_SettingsLoaded(object sender, System.Configuration.SettingsLoadedEventArgs e)
        {
            if (Settings.Default.Setting == null)
                Settings.Default.Setting = new imageClipPaste.Settings.ImageClipPasteSetting();
        }
    }
}
