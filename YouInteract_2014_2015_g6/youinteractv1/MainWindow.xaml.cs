
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

public struct schedulers
{
    public int id { get; set; }
    public string startAt { get; set; }
    public string endAt { get; set; }
    public int monday { get; set; }
    public int tuesday { get; set; }
    public int wednesday { get; set; }
    public int thursday { get; set; }
    public int friday { get; set; }
    public int saturday { get; set; }
    public int sunday { get; set; }
    public string startTime { get; set; }
    public string endTime { get; set; }
    public string path { get; set; }
    public string type { get; set; }
    public string active { get; set; }
}

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
            /* TEMP 5*/

            Reference = this;
            youMenu = new YouMenu();
            screensaver = new Screensaver();
            //CallOfAttention = new YouInteractV1.CallOfAttention();
            string[] activeapps = YouInteractV1.LoaderData.ManageStructs.GetActiveApps().ToArray();
            //string[] activesche = YouInteractV1.LoaderData.ManageStructs.GetActiveSchedulers().ToArray();
            Loaded += KinectApi.onLoaded;
            PluginManager = new YouPluginManager(activeapps);
          //  PluginManager = new YouPluginManager(activesche);
            //YouFrame.Navigate(CallOfAttention);
            PluginManager.FrameReady += PluginManager_FrameReady;
            YouNavigation.NavigateToMainMenu += GoMain;
            KinectApi.bindRegion(youMenu.getRegion());
            
            Scheduler.Scheduler.initialize();
            //Scheduler.Scheduler.skeleton += Scheduler_skeleton;
            Scheduler.Scheduler.timeOut += Scheduler_timeOut;
            
            
            scrsaver = false;
            this.GoMain("");
        }

   
        void Scheduler_timeOut()
        {
            if (!scrsaver)
            {
                //Console.WriteLine("timeout");
                Scheduler.Scheduler.skeleton += Scheduler_skeleton;
                Scheduler.Scheduler.timeOut -= Scheduler_timeOut;
                screensaver.activateTimer();
                Reference.YouFrame.Navigate(screensaver);
                scrsaver = true;
            }
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
                //this.YouFrame.Navigate(CallOfAttention);
                Scheduler.Scheduler.skeleton -= Scheduler_skeleton;
                Scheduler.Scheduler.timeOut += Scheduler_timeOut;
                YouFrame.Navigate(youMenu);
                scrsaver = false;
                screensaver.stopTimer();
            }
        }
    }

}
