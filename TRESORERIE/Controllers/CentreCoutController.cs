using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRESORERIE.Models;

namespace TRESORERIE.Controllers
{
    public class CentreCoutController : Controller
    {
        //
        // GET: /CentreCout/
        TresorerieEntities BD = new TresorerieEntities();
        public ActionResult AllCategorie()
        {
            List<CATEGORIES_CENTRES_COUTS> Liste = BD.CATEGORIES_CENTRES_COUTS.ToList();
            return View(Liste);
        }
        public ActionResult FormCat(string Mode, string Code)
        {
            CATEGORIES_CENTRES_COUTS Element = new CATEGORIES_CENTRES_COUTS();
            if (Mode == "Create")
            {
                ViewBag.Titre = "Ajouter une nouvelle catégorie de centre de couts ";
            }
            if (Mode == "Edit")
            {
                int CodeInt = int.Parse(Code);
                Element = BD.CATEGORIES_CENTRES_COUTS.Find(CodeInt);
                ViewBag.Titre = "Modifier une catégorie de centre de couts ";
            }
            ViewBag.Mode = Mode;
            ViewBag.Code = Code;
            return View(Element);
        }
        [HttpPost]
        public ActionResult SendFormCat(string Mode, string Code)
        {
            string CODE_CAT = Request.Params["CODE_CAT"] != null ? Request.Params["CODE_CAT"].ToString() : string.Empty;
            string LIBELLE = Request.Params["LIBELLE"] != null ? Request.Params["LIBELLE"].ToString() : string.Empty;
            if (Mode == "Create")
            {
                CATEGORIES_CENTRES_COUTS NewElement = new CATEGORIES_CENTRES_COUTS();
                NewElement.CODE = CODE_CAT;
                NewElement.LIBELLE = LIBELLE;
                BD.CATEGORIES_CENTRES_COUTS.Add(NewElement);
                BD.SaveChanges();
            }
            if (Mode == "Edit")
            {
                int CodeInt = int.Parse(Code);
                CATEGORIES_CENTRES_COUTS SelectedElement = BD.CATEGORIES_CENTRES_COUTS.Find(CodeInt);
                SelectedElement.CODE = CODE_CAT;
                SelectedElement.LIBELLE = LIBELLE;
                BD.SaveChanges();
            }
            return RedirectToAction("AllCategorie");
        }
        public ActionResult DeleteCat(string Code)
        {
            int CodeInt = int.Parse(Code);
            CATEGORIES_CENTRES_COUTS SelectedElement = BD.CATEGORIES_CENTRES_COUTS.Find(CodeInt);
            BD.CATEGORIES_CENTRES_COUTS.Remove(SelectedElement);
            BD.SaveChanges();
            return RedirectToAction("AllCategorie");
        }
        public ActionResult CentreCout()
        {
            List<CENTRES_COUTS> Liste = BD.CENTRES_COUTS.ToList();
            return View(Liste);
        }
        public ActionResult Form(string Mode, string Code)
        {
            CENTRES_COUTS Element = new CENTRES_COUTS();
            if (Mode == "Create")
            {
                ViewBag.Titre = "Ajouter un nouveau centre de couts ";
            }
            if (Mode == "Edit")
            {
                int CodeInt = int.Parse(Code);
                Element = BD.CENTRES_COUTS.Find(CodeInt);
                ViewBag.Titre = "Modifier un centre de couts ";
            }
            ViewBag.Mode = Mode;
            ViewBag.Code = Code;
            return View(Element);
        }
        [HttpPost]
        public ActionResult SendForm(string Mode, string Code)
        {
            string CODE_CENTRE = Request.Params["CODE_CENTRE"] != null ? Request.Params["CODE_CENTRE"].ToString() : string.Empty;
            string LIBELLE = Request.Params["LIBELLE"] != null ? Request.Params["LIBELLE"].ToString() : string.Empty;
            string CATEGORIE = Request.Params["CATEGORIE"] != null ? Request.Params["CATEGORIE"].ToString() : string.Empty;
            if (Mode == "Create")
            {
                CENTRES_COUTS NewElement = new CENTRES_COUTS();
                NewElement.CODE = CODE_CENTRE;
                NewElement.LIBELLE = LIBELLE;
                int ID_CATEGORIE = int.Parse(CATEGORIE);
                CATEGORIES_CENTRES_COUTS Cat = BD.CATEGORIES_CENTRES_COUTS.Find(ID_CATEGORIE);
                NewElement.CATEGORIE = ID_CATEGORIE;
                NewElement.CATEGORIES_CENTRES_COUTS = Cat;
                BD.CENTRES_COUTS.Add(NewElement);
                BD.SaveChanges();
            }
            if (Mode == "Edit")
            {
                int CodeInt = int.Parse(Code);
                CENTRES_COUTS SelectedElement = BD.CENTRES_COUTS.Find(CodeInt);
                SelectedElement.CODE = CODE_CENTRE;
                SelectedElement.LIBELLE = LIBELLE;
                int ID_CATEGORIE = int.Parse(CATEGORIE);
                CATEGORIES_CENTRES_COUTS Cat = BD.CATEGORIES_CENTRES_COUTS.Find(ID_CATEGORIE);
                SelectedElement.CATEGORIE = ID_CATEGORIE;
                SelectedElement.CATEGORIES_CENTRES_COUTS = Cat;
                BD.SaveChanges();
            }
            return RedirectToAction("CentreCout");
        }
        public ActionResult Delete(string Code)
        {
            int CodeInt = int.Parse(Code);
            CENTRES_COUTS SelectedElement = BD.CENTRES_COUTS.Find(CodeInt);
            BD.CENTRES_COUTS.Remove(SelectedElement);
            BD.SaveChanges();
            return RedirectToAction("CentreCout");
        }
        public JsonResult GetAllCategorie()
        {
            BD.Configuration.ProxyCreationEnabled = false;
            List<CATEGORIES_CENTRES_COUTS> Liste = BD.CATEGORIES_CENTRES_COUTS.ToList();
            return Json(Liste, JsonRequestBehavior.AllowGet);
        }

    }
}
