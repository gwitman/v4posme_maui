﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Posme.Maui.ViewModels;

namespace Posme.Maui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataGridPage : ContentPage
    {
        public DataGridPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new DataGridViewModel();
        }

        DataGridViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}