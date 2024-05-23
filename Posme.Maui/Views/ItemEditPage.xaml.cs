using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Maui.Core;
using DevExpress.Maui.DataForm;

namespace Posme.Maui.Views;

public partial class ItemEditPage : ContentPage
{
    
    DetailEditFormViewModel ViewModel => (DetailEditFormViewModel)BindingContext;
    
    public ItemEditPage()
    {
        InitializeComponent();
    }
    
    void SaveItemClick(object sender, EventArgs e) {
        if (!DataForm.Validate())
            return;
        DataForm.Commit();
        ViewModel.Save();
    }


    void dataForm_ValidateProperty(object sender, DataFormPropertyValidationEventArgs e) {
       
    }
}