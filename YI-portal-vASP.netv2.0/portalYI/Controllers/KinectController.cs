using System;
using System.Data.Entity; 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net; 
using System.Web.Mvc;
using portalYI.Models;
using System.IO;


namespace portalYI.Controllers
{
    public class KinectController : Controller
    {


        public ActionResult Device()
        {
            DatabaseEntities2 db = new DatabaseEntities2();
            List<kinect_devices> ls = new List<kinect_devices>();
            ls = db.kinect_devices.ToList();
            return View(ls);
        }

        public ActionResult Config()
        {
            DatabaseEntities2 db = new DatabaseEntities2();
            List<kinect_configs> ls = new List<kinect_configs>();
            ls = db.kinect_configs.ToList();

            return View(ls);
        }


        public ActionResult InsertConfig(FormCollection formulario)
        {
            string titulo = formulario["titleConfig"];
            string descricao = formulario["descriConfig"];
            int idTemplate = int.Parse(formulario["idTemplate"]);
            int idScheduler = int.Parse(formulario["idScheduler"]);

            DatabaseEntities2 dc = new DatabaseEntities2();
            kinect_configs kt = new kinect_configs();

            kt.title = titulo;
            kt.description = descricao;
            kt.template_id = idTemplate;
            kt.scheduler_id = idScheduler; 
            kt.created_at = DateTime.Now;
            kt.updated_at = DateTime.Now;

            dc.kinect_configs.Add(kt);
            dc.SaveChanges();

            return RedirectToAction("Config");

        }



        public ActionResult Theme()
        {
            DatabaseEntities2 db = new DatabaseEntities2();
            List<kinect_templates> ls = new List<kinect_templates>();
            ls = db.kinect_templates.ToList();
            return View(ls);
        }


        public ActionResult Delete(int? id)
        {
            DatabaseEntities2 db = new DatabaseEntities2();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kinect_templates movie = db.kinect_templates.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: /Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DatabaseEntities2 db = new DatabaseEntities2();
            kinect_templates movie = db.kinect_templates.Find(id);
            db.kinect_templates.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Themes");
        }


        [HttpPost]
        public ActionResult InsertTheme(FormCollection formulario, HttpPostedFileBase file)
        {
            string titulo = formulario["titleTheme"];
            string descricao = formulario["descriTheme"];
            string caminho = formulario["urlTheme"];


            DatabaseEntities2 dc = new DatabaseEntities2();
            kinect_templates kt = new kinect_templates();

            kt.title = titulo;
            kt.description = descricao;
            kt.preview = "prev";
            kt.created_at = DateTime.Now;
            kt.updated_at = DateTime.Now;


            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/BGThemes"), fileName);
                file.SaveAs(path);
                kt.path = path;
            }

            dc.kinect_templates.Add(kt);
            dc.SaveChanges();

            return RedirectToAction("Theme");
        }



        [HttpPost]
        public ActionResult InsertAPP(FormCollection formulario, HttpPostedFileBase file)
        {
            string titulo = formulario["NameApps"];
            string descricao = formulario["descAPPS"];
            string versao = formulario["versionAps"];


            DatabaseEntities2 dc = new DatabaseEntities2();
            kinect_items kt = new kinect_items();

            kt.title = titulo;
            kt.description = descricao;
            kt.version = versao;
            kt.created_at = DateTime.Now;
            kt.updated_at = DateTime.Now;


            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/APPsKinect"), fileName);
                file.SaveAs(path);
                kt.dll = path;
            }

            dc.kinect_items.Add(kt);
            dc.SaveChanges();

            return RedirectToAction("APPs");
        }


        public ActionResult APPs()
        {
            DatabaseEntities2 db = new DatabaseEntities2();
            List<kinect_items> ls = new List<kinect_items>();
            ls = db.kinect_items.ToList();
            return View(ls);
        }

        public ActionResult Scheduler()
        {
            return View();
        }

        public ActionResult EditDevice()
        {
            return View();

        }

        public ActionResult ChoiceAPPs()
        {
            return View();
        }

        public ActionResult EditConfig()
        {
            return View();
        }
    }
}