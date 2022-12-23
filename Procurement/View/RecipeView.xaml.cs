using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Procurement.ViewModel;

namespace Procurement.View
{
    /// <summary>
    /// Interaction logic for RecipeView.xaml
    /// </summary>
    public partial class RecipeView : UserControl, IView
    {
        public RecipeView()
        {
            InitializeComponent();
            this.DataContext = new RecipeResultViewModel();
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }

        public void RefreshRecipes()
        {
            (this.DataContext as RecipeResultViewModel).RefreshRecipes();
        }
    }
}
