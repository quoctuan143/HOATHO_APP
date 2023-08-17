using APP_HOATHO.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Template
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ButtonSearch : ContentView
    {
        public event EventHandler ClickedButton;

        public void OnClicked()
        {
            ClickedButton?.Invoke(this, EventArgs.Empty);
        }
        public ButtonSearch()
        {
            InitializeComponent();
        }
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(ButtonSearch), string.Empty);
        public static readonly BindableProperty TextProperty  = BindableProperty.Create(nameof(Text), typeof(string), typeof(ButtonSearch), string.Empty);
        public static readonly BindableProperty CodeProperty = BindableProperty.Create(nameof(Code), typeof(string), typeof(ButtonSearch), string.Empty);
        public static readonly BindableProperty ActionCommandProperty = BindableProperty.Create(nameof(ActionCommand), typeof(ICommand), typeof(ButtonSearch));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value); 
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public string Code 
        {
            get => (string)GetValue(CodeProperty);
            set => SetValue(CodeProperty, value);
        }
        public ICommand ActionCommand
        {
            get => (ICommand)GetValue(ActionCommandProperty);
            set => SetValue(ActionCommandProperty, value);
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActionCommand.Execute(null);
            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
            
        }
    }
}