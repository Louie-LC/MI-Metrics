using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MI_Metrics
{
    public enum PageType
    {
        Home = 0,
        Upload = 1,
        Template = 2,
        View = 3,
        Organize = 4
    }
    public partial class MainWindow : Window
    {
        public static Database database;
        public static ProductManager productManager;
        public static string currentDir;

        private static Page[] allPages;

        public MainWindow()
        {
            // Set the current directory to the base directory where the executable is.
            currentDir = AppDomain.CurrentDomain.BaseDirectory;
            database = new Database("LocalScannerMetrics");
            productManager = new ProductManager();
            int pageCount = Enum.GetNames(typeof(PageType)).Length;
            allPages = new Page[pageCount];
            InitializeComponent();
            SetProductToView(productManager.Products[0]);
            NavigateToViewPage();
        }


        public static Page GetPage(PageType page)
        {
            // This method ensures that only one object of each page is ever made.
            if(allPages[(int)page] == null)
            {
                switch (page)
                {
                    case PageType.Home:
                        allPages[(int)page] = new HomePage();
                        break;
                    case PageType.Upload:
                        allPages[(int)page] = new UploadPage();
                        break;
                    case PageType.Template:
                        allPages[(int)page] = new TemplatePage();
                        break;
                    // The view page needs to be handled seperately since a new page may need to be generated for it depending on the product selected.
                    case PageType.View:
                        break;
                    case PageType.Organize:
                        allPages[(int)page] = new OrganizePage();
                        break;
                    
                }
            }
            return allPages[(int)page];
        }
        
        public void SetNavigationDefaultStyle()
        {
            NavigationSelectSystem.Style = (Style)Application.Current.Resources["TextIconButton"];
            NavigationViewData.Style = (Style)Application.Current.Resources["TextIconButton"];
            NavigationUploadData.Style = (Style)Application.Current.Resources["TextIconButton"];
            NavigationManageTemplate.Style = (Style)Application.Current.Resources["TextIconButton"];
            NavigationOrganizeData.Style = (Style)Application.Current.Resources["TextIconButton"];
        }

        private void NavigationSelectSystem_Click(object sender, RoutedEventArgs e)
        {
            NavigateToSelectSystem();
        }

        private void NavigationUploadData_Click(object sender, RoutedEventArgs e)
        {
            NavigateToUploadData();
        }

        private void NavigationViewData_Click(object sender, RoutedEventArgs e)
        {
            NavigateToViewPage();
        }

        private void NavigationManageTemplate_Click(object sender, RoutedEventArgs e)
        {
            NavigateToManageTemplate();
        }

        private void NavigationOrganizeData_Click(object sender, RoutedEventArgs e)
        {
            NavigateToOrganizePage();
        }

        private void NavigateToSelectSystem()
        {
            Page homePage = MainWindow.GetPage(PageType.Home);
            WindowFrame.NavigationService.Navigate(homePage);
            SetNavigationDefaultStyle();
            NavigationSelectSystem.Style = (Style)Application.Current.Resources["TextIconButtonSelected"];
            this.Title = homePage.Title;
        }

        private void NavigateToUploadData()
        {
            Page uploadPage = MainWindow.GetPage(PageType.Upload);
            WindowFrame.NavigationService.Navigate(uploadPage);
            SetNavigationDefaultStyle();
            NavigationUploadData.Style = (Style)Application.Current.Resources["TextIconButtonSelected"];
            this.Title = uploadPage.Title;
        }
        
        public void NavigateToManageTemplate()
        {
            Page templatePage = MainWindow.GetPage(PageType.Template);
            WindowFrame.NavigationService.Navigate(templatePage);
            SetNavigationDefaultStyle();
            NavigationManageTemplate.Style = (Style)Application.Current.Resources["TextIconButtonSelected"];
            this.Title = templatePage.Title;
        }

        public void NavigateToViewPage()
        {
            Page viewPage = MainWindow.GetPage(PageType.View);
            WindowFrame.NavigationService.Navigate(viewPage);
            SetNavigationDefaultStyle();
            NavigationViewData.Style = (Style)Application.Current.Resources["TextIconButtonSelected"];
            this.Title = viewPage.Title;
        }

        public void SetProductToView(Product product)
        {
            ViewPage viewPage = (ViewPage)allPages[(int)PageType.View];
            if (viewPage == null || viewPage.GetViewedProduct() != product)
            {
                viewPage = new ViewPage(product);
                allPages[(int)PageType.View] = viewPage;
            }
            NavigationViewData.IsEnabled = true;
            NavigationViewData.Content = product.Name + " Data";
        }

        public void NavigateToOrganizePage()
        {
            Page organizePage = MainWindow.GetPage(PageType.Organize);
            WindowFrame.NavigationService.Navigate(organizePage);
            SetNavigationDefaultStyle();
            NavigationOrganizeData.Style = (Style)Application.Current.Resources["TextIconButtonSelected"];
            this.Title = organizePage.Title;
        }
    }
}
