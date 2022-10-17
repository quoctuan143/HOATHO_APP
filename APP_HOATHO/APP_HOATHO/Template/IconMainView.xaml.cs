using APP_HOATHO.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_HOATHO.Template
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IconMainView : ContentView
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(IconMainView), string.Empty);
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(IconImageSource), typeof(ImageSource), typeof(IconMainView),default(ImageSource));
        public static readonly BindableProperty BadgeTextProperty = BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(IconMainView), string.Empty);
        public static readonly BindableProperty ActionCommandProperty = BindableProperty.Create(nameof(ActionCommand), typeof(ICommand), typeof(IconMainView));
        public static readonly BindableProperty DisableProperty = BindableProperty.Create(nameof(Disable), typeof(bool), typeof(IconMainView),false); 
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public string BadgeText 
        {
            get => (string)GetValue(BadgeTextProperty);
            set => SetValue(BadgeTextProperty, value);
        }
        public bool Disable
        {
            get => (bool)GetValue(DisableProperty);
            set => SetValue(DisableProperty, value);
        }

        public ImageSource IconImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
        
        public ICommand ActionCommand 
        {
            get =>(ICommand)GetValue(ActionCommandProperty);
            set => SetValue(ActionCommandProperty, value);
        }


        public IconMainView()
        {
            InitializeComponent();            
        }           

        private  void Frame_Tapped(object sender, EventArgs e)
        {
            ActionCommand.Execute(null);
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}