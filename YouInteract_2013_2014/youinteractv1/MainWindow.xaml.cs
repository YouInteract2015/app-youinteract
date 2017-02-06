
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YouInteract;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;
using YouInteract.YouPlugin_System;
using YouInteractV1.LoaderData;
using YouInteractV1.Scheduler;

namespace YouInteractV1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal YouMenu youMenu;
        private YouPluginManager PluginManager;
        private CallOfAttention CallOfAttention;
        private Screensaver screensaver;
        private bool scrsaver;
        public static MainWindow Reference;
        private KinectRequirements r;
        public static bool debug;

        public MainWindow()
        {
            debug = true;
            InitializeComponent();

            YouWindow.setWindow(this.Height, this.Width);

            /* TEMP */
            Console.WriteLine(Loader.Load() ? "Load was successful" : "LOAD WAS NOT SUCCESSFUL!");
            /* TEMP */

            Reference = this;
            youMenu = new YouMenu();
            //CallOfAttention = new YouInteractV1.CallOfAttention();
            string[] activeapps = YouInteractV1.LoaderData.ManageStructs.GetActiveApps().ToArray();
            Loaded += KinectApi.onLoaded; // comentar para nao usar kinect

            PluginManager = new YouPluginManager(activeapps);
            //YouFrame.Navigate(CallOfAttention);
            PluginManager.FrameReady += PluginManager_FrameReady;
            YouNavigation.NavigateToMainMenu += GoMain;
            KinectApi.bindRegion(youMenu.getRegion());
            //Scheduler.Scheduler.initialize();
            //Scheduler.Scheduler.timeOut += Scheduler_timeOut;
            //Scheduler.Scheduler.skeleton += Scheduler_skeleton;
            //screensaver = new Screensaver();
            //scrsaver = false;
            this.GoMain("shit");
        }

   
        void Scheduler_timeOut()
        {
            Console.WriteLine("timeout");
            screensaver.activateTimer();
            Reference.YouFrame.Navigate(screensaver);
            scrsaver = true;
        }

        private void GoMain(string e)
        {
            YouFrame.Navigate(youMenu);
            KinectApi.bindRegion(youMenu.getRegion());
        }

        void PluginManager_FrameReady(YouPluginManager.NavigateArgs e, KinectRequirements req)
        {
            YouPluginManager.updateKinect(req);
            YouFrame.Navigate(e.Destination);
        }

        internal void MainMenuChange(String appname)
        {
            KinectApi.unbindRegion(youMenu.getRegion());
            PluginManager.freeNavigate(appname);
        }

        private void Scheduler_skeleton()
        {
            if (scrsaver)
            {
                this.YouFrame.Navigate(CallOfAttention);
                scrsaver = false;
                screensaver.stopTimer();
            }
        }
    }

}
